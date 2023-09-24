/*
   Titolo   : NFC Access Control - Elettronica IN
   Hardware : Arduino nano
   Ver      : 1.0V
   Data     : 22/09/2023
   Autore   : Alessio Savelli - Futura Group
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
#define DEVICE_ID switch_id
#define START_INDEX_ID 0  //Variare questo numero solo in caso in cui ci siano piu' di 16 dispositivi con lo stesso nome ma ID Diverso

#include <Scheduler.h>
#include "Macro.h"
#include "Setup.h"
#include "VALIDCARD.h"
#include "NFC.h"
#include "LCD.h"
#include "IO.h"
#include "Internet.h"
#include "mqtt.h"
#include "MY_CAN.h"

#define TERMINAL Serial  //Definisco la periferica seriale su cui fare debug
/*
   Definisco le variabili per gli stati dei componenti hardware
*/

bool nfc_status = false;
bool lcd_status = false;

/*
   Definisco le macro per lo stato dei componenti hardware
*/


#define NFC_CONNECTED nfc_status
#define LCD_CONNECTED lcd_status
//Controlla in base al programma se l'output deve essere bistable oppure no
#define IS_BISTABLE_OUTPUT(prg) ((prg == OUTPUT_TRG_EVENT_VALIDBADGE) || (prg == OUTPUT_TRG_EVENT_INVALIDBADGE) || (prg == OUTPUT_TRG_EVENT_MQTT_CONTROLLED_TRG)) ? (true) : (false)

OUTPUT_INFO my_output[3];
INPUT_INFO my_input[2];

int switch_id = 0;  //Ottiene l'id macchina impostato con gli switch

void read_badge();
void loop2();
void loop3();
void loop4();

void setup() {

  // Inizializzo la seriale
  TERMINAL.begin(115200);

  TERMINAL.print("NFC Access Controller ");
  TERMINAL.println(VERSIONE_FW);

  pinMode(DIP_SW1, INPUT);
  pinMode(DIP_SW2, INPUT);
  pinMode(DIP_SW3, INPUT);
  pinMode(DIP_SW4, INPUT);
  //Calcolo Device ID
  DEVICE_ID = (START_INDEX_ID * 16) + (digitalRead(DIP_SW1) | (digitalRead(DIP_SW2) << 1) | (digitalRead(DIP_SW3) << 2) | (digitalRead(DIP_SW4) << 3));
  TERMINAL.print("DEVICE ID =");
  TERMINAL.println(DEVICE_ID);
  delay(100);


  if (nfc_setup()) NFC_CONNECTED = true;  // imposta lo stato dell'NFC OK
  if (lcd_setup()) LCD_CONNECTED = true;  // imposta lo stato dell'LCD OK
  TERMINAL.print("Modulo NFC : ");
  TERMINAL.print((NFC_CONNECTED) ? (" trovato - ") : (" non trovato\n"));
  if (NFC_CONNECTED) nfc_print_version();  //Stampa la versione del modulo trovato

  TERMINAL.print("Modulo LCD : ");
  TERMINAL.println((LCD_CONNECTED) ? (" trovato") : (" non trovato"));
  lcd_startup_message();

  delay(100);
  //Setup pin out
  my_output[0] = io_output_setup(LED_SUCCESS, LOW, IS_BISTABLE_OUTPUT(LED_SUCCESS_EVENT), LED_SUCCESS_EVENT, TRG_MS_LED_SUCCESS_EVENT);
  my_output[1] = io_output_setup(LED_FAIL, LOW, IS_BISTABLE_OUTPUT(LED_FAIL_EVENT), LED_FAIL_EVENT, TRG_MS_LED_FAIL_EVENT);
  my_output[2] = io_output_setup(OUTPUT_RELE, LOW, IS_BISTABLE_OUTPUT(OUTPUT_EVENT), OUTPUT_EVENT, TRG_MS_OUTPUT_EVENT);

  pinMode(BUZZER_SOUND, OUTPUT);

  my_input[0] = io_input_setup(INPUT_PROG1, INPUT1_EVENT);
  my_input[1] = io_input_setup(INPUT_TAMPER, INPUT_TAMPER_EVENT);

  pinMode(IRQ_NFC, INPUT);


#ifdef CREATE_NFC_TAG
  TERMINAL.println("Pronto per la creazione del TAG");
  while (true) {  //Crea solo il TAG
    TERMINAL.println("Place your NDEF formatted Mifare Classic 1K card on the reader");
    TERMINAL.println("and press any key to continue ...");
    // Wait for user input before proceeding
    while (!TERMINAL.available())
      ;
    while (TERMINAL.available()) TERMINAL.read();
    create_nfc_tag();
    delay(100);
  }
#endif
  //Delay di benvenuto
  for (int i = 0; i < 3; i++) {
    digitalWrite(BUZZER_SOUND, HIGH);
    delay(150);
    digitalWrite(BUZZER_SOUND, LOW);
    delay(350);
  }
  digitalWrite(BUZZER_SOUND, LOW);

  Scheduler.startLoop(loop2);
  Scheduler.startLoop(loop3);
  Scheduler.startLoop(loop4);
}

void loop() {

  loop_internet();


#ifdef ENABLE_MQTT
  static unsigned long mqtt_timer0 = millis();
  if (IS_DELAYED(mqtt_timer0, 0.5 * SECONDS)) {
    mqtt_output_status(my_output);
    mqtt_input_status(my_input);
    START_TIMER(mqtt_timer0);
  }
#endif
  // IMPORTANT:
  // We must call 'yield' at a regular basis to pass
  // control to other tasks.
  yield();
}


void loop2() {
  if (NFC_CONNECTED && !EVENT_HANDLER.DISABLE_NFC) nfc_loop();
  if (LCD_CONNECTED) lcd_loop();

  if (!EVENT_HANDLER.DISABLE_NFC) read_badge();
  // IMPORTANT:
  // We must call 'yield' at a regular basis to pass
  // control to other tasks.
  yield();
}
void loop3() {
  static unsigned int index_output = 0;
  static unsigned int index_input = 0;
  //gestisco gli un output per ogni ciclo di loop
  io_output_write(&my_output[index_output]);  //Mantiene aggiornato costantemete lo stato delle uscite
  CIRC_BUFFER_ONESCROLL(index_output, 3);

  //gestisco gli un input per ogni ciclo di loop
  io_input_read(&my_input[index_input]);  //Mantiene aggiornato costantemete lo stato delle uscite
  CIRC_BUFFER_ONESCROLL(index_input, 2);
  // IMPORTANT:
  // We must call 'yield' at a regular basis to pass
  // control to other tasks.
  yield();
}
void loop4() {
#ifdef ENABLE_CAN
  loop_can(my_output, my_input);
#endif
  // IMPORTANT:
  // We must call 'yield' at a regular basis to pass
  // control to other tasks.
  yield();
}

void read_badge() {
  //Attende di leggere una nuova carta
  NFC_CARD card;
  static uint8_t old_uid[7];
  static unsigned long timeout_challange = 0;  //questo timer serve a disabilitare la challange dopo 5s
  if (NFC_GET_CARD(&card) && NFC_CONNECTED) {
    //Controlla che la carta appena processata venga rimossa dal lettore
    int bage_challenge = 0;
    if (!IS_DELAYED(timeout_challange, 5 * SECONDS)) {  //se non sono passati i 5s allora deve fare la challange
      for (int i = 0; i < 7; i++) {
        if (old_uid[i] == card.uid[i]) bage_challenge += 1;  // se i valori sono uguali perde una challenge
      }
    }  //Fine controllo challange
    //se alla fine della challange il numero di perdite e' uguale a 7 significa che il badge e' ancora quello vecchio
    if (bage_challenge != 7) {
#ifdef ENABLE_MQTT
      mqtt_send_badge(&card);
#endif
#ifdef ENABLE_CAN
      can_send_badge(&card);
#endif
      //Bisogna inserire che quando rileva una card valida ignora per i prossimo X tempo le carte non valide
      if (card.login_result) lcd_clear();  //Se la card e' valida da priorita' a questa pagina
      lcd_print_access(card.block2, card.login_result);
      //Triggero l'evento valid badge o invalid
      (card.login_result) ? (EVENT_HANDLER.VALID_BADGE = true) : (EVENT_HANDLER.INVALID_badge = true);

      //si salva il badge letto per fare la challenge
      for (int i = 0; i < 7; i++) {
        old_uid[i] = card.uid[i];  // si salva l'uid appena processato
      }
      START_TIMER(timeout_challange);                          //Abilita la challange per 5s ( evita di leggere lo stesso badge consecutivamente in meno di 5s)
    }                                                          //Fine controllo challange
  } else if (IS_DELAYED(timeout_challange, 0.15 * SECONDS)) {  //dopo 150ms resetto i flag
    EVENT_HANDLER.VALID_BADGE = false;
    EVENT_HANDLER.INVALID_badge = false;
  }  //il flag dello stato del badge dura qualche ms
}  ///Fine lettura badge