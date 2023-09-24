bool nfc_setup() {  //Inizializzo L'NFC
  nfc.begin();

  NFC_VERSION = nfc.getFirmwareVersion();

  if (!NFC_VERSION) {
    return false;
  }
  // cconfigura la board a leggere i tag RFID
  nfc.SAMConfig();
  return true;
}

void nfc_print_version() {
  TERMINAL.print("PN5");
  TERMINAL.print((NFC_VERSION >> 24) & 0xFF, HEX);
  TERMINAL.print(" FW: ");
  TERMINAL.print((NFC_VERSION >> 16) & 0xFF, DEC);
  TERMINAL.print('.');
  TERMINAL.println((NFC_VERSION >> 8) & 0xFF, DEC);
}

void nfc_loop() {  //Leggo L'NFC
  static uint8_t nfc_validate_passkey[16]{ NFC_LOGIN_TAG };

  static bool success;
  static uint8_t uid[] = { 0, 0, 0, 0, 0, 0, 0 };  // Buffer per salvare per UID
  static uint8_t uidLength;                        // Lunghezza UID (4 or 7 bytes dipende odal tipo di carta)
  static uint8_t customTrailer[16];                //usa per salvare i privileggi letti e i crc di convalida
  static uint8_t block1[16];                       //usa per salvare il settore della passkey
  static uint8_t block2[16];                       //usa per salvare il settore della custom line2

  static uint8_t index = 0;  // questo valore non superera' mai 255

  uint8_t byte0[4] = { 0 };  //quasta variabile e' di ausilio ai calcoli x la macchina a stati

  //TERMINAL.print("St :");
  //TERMINAL.println(st_nfc_index);
  switch (st_nfc_index) {
    case NFC_JUST_WAITING:                                                            //grazie alla libreria custom permette di fare la lettura Async della presenza del tag
      success = nfc_async_wait_passive_tag(PN532_MIFARE_ISO14443A, uid, &uidLength);  //Richiama sempre la macchina a stati che deve leggere il TAG
      if (success) st_nfc_index = NFC_READING_CARD;
      break;

    case NFC_READING_CARD:
      // Display some basic information about the card
#ifdef NFC_DEBUG_PRINTCARDINFO
      TERMINAL.println("Found an ISO14443A card");
      TERMINAL.print("  UID Length: ");
      TERMINAL.print(uidLength, DEC);
      TERMINAL.println(" bytes");
      TERMINAL.print("  UID Value: ");
      nfc.PrintHex(uid, uidLength);
      TERMINAL.println("");
#endif
      //Ora cerca un settore valido
      st_nfc_index = NFC_SEARCH_SECTOR;
      RESET_COUNTER(index);            //Resetta il contatore degli index letti
      if (uidLength != 4) {            //Controlla che l'uid della carta sia valido
        st_nfc_index = NFC_END_CICLE;  //Cosi fa un reset della lettura della card attuale
      }
      break;
    case NFC_SEARCH_SECTOR:  //Legge i settori fino a trovarne uno accessibile
      ADD_TO_INDEX(index, 1);
      if (index > NFC_MAX_SECTOR_READ) {  //controlla se i settori sono finiti
        st_nfc_index = NFC_ADD_INVALID_CARD;
        break;  //Esce dallo stato e non continua il codice sottostante
      }

      //Cerca di autentificarsi al settore
      success = nfc.mifareclassic_AuthenticateBlock(uid, uidLength, BLOCK_NUMBER_OF_SECTOR_TRAILER(index), 0, NFC_AUTH_KEYA);
      if (success) {
#ifdef NFC_DEBUG_PRINTCARDINFO
        TERMINAL.print("----Lettura settore ");
        TERMINAL.print(index);
        TERMINAL.println("----");
#endif
        st_nfc_index = NFC_READ_BLOCK0;  //Va alla ricerca della PassKey - sposta lo stato nella ricerca della PassKey altrimenti rimane qui
      }
      break;
    case NFC_READ_BLOCK0:  //Legge il blocco 0 dove il custom trailer
      //Legge il settore a cui si e' autentificato
      success = nfc.mifareclassic_ReadDataBlock(BLOCK_NUMBER_OF_SECTOR_TRAILER(index) - 3, customTrailer);  // Legge il settore dei privileggi e del crc
      (success) ? (st_nfc_index = NFC_READ_BLOCK1) : (st_nfc_index = NFC_SEARCH_SECTOR);
#ifdef NFC_DEBUG_PRINTCARDINFO
      if (!success) return;
      // legge i dati e li invia su seriale
      TERMINAL.print("Block 0  ");
      nfc.PrintHexChar(customTrailer, 16);
#endif
      break;
    case NFC_READ_BLOCK1:  //Legge il blocco 1 dove la passkey
      //Legge il settore a cui si e' autentificato
      success = nfc.mifareclassic_ReadDataBlock(BLOCK_NUMBER_OF_SECTOR_TRAILER(index) - 2, block1);  // Legge il settore della passkey
      (success) ? (st_nfc_index = NFC_READ_BLOCK2) : (st_nfc_index = NFC_SEARCH_SECTOR);
#ifdef NFC_DEBUG_PRINTCARDINFO
      if (!success) return;
      // legge i dati e li invia su seriale
      TERMINAL.print("Block 1  ");
      nfc.PrintHexChar(block1, 16);
#endif
      break;
    case NFC_READ_BLOCK2:  //Legge il blocco 1 dove la passkey
      //Legge il settore a cui si e' autentificato
      success = nfc.mifareclassic_ReadDataBlock(BLOCK_NUMBER_OF_SECTOR_TRAILER(index) - 1, block2);  // Legge il settore del nome
      (success) ? (st_nfc_index = NFC_VALIDATE_BADGE) : (st_nfc_index = NFC_SEARCH_SECTOR);
#ifdef NFC_DEBUG_PRINTCARDINFO
      if (!success) return;
      // legge i dati e li invia su seriale
      TERMINAL.print("Block 2  ");
      nfc.PrintHexChar(block2, 16);
      TERMINAL.println("---------");
#endif
      break;
    case NFC_VALIDATE_BADGE:
      /* 
      valida i crc dei settori
      */
      byte0[0] = CRC8(customTrailer, 15);  //calcoliamo il crc8 del customTrailer
      byte0[1] = CRC8(block1, 16);         //calcoliamo il crc8 del settore inerente alla passkey
      byte0[2] = CRC8(block2, 16);         //calcoliamo il crc8 del settore inerente alla riga del nome

#ifdef NFC_DEBUG_PRINTCARDINFO
        // legge i dati e li invia su seriale
      TERMINAL.print("CRC8 trailer :");
      TERMINAL.print(byte0[0], HEX);
      TERMINAL.print(" = ");
      TERMINAL.println(customTrailer[15], HEX);
      TERMINAL.print("CRC8 passKey :");
      TERMINAL.print(byte0[1], HEX);
      TERMINAL.print(" = ");
      TERMINAL.println(customTrailer[13], HEX);
      TERMINAL.print("CRC8 nome :");
      TERMINAL.print(byte0[2], HEX);
      TERMINAL.print(" = ");
      TERMINAL.println(customTrailer[14], HEX);
      TERMINAL.println("---------");
#endif

      //validiamo i CRC
      if (!((byte0[0] == customTrailer[15]) && (byte0[1] == customTrailer[13]) && (byte0[2] == customTrailer[14]))) {  //se tutti i crc sono sbagliati va in errore
#ifdef NFC_DEBUG_PRINTCARDINFO
        TERMINAL.println("CRC non validi");
#endif
        st_nfc_index = NFC_SEARCH_SECTOR;
        return;
      }
      st_nfc_index = NFC_LOGIN_BADGE;
      break;
    case NFC_LOGIN_BADGE:
      for (int i = 0; i < (16 - 3); i++) {           // Non tiene conto di convalidare il CRC
        if (nfc_validate_passkey[i] != customTrailer[i]) {  //Se anche un solo carattere e' diverso
#ifdef NFC_DEBUG_PRINTCARDINFO
          TERMINAL.println("Pass Key Errato!");
#endif
          st_nfc_index = NFC_SEARCH_SECTOR;  //Se il passkey e' errato prova in un'altro settore cosi da verificare tutti i settori prima di dare accesso negato
          return;                            //interrompe lo stato di validazione
        }
      }
      //PassKey valido, ha riconosciuto correttamente il badge
      st_nfc_index = NFC_ADD_VALID_CARD;
      break;
    case NFC_ADD_VALID_CARD:
      NFC_CARD_NEW(uid, uidLength, customTrailer, block1, block2, true);
#ifdef NFC_DEBUG_PRINTCARDINFO
      TERMINAL.println("Carta valida, Login = success");
#endif
      st_nfc_index = NFC_END_CICLE;
      break;
    case NFC_ADD_INVALID_CARD:
      NFC_CARD_NEW(uid, uidLength, customTrailer, block1, block2, false);
#ifdef NFC_DEBUG_PRINTCARDINFO
      TERMINAL.println("Carta non valida, Login = fail");
#endif
      st_nfc_index = NFC_END_CICLE;
      break;
    case NFC_END_CICLE:  // resetta i dati del tag
      success = false;
      for (int i = 0; i < 7; i++) {
        uid[i] = 0x0;
      }
      uidLength = 0x0;
      st_nfc_index = NFC_JUST_WAITING;
      break;


  }  //end case
}  //end void

//Macchina a stati che legge il valore del TAG
bool nfc_async_wait_passive_tag(uint8_t cardbaudrate, uint8_t* uid, uint8_t* uidLength) {
  switch (st_nfc_wait_index) {
    case NFC_WAIT_SEND_COMMAND:  //Equivalente di sendCommandCheckAck
      st_nfc_wait_index = NFC_WAIT_READY;

      //Prepara il payload per la ricerca
      _pn532_packetbuffer[0] = PN532_COMMAND_INLISTPASSIVETARGET;
      _pn532_packetbuffer[1] = 1;  // max 1 cards at once (we can set this to 2 later)
      _pn532_packetbuffer[2] = cardbaudrate;
      nfc.writecommand(_pn532_packetbuffer, 3);  //Invia il payload

      START_TIMER(timer0_NFC);  //Resetta il timer per fare i controlli
      break;
    case NFC_WAIT_READY:  //Attende che il chip sia pronto
      //Controlla che lo stato di comunicazione del PN532 e' impostato su ready
      if (NFC_IS_READY_I2C) st_nfc_wait_index = NFC_WAIT_READ_ACK;
      else if (IS_DELAYED(timer0_NFC, NFC_TIMEOUT_READING)) {
        st_nfc_wait_index = NFC_DELAY_NFC;
        //resetta il timer
        START_TIMER(timer0_NFC);
      }
      break;
    case NFC_WAIT_READ_ACK:  // legge acknowledgement del chip
      /*st_nfc_wait_index = NFC_DELAY_NFC;  //di base ipotizza che il read ack e' andato male
      //Legge realmente l'ack e in caso di esito positivo cambia lo stato della macchina
      if (nfc.readack()) st_nfc_wait_index = NFC_WAIT_RETURN_TRUE;*/
      //La versione su una riga del codice scritto nei commenti sopra.
      st_nfc_wait_index = ((nfc.readack()) ? (NFC_WAIT_RETURN_TRUE) : (NFC_DELAY_NFC));
      START_TIMER(timer0_NFC);
      break;
    case NFC_WAIT_RETURN_TRUE:
      //Attende che il badge si avvicini al campo del lettore.
      if (NFC_IS_READY_I2C) {
        st_nfc_wait_index = NFC_DELAY_NFC;                       //Resetto la macchina a stati
        START_TIMER(timer0_NFC);                                 //Resetta il timer per fare i controlli
        return nfc.readDetectedPassiveTargetID(uid, uidLength);  //Ritorno i dati del tag letto
      }
      break;
    case NFC_DELAY_NFC:
      if (IS_DELAYED(timer0_NFC, 50)) {             //aspetta 50ms di prima di ricominciare la lettura di un nuovo tag
        st_nfc_wait_index = NFC_WAIT_SEND_COMMAND;  //Resetto la macchina a stati
      }
      break;
  }
  return false;  //Di base ritorna che non ha rilevato nessun Tag valido se relase e' impostato su false
}

uint8_t CRC8(const uint8_t* data, uint8_t len) {
  uint8_t crc = 0x00;
  while (len--) {
    uint8_t extract = *data++;
    for (uint8_t tempI = 8; tempI; tempI--) {
      uint8_t sum = (crc ^ extract) & 0x01;
      crc >>= 1;
      if (sum) {
        crc ^= 0x8C;
      }
      extract >>= 1;
    }
  }
  return crc;
}