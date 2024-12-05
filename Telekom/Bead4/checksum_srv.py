import sys
from socket import socket, AF_INET, SOCK_STREAM
import select
import time

# 1 - saját ip
# 2 - saját port

class ChecksumData:
    fileID : int
    remTime : int
    length : int
    checksum : str
    arrivedTime : int

    def __init__(self, fileid : int, time : int, csLength : int, cs : str, arrived : int):
        self.fileID = fileid
        self.remTime = time
        self.length = csLength
        self.checksum = cs
        self.arrivedTime = arrived


checksums : list[ChecksumData] = []

def Add(fileID : int, validTime : int, checksumLength : int, checksum : str):
    arrived : int = time.time()
    checksums.append(ChecksumData(fileID, validTime, checksumLength, checksum, arrived))

def Remove(fileID : int) -> str:
    for ch in checksums:
        if ch.fileID == fileID:
            return ch.checksum

def RemoveOutdated():
    nowTime : int = time.time()
    for ch in checksums:
        if nowTime - ch.arrivedTime >= ch.remTime:
            checksums.remove(ch)

#Actual Code
serverAddr = (sys.argv[1], int(sys.argv[2])) # localhost és port

with socket(AF_INET, SOCK_STREAM) as server:
    inputs = [ server ]
    server.bind(serverAddr)
    server.listen(10) # a 10 azt jelenti, hogy hány érkező kapcsolatot tud eltárolni, mielőtt nem fogad többet
    
    while True:
        readable, writable, errorThrown = select.select(inputs, inputs, inputs, 1) # 1: 1 seces timeout

        if not readable: # ha nincs semmi a fogadott csatornán
            continue

        #Minden egyes olvasható üzenetet végignézünk
        for sct in readable:
            # Ha az üzenet a server, akkor klienst kell fogadni
            if sct is server:
                client, clientAddr = sct.accept()
                inputs.append(client)
                print("Csatlakozott kliens: ", clientAddr)
            # Ha nem server, akkor egy kérés érkezett
            else:
                inc = sct.recv(1024)
                # Ha nem olvasunk be semmit - data is null
                if not inc:
                    inputs.remove(sct)
                    sct.close()
                    print("Kliens lecsatlakoztatva...")
                # Amennyiben olvastunk be adatot - data not null
                else:
                    msg : str = inc.decode()
                    data = msg.split("|")
                    print("Kérés érkezett: ", data)
                    if data[0] == 'KI':
                        RemoveOutdated()
                        requested : str = Remove(int(data[1]))
                        length : int = len(requested)
                        response : str = "|".join([ str(length), requested ])
                        sct.sendall(response.encode())
                    elif data[0] == 'BE':
                        Add(int(data[1]), int(data[2]), int(data[3]), data[4])
