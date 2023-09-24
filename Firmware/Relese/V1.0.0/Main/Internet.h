#include <WiFiNINA.h>
#include <utility/wifi_drv.h>
#if INTERNET_CHIP == WiFi_NINA_Chip
//importa le librerie per il WiFi
WiFiClient mqttClient;

#elif INTERNET_CHIP == Eth_W5500_Chip

//Gestisce le funzioni dedicate all'Ethernet
EthernetClient mqttClient;
#endif

#define MQTT_LOOP_TIME 50  //Ogni 50ms esegue il loop di telegram

enum st_internet {
  INTERNET_SETUP,  //Vede quale periferica deve utilizzare

  INTERNET_WiFi_SETUP,    //Fa il setup WiFi
  INTERNET_WIFI_WAITING,  //Attende 10s tra un tentativo e l'altro


  INTERNET_ETHERNET_SETUP,  //Fa il setup della periferica Ethernet


  INTERNET_CONNECTED,  //Va in questo se la periferica internet e' connessa

  INTERNET_LEVEL,  //Misura il livello di RSSI Per il WiFi o rileva la connessione del cavo ethernet

  INTERNET_LOOP_MQTT,  //Esegue il loop di telegram

  INTERNET_LOOP_GETIME,  //Sincroniza l'rtc tramite il protocollo NTP

};

uint8_t st_internet_index = INTERNET_SETUP;

void loop_internet();
void WiFi_checkRSSI();