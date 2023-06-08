#include <LiquidCrystal.h>
#include <SoftwareSerial.h>

// initialize the library with the numbers of the interface pins
LiquidCrystal lcd(8, 9, 4, 5, 6, 7);

const int stepPin = 28;
const int dirPin = 26;
const int enPin = 24;

int targetSteps = 0; // Variable pour stocker le nombre de pas à effectuer
int currentStep = 0; // Variable pour suivre le nombre de pas déjà effectués

void setup() {
  // set up the LCD's number of columns and rows:
  lcd.begin(16, 2);

  // Alimentation du potentiomètre
  pinMode(53, OUTPUT);
  digitalWrite(53, HIGH);

  // GND de la commande
  pinMode(22, OUTPUT);
  digitalWrite(22, LOW);

  // La commande du moteur
  pinMode(stepPin, OUTPUT);
  pinMode(dirPin, OUTPUT);
  pinMode(enPin, OUTPUT);
  digitalWrite(enPin, LOW);

  // Vitesse Serial Monitor
  Serial.begin(9600);
}

int LectureVitesse() {
  // Affichage de la vitesse
  lcd.clear();
  lcd.print("Vitesse : ");
  lcd.setCursor(10, 0);

  int AnalogIn = analogRead(8);
  int Vitesse = map(AnalogIn, 0, 1023, 0, 300);

  lcd.print(Vitesse);
  lcd.print("rpm");

  return Vitesse;
}

// Periode et Nombre d'impulsions
void Impulsions(int N, int T) {
  for (int x = 0; x < N; x++) {
    digitalWrite(stepPin, HIGH);
    delayMicroseconds(T);
    digitalWrite(stepPin, LOW);
    delayMicroseconds(T);
  }
}

int Sens() {
  int AnalogInBtn = analogRead(0);

  if (AnalogInBtn == 480) {
    lcd.setCursor(0, 1);
    lcd.print("Clockwise");
    return -1;
  }

  if (AnalogInBtn == 0) {
    lcd.setCursor(0, 1);
    lcd.print("CounterClockwise");
    return 1;
  }

  return 0;
}

void loop() {
  if (Serial.available()) {
    char command = Serial.read();

    if (command == 'F' || command == 'B'|| command == 'S') {
      if (command == 'F') {
        lcd.setCursor(0, 1);
        lcd.print("Clockwise");
      }

      if (command == 'B') {
        lcd.setCursor(0, 1);
        lcd.print("CounterClockwise");
      }
      
      if (command == 'S') {
      targetSteps = Serial.parseInt();
      targetSteps = abs(targetSteps); // S'assurer que le nombre de pas est toujours positif
      }



      int speed = Serial.parseInt();
      speed = constrain(speed, 0, 300); // Limiter la vitesse dans la plage de 0 à 300

      lcd.clear();
      lcd.print("Vitesse : ");
      lcd.setCursor(10, 0);
      lcd.print(speed);
      lcd.print("rpm");

      int T = 150000 / speed;
      digitalWrite(dirPin, (command == 'F') ? HIGH : LOW); // Définir la direction en fonction de la commande
      Impulsions(targetSteps, T);
    }

    /*if (command == 'S') {
      targetSteps = Serial.parseInt();
      targetSteps = abs(targetSteps); // S'assurer que le nombre de pas est toujours positif
      
      
      currentStep = 0; // Réinitialiser le nombre de pas actuel

      int direction = Sens(); // Obtenir la direction actuelle

      while (currentStep < targetSteps) {
        int speed = LectureVitesse(); // Lire la vitesse actuelle
        
        lcd.setCursor(0, 1);
        lcd.print(targetSteps);

        int T = 150000 / speed;

        digitalWrite(dirPin, direction); // Définir la direction

        int stepsToMove = min(targetSteps - currentStep, 100); // Nombre de pas à effectuer en une itération
        Impulsions(stepsToMove, T);

        currentStep += stepsToMove; // Mettre à jour le nombre de pas actuel
      }
    }*/
  }
}
