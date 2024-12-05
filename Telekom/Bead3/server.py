import random
from socket import socket, AF_INET, SOCK_STREAM
import select
import sys
import struct

targetNumber : int

def SelectNumber():
    global targetNumber
    targetNumber = random.randrange(start = 1, stop = 101)
    print('A kiválaszott szám: ', targetNumber)

def DetermineGuessResponse(guess : int, type : str):
    if type == '=':
        if targetNumber == guess:
            return 'Y'
        else:
            return 'K'
    elif type == '>':
        if targetNumber > guess:
            return 'I'
        else:
            return 'N'
    else:
        if targetNumber < guess:
            return 'I'
        else:
            return 'N'


#Actual code
server_addr = (sys.argv[1], int(sys.argv[2])) # localhost és portszám
packer = struct.Struct('si')
SelectNumber()

with socket(AF_INET, SOCK_STREAM) as server:
    inputs = [ server ]
    server.bind(server_addr)
    server.listen(5)

    ongoingGame : bool = True
    
    while True:
        timeout : int = 1
        readable, writable, errorThrown = select.select(inputs, inputs, inputs, timeout)
        #r - olvasható socketek
        # ha a főszerver: be el tudunk fogadni egy bejövő kapcsolatot
        # ha egy meglévő kapcsolat klienstől: ki tudunk olvasni adatot
        # ha egy meglévő kapcsolat klienstől, de nincs adat: be lehet zárni, a kliens lekapcsolódott
        
        if not (readable or writable or errorThrown):
            continue
        
        for sct in readable:
            if sct is server:
                client, client_addr = sct.accept()
                inputs.append(client)
                print("Csatlakozott: ",client_addr)
            else:
                data = sct.recv(1024)
                #Ha nem tudunk beolvasni adatot - data null:
                if not data:
                    #A kliens kilépett
                    inputs.remove(sct)
                    sct.close()
                    print("Kliens lecsatlakoztatva...")
                #Ha be tudunk olvasni egy adatot - data nem null:
                else:
                    #Csomagoljuk ki a beérkezett adatot
                    unp_data = packer.unpack(data)
                    typeStr : str = unp_data[0].decode()
                    guessedNumber : int = unp_data[1]

                    print("Érkezett guess: (", typeStr, ", ", guessedNumber, ")")

                    # Megnézem hogy megy-e még a játék
                    if ongoingGame:
                        # Ha megy kell egy választ generálni
                        answer : str = DetermineGuessResponse(guessedNumber, typeStr)
                        # Ha eltalálta a kliens akkor be kell fejezni a játékot
                        if answer == 'Y':
                            ongoingGame = False
                            print('Valaki kitalálta!')
                        print('A válaszom: ', answer)
                        data = packer.pack(*(answer.encode(), 0))
                        sct.sendall(data) # visszaküldöm csak a kérdező kliensnek
                    else:
                        # Ha nem megy a játék akkor annyit kell visszaküldenem, hogy vége a játéknak
                        data = packer.pack(*('V'.encode(), guessedNumber))
                        sct.sendall(data)
