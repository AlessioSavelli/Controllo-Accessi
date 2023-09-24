struct OUTPUT_INFO {
  unsigned int pin;           //indica il pin dell'IO
  unsigned int event;         //Indica l'evento a cui e' associato il pin
  bool status = false;        //Indica lo stato dell'output
  bool bistable = false;      //indica se l'io funziona tramite trigger o no
  unsigned int trg_time = 0;  //indica il tempo per quanto deve rimanere triggerato
  unsigned long trg_timer;    //timer per gestire il trigger
};
struct INPUT_INFO {
  unsigned int pin;              //indica il pin dell'IO
  unsigned int event;            //Indica l'evento a cui e' associato il pin
  unsigned int filter_debounce;  //il filtro di dobounce espresso in ms
  bool status = false;           //Indica lo stato dell'INPUT
  unsigned long HIGH_timer;      //timer che si attiva quando passa dallo stato LOW allo stato HIGH
  unsigned long LOW_timer;       //timer che si attiva quando passa dallo stato HIGH allo stato LOW
};

struct EVENT_MANAGER {
  bool RELE_MQTT_TRG = false;  //fa da trigger per cambiare lo stato del rele
  bool RELE_CAN_TRG = false;  //fa da trigger per cambiare lo stato del rele
  bool RELE_INPUT_TRG = false; //L'input triggera l'output
  bool RELE_TRG = false;       //fa da trigger per cambiare lo stato del rele   n.B IL CAN  puo' PILOTTARE DIRETTAMENTE IL RELE
  bool AC_MISSING = false;     //indica lo stato dell'alimentazione
  bool DISABLE_NFC = false;    //Disabilita la lettura NFC
  bool VALID_BADGE = false;    //Evento di valid badge
  bool INVALID_badge = false;  //Evento di valid badge
};
EVENT_MANAGER EVENT_HANDLER;

OUTPUT_INFO io_output_setup(unsigned int pin, bool status, bool bistable, unsigned int event, unsigned int trg_time);
void io_output_write(OUTPUT_INFO* OUTPUT);

INPUT_INFO io_input_setup(unsigned int pin, unsigned int event);
void io_input_read(INPUT_INFO* INPUT);
