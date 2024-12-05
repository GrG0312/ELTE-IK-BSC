import json
import sys


#Classes

class Connection:
    endPoints : list[str]
    capacity : int
    def __init__(self, p : list[str], c : int) -> None:
        self.endPoints = p
        self.capacity = c

class Route:
    points : list[str]
    relevantConnections : list[Connection]
    def __init__(self, p : list[str]) -> None:
        self.points = p
        self.relevantConnections = []
        for i in range(len(self.points) - 1):
            for j in range(len(connections)):
                if(connections[j].endPoints.__contains__(self.points[i]) and connections[j].endPoints.__contains__(self.points[i+1])):
                    self.relevantConnections.append(connections[j])
                    break
    def CheckRouteCapacity(self, amount : int) -> bool:
        for i in range(len(self.relevantConnections)):
            if(self.relevantConnections[i].capacity < amount):
                return False
        return True

    def ReserveRouteCapacity(self, amount : int) -> int:
        for i in range(len(self.relevantConnections)):
            self.relevantConnections[i].capacity -= amount

class Demand:
    startTime : int
    endTime : int
    startPoint : str
    endPoint : str
    amount : int
    route : Route
    def __init__(self, st : int, et : int, sp : str, ep : str, a : int) -> None:
        self.startTime = st
        self.startPoint = sp
        self.endTime = et
        self.endPoint = ep
        self.amount = a
        self.route = None

#Methods
def StartDemands(currentTime : int) -> None:
    global actions
    for i in range(len(demands)):
        retval : int = StartSingleDemand(currentTime, i)
        if(retval == 0):
            #Ha sikerült elindítani
            actions = actions + 1
            print(str(actions) + ". igény foglalás: " + demands[i].startPoint + "<->" + demands[i].endPoint + " st:" + str(currentTime) + " - sikeres")
        elif(retval == -1):
            #Ha nem sikerült
            actions = actions + 1
            print(str(actions) + ". igény foglalás: " + demands[i].startPoint + "<->" + demands[i].endPoint + " st:" + str(currentTime) + " - sikertelen")

def StartSingleDemand(currentTime : int, index : int) -> int:
    if(demands[index].startTime == currentTime):
        #Keresünk egy utat a Demandhoz
        routeIndex : int = GetRouteIndex(demands[index].startPoint, demands[index].endPoint, demands[index].amount)
        if(routeIndex != -1):
            #Ha találtunk egy utat, akkor lefoglaljuk a kapacitását
            routes[routeIndex].ReserveRouteCapacity(demands[index].amount)
            #Elmentjük, hogy melyik úton sikerült, hogy később ne kelljen kikeresni
            demands[index].route = routes[routeIndex]
            #És visszaadunk egy helyeslő 0-t
            return 0
        else:
            #Ha -1-el tért vissza az útkeresés, akkor nem taléltunk megfelelő utat, mi is adjunk vissza -1et
            return -1
    else:
        #Visszaadok 1-t ha nem kellett elindítani
        return 1

def ReleaseDemands(currentTime : int):
    global actions
    for i in range(len(demands)):
        if(demands[i].endTime == currentTime):
            retval : bool = ReleaseSingleDemand(i)
            if(retval):
                actions = actions + 1
                print(str(actions) + ". igény felszabadítás: " + demands[i].startPoint + "<->" + demands[i].endPoint + " st:" + str(currentTime))

def ReleaseSingleDemand(index : int) -> bool:
    #Amennyiben el van mentve a foglaláshoz egy út
    if(demands[index].route):
        #Akkor felszabadítjuk az úton a kapacitást
        demands[index].route.ReserveRouteCapacity(-demands[index].amount)
        return True
    #Amennyiben nincs elmentve út, úgy nem kell felszabadítani
    return False

def GetRouteIndex(sp : str, ep : str, a : int) -> int:
    for i in range(len(routes)):
        if(routes[i].points.__contains__(sp) and routes[i].points.__contains__(ep)):
            if(routes[i].CheckRouteCapacity(a)):
                #Ha elég a kapacitás akkor visszaadom az indexét
                return i
    #Ha nem találtunk olyan utat ami megfelel, akkor -1
    return -1


#-----------------------------------
#Reading from file
#-----------------------------------
with open(sys.argv[1],'r') as file:
    raw=json.load(file)

#---------------------------
#Variables
#---------------------------
actions : int = 0
duration = raw["simulation"]["duration"]
demands : list[Demand] = []
for i in range(len(raw["simulation"]["demands"])):
    demands.append(Demand(
        raw["simulation"]["demands"][i]["start-time"], 
        raw["simulation"]["demands"][i]["end-time"], 
        raw["simulation"]["demands"][i]["end-points"][0], 
        raw["simulation"]["demands"][i]["end-points"][1], 
        raw["simulation"]["demands"][i]["demand"]))
    
connections : list[Connection] = []
for i in range(len(raw["links"])):
    connections.append(Connection(raw["links"][i]["points"], raw["links"][i]["capacity"]))

routes : list[Route] = []
for i in range(len(raw["possible-circuits"])):
    routes.append(Route(raw["possible-circuits"][i]))

#--------------------------------------
#Actual Program
#--------------------------------------
for i in range(duration):
    StartDemands(i)
    ReleaseDemands(i)