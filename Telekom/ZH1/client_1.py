from socket import socket, AF_INET, SOCK_STREAM
import struct

serverAddr = ('localhost', 10000)

with socket(AF_INET, SOCK_STREAM) as client:
    client.connect(serverAddr)
    packer = struct.Struct('6s i')
    data = packer.pack(*('ZVP7EJ'.encode(), 593))

    print("Sending data: ", data)
    client.sendall(data)

    data = client.recv(1024)
    answer = data.decode()
    print(answer)

    packer = struct.Struct('i 3s')
    data = packer.pack(*(593, (answer[6] + answer[2] + answer[8]).encode()))
    client.sendall(data)

    data = client.recv(1024)
    answer = data.decode()
    print(answer)