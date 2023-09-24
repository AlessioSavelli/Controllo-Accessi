
void NFC_CARD_NEW(uint8_t uid[7], uint8_t uidLength, uint8_t block0[16], uint8_t block1[16], uint8_t block2[16], bool login_result) {

  if (NFC_IS_CLONE_CARD(uid)) {
#ifdef NFC_DEBUG_PRINTCARDINFO
    TERMINAL.println("La carta da inserire ha un clone valido");
#endif
    return;  //se c'e un clone valido non lo inserisce
  }
  NFC_CARD newCard;
  newCard.uidLength = uidLength;

  for (int i = 0; i < 16; i++) {  //con un unico for copia tutti i dati
    if (i < 7) newCard.uid[i] = uid[i];
    newCard.block0[i] = block0[i];
    newCard.block1[i] = block1[i];
    newCard.block2[i] = block2[i];
  }
  TERMINAL.println("");
  newCard.card_readed = false;  //indica che la card va letta
  newCard.login_result = login_result;
  newCard.time = millis();                                //imposta un riferimento temporale di quando rileva la card
  CARDS[card_index] = newCard;                            //aggiunge la nuova card alla lista
  CIRC_BUFFER_ONESCROLL(card_index, CARD_HISTORY_INDEX);  //aumenta l'index del buffer circolare
}

bool NFC_IS_CLONE_CARD(uint8_t uid[7]) {
  for (int scan = 0; scan < CARD_HISTORY_INDEX; scan++) {
    //cerca solo tra le carte non scadute
    if (!IS_DELAYED(CARDS[scan].time, CARD_VALID_TIME)) {
      bool is_clone = true;  // di base da per scontato che la card e' un clone

      for (int i = 0; i < 7; i++) {
        if (CARDS[scan].uid[i] != uid[i]) {
          is_clone = false;  //indica che non e' un clone
          break;             //se l'uid della carta non e' uguale allora si sposta al prossimo for
        }
      }
      //Se arriva qui vuol dire che c'e' almeno un clone valido nella lista
      if (is_clone) {                   //se e' un clone allora resetta il timer di validita'
        START_TIMER(CARDS[scan].time);  //Resetta il timer in quanto ha letto un badge uguale, e gli da altro tempo di validita'
        return true;
      }

    }            //end if
  }              //end for
  return false;  //indica che il bage non e' un clone
}

bool NFC_GET_CARD(NFC_CARD* getCard) {  //Legge solo l'ultima carta inserita in lista

  int _card_index = card_index - 1;                     //Converte l'indice per l'eggere l'ultima carta inserita
  if (card_index < 0) card_index = CARD_HISTORY_INDEX;  //se l'indice e' 0 e quindi facendo meno uno va in negativo, lo porta al valore massimo (N.B trattasi di un array circolare)

  if (!CARDS[_card_index].card_readed) {                         //Se la carta non e' ancora stata letta allora la processa
    CARDS[_card_index].card_readed = true;                       //Indica che la carta e' stat processata
    if (IS_DELAYED(CARDS[_card_index].time, CARD_VALID_TIME)) {  //Controlla se la carta e' scaduta
#ifdef NFC_DEBUG_PRINTCARDINFO
      TERMINAL.println("Scadenza card : scaduta");
#endif
      return false;  //carta non valida
    }

    *getCard = CARDS[_card_index];  //Passa il valore della carta processata
#ifdef NFC_DEBUG_PRINTCARDINFO
    TERMINAL.println("Scadenza card : valida");
#endif
    return true;  //carta valida
  }
  return false;  //non ci sono carte da leggere
}
