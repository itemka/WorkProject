//Скорость передачи для последовательного порта
int BaudRate = 9600;
// Реле модуль подключен к цифровому выводу 4
int Relay = 4;
//Лампочка подключен к цифровому выводу 13
int ledPin = 13;

void setup()
{	
	Serial.begin(BaudRate);
	pinMode(Relay, OUTPUT);
	delay(50);
	//...
	digitalWrite(Relay, HIGH);  // реле выключено

	//Режим работы 13-ого цифрового пина в качестве выхода
	pinMode(ledPin, OUTPUT);
	//Пин для концевика, для проверки попадания напряжения на вход (GNB обязательно для концевика)
	pinMode(2, INPUT_PULLUP);
}

void loop() {
	/////////////////////////////////////Для концевика
	int returnMessage;
	if (digitalRead(2) == true)  //Если напряжение есть
	{	
		digitalWrite(ledPin, LOW);  //Выключаем
		returnMessage = 'b';
	}
	else
	{
		digitalWrite(ledPin, HIGH); //Включаем
		returnMessage = 'a';
	}
	//Для запуска программы
	if (returnMessage == 'a')
	{
		delay(50);
		Serial.write('a');
	}
	else
	{
		delay(50);
		Serial.write('b');
	}


	/////////////////////////////////////Для реле
	if (Serial.available())
	{
		int message = Serial.read();
		if (message == '1')
		{
			digitalWrite(Relay, LOW);   // реле включено
		}
		if (message == '0')
		{
			digitalWrite(Relay, HIGH);  // реле выключено
		}
	}
}