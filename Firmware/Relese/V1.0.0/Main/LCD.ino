
bool lcd_setup() {
  lcd.init();
  lcd.backlight();
  lcd.setCursor(0, 0);
  return true;
}
void lcd_startup_message() {
  lcd.print(" Controllo  Accessi");
  lcd.setCursor(7, 2);
  lcd.print(VERSIONE_FW);
  lcd.setCursor(0, 3);
  lcd.print("by: ALESSIO  SAVELLI");
}


void lcd_loop() {
  //if (LCD_IDLE != st_lcd_index) Serial.println(st_lcd_index);
  static unsigned long timer0;
  switch (st_lcd_index) {
    case LCD_IDLE:
      lcd.clear();    
      break;
    case LCD_VALID_BADGE:
      lcd_on();
      lcd.setCursor(0, 1);
      for (byte i = 0; i < 16; i++) {
        lcd.write(lcd_custom_line[i]);
      }
      lcd.setCursor(6, 2);  //Serve a centrare la scritta
      lcd.print("Benvenuto");
      st_lcd_index = LCD_VALID_BADGE_BEEP;
      digitalWrite(BUZZER_SOUND, HIGH);
      START_TIMER(timer0);
      break;
    case LCD_VALID_BADGE_BEEP:
      //Beep veloce e custom per indicare che e' vaido
      if (IS_DELAYED(timer0, 650)) {  //Spegne il buzzer
        digitalWrite(BUZZER_SOUND, LOW);
        st_lcd_index = LCD_SHOWING_DELAY;
        START_TIMER(timer0);
      } else if (IS_DELAYED(timer0, 500)) {  //Accende il buzzer
        digitalWrite(BUZZER_SOUND, HIGH);
      } else if (IS_DELAYED(timer0, 150)) {  //Spegna il buzzer
        digitalWrite(BUZZER_SOUND, LOW);
      }
      break;
    case LCD_INVALID_BADGE:
      lcd_on();
      lcd.setCursor(3, 2);  //Serve a centrare la scritta
      lcd.print("Accesso Negato");
      st_lcd_index = LCD_INVALID_BADGE_BEEP;
      digitalWrite(BUZZER_SOUND, HIGH);
      START_TIMER(timer0);
      break;
    case LCD_INVALID_BADGE_BEEP:
      //Beep lento e custom per indicare che non e' vaido
      if (IS_DELAYED(timer0, 850)) {  //Spegne il buzzer
        lcd.backlight();
        digitalWrite(BUZZER_SOUND, LOW);
        st_lcd_index = LCD_SHOWING_DELAY;
        START_TIMER(timer0);
      } else if (IS_DELAYED(timer0, 500)) {  //Accende il buzzer
        lcd.backlight();
        digitalWrite(BUZZER_SOUND, HIGH);
      } else if (IS_DELAYED(timer0, 350)) {  //Spegna il buzzer
        digitalWrite(BUZZER_SOUND, LOW);
      }
      break;
    case LCD_SHOWING_DELAY:
      if (IS_DELAYED(timer0, LCD_SHOW_DELAY_PAGE)) {
        st_lcd_index = LCD_IDLE;
      }
      break;
  }
}

void lcd_clear() {
  LCD_SET_CLEARSTATE;
}
void lcd_on() {
  lcd.backlight();
  lcd.display();
}

void lcd_print_access(uint8_t value[16], bool st) {  //Mostra le pagine sono se il display e' libero

#ifdef NFC_PRINT_CARD_NAME_ONLCD
  for (uint8_t i = 0; i < 16; i++) {
    lcd_custom_line[i] = value[i];
  }
#endif
  (st) ? (LCD_SET_VALIDBADGE) : (LCD_SET_INVALIDBADGE);
}