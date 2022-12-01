#include "M5Atom.h"

String inputString = "";         // a string to hold incoming data
boolean stringComplete = false;  // whether the string is complete
String commandString = "";
String result = ""; 
const int BUFFER_SIZE = 50;
char buf[BUFFER_SIZE];

boolean isConnected = false;

void setup() {
  M5.begin(true, true, true); 
  M5.dis.fillpix(CRGB::Pink);
}

void loop() {
  if(Serial.available() > 0){
    int rlen = Serial.readBytes(buf, BUFFER_SIZE);
    for(int i = 0; i < rlen; i++)
      result += buf[i];
    
    if (result == "COL1n")
    {
      M5.dis.fillpix(CRGB::Blue);
    }
    if (result == "COL2n")
    {
      M5.dis.fillpix(CRGB::Red);
    }
    if (result == "COL3n")
    {
      M5.dis.fillpix(CRGB::Green);
    }

    result = "";
  }
}

