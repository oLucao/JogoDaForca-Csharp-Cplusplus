#define led1  2
#define led2  3
#define led3  4
#define led4  5
#define led5  6
#define led6  7
#define led7  8
#define led8  9

void setup() {
   pinMode(led1, OUTPUT);
   pinMode(led2, OUTPUT);
   pinMode(led3, OUTPUT);
   pinMode(led4, OUTPUT);
   pinMode(led5, OUTPUT);
   pinMode(led6, OUTPUT);
   pinMode(led7, OUTPUT);
   pinMode(led8, OUTPUT);
   Serial.begin(9600); 
   }
 

void loop(){
  
    if (Serial.available()){
      char valor = Serial.read();
      if (valor == 'A'){
        digitalWrite(led1, HIGH);
      }
      if (valor == 'B'){
        digitalWrite(led2, HIGH);
      }
      if (valor == 'C'){
        digitalWrite(led3, HIGH);
      }
      if (valor == 'D'){
        digitalWrite(led4, HIGH);
      }
      if (valor == 'E'){
        digitalWrite(led5, HIGH);
      }
      if (valor == 'F'){
        digitalWrite(led6, HIGH);
      }
      if (valor == 'G'){
        digitalWrite(led7, HIGH);
      }
      if (valor == 'H'){
        digitalWrite(led8, HIGH);
      }
      if (valor == 'I'){
        digitalWrite(led1, HIGH);
        digitalWrite(led2, HIGH);
        digitalWrite(led3, HIGH);
        digitalWrite(led4, HIGH);
        digitalWrite(led5, HIGH);
        digitalWrite(led6, HIGH);
        digitalWrite(led7, HIGH);
        digitalWrite(led8, HIGH);
        Serial.write("Programa iniciado com sucesso!");
        }
        if (valor == 'J'){
        char value = Serial.read();
        while(value != 'I')
        {
        digitalWrite(led1, HIGH);
        digitalWrite(led2, HIGH);
        digitalWrite(led3, HIGH);
        digitalWrite(led4, HIGH);
        digitalWrite(led5, HIGH);
        digitalWrite(led6, HIGH);
        digitalWrite(led7, HIGH);
        digitalWrite(led8, HIGH);
        delay(100);
        digitalWrite(led1, LOW);
        digitalWrite(led2, LOW);
        digitalWrite(led3, LOW);
        digitalWrite(led4, LOW);
        digitalWrite(led5, LOW);
        digitalWrite(led6, LOW);
        digitalWrite(led7, LOW);
        digitalWrite(led8, LOW);
        delay(100);
        }
      }
        if (valor == 'K'){
        digitalWrite(led1, LOW);
        digitalWrite(led2, LOW);
        digitalWrite(led3, LOW);
        digitalWrite(led4, LOW);
        digitalWrite(led5, LOW);
        digitalWrite(led6, LOW);
        digitalWrite(led7, LOW);
        digitalWrite(led8, LOW);
        Serial.write("Programa reiniciado com sucesso!");
        }
    }
    delay(100);
}
