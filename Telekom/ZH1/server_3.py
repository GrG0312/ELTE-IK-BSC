from socket import socket, AF_INET, SOCK_STREAM
import struct
import select

serverAddr = ('localhost', 12000)

with socket(AF_INET, SOCK_STREAM) as server:
    inputs = [ server ]
    server.bind(serverAddr)
    server.listen(10)
    
    while True:
        timeout : int = 1
        readable, _, _ = select.select(inputs, inputs, inputs, timeout)
        #r - olvasható socketek
        # ha a főszerver: be el tudunk fogadni egy bejövő kapcsolatot
        # ha egy meglévő kapcsolat klienstől: ki tudunk olvasni adatot
        # ha egy meglévő kapcsolat klienstől, de nincs adat: be lehet zárni, a kliens lekapcsolódott
        
        if not (readable):
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
                    packer = struct.Struct('20s i ?')
                    unp_data = packer.unpack(data)

                    print('Kapott üzenet: ', unp_data)
                    stringData : str = unp_data[0].decode()
                    stringLength = len(stringData)
                    print('String hossza: ', stringLength)

                    answer = ""
                    if unp_data[2]: # ha a logikai érték igaz, akkor az elejéről veszek X karaktert
                        for i in range(0, unp_data[1]):
                            answer = answer + stringData[i]
                    else:
                        
                        for i in range(stringLength - unp_data[1], stringLength):
                            answer = answer + stringData[i]

                    packer = struct.Struct('10s')
                    data = packer.pack(answer.encode())
                    sct.sendall(data)

