//�������� �������� ��� ����������������� �����
int BaudRate = 9600;
// ���� ������ ��������� � ��������� ������ 4
int Relay = 4;
//�������� ��������� � ��������� ������ 13
int ledPin = 13;

void setup()
{	
	Serial.begin(BaudRate);
	pinMode(Relay, OUTPUT);
	delay(50);
	//...
	digitalWrite(Relay, HIGH);  // ���� ���������

	//����� ������ 13-��� ��������� ���� � �������� ������
	pinMode(ledPin, OUTPUT);
	//��� ��� ���������, ��� �������� ��������� ���������� �� ���� (GNB ����������� ��� ���������)
	pinMode(2, INPUT_PULLUP);
}

void loop() {
	/////////////////////////////////////��� ���������
	int returnMessage;
	if (digitalRead(2) == true)  //���� ���������� ����
	{	
		digitalWrite(ledPin, LOW);  //���������
		returnMessage = 'b';
	}
	else
	{
		digitalWrite(ledPin, HIGH); //��������
		returnMessage = 'a';
	}
	//��� ������� ���������
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


	/////////////////////////////////////��� ����
	if (Serial.available())
	{
		int message = Serial.read();
		if (message == '1')
		{
			digitalWrite(Relay, LOW);   // ���� ��������
		}
		if (message == '0')
		{
			digitalWrite(Relay, HIGH);  // ���� ���������
		}
	}
}