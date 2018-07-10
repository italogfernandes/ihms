
/* UNIVERSIDADE FEDERAL DE UBERLANDIA
   Biomedical Engineering Lab
   Autor: Ítalo G S Fernandes
   contact: italogsfernandes@gmail.com
   URL: https://github.com/italogsfernandes/
  
  Decrição:
  
  Comandos:
  
  Pacotes:
  
  Montagem:
  Arduino  | Dispositivo
  
  Opções de compilação:
    
*/

//////////////////////
//Variaveis globais //
//////////////////////
int valor_lido_ad;
float valor_em_volts;

//////////////////
//Main Function //
//////////////////
void setup() {
  Serial.begin(115200);
}

void loop() {
  valor_lido_ad = analogRead(A0);
  valor_em_volts = valor_lido_ad * 5.0 / 1023.0;
  Serial.println(valor_em_volts, 2);
  delay(50); //50ms equivale a +-20Hz
}

