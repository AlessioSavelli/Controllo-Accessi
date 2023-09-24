#ifdef ENABLE_CAN
#include <SPI.h>
#include "mcp2515.h"

#define TIMEOUTVALUE 3500  //Non modificare

MCP2515 CAN(NULL_CAN);



//ID messaggi da inviare
#define CAN_ID_STATUS (0x00 + CAN_BUS_MYID + CAN_BUS_MYINDEX)  // Id del messaggio + id di partenza (MYID) + shift di id (MYINDEX)

#define CAN_ID_CARD_UID (0x01 + CAN_BUS_MYID + CAN_BUS_MYINDEX)      // Id del messaggio + id di partenza (MYID) + shift di id (MYINDEX)
#define CAN_ID_CARD_BLOCK11 (0x02 + CAN_BUS_MYID + CAN_BUS_MYINDEX)  // Id del messaggio + id di partenza (MYID) + shift di id (MYINDEX)
#define CAN_ID_CARD_BLOCK12 (0x03 + CAN_BUS_MYID + CAN_BUS_MYINDEX)  // Id del messaggio + id di partenza (MYID) + shift di id (MYINDEX)
#define CAN_ID_CARD_BLOCK21 (0x04 + CAN_BUS_MYID + CAN_BUS_MYINDEX)  // Id del messaggio + id di partenza (MYID) + shift di id (MYINDEX)
#define CAN_ID_CARD_BLOCK22 (0x05 + CAN_BUS_MYID + CAN_BUS_MYINDEX)  // Id del messaggio + id di partenza (MYID) + shift di id (MYINDEX)
#define CAN_ID_CARD_BLOCK31 (0x06 + CAN_BUS_MYID + CAN_BUS_MYINDEX)  // Id del messaggio + id di partenza (MYID) + shift di id (MYINDEX)
#define CAN_ID_CARD_BLOCK32 (0x07 + CAN_BUS_MYID + CAN_BUS_MYINDEX)  // Id del messaggio + id di partenza (MYID) + shift di id (MYINDEX)
//ID messaggi da ricevere
//#define CAN_ID_TRG  (0xA0 + CAN_BUS_MYID + CAN_BUS_MYINDEX)  // Id del messaggio + id di partenza (MYID) + shift di id (MYINDEX)




enum st_can {
  CAN_STARTUP_SPI,
  CAN_WAIT_READING,
  CAN_SEND_MESSAGE
};
uint8_t st_can_index = CAN_STARTUP_SPI;

void loop_can(OUTPUT_INFO _my_output[3], INPUT_INFO _my_input[2]);
void can_send_badge(NFC_CARD *card);
#endif