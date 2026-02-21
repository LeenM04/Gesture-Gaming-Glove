import socket
import time

# إعدادات الاستقبال
UDP_IP = "0.0.0.0"          # يستقبل من أي IP
UDP_PORT = 4210             # نفس البورت اللي بالكود تبع ESP32

# مسار الملف اللي Unity بتقرأ منه
file_path = r"C:\Users\user\Desktop\zomboid-survival-master_1\zomboid-survival-master\Assets\serial_output.txt"

# إعداد سوكيت الـ UDP
sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
sock.bind((UDP_IP, UDP_PORT))

print(f"Listening for UDP data on port {UDP_PORT}...")

try:
    while True:
        # استقبل البيانات من ESP32
        data, addr = sock.recvfrom(1024)
        line = data.decode('utf-8', errors='ignore').strip()

        if line:
            print(f"Writing IMU line: {line}")
            try:
                with open(file_path, "r+", encoding="utf-8") as f:
                    f.seek(0)
                    f.write(line)
                    f.truncate()
            except PermissionError:
                print("File is currently locked by Unity. Skipping this write.")

        time.sleep(0.05)

except KeyboardInterrupt:
    print("UDP listener stopped by user.")
finally:
    sock.close()