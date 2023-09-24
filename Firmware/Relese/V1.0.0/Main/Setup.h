/*
   In questo file è presente tutta la configurazione del Software
*/
#define VERSIONE_FW "1.0.0V"

//----Define per il debug seriale
//#define NFC_DEBUG_PRINTCARDINFO  //Scrive su seriale i dati base della carta e fa un focus sul settore rilevato con la chiave di accesso valida
//#define INTERNET_DEBUG_BASE  //Scrive su seriale i dati di debug per internet, connessione ecc.
//#define INTERNET_DEBUG_TG  //Scrive su seriale i dati di debug di Telegram

//#define INTERNET_MQTT_BASE  //Scrive su seriale i dati di connessione MQTT

//----
//--------Setup regole sull'output informazioni-----
#define NFC_PRINT_CARD_NAME_ONLCD  //Se questo define e' attivo, scriverà i dati conservati in 'NFC_DATA_LINE2' della card sul display i2c

//--------

//---Definisco byte per i livelli di autorizzazione
#define AUT_UTENTE_GUEST 0x0C, 0xCC, 0x00, 0x05
#define AUT_UTENTE_LV1 0x00, 0xAD, 0x00, 0x25
#define AUT_UTENTE_LV2 0x01, 0xAD, 0x05, 0x25
#define AUT_UTENTE_LV3 0x02, 0xAD, 0xA0, 0x25
#define AUT_UTENTE_ADMIN 0xFA, 0xAD, 0x05, 0x2A
//-----
// Parametri del block trailer riferito al sistema accessi che lo genera
#define SYSTEM_ID_2BYTE 0x0F, 0xA5
#define AUTORIZZAZZIONI_UTENTE AUT_UTENTE_LV1  //Più avanti nel codice sono presenti i vari livelli di autorizzazione
//------

#define I2C_PN532_ADDRESS 0x24  //Definisce il l'address del modulo NFC
#define I2C_LCD_ADDRESS 0x27    //Definisce il l'address dell'LCD

#define I2C_LCD_LINES 4   //Definisce il le righe del display
#define I2C_LCD_CHARS 20  //Definisce il le caratteri del display

#define OUTPUT_RELE 4   //Indica il pin associato al relè
#define LED_SUCCESS 2   //Indica il pin del led che conferma il login
#define LED_FAIL 3      //Indica il pin del led che segnale l'errore di login
#define BUZZER_SOUND 5  //Indica il pin per il buzzer

#define INPUT_PROG1 A5   //Indica il pin per l'input isolato
#define INPUT_TAMPER A6  //Indica il del tamper

#define SPI_CANBUS_CS 6    //Definisce il pin di CS per il modulo CAN
#define SPI_ETHERNET_CS 7  //Definisce il pin di CS per il modulo ETHERNET W5500

#define IRQ_NFC 1     //Definisce il pin di IRQ per L'NFC
#define RESET_NFC 50  //pin 50 not used

#define DIP_SW1 A4
#define DIP_SW2 A3
#define DIP_SW3 A2
#define DIP_SW4 A1
/*
Definisco che tipo di connessione voglio usare
*/
#define WiFi_NINA_Chip 0x01  //Definisco il chip WiFi
//#define Eth_W5500_Chip 0x02  //Definisco il chip Eth - Futura implementazione

#define INTERNET_CHIP WiFi_NINA_Chip  //Definisco che chip per l'accesso a internete deve usare il WiFi

/*
Definisco le credenziali di accesso al WiFi
*/
#define WiFi_SSID "Internet-SSID"
#define WiFi_PASS "FuturaElettronica-PASS"

#define ENABLE_MQTT  //Commentando questo parametro disabilito il servizio client mqtt
//Configuro i parametri della connessione al server MQTT

#define MQTT_BROKER "192.168.1.005"
#define MQTT_BROKER_PORT 8883
#define MQTT_DEVICE_ID "Arduino Attivatore"
#define MQTT_USERNAME "Alduino"
#define MQTT_PASSWORD "test"
#define MQTT_START_TOPIC "ControlloAccessi/CasaAlMare/"  //Topic che identifica dove il controllo accessi può essere raggiunto tramite MQTT

/*
   Definisco le funzioni per i led programmabili
*/
#define OUTPUT_EMPITY 0x00  //Output non usato

#define OUTPUT_TRG_EVENT_VALIDBADGE 0x04    //Triggera l'output quando legge un badge valido
#define OUTPUT_TRG_EVENT_INVALIDBADGE 0x05  //Triggera l'output quando legge un badge valido

#define OUTPUT_TRG_EVENT_MQTT_CONTROLLED_TRG 0x06  //Indica che l'output viene triggerato tramite MQTT
#define OUTPUT_TRG_EVENT_MQTT_CONTROLLED 0x07      //Indica che l'output viene pilotato tramite MQTT

#define OUTPUT_TRG_EVENT_INPUT_TRG 0x09  //Indica che l'output può venire triggerato dall'input
#define OUTPUT_TRG_EVENT_AS_INPUT 0x0A   //Indica che l'output può viene pilotato in base all'input

//Di base lo stato dell'input può essere letto sia da CAN che da MQTT
#define INPUT_EMPITY 0x00     //Input non usato
#define INPUT_AS_TAMPER 0x01  //Indica che l'input è usato per il tamper

#define INPUT_TRG_OUTPUT_EVENT 0x02  //l'input triggera l'evento per pilotare l'output
#define INPUT_TRG_NO_POWER_AC 0x03   //l'input triggera l'evento della mancanza rete elettrica
#define INPUT_TRG_DISABLE_NFC 0x04   //l'input triggera l'evento che disabilita l'uso dell'attivatore


/*
 Definisco le programmazioni degli I/O
*/
#define LED_SUCCESS_EVENT OUTPUT_TRG_EVENT_VALIDBADGE
#define LED_FAIL_EVENT OUTPUT_TRG_EVENT_INVALIDBADGE
#define OUTPUT_EVENT OUTPUT_TRG_EVENT_VALIDBADGE

#define INPUT1_EVENT INPUT_EMPITY
#define INPUT_TAMPER_EVENT INPUT_AS_TAMPER

//Definisco i tempi di trigger degli I/O, se gli output sono gestiti tramite MQTT o CAN questi tempi possono essere ignorati
//Gli stati degli input vengono aggiornati ogni 0.5s da remoto, quindi se i livelli di trigger stanno sotto 0.5s non verranno visualizzati sul'mqtt

#define TRG_MS_LED_SUCCESS_EVENT 2 * SECONDS  //da un impulso di 1secondo
#define TRG_MS_LED_FAIL_EVENT 1 * SECONDS     //da un impulso di 2secondo
#define TRG_MS_OUTPUT_EVENT 1 * SECONDS       //da un impulso di 1secondo

/*
   Definisco il mio ID del protocollo CAN - la velocita del can la trovi in CAN.h. di base è 500KBPS
*/

#define ENABLE_CAN  //Sec commentato disattiva il CAN
#ifdef ENABLE_CAN
#include "mcp2515.h"
#define CAN_SPEED CAN_500KBPS
#define CAN_CRYSTAL MCP_8MHZ    //disponibile anche per cristalli da 16MHz e 20Mhz
#define CAN_SPI_SPEED 10 * MHz  //Anche il modulo ethernet va a 10Mhz
#endif

#define CAN_BUS_MYID 0x0A     //Serve per indentificare un ID CAN base alla centralina accessi
#define CAN_BUS_MYINDEX 0x00  //Serve per shiftare tutti i dati all'interno del protocollo CAN

/*
   Configurazione NFC per effettuare l'auto convalida
*/


#define NFC_AUTH_LOGIN_KEY 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF 
uint8_t nfc_login_key[6]{ NFC_AUTH_LOGIN_KEY };
#define NFC_AUTH_KEYA nfc_login_key

//PassKey della card
#define NFC_LOGIN_TAG SYSTEM_ID_2BYTE, AUTORIZZAZZIONI_UTENTE, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
//---------------------------
