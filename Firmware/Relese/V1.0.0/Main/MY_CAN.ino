#ifdef ENABLE_CAN
void loop_can(OUTPUT_INFO _my_output[3], INPUT_INFO _io_input_setup[2]) {
  static unsigned long timer_can0 = 0;
  struct can_frame sendMessage;
  switch (st_can_index) {
    case CAN_STARTUP_SPI:
      //Inizializza il can
      if (CAN.begin(SPI_CANBUS_CS, CAN_SPI_SPEED)) {
        CAN.reset();
        CAN.setBitrate(CAN_SPEED, CAN_CRYSTAL);
        st_can_index = CAN_WAIT_READING;
      } else {
        delay(100);
      }
      break;
    case CAN_WAIT_READING:
      /* if (checkReceive) {  //Da implementare in futuro
        read_can();
      }*/
      st_can_index = CAN_SEND_MESSAGE;
      break;

    case CAN_SEND_MESSAGE:
      if (IS_DELAYED(timer_can0, 250)) {

        sendMessage.data[0] = (byte)_my_output[2].status;  //Output Rele
        sendMessage.data[1] = (byte)_my_output[0].status;  //Output LED1
        sendMessage.data[2] = (byte)_my_output[1].status;  //Output LED2

        sendMessage.data[3] = (byte)_io_input_setup[1].status;  //Input Tamper
        sendMessage.data[4] = (byte)_io_input_setup[0].status;  //Intput Programmabile

        sendMessage.data[5] = 0x00;
        sendMessage.data[6] = 0x00;
        sendMessage.data[7] = 0x00;
        sendMessage.can_id = CAN_ID_STATUS;
        sendMessage.can_dlc = 8;
        CAN.sendMessage(&sendMessage);
      }
      st_can_index = CAN_WAIT_READING;
      break;
  }
}
void can_send_badge(NFC_CARD *card) {
  struct can_frame frameBadge1;
  struct can_frame frameBadge2;
  struct can_frame frameBadge3;
  struct can_frame frameBadge4;
  struct can_frame frameBadge5;
  struct can_frame frameBadge6;
  if (st_can_index == CAN_STARTUP_SPI) return;
  frameBadge1.data[0] = card->uidLength;
  for (int i = 0; i <= 6; i++) {
    frameBadge1.data[1 + i] = card->uid[i];
  }
  frameBadge1.can_dlc = 8;
  frameBadge1.can_id = CAN_ID_CARD_UID;
  CAN.sendMessage(&frameBadge1);  //Mando la lunghezza UID e l'uid
  frameBadge1.can_id = CAN_ID_CARD_BLOCK11;

  frameBadge2.can_dlc = 8;
  frameBadge2.can_id = CAN_ID_CARD_BLOCK12;

  frameBadge3.can_dlc = 8;
  frameBadge3.can_id = CAN_ID_CARD_BLOCK21;

  frameBadge4.can_dlc = 8;
  frameBadge4.can_id = CAN_ID_CARD_BLOCK22;

  frameBadge5.can_dlc = 8;
  frameBadge5.can_id = CAN_ID_CARD_BLOCK31;

  frameBadge6.can_dlc = 8;
  frameBadge6.can_id = CAN_ID_CARD_BLOCK32;

  for (int i = 0; i < 8; i++) {
    frameBadge1.data[i] = card->block0[i];
    frameBadge2.data[i] = card->block0[8 + i];

    frameBadge3.data[i] = card->block1[i];
    frameBadge4.data[i] = card->block1[8 + i];

    frameBadge5.data[i] = card->block2[i];
    frameBadge6.data[i] = card->block2[8 + i];
  }
  CAN.sendMessage(&frameBadge1);  //Mando il primo pezzo del block0
  CAN.sendMessage(&frameBadge2);  //Mando il secondo pezzo del block0


  CAN.sendMessage(&frameBadge3);  //Mando il primo pezzo del block1
  CAN.sendMessage(&frameBadge4);  //Mando il secondo pezzo del block1

  CAN.sendMessage(&frameBadge5);  //Mando il primo pezzo del block2
  CAN.sendMessage(&frameBadge6);  //Mando il secondo pezzo del block2
}

void read_can() {
  /*
  // CAN RX Variables
  long unsigned int rxId;
  unsigned char len;
  unsigned char rxBuf[8];
  CAN.readMsgBuf(&rxId, &len, rxBuf);
  switch (rxId) {
    case CAN_ID_TRG:  //Legge i byte di trigger
      //B0          B1           B2           B3            B4      B5     B6      B7
      //Trg Rele    --            --           --            --      --     --      --
      //(rxBuf[0]) ? (EVENT_HANDLER.RELE_CAN_TRG = true) : (EVENT_HANDLER.RELE_CAN_TRG = false);
      break;
  }*/
}
#endif