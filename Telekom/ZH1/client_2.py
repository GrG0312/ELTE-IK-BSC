from socket import socket, AF_INET, SOCK_DGRAM, timeout
import struct

serverAddr = ('localhost', 10000)

with socket(AF_INET, SOCK_DGRAM) as client:

    packer = struct.Struct('6s i')
    data = packer.pack(*('ZVP7EJ'.encode(), 336))

    print("Sending data: ", data)
    client.sendto(data, serverAddr)

    data, _ = client.recvfrom(1024)
    answer = data.decode()
    print(answer)

    packer = struct.Struct('i 3s')
    data = packer.pack(*(336, (answer[3] + answer[7] + answer[1]).encode()))
    client.sendto(data, serverAddr)

    data, _ = client.recvfrom(1024)
    answer = data.decode()
    print(answer)