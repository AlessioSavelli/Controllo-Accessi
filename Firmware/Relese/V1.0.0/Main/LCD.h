/*
   In quensto file viene gestita la visualizzazione dei messaggi e il buzzer
*/
#include <LiquidCrystal_I2C.h>
LiquidCrystal_I2C lcd(I2C_LCD_ADDRESS, I2C_LCD_CHARS, I2C_LCD_LINES);

#define LCD_SHOW_DELAY_PAGE 2 * SECONDS

//definisco le macro dell'lcd  - Le paggine vengono visualizzate solo se il display e' libero. Le paggine che si vogliono sovrapporre verranno ignorate
#define LCD_IS_FREE (st_lcd_index == LCD_IDLE)
#define LCD_SET_CLEARSTATE (LCD_IS_FREE) ? (st_lcd_index = LCD_IDLE) : (false)
#define LCD_SET_VALIDBADGE (LCD_IS_FREE) ? (st_lcd_index = LCD_VALID_BADGE) : (false)
#define LCD_SET_INVALIDBADGE (LCD_IS_FREE) ? (st_lcd_index = LCD_INVALID_BADGE) : (false)
enum st_lcd {
  LCD_IDLE,  //Attende di visualizzare qualcosa

  LCD_VALID_BADGE,       //Mostra la schermata di validita' del badge
  LCD_VALID_BADGE_BEEP,  //Fa suonare il buzzer

  LCD_INVALID_BADGE,       //Mostra la schermata indicando che il badge non e' valido
  LCD_INVALID_BADGE_BEEP,  //Fa suonare il buzzer

  LCD_SHOWING_DELAY  //Attende del tempo prima che resetta il display
};

uint8_t st_lcd_index = LCD_IDLE;

uint8_t lcd_custom_line[16] = { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };

bool lcd_setup();
void lcd_startup_message();
void lcd_loop();
void lcd_clear();
void lcd_on();
void lcd_print_access(uint8_t value[16], bool st);