# 🧑‍🔬 Arduino Lab Reservation System  

## 📌 Overview  
The **Arduino Lab Reservation System** is an **IoT-based access control project** that integrates a web application with Arduino hardware.  
Students can reserve labs through a web interface, and access to the lab is granted using a keypad + password verification system.  

If a valid and approved reservation exists, the Arduino unlocks the lab door via a **servo motor**, displays status messages on an **LCD screen**, and logs activity on an **SD card**.  

---

## 🎯 Key Features  
- 🔑 **Student Authentication** – login with student number & password  
- 📅 **Reservation Management** – web interface for lab booking  
- ✅ **Approval Workflow** – admin approves/rejects reservations  
- ⌨️ **Keypad Integration** – input student number & password  
- 🔐 **API Validation** – Arduino + ESP8266 communicates with a .NET API  
- 📟 **LCD Display** – access granted/denied messages  
- 🔄 **Logging** – entry/exit logs stored on SD card  
- 🚪 **Servo Motor** – controls lab door opening/closing  

---

## 🛠️ Technologies & Components  

### **Software**  
- ASP.NET Core Web API  
- Entity Framework Core  
- PostgreSQL  
- Arduino IDE (C++) / VS Code with Arduino extension  

### **Hardware**  
- Arduino Uno  
- ESP8266 Wi-Fi Module  
- 4x4 Keypad  
- I2C LCD Display  
- Servo Motor  
- SD Card Module  

---

## 🔗 System Architecture  

```mermaid
flowchart LR
    A[Web UI] -->|Reservation Request| B[.NET Backend API]
    B -->|Store & Validate| C[(PostgreSQL DB)]
    D[Admin Panel] -->|Approve/Reject| B
    E[Arduino + ESP8266] -->|API Call| B
    E -->|Display| F[LCD Screen]
    E -->|Control| G[Servo Motor]
    E -->|Log| H[SD Card Module]
