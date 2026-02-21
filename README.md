# 🧤 Gesture-Controlled Gaming Glove | Embedded Systems Project

An immersive gaming wearable that captures hand gestures using sensors and translates them into in-game actions. Developed as part of the Embedded Systems course at the **University of Jordan**.

---

## 📽️ Project Overview
The system uses an **ESP32** microcontroller as the "brain" to read data from an **MPU6050** (Accelerometer & Gyroscope) and a **Flex Sensor**. The data is transmitted via **UDP protocol** over Wi-Fi to a laptop, where a Python script maps the gestures to game controls (e.g., movement, shooting).

---

## 🚀 Key Features
* **Wireless Control**: Uses Wi-Fi (UDP) for low-latency communication between the glove and the game.
* **Gesture Mapping**:
    * **Flex Sensor**: Detects finger bending (e.g., shooting/clicking).
    * **MPU6050**: Detects hand tilt and movement (e.g., walking, aiming).
* **Immersive Feedback**: Real-time interaction with a "Zombie Killer" game built for testing.
* **Affordability**: A low-cost alternative to expensive VR motion controllers.

---

## 🛠️ Hardware Components
* **ESP32 Microcontroller**: Manages data acquisition and wireless transmission.
* **MPU6050**: 6-axis motion tracking (3-axis Gyro + 3-axis Accelerometer).
* **Flex Sensor**: Measures the degree of finger bending.
* **Resistors & Breadboard**: For circuit stability and connections.

---

## 💻 Tech Stack
* **Firmware**: C++/Arduino (for ESP32).
* **Software**: Python (for data processing and key mapping).
* **Communication**: UDP Protocol (Wi-Fi).
* **Modeling**: System flowcharts and circuit schematics included in the `Docs/` folder.

---


## 👥 The Development Team
**Supervisor**: Dr. Musa Al-Yaman

* Asya Samer
* Dana Abu Al Ruz
* Ethar Al Salameh
* Layla Al Laham
* Jana Saleh Godieh
* **Leen Al Masarweh**

---
*King Abdullah II School of Information Technology - University of Jordan.*
