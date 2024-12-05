import struct
import sys


#Elso feladat
def ReadFromFile(argNum : int, format : str):
    packer = struct.Struct(format)
    with open(sys.argv[argNum], 'rb') as file:
        file.seek(packer.size)
        data = file.read(packer.size)
        unp_data = packer.unpack(data)
        print(unp_data)

ReadFromFile(1, '9s i f')
ReadFromFile(2, 'f ? c')
ReadFromFile(3, 'c i 9s')
ReadFromFile(4, 'f 9s ?')

#Masodik feladat
def EncodeIntoBinary(param, format : str):
    packedData = struct.pack(format, *param)
    print(packedData)

EncodeIntoBinary(("elso".encode(),86,True), '18s i ?')
EncodeIntoBinary((89.5, False, 'X'), 'f ? c')
EncodeIntoBinary((77, "masodik".encode(), 96.9), 'i 16s f')
EncodeIntoBinary(('Z', 108, "harmadik".encode()), 'c i 19s')