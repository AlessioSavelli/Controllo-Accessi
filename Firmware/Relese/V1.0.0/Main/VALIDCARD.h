/*
Qui vengono salvati i dati dell'ultima carta valida
*/
#define CARD_VALID_TIME 150    //Dopo che la carta viene letta e' valida , n.b se non viene elaborata allora scade
#define CARD_HISTORY_INDEX 10  //Salva in memoria le ultime N card lette

struct NFC_CARD {
  uint8_t uid[7] = { 0, 0, 0, 0, 0, 0, 0 };  // Buffer per salvare per UID
  uint8_t uidLength;                         // Lunghezza UID (4 or 7 bytes dipende odal tipo di carta)
  uint8_t block0[16];                        // Settore con il customTrailer
  uint8_t block1[16];                        // Settore con il nome dell'utilizzatore
  uint8_t block2[16];                        // Settore con la passkey
  bool login_result = false;                 // Dice se il login e' andato a buon fine oppure no
  bool card_readed = true;                   // Indica se la carta viene letta
  unsigned long time;                        // Salva in memoria quando viene letta la card
};

NFC_CARD CARDS[CARD_HISTORY_INDEX];  //Array delle carte lette
unsigned int card_index = 0;         //Serve per scorrere dove salvare le card


void NFC_CARD_NEW(uint8_t uid[7], uint8_t uidLength, uint8_t block0[16], uint8_t block1[16], uint8_t block2[16], bool login_result);
bool NFC_IS_CLONE_CARD(uint8_t uid[7]);
bool NFC_GET_CARD(NFC_CARD* getCard);