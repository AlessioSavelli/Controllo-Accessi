
void loop_internet() {
  static unsigned int timer0 = 0;
  static unsigned int timer1 = 0;
  static int counter0 = 0;


  switch (st_internet_index) {
    case INTERNET_SETUP:

      WiFiDrv::pinMode(25, OUTPUT);  //define green pin
      WiFiDrv::pinMode(26, OUTPUT);  //define red pin
      WiFiDrv::pinMode(27, OUTPUT);  //define blue pin

      WiFiDrv::analogWrite(25, 0);  //GREEN
      WiFiDrv::analogWrite(26, 0);  //RED
      WiFiDrv::analogWrite(27, 0);  //BLUE




#if INTERNET_CHIP == WiFi_NINA_Chip
      st_internet_index = INTERNET_WiFi_SETUP;
#elif INTERNET_CHIP == Eth_W5500_Chip
      st_internet_index = INTERNET_ETHERNET_SETUP;
#endif
      break;

#if INTERNET_CHIP == WiFi_NINA_Chip
    case INTERNET_WiFi_SETUP:
      st_internet_index = INTERNET_WIFI_WAITING;
      if (WiFi.begin(WiFi_SSID, WiFi_PASS) == WL_CONNECTED) st_internet_index = INTERNET_CONNECTED;
      counter0 = 0;
      START_TIMER(timer0);
      break;
    case INTERNET_WIFI_WAITING:  //Attende 10 Secondi
      if (IS_DELAYED(timer0, 350)) {
        WiFiDrv::analogWrite(25, 255);  //GREEN
        counter0++;
      } else if (counter0 > ((10 * SECONDS) / 350)) {
        st_internet_index = INTERNET_WiFi_SETUP;
      }


      break;
#elif INTERNET_CHIP == Eth_W5500_Chip
    case INTERNET_ETHERNET_SETUP:
      Ethernet.begin(mac);

      break;

#endif
    case INTERNET_CONNECTED:
#ifdef INTERNET_DEBUG_BASE
#if INTERNET_CHIP == WiFi_NINA_Chip

      // print your board's IP address:
      TERMINAL.print("IP Address: ");
      TERMINAL.println(WiFi.localIP());

      TERMINAL.println();
      TERMINAL.println("Network Information:");
      TERMINAL.print("SSID: ");
      TERMINAL.println(WiFi.SSID());

      // print the received signal strength:
      TERMINAL.print("signal strength (RSSI):");
      TERMINAL.println(WiFi.RSSI());

      TERMINAL.print("Encryption Type:");
      TERMINAL.println(WiFi.encryptionType(), HEX);
      TERMINAL.println();


#elif INTERNET_CHIP == Eth_W5500_Chip

#endif
#endif  //Fine codice di debug
#ifdef ENABLE_TELEGRAM
      //Colore azzuro per indicare che sta configurando telegram
      WiFiDrv::analogWrite(25, 127);  //GREEN
      WiFiDrv::analogWrite(26, 0);    //RED
      WiFiDrv::analogWrite(27, 255);  //BLUE

      telegram_init();  //Inizializzo il bot telegram
#endif

      START_TIMER(timer0);
      INSTANT_TRIGGER_TIMER(timer0);
      START_TIMER(timer1);
      INSTANT_TRIGGER_TIMER(timer1);
      st_internet_index = INTERNET_LEVEL;
      break;
    case INTERNET_LEVEL:
#if INTERNET_CHIP == WiFi_NINA_Chip
      //deve gestire la disconnessione


      //----
      if (IS_DELAYED(timer0, 1.5 * SECONDS)) {
        WiFi_checkRSSI();
        START_TIMER(timer0);
      }

#elif INTERNET_CHIP == Eth_W5500_Chip

#endif

      st_internet_index = INTERNET_LOOP_MQTT;
      break;

    case INTERNET_LOOP_MQTT:
#ifdef ENABLE_MQTT
      //Inizializza la connessione al server mqtt
      if (IS_DELAYED(timer0, MQTT_LOOP_TIME)) {  
        MQTT_loop();
        START_TIMER(timer0);
      }
#endif
      st_internet_index = INTERNET_LOOP_GETIME;
      break;


    case INTERNET_LOOP_GETIME:

#if INTERNET_CHIP == WiFi_NINA_Chip
      //deve gestire la disconnessione

      if (IS_DELAYED(timer1, 10*SECONDS)) {  // Ogni 10s sincroniza l'rtc con il protocollo NTP
        //setTime(WiFi.getTime());
        START_TIMER(timer1);
      }
#elif INTERNET_CHIP == Eth_W5500_Chip

#endif
      st_internet_index = INTERNET_LEVEL;
      break;
  }  //Fini switch


}  //Fine void

//Check RSSI
void WiFi_checkRSSI() {
  int RSSI = WiFi.RSSI();
  if (RSSI < (-89)) {                              //Led bianco significa segnale troppo debole
    WiFiDrv::analogWrite(25, 255);                 //GREEN
    WiFiDrv::analogWrite(26, 255);                 //RED
    WiFiDrv::analogWrite(27, 255);                 //BLUE
  } else if ((RSSI >= (-89)) && (RSSI < (-67))) {  //Qualita' segnale pessima
    WiFiDrv::analogWrite(25, 0);                   //GREEN
    WiFiDrv::analogWrite(26, 255);                 //RED
    WiFiDrv::analogWrite(27, 0);                   //BLUE
  } else if ((RSSI >= (-67)) && (RSSI < (-55))) {  //Qualita' segnale decente
    WiFiDrv::analogWrite(25, 165);                 //GREEN
    WiFiDrv::analogWrite(26, 255);                 //RED
    WiFiDrv::analogWrite(27, 0);                   //BLUE
  } else if ((RSSI >= (-55)) && (RSSI < (-30))) {  //Qualita' segnale Ottima
    WiFiDrv::analogWrite(25, 255);                 //GREEN
    WiFiDrv::analogWrite(26, 0);                   //RED
    WiFiDrv::analogWrite(27, 0);                   //BLUE
  } else if (RSSI >= (-30)) {                      //Qualita' segnale Troppo forte
    WiFiDrv::analogWrite(25, 0);                   //GREEN
    WiFiDrv::analogWrite(26, 0);                   //RED
    WiFiDrv::analogWrite(27, 255);                 //BLUE
  }
}