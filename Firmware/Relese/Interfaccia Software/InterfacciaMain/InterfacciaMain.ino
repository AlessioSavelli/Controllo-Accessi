/*
Licenza MIT (Massachusetts Institute of Technology)

Copyright (c) 2023  Alessio Savelli

È concesso in licenza gratuitamente a chiunque ottenga una copia di questo software e dei file di documentazione associati (il "Software"), di trattare il Software senza restrizioni, compresi, ma non limitati ai diritti di utilizzo, copia, modifica, fusione, pubblicazione, distribuzione, sublicenza e/o vendita delle copie del Software e di consentire alle persone a cui il Software è fornito di farlo, a condizione che le seguenti condizioni siano soddisfatte:

Il testo completo della Licenza MIT (in inglese):
MIT License

Copyright (c) 2023 Alessio Savelli

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.


*/

#include <SoftwareSerial.h>
#include "PN532_SWHSU.h"
#include "PN532.h"

#define STD_KEYA 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF  //Sono le chiavi per accedere al settore da modificare
#define STD_KEYB 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF  //Sono le chiavi per accedere al settore da modificare

#define ACCESS_BITS 0xFF, 0x07, 0x80

//Qualora dopo aver popolato il settore si volessero cambiare le chiavi vanno specificate qui
#define NEW_STD_KEYA STD_KEYA
#define NEW_STD_KEYB STD_KEYB



#define CALCULATE_BLOCK_FROM_SECTOR(x) (x * 4)
#define BLOCK_INDEX_FROM_SECTOR(sector, block) (CALCULATE_BLOCK_FROM_SECTOR(sector) + block)

unsigned long timer0 = 0;
unsigned long timer1 = 0;
unsigned long timer2 = 0;
const byte rxPin = 2;
const byte txPin = 3;


uint8_t uid[] = { 0, 0, 0, 0, 0, 0, 0 };  // Buffer to store the returned UID
uint8_t uidLength;                        // Length of the UID (4 or 7 bytes depending on ISO14443A card type)

SoftwareSerial SWSerial(txPin, rxPin);

PN532_SWHSU pn532swhsu(SWSerial);
PN532 nfc(pn532swhsu);

enum SERIAL_HEADER {
  IDLE_WAITING_HEADER = 0x00,
  ERROR_MODULE = 0x01,
  VERS_MODULE = 0x02,
  WAIT_BADGE = 0x03,
  PRESENT_BADGE = 0x04,
  GET_UID_BADGE = 0x05,
  WRITE_BADGE = 0x06,
  SUCCESS_WRITE_BADGE = 0x07,

  //WritingBadgeProcess
  WRITE_BADGE_GET_LENGHT = 0xA0,
  WRITE_BADGE_GET_LENGHT_ERROR = 0xA1,
  WRITE_BADGE_GET_BRENDSECTOR = 0xA2,
  WRITE_BADGE_GET_LINE1 = 0xA3,
  WRITE_BADGE_GET_LINE2 = 0xA4,
  WRITE_BADGE_PUSHDATA = 0xA5,
  //----



  ERROR_WRITE_BADGE_UID = 0xF1,
  ERROR_WRITE_BADGE_AUTH1 = 0xF2,
  ERROR_WRITE_BADGE_AUTH2 = 0xF3,
  ERROR_WRITE_BADGE_BRENDSECTOR = 0xF4,
  ERROR_WRITE_BADGE_LINE1 = 0xF5,
  ERROR_WRITE_BADGE_LINE2 = 0xF6,
  ERROR_WRITE_BADGE_TRAILER = 0xF7,

  ERROR_FRAME_LENGHT = 0xFD,
  ERROR_FRAME = 0xFE,
  END_COMUNICATE = 0xFF
};
int serial_header_index = IDLE_WAITING_HEADER;


enum STATE {
  IDLE,
  WAITING_FOR_HEADER,
  PROCESSING_COMMAND
};

STATE state = IDLE;
byte header;


const int BUFFER_SIZE = 64 * 2;  // Dimensione del buffer per la lettura dei dati
byte buffer[BUFFER_SIZE];
int bufferIndex_w = 0;
int bufferIndex_r = 0;
//----Definsico le macro per i buffer circolare---
#define CIRC_BUFFER_MULTISCROLL(index, scroll, maxItem) (index = (index + scroll) % maxItem)
#define CIRC_BUFFER_ONESCROLL(index, maxItem) (CIRC_BUFFER_MULTISCROLL(index, 1, maxItem))
#define REMOVE_FROM_INDEX(index, len) (index -= len)
#define ADD_TO_INDEX(index, len) (index += len)
#define NEED_TO_READ_SERIAL(read_index, write_index) (read_index != write_index)
//------


uint32_t versiondata = 0x00;

void read_serial();
void processing_payload();

void waiting_badge();
byte writing_badge(uint8_t brendSector[16], uint8_t b0_ln1[16], uint8_t b0_ln2[16], int SECTOR_TO_WRITE);
byte CRC8(const byte *data, byte len);

void setup() {
  pinMode(13, OUTPUT);
  // Inizializza le porte seriali
  Serial.begin(115200);  // Inizializza la porta seriale predefinita
  nfc.begin();
  versiondata = nfc.getFirmwareVersion();
  if (!versiondata) {
    while (true) {
      Serial.write(ERROR_MODULE);
      delay(250);
    }
  }
  // Configure board to read RFID tags
  nfc.SAMConfig();

  timer1 = millis();
  timer2 = millis();
}

void loop() {
  if (serial_header_index == IDLE_WAITING_HEADER) {
    if (millis() - timer2 < 30 * 1000) {  //dall'ultimo evento lampeggia per ulteriori 10s dopo di che non lo fa piu'
      //se arduino e' in modalita idle fa un blink per indicare che il sistema e' accesso
      if (millis() - timer1 >= 2000) {
        digitalWrite(13, HIGH);
        timer1 = millis();
      } else if (millis() - timer1 >= 100 && millis() - timer1 <= 130) {
        digitalWrite(13, LOW);
      }

    } else {
      digitalWrite(13, LOW);
      timer1 = millis();
    }
  } else if (millis() - timer0 >= 2000) {
    digitalWrite(13, LOW);
  }
  read_serial();  //Legge la seriale e la salva in un buffer circolare. legge in byte per volta dalla seriale
  //dopo aver letto un byte si sposta nel loop che lo processera'
  if (NEED_TO_READ_SERIAL(bufferIndex_w, bufferIndex_r)) {  //se ci sono dati da leggere nel buffer esegue l'operazione
    processing_payload();                                   //legge i payload ricevuti e li processa
  }
}
void read_serial() {
  if (Serial.available()) {
    buffer[bufferIndex_w] = Serial.read();
    CIRC_BUFFER_ONESCROLL(bufferIndex_w, BUFFER_SIZE);  //sponta l'indice di scrittura
  }
}
void processing_payload() {

  static byte sector_id;
  static byte lengt_payload = 0;
  static byte index_payload = 0;

  //  Gli ultimi 3 byte sono il CRC         !CRCLN1!CRCLN2!CRC!
  static uint8_t brendSector[16]{ 0 };
  static uint8_t b0_ln1[16] = { 0 };
  static uint8_t b0_ln2[16] = { 0 };

  byte value = 0x00;
  if (NEED_TO_READ_SERIAL(bufferIndex_w, bufferIndex_r)) {
    value = buffer[bufferIndex_r];
    CIRC_BUFFER_ONESCROLL(bufferIndex_r, BUFFER_SIZE);
    //ogni volta che legge qualcosa resetta il timer di idle
    timer2 = millis();
  }
  switch (serial_header_index) {
    case IDLE_WAITING_HEADER:
      //Se la macchina a stati e' in idle e il valore contiene un messaggio diverso da Error Write Badge , allora lo imposta come header
      if (value != END_COMUNICATE) {
        serial_header_index = value;
      }

      break;
    //Adesso processa gli header letti
    case VERS_MODULE:
      Serial.write(VERS_MODULE);
      Serial.write(versiondata);
      Serial.write(END_COMUNICATE);
      serial_header_index = IDLE_WAITING_HEADER;
      break;
    case WAIT_BADGE:
      waiting_badge();
      Serial.write(PRESENT_BADGE);
      Serial.write(END_COMUNICATE);
      serial_header_index = IDLE_WAITING_HEADER;
      break;
    case GET_UID_BADGE:
      Serial.write(GET_UID_BADGE);
      Serial.write(uidLength);
      for (int i = 0; i < uidLength; i++) {
        Serial.write(uid[i]);
      }
      Serial.write(END_COMUNICATE);
      serial_header_index = IDLE_WAITING_HEADER;
      break;
    case WRITE_BADGE:
      sector_id = value;  //ottiene il settore dove scrivere i dati
      serial_header_index = WRITE_BADGE_GET_LENGHT;
      break;
    case WRITE_BADGE_GET_LENGHT:
      lengt_payload = value;
      //Verifica la lunghezze del pyload che sia pari a 16 byte per blocco, leggendo 3 blocchi il valore deve essere 16*3 48
      serial_header_index = WRITE_BADGE_GET_BRENDSECTOR;
      index_payload = 0;
      if (lengt_payload != 48) {  // se non corrisponde a 48 allora va in errore
        serial_header_index = WRITE_BADGE_GET_LENGHT_ERROR;
      }
      break;
    case WRITE_BADGE_GET_LENGHT_ERROR:
      //Gestisce l'errore leggendo a vuoto il payload
      index_payload++;
      if (index_payload >= (lengt_payload + 1)) {  //a letto tutto il payload errato + l'end frame allora ritorna il messaggio di errore
        Serial.write(ERROR_FRAME_LENGHT);
        Serial.write(END_COMUNICATE);
        serial_header_index = IDLE_WAITING_HEADER;
      }
      break;
    case WRITE_BADGE_GET_BRENDSECTOR:
      //legge il payload
      brendSector[index_payload] = value;
      index_payload++;
      if (index_payload >= 16) {  // se ha letto i suoi 16 byte passa a leggere il prossimo settore
        index_payload = 0;
        serial_header_index = WRITE_BADGE_GET_LINE1;
      }
      break;
    case WRITE_BADGE_GET_LINE1:
      //legge il payload
      b0_ln1[index_payload] = value;
      index_payload++;
      if (index_payload >= 16) {  // se ha letto i suoi 16 byte passa a leggere il prossimo settore
        index_payload = 0;
        serial_header_index = WRITE_BADGE_GET_LINE2;
      }
      break;
    case WRITE_BADGE_GET_LINE2:
      //legge il payload
      b0_ln2[index_payload] = value;
      index_payload++;
      if (index_payload >= 16) {  // se ha letto i suoi 16 byte passa a leggere il prossimo settore
        index_payload = 0;
        serial_header_index = WRITE_BADGE_PUSHDATA;
      }
      break;
    case WRITE_BADGE_PUSHDATA:  //Attende l'end comunicate per scrivere i dati nel badge
      if (value == END_COMUNICATE) {
        Serial.write(writing_badge(brendSector, b0_ln1, b0_ln2, sector_id));
        Serial.write(END_COMUNICATE);
        serial_header_index = IDLE_WAITING_HEADER;
      }
      break;
    default:  //nel caso in cui non conosce l'header o non sa cosa fare, fa l'echo dei dati che riceve
      Serial.write(ERROR_FRAME);
      Serial.write(END_COMUNICATE);
      serial_header_index = IDLE_WAITING_HEADER;
      break;
  }
}
byte writing_badge(uint8_t brendSector[16], uint8_t b0_ln1[16], uint8_t b0_ln2[16], int SECTOR_TO_WRITE) {

  digitalWrite(13, HIGH);
  uint8_t success;
  uint8_t uid[] = { 0, 0, 0, 0, 0, 0, 0 };  // Buffer to store the returned UID
  uint8_t uidLength;                        // Length of the UID (4 or 7 bytes depending on ISO14443A card type)

  success = nfc.readPassiveTargetID(PN532_MIFARE_ISO14443A, uid, &uidLength);

  if (success) {
    if (uidLength != 4) {
      digitalWrite(13, LOW);
      return ERROR_WRITE_BADGE_UID;
    }
    byte keyA[6] = { STD_KEYA };
    uint8_t sectorIndex = SECTOR_TO_WRITE * 4;  // Calculate the sector index once
    // Authenticate to the block
    success = nfc.mifareclassic_AuthenticateBlock(uid, uidLength, CALCULATE_BLOCK_FROM_SECTOR(SECTOR_TO_WRITE), 0, keyA);
    if (!success) {
      digitalWrite(13, LOW);
      return ERROR_WRITE_BADGE_AUTH2;
    }

    // Calculate and set CRC values
    brendSector[13] = CRC8(b0_ln1, 16);
    brendSector[14] = CRC8(b0_ln2, 16);
    brendSector[15] = CRC8(brendSector, 15);  // Calculate the final CRC


    success = nfc.mifareclassic_WriteDataBlock(BLOCK_INDEX_FROM_SECTOR(SECTOR_TO_WRITE, 0), brendSector);
    if (!success) {
      return ERROR_WRITE_BADGE_BRENDSECTOR;
    }
    success = nfc.mifareclassic_WriteDataBlock(BLOCK_INDEX_FROM_SECTOR(SECTOR_TO_WRITE, 1), b0_ln1);
    if (!success) {
      return ERROR_WRITE_BADGE_LINE1;
    }
    success = nfc.mifareclassic_WriteDataBlock(BLOCK_INDEX_FROM_SECTOR(SECTOR_TO_WRITE, 2), b0_ln2);
    if (!success) {
      return ERROR_WRITE_BADGE_LINE2;
    }
    uint8_t magicLine[16]{ NEW_STD_KEYA, ACCESS_BITS, 0x69, NEW_STD_KEYB };
    success = nfc.mifareclassic_WriteDataBlock(BLOCK_INDEX_FROM_SECTOR(SECTOR_TO_WRITE, 3), magicLine);  //Settore delle chiavi
    if (!success) {
      return ERROR_WRITE_BADGE_TRAILER;
    }
    timer0 = millis();
    return SUCCESS_WRITE_BADGE;
  }
  return ERROR_WRITE_BADGE_AUTH1;
}

void waiting_badge() {
  digitalWrite(13, HIGH);
  while (!nfc.readPassiveTargetID(PN532_MIFARE_ISO14443A, &uid[0], &uidLength)) {
    delay(1000);
    // Puoi aggiungere un ritardo breve qui se desideri evitare di sovraccaricare il modulo NFC
  }
  digitalWrite(13, LOW);
}
byte CRC8(const byte *data, byte len) {
  byte crc = 0x00;
  while (len--) {
    byte extract = *data++;
    for (byte tempI = 8; tempI; tempI--) {
      byte sum = (crc ^ extract) & 0x01;
      crc >>= 1;
      if (sum) {
        crc ^= 0x8C;
      }
      extract >>= 1;
    }
  }
  return crc;
}