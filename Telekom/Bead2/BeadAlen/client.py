import struct
import argparse

#Feladat1
parser=argparse.ArgumentParser()
parser.add_argument('db1',type=str)
parser.add_argument('db2',type=str)
parser.add_argument('db3',type=str)
parser.add_argument('db4',type=str)
args=parser.parse_args()

format1="c9si"
size1=struct.calcsize(format1)
format2="if?"
size2=struct.calcsize(format2)
format3="?9sc"
size3=struct.calcsize(format3)
format4="icf"
size4=struct.calcsize(format4)

with open(args.db1,'rb') as f:
    unpackedData=struct.unpack(format1,f.read(size1))
    print(unpackedData)

with open(args.db2,'rb') as f:
    unpackedData=struct.unpack(format2,f.read(size2))
    print(unpackedData)

with open(args.db3,'rb') as f:
    unpackedData=struct.unpack(format3,f.read(size3))
    print(unpackedData)

with open(args.db4,'rb') as f:
    unpackedData=struct.unpack(format4,f.read(size4))
    print(unpackedData)

#Feladat2

format11="18si?"
format21="f?c"
format31="i16sf"
format41="ci19s"

param1=("elso".encode('utf-8'), 48, True)
param2=(51.5, False, 'X'.encode('utf-8'))
param3=(39, "masodik".encode('utf-8'), 58.9)
param4=('Z'.encode('utf-8'), 70, "harmadik".encode('utf-8'))

packedData1=struct.pack(format11,*param1)
packedData2=struct.pack(format21,*param2)
packedData3=struct.pack(format31,*param3)
packedData4=struct.pack(format41,*param4)

print(packedData1)
print(packedData2)
print(packedData3)
print(packedData4)



