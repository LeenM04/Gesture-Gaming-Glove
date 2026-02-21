import socket

# إعدادات الاستقبال
UDP_IP = "0.0.0.0"
UDP_PORT = 4210

file_path = r"C:\Users\user\Desktop\zomboid-survival-master_1\zomboid-survival-master\Assets\serial_output.txt"

sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
sock.bind((UDP_IP, UDP_PORT))

print(f"Listening for UDP data on port {UDP_PORT}...")

try:
    while True:
        data, addr = sock.recvfrom(1024)
        command = data.decode('utf-8', errors='ignore').strip().lower()

        if command:
            print(f"Received: {command}")

            try:
                with open(file_path, "w", encoding="utf-8") as f:
                    f.write(command)
            except PermissionError:
                print("File locked by Unity, skipping...")

except KeyboardInterrupt:
    print("UDP listener stopped.")
finally:
    sock.close()
