from socket import socket, AF_INET, SOCK_STREAM
import sys
from socket import socket, AF_INET, SOCK_STREAM
import hashlib

# 1 - netcopy ip
# 2 - netcopy port
# 3 - checksum ip
# 4 - checksum port
# 5 - fileID (valami random int)
# 6 - filenév elérési úttal

# Actual Code
netcpyServerAddr = (sys.argv[1], int(sys.argv[2])) # az ip és port a netcopy szerverhez
csServerAddr = (sys.argv[3], int(sys.argv[4])) # az ip és port a checksum szerverhez

fileID : int = int(sys.argv[5])
filePath : str = sys.argv[6]

with socket(AF_INET,SOCK_STREAM) as client: # nyitunk egy klienst
	md5Checksum = hashlib.md5(''.encode())
	with open(filePath, "rb") as file: # megnyitjuk a fájlt és elkészítjük a checksumot
		client.connect(netcpyServerAddr)
		l = file.read(10)
		md5Checksum.update(l)
		while l:
			client.sendall(l)
			l = file.read(10)
			md5Checksum.update(l)

print("Fájl beolvasva teljesen...")

with socket(AF_INET,SOCK_STREAM) as client: # amikor elküldjük a végső checksumot 
	client.connect(csServerAddr)
	check = md5Checksum.hexdigest()
	message : str = "|".join([ "BE", str(fileID), str(60), str(len(check)), check ])
	print("Az elküldött üzenet: ", message)
	client.sendall(message.encode())