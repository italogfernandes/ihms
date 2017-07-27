#include<Timer.h>

//#define DEBUG_MODE
#ifdef DEBUG_MODE
#define DEBUG_PRINT_(X) Serial.print(X)
#endif
#ifndef DEBUG_MODE
#define DEBUG_PRINT(X)
#endif /*DEBUG_MODE*/


//Defines
#define FreqAcq 20
#define uart_baudrate 115200
#define uart_start '$'
#define uart_end '\n'
#define EMGCH0 A0
#define EMGCH1 A1
#define LED_PIN 13

//Variables
Timer aquiringTimer;
int aquiringTimerId;
bool led_status = false;
uint16_t readingCH0, readingCH1;
String serialOp;

void setup() {
  pinMode(LED_PIN, OUTPUT);
  pinMode(EMGCH0, INPUT);
  pinMode(EMGCH1, INPUT);
  Serial.begin(uart_baudrate);
//#ifdef DEBUG_MODE
  aquiringTimerId = aquiringTimer.every(1000 / FreqAcq, readEMGs);
//#endif
}

void loop() {
  aquiringTimer.update();
  //Menu
  if (Serial.available() > 0) {
    serialOp = Serial.readString();
    if (serialOp == "CMDSTART") {
      aquiringTimerId = aquiringTimer.every(1000 / FreqAcq, readEMGs);
    } else if (serialOp == "CMDSTOP") {
      digitalWrite(LED_PIN, LOW);
      aquiringTimer.stop(aquiringTimerId);
    }
  }
}

void readEMGs() {
  digitalWrite(LED_PIN, led_status);
  led_status = !led_status;

  readingCH0 = analogRead(EMGCH0);
  readingCH1 = analogRead(EMGCH1);

#ifdef DEBUG_MODE
  Serial.print(readingCH0); Serial.print("\t");
  Serial.print(readingCH1); Serial.print("\n");
#endif
#ifndef DEBUG_MODE
  Serial.write((uint8_t) uart_start);
  Serial.write((uint8_t) (readingCH0 >> 8));
  Serial.write((uint8_t) readingCH0);
  Serial.write((uint8_t) (readingCH1 >> 8));
  Serial.write((uint8_t) readingCH1);
  Serial.write((uint8_t) uart_end);
#endif
}

