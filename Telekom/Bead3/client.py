from socket import socket, AF_INET, SOCK_STREAM
import random
import time
import sys
import struct
import math

guessRange = [1, 100]

def GetRandomDirection():
    randomNum : int = random.randrange(start = 0, stop = 2)
    if randomNum == 0:
        return '<'
    else:
        return '>'

def GetMiddleNumber():
    global guessRange
    middlepoint : float = (guessRange[0] + guessRange[1]) / 2
    frac, whole = math.modf(middlepoint)
    return int(whole)

def AnalizeAnswer(answer : str, direction : str, myGuess : int):
    global guessRange
    if direction == '>': # ha arra kérdeztem, hogy a gondolt szám nagyobb ...
        # ... és a kapott válasz igen ...
        if answer == 'I':
            # ... akkor a kisebb határt kell mozgatnom felfele
            if guessRange[0] == myGuess:
                guessRange[0] += 1
            else:
                guessRange[0] = myGuess
        # ... és a kapott válasz nem ...
        else:
            # ... akkor a nagyobb határt kell lefele mozgatnom
            direction = '=<'
            if guessRange[1] == myGuess:
                guessRange[1] -= 1
            else:
                guessRange[1] = myGuess
    elif direction == '<': # ha arra kérdeztem rá, hogy a gondolt szám kisebb ...
        # ... és a kapott válasz igen ...
        if answer == 'I':
            # ... akkor a nagyobb határt kell mozgatnom lefele
            if guessRange[1] == myGuess:
                guessRange[1] -= 1
            else:
                guessRange[1] = myGuess
        # ... és a kapott válasz nem ...
        else:
            # ... akkor a kisebb határt kell felfele mozgatnom
            direction = '>='
            if guessRange[0] == myGuess:
                guessRange[0] += 1
            else:
                guessRange[0] = myGuess
    else: # ha = egy számmal
        print('Az eredmény: GONDOLT =', myGuess)
        return
    print('A válasz alapján: GONDOLT ', direction, " ", myGuess, " tehát: ", guessRange, "\n")

#Actual code
server_addr = (sys.argv[1], int(sys.argv[2])) # localhost és port
packer = struct.Struct('si')

with socket(AF_INET, SOCK_STREAM) as client:
    client.connect(server_addr)

    #Ameddig egy tényleges tartomány a range, nem pedig egyetlen szám
    while guessRange[0] != guessRange[1]:

        guess : int = GetMiddleNumber()
        tippDirection : str = GetRandomDirection()
        data = packer.pack(*(tippDirection.encode(), guess))

        # print("Ez a mostani range: ", guessRange)
        print("Ez a mostani tipp: ", tippDirection, guess)

        client.sendall(data)
        
        data = client.recv(1024)
        unp_data = packer.unpack(data)
        answerStr : str = unp_data[0].decode()
        guessedNumber : int = unp_data[1]

        print("Választ kaptam: (", answerStr, ", ", guessedNumber, ")")
        AnalizeAnswer(answerStr, tippDirection, guessedNumber)
        #time.sleep(5.0)

    #Amikor már a két szám egyenlő:
    guess : int = guessRange[0]
    print("Ez a mostani tipp: ", '=', guess)
    data = packer.pack(*('='.encode(), guess))
    client.sendall(data)
    #Várom az eredményt
    data = client.recv(1024)
    unp_data = packer.unpack(data)
    answerStr : str = unp_data[0].decode()
    guessedNumber : int = unp_data[1]
    print("Választ kaptam: (", answerStr, ", ", guessedNumber, ")")