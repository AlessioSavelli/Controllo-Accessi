OUTPUT_INFO io_output_setup(unsigned int pin, bool status, bool bistable, unsigned int event, unsigned int trg_time) {
  OUTPUT_INFO out;
  pinMode(pin, OUTPUT);
  digitalWrite(pin, status);
  out.pin = pin;
  out.event = event;
  out.status = status;
  out.bistable = bistable;
  out.trg_time = trg_time;
  STOP_TIMER(out.trg_timer);
  return out;
}
INPUT_INFO io_input_setup(unsigned int pin, unsigned int event) {
  INPUT_INFO input;
  pinMode(pin, INPUT);
  input.pin = pin;
  input.event = event;
  input.status = digitalRead(pin);
  input.filter_debounce = 50;  //50ms di filtro fisso su tutti gli input
  (input.status) ? (START_TIMER(input.HIGH_timer)) : (START_TIMER(input.LOW_timer));
  return input;
}

//da fare
void io_output_write(OUTPUT_INFO* OUTPUT) {
  switch (OUTPUT->event) {
    case OUTPUT_TRG_EVENT_VALIDBADGE:
      if (EVENT_HANDLER.VALID_BADGE && !ISALIVE_TIMER(OUTPUT->trg_timer)) {
        EVENT_HANDLER.RELE_TRG = true;
      }
          if (OUTPUT->bistable) {
        if (EVENT_HANDLER.RELE_TRG || IS_DELAYED(OUTPUT->trg_timer, OUTPUT->trg_time)) {
          digitalWrite(OUTPUT->pin, !OUTPUT->status);
          OUTPUT->status = !OUTPUT->status;
          (EVENT_HANDLER.RELE_TRG) ? (START_TIMER(OUTPUT->trg_timer)) : (STOP_TIMER(OUTPUT->trg_timer));
          if (EVENT_HANDLER.RELE_TRG) EVENT_HANDLER.RELE_TRG = false;  //indico di aver eseguito il trigger
        }
      }
      break;
    case OUTPUT_TRG_EVENT_INVALIDBADGE:
      if (EVENT_HANDLER.INVALID_badge && !ISALIVE_TIMER(OUTPUT->trg_timer)) {
        EVENT_HANDLER.RELE_TRG = true;
      }
          if (OUTPUT->bistable) {
        if (EVENT_HANDLER.RELE_TRG || IS_DELAYED(OUTPUT->trg_timer, OUTPUT->trg_time)) {
          digitalWrite(OUTPUT->pin, !OUTPUT->status);
          OUTPUT->status = !OUTPUT->status;
          (EVENT_HANDLER.RELE_TRG) ? (START_TIMER(OUTPUT->trg_timer)) : (STOP_TIMER(OUTPUT->trg_timer));
          if (EVENT_HANDLER.RELE_TRG) EVENT_HANDLER.RELE_TRG = false;  //indico di aver eseguito il trigger
        }
      }
      break;
    case OUTPUT_TRG_EVENT_INPUT_TRG:
        if (EVENT_HANDLER.RELE_INPUT_TRG) {
        EVENT_HANDLER.RELE_INPUT_TRG = false;
        EVENT_HANDLER.RELE_TRG = true;
      }
          if (OUTPUT->bistable) {
        if (EVENT_HANDLER.RELE_TRG || IS_DELAYED(OUTPUT->trg_timer, OUTPUT->trg_time)) {
          digitalWrite(OUTPUT->pin, !OUTPUT->status);
          OUTPUT->status = !OUTPUT->status;
          (EVENT_HANDLER.RELE_TRG) ? (START_TIMER(OUTPUT->trg_timer)) : (STOP_TIMER(OUTPUT->trg_timer));
          if (EVENT_HANDLER.RELE_TRG) EVENT_HANDLER.RELE_TRG = false;  //indico di aver eseguito il trigger
        }
      }
      break;
    case OUTPUT_TRG_EVENT_MQTT_CONTROLLED_TRG:
      if (EVENT_HANDLER.RELE_MQTT_TRG) {
        EVENT_HANDLER.RELE_MQTT_TRG = false;
        EVENT_HANDLER.RELE_TRG = true;
      }
      if (OUTPUT->bistable) {
        if (EVENT_HANDLER.RELE_TRG || IS_DELAYED(OUTPUT->trg_timer, OUTPUT->trg_time)) {
          digitalWrite(OUTPUT->pin, !OUTPUT->status);
          OUTPUT->status = !OUTPUT->status;
          (EVENT_HANDLER.RELE_TRG) ? (START_TIMER(OUTPUT->trg_timer)) : (STOP_TIMER(OUTPUT->trg_timer));
          if (EVENT_HANDLER.RELE_TRG) EVENT_HANDLER.RELE_TRG = false;  //indico di aver eseguito il trigger
        }
      }
      break;
    case OUTPUT_TRG_EVENT_MQTT_CONTROLLED:
      digitalWrite(OUTPUT->pin, !OUTPUT->status);
    case OUTPUT_TRG_EVENT_AS_INPUT:
      digitalWrite(OUTPUT->pin, EVENT_HANDLER.RELE_TRG);
      OUTPUT->status = EVENT_HANDLER.RELE_TRG;
      break;
  }
}
//Da fare
void io_input_read(INPUT_INFO* INPUT) {

  //Lettura dello stato del pin
  bool pin_nStatus = digitalRead(INPUT->pin);
  switch (pin_nStatus) {
    case HIGH:
      if (!INPUT->status) {  //Se lo stato attuale e' su LOW attiva i filtri per passarlo su HIGH
        if (!ISALIVE_TIMER(INPUT->HIGH_timer)) {
          START_TIMER(INPUT->HIGH_timer);  //Avvia il filtro di debounce
          STOP_TIMER(INPUT->LOW_timer);    //Avvia il filtro di debounce
        }
      }
      //Check debounce
      if (IS_DELAYED(INPUT->HIGH_timer, INPUT->filter_debounce)) {  //Filtra l'input
        INPUT->status = HIGH;
        STOP_TIMER(INPUT->LOW_timer);  //Stoppo l'altro timer
      }
      break;
    case LOW:
      if (INPUT->status) {  //Se lo stato attuale e' su HIGH attiva i filtri per passarlo su HIGH
        if (!ISALIVE_TIMER(INPUT->LOW_timer)) {
          START_TIMER(INPUT->LOW_timer);  //Avvia il filtro di debounce
          STOP_TIMER(INPUT->HIGH_timer);  //Avvia il filtro di debounce
        }
      }
      //Check debounce
      if (IS_DELAYED(INPUT->LOW_timer, INPUT->filter_debounce)) {

        INPUT->status = LOW;
        STOP_TIMER(INPUT->HIGH_timer);  //Stoppo l'altro timer
      }
      break;
  }

  switch (INPUT->event) {
    case INPUT_AS_TAMPER:
      //per il momento non e' ancora usato
      break;
    case INPUT_TRG_OUTPUT_EVENT:
      if (INPUT->status) EVENT_HANDLER.RELE_TRG = true;
      break;
    case INPUT_TRG_NO_POWER_AC:
      EVENT_HANDLER.AC_MISSING = !(INPUT->status);
      break;
    case INPUT_TRG_DISABLE_NFC:
      EVENT_HANDLER.DISABLE_NFC = (INPUT->status);
      break;
  }
}
