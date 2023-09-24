
#include <Wire.h>
#include "Adafruit_PN532.h"


//Macro per accedere al settore 0
#define SECTOR0LINE0(x) nfc_sector0data[x]
#define SECTOR0LINE1(x) nfc_sector0data[16 + x]
#define SECTOR0LINE2(x) nfc_sector0data[32 + x]




#define NR_SHORTSECTOR (32)          // Number of short sectors on Mifare 1K/4K
#define NR_LONGSECTOR (8)            // Number of long sectors on Mifare 4K
#define NR_BLOCK_OF_SHORTSECTOR (4)  // Number of blocks in a short sector
#define NR_BLOCK_OF_LONGSECTOR (16)  // Number of blocks in a long sector

// Determine the sector trailer block based on sector number
#define BLOCK_NUMBER_OF_SECTOR_TRAILER(sector) (((sector) < NR_SHORTSECTOR) ? ((sector)*NR_BLOCK_OF_SHORTSECTOR + NR_BLOCK_OF_SHORTSECTOR - 1) : (NR_SHORTSECTOR * NR_BLOCK_OF_SHORTSECTOR + (sector - NR_SHORTSECTOR) * NR_BLOCK_OF_LONGSECTOR + NR_BLOCK_OF_LONGSECTOR - 1))

// Determine the sector's first block based on the sector number
#define BLOCK_NUMBER_OF_SECTOR_1ST_BLOCK(sector) (((sector) < NR_SHORTSECTOR) ? ((sector)*NR_BLOCK_OF_SHORTSECTOR) : (NR_SHORTSECTOR * NR_BLOCK_OF_SHORTSECTOR + (sector - NR_SHORTSECTOR) * NR_BLOCK_OF_LONGSECTOR))


Adafruit_PN532 nfc(IRQ_NFC, RESET_NFC);  //Creo l'oggetto per usare l'NFC

#define NFC_IS_READY_I2C (!digitalRead(IRQ_NFC))  //Se, usante l'i2c il pin IRQ e' LOW vuol dire che il modulo nfc e' pronto a comunicare
#define NFC_TIMEOUT_READING 1 * SECONDS           //Tenta la lettura dell'NFC e va in timeout dopo 3S
#define NFC_MAX_SECTOR_READ 16                    //Legge massimo i primi 16 settori

#define NFC_LOGIN_FAIL false    //Definisce lo stato di login al settore errato
#define NFC_LOGIN_SUCCESS true  //Definisce lo stato di login al settore corretto

uint32_t NFC_VERSION = 0;

/*
   Definisco i timer che usa il modulo dell NFC
*/
unsigned long timer0_NFC = 0;

/*
   Macchina a stati per la gestione dell'NFC
*/

enum st_nfc {
  NFC_JUST_WAITING,   //Attende un certo tempo tra una lettura e l'altra
  NFC_READING_CARD,   //Attende la carta NFC
  NFC_SEARCH_SECTOR,  //Ricerca un settore accessibile
  //Legge i 3 blocchi importati del settore
  NFC_READ_BLOCK0,  //Legge il custom trailer
  NFC_READ_BLOCK1,  //Custom line 2 usata come passkey
  NFC_READ_BLOCK2,  //Custom line 1 usata come nome da visualizzare

  NFC_VALIDATE_BADGE,  //Valida i CRC e la passkey del badge e assicura l'integrita' dei dati letti
  NFC_LOGIN_BADGE,     //Fa il login del badge e controlla se la passkey e' valida

  NFC_ADD_VALID_CARD,    //Carta valida trovata
  NFC_ADD_INVALID_CARD,  //Indica che la card scansionata non e' valida

  NFC_END_CICLE  //Indica che ha finito il ciclo e fa i reset opportuni
};

uint8_t st_nfc_index = NFC_JUST_WAITING;


/*
   Macchina a stati per la gestione dell'attesa lettura Tag
*/
enum st_nfc_wait {
  NFC_WAIT_SEND_COMMAND,  //Invia il payload per la ricerca
  NFC_WAIT_READY,         //Attende che il modulo sia pronto
  NFC_WAIT_READ_ACK,      //Legge la risposta del messaggio sul bus i2c
  NFC_WAIT_RETURN_TRUE,   //Dice che ha rilevato il tag
  NFC_DELAY_NFC           //Aspetta del tempo prima di resettare la macchina a stati
};

uint8_t st_nfc_wait_index = NFC_WAIT_SEND_COMMAND;


//Buffer per il payload di ricerca tag
#define _PN532_PACKBUFFSIZ 3
uint8_t _pn532_packetbuffer[_PN532_PACKBUFFSIZ];


/*
   Definisco i prototipi delle funzioni
*/

bool nfc_setup();
void nfc_print_version();
void nfc_loop();

bool nfc_async_wait_passive_tag(uint8_t cardbaudrate, uint8_t* uid, uint8_t* uidLength);  //L'aggiunta di realse serve per capire quando il tag viene rimosso dal lettore

uint8_t CRC8(const uint8_t* data, uint8_t len);