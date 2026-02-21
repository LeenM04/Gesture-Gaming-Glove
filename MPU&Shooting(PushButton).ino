#include <WiFi.h>
#include <Wire.h>
#include <WiFiUdp.h>
#include <math.h>

// === إعدادات WiFi و UDP ===
const char* ssid = "iiii";
const char* password = "123456as";
const char* host = "10.86.167.20";  // IP الكمبيوتر اللي يشغّل Unity
const int port = 4210;

// === زر الإطلاق ===
const int buttonPinS = 35;
bool lastStateS = HIGH;
unsigned long lastShootTime = 0;
const unsigned long debounceDelay = 50;

// === MPU6050 ===
WiFiUDP udp;
const int MPU_ADDR = 0x68;
int16_t accX, accY, accZ;

void setup() {
  Serial.begin(115200);
  Wire.begin(21, 22);
  pinMode(buttonPinS, INPUT_PULLUP);

  // اتصال WiFi
  WiFi.begin(ssid, password);
  Serial.print("Connecting to WiFi");
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  Serial.println("\nWiFi connected! IP: " + WiFi.localIP().toString());

  // تهيئة MPU6050
  Wire.beginTransmission(MPU_ADDR);
  Wire.write(0x6B);
  Wire.write(0);
  Wire.endTransmission(true);
}

void sendUDP(String msg) {
  udp.beginPacket(host, port);
  udp.print(msg);
  udp.endPacket();
}

void loop() {

  // === زر الإطلاق ===
  bool currentStateS = digitalRead(buttonPinS);
  unsigned long now = millis();

  if (lastStateS == HIGH && currentStateS == LOW && (now - lastShootTime > debounceDelay)) {
    lastShootTime = now;
    sendUDP("shoot");
    Serial.println("shoot");
  }
  lastStateS = currentStateS;

  // === قراءة التسارع ===
  Wire.beginTransmission(MPU_ADDR);
  Wire.write(0x3B); // ACCEL_XOUT_H
  Wire.endTransmission(false);
  Wire.requestFrom(MPU_ADDR, 6, true);

  accX = Wire.read() << 8 | Wire.read();
  accY = Wire.read() << 8 | Wire.read();
  accZ = Wire.read() << 8 | Wire.read();

  float pitch = atan2(accX, accZ) * 180.0 / PI;  // أمام ↔ خلف
  float roll  = atan2(accY, accZ) * 180.0 / PI;  // يمين ↔ يسار


  String command = "";

if (pitch > 50) {
  command += "backward,";
}
if (pitch < -50) {
  command += "forward,";
}
if (roll > 50) {
  command += "right,";
}
if (roll < -50) {
  command += "left,";
}

// إذا ما في حركة
if (command == "") {
  command = "stop";
}
else {
  command.remove(command.length() - 1); // حذف الفاصلة الأخيرة
}


  sendUDP(command);
  Serial.println(command);

  delay(50); // تحديث كل 50ms
}
