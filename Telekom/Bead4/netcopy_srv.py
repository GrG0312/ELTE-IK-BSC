from socket import socket, AF_INET, SOCK_STREAM
import sys
import hashlib

# 1 - saját ip
# 2 - saját port
# 3 - checksum ip
# 4 - checksum port
# 5 - fileID
# 6 - elérési útvonal

# Actual Code
thisServerAddr = (sys.argv[1], int(sys.argv[2])) # az ip és port a saját szervernek
csServerAddr = (sys.argv[3], int(sys.argv[4])) # az ip és port a checksum szervernek

with socket(AF_INET, SOCK_STREAM) as server:
	server.bind(thisServerAddr)
	server.listen(1)
	
	md5Checksum = hashlib.md5(''.encode())
	client, client_addr = server.accept()
	end = False
	
	with open(sys.argv[6], "wb") as file:
		while not end:
			data = client.recv(10)
			if data:
				file.write(data)
				md5Checksum.update(data)
			else:
				client.close()
				end = True
	
	print("A teljes fájl beérkezett")
	self_check = md5Checksum.hexdigest() # véglegesítjük a checksumot

with socket(AF_INET, SOCK_STREAM) as client:
	client.connect(csServerAddr) # csatlakozok a CS szerverhez
	
	message : str = "|".join([ "KI", sys.argv[5]]) # összerakom az üzenetet
	print("Üzenetet küldök: ", message)
	client.sendall(message.encode()) # elküld
	data = client.recv(1024)
	message = data.decode().split("|")
	print("Üzenetet kaptam: ", message)
	recvChecksum = message[1]
	
	if recvChecksum == self_check:
		print("CSUM OK")
	else:
		print("CSUM CORRUPTED")
