
#ifdef ENABLE_MQTT
void MQTT_loop() {
  static unsigned long mqtt_timer0 = 0;
  switch (st_mqtt_index) {
    case MQTT_STARTUP:
#ifdef INTERNET_MQTT_BASE
      TERMINAL.println("Setup dati server MQTT");
#endif
      mqtt.setServer(MQTT_BROKER, MQTT_BROKER_PORT);
      mqtt.setCallback(mqtt_callback);
      //mqtt.setSocketTimeout(2);//2s di time out
      st_mqtt_index = MQTT_CONNECT;
      break;
    case MQTT_CONNECT:
      START_TIMER(mqtt_timer0);
      st_mqtt_index = MQTT_DISCONNECT;

      if (mqtt.connect(MQTT_DEVICE_ID, MQTT_USERNAME, MQTT_PASSWORD, MQTT_START_TOPIC , 0, true, "Off-Line")) st_mqtt_index = MQTT_SUBSCRIBE;
      break;
    case MQTT_DISCONNECT:
      if (IS_DELAYED(mqtt_timer0, 5 * SECONDS)) {
#ifdef INTERNET_MQTT_BASE
        TERMINAL.println("Tentativo di riconnessione al servizio MQTT");
#endif
        st_mqtt_index = MQTT_CONNECT;
      }
      break;
    case MQTT_SUBSCRIBE:
#ifdef INTERNET_MQTT_BASE
      TERMINAL.println("Connesso al server MQTT");
#endif

      mqtt.publish(MQTT_START_TOPIC, "On-Line", true);

      //mqtt.subscribe(MQTT_CARD_ID_TOPIC);
      // mqtt.subscribe(MQTT_CARD_LOGIN_RESULT);

      //mqtt.subscribe(MQTT_CARD_BLOCK0_TOPIC);
      //mqtt.subscribe(MQTT_CARD_BLOCK1_TOPIC);
      //mqtt.subscribe(MQTT_CARD_BLOCK2_TOPIC);
      //mqtt.subscribe(MQTT_CARD_READED_TIME);

      //mqtt.subscribe(MQTT_INPUT_STATUS);
      //mqtt.subscribe(MQTT_TAMPER_STATUS);

      mqtt.subscribe(MQTT_OUTPUT_RELE_TRIGGER);
      //mqtt.subscribe(MQTT_OUTPUT_RELE_STATUS);

      //predisposte per la prossima versione
      //mqtt.subscribe(MQTT_OUTPUT_LED1_TRG);
      //mqtt.subscribe(MQTT_OUTPUT_LED2_TRG);


      //mqtt.subscribe(MQTT_OUTPUT_LED1_STATUS);
      //mqtt.subscribe(MQTT_OUTPUT_LED2_STATUS);

      //mqtt.subscribe(MQTT_SYS_INFORMATION_TIME);
      //mqtt.subscribe(MQTT_SYS_INFORMATION_FWVERS);
      //mqtt.subscribe(MQTT_SYS_INFORMATION_DISABELNFC);

      st_mqtt_index = MQTT_PUBLISH_ONESHOT;


      break;
    case MQTT_PUBLISH_ONESHOT:
      mqtt.publish(MQTT_SYS_INFORMATION_FWVERS, VERSIONE_FW, true);  // La versione FW sara' sempre disponibile anche dopo la disconnessione
      //Popoliamo l'mqtt
      st_mqtt_index = MQTT_CHECK_CONNECTION;
      START_TIMER(mqtt_timer0);
      break;
    case MQTT_CHECK_CONNECTION:
      if (IS_DELAYED(mqtt_timer0, 2 * SECONDS)) {
        START_TIMER(mqtt_timer0);
        if (!mqtt.connected()) {
#ifdef INTERNET_MQTT_BASE
          TERMINAL.println("Disconnesso dal server MQTT");
#endif
          st_mqtt_index = MQTT_CONNECT;                                     //Tenta di riconnettersi al server
        } else {                                                            //Se l'mqtt e' connesso aggiorno il sys time reference
          mqtt.publish(MQTT_SYS_INFORMATION_TIME, TIMEFORMAT_MILLIS_CHAR);  //da un riferimento dell'uptime
                                                                            // mqtt.publish(MQTT_SYS_INFORMATION_DISABELNFC, (String(!EVENT_HANDLER.DISABLE_NFC)).c_str(), true);
        }
      }
      mqtt.loop();
      break;
  }
}

void mqtt_send_badge(NFC_CARD* card) {
  if (MQTT_CHECK_CONNECTION != st_mqtt_index) return;
  String builder;
  String builder1;
  String builder2;
  //creiamo l'uid
  for (int i = 0; i < card->uidLength; i++) {
    builder += "0x" + String(card->uid[i], HEX) + " ";
  }
  mqtt.publish(MQTT_CARD_ID_TOPIC, builder.c_str());                         //invio l'uid sull'mqtt
  mqtt.publish(MQTT_CARD_READED_TIME, TIMEFORMAT_MYVALUE_CHAR(card->time));  //il momento in cui ha letto la card
  mqtt.publish(MQTT_CARD_LOGIN_RESULT, (String(card->login_result)).c_str());
  //Invio i 3 blocchi letti
  builder = "";
  builder1 = "";
  builder2 = "";
  for (int i = 0; i < 16; i++) {
    builder += "0x" + String(card->block0[i], HEX) + " ";
    builder1 += "0x" + String(card->block1[i], HEX) + " ";
    builder2 += "0x" + String(card->block2[i], HEX) + " ";
  }
  mqtt.publish(MQTT_CARD_BLOCK0_TOPIC, builder.c_str());   //il momento in cui ha letto la card
  mqtt.publish(MQTT_CARD_BLOCK1_TOPIC, builder1.c_str());  //il momento in cui ha letto la card
  mqtt.publish(MQTT_CARD_BLOCK2_TOPIC, builder2.c_str());  //il momento in cui ha letto la card
}

void mqtt_output_status(OUTPUT_INFO my_output[3]) {
  if (MQTT_CHECK_CONNECTION != st_mqtt_index) return;
  mqtt.publish(MQTT_OUTPUT_RELE_STATUS, (String(my_output[2].status).c_str()));
  mqtt.publish(MQTT_OUTPUT_LED1_STATUS, (String(my_output[1].status).c_str()));
  mqtt.publish(MQTT_OUTPUT_LED2_STATUS, (String(my_output[0].status).c_str()));
}
void mqtt_input_status(INPUT_INFO my_input[2]) {
  if (MQTT_CHECK_CONNECTION != st_mqtt_index) return;
  mqtt.publish(MQTT_INPUT_STATUS, (String(my_input[0].status).c_str()));
  mqtt.publish(MQTT_TAMPER_STATUS, (String(my_input[1].status).c_str()));
}

void mqtt_callback(char* topic, byte* payload, unsigned int length) {
  TERMINAL.println(topic);
  if (strcmp(topic, MQTT_OUTPUT_RELE_TRIGGER) == 0) {
    if (length == 1) {
      (payload[0] == '1') ? (EVENT_HANDLER.RELE_MQTT_TRG = true) : (EVENT_HANDLER.RELE_MQTT_TRG = false);
    }
  } /*else if (strcmp(topic, MQTT_SYS_INFORMATION_DISABELNFC) == 0) {
    if (length == 1) {
      //(payload[0] == '1') ? (EVENT_HANDLER.DISABLE_NFC = true) : (EVENT_HANDLER.DISABLE_NFC = false);
      //TERMINAL.println("NFC INTERACTION");
    }
  }*/
}


#endif