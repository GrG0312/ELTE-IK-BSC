import json
import argparse

class Demand:
    def __init__(self,start_point,end_point,demand,start_time,end_time,used_links):
        self.start_point=start_point
        self.end_point=end_point
        self.demand=demand
        self.start_time=start_time
        self.end_time=end_time
        self.used_links=used_links
        self.free=True

parser=argparse.ArgumentParser()
parser.add_argument('filename', type=str)
args=parser.parse_args()

with open(args.filename,'r') as file:
    cs=json.load(file)

free_links=cs["links"]
unavailable_links=[]
temp_unavailable_links=[]
circuits=cs["possible-circuits"]
duration=cs["simulation"]["duration"]
demands=cs["simulation"]["demands"]
Demands_list=[]
counter=0
message=0

for i in range(len(demands)): 
    start_point=demands[i]["end-points"][0]
    end_point=demands[i]["end-points"][1]
    demand=demands[i]["demand"]
    start_time=demands[i]["start-time"]
    end_time=demands[i]["end-time"]
    used_links=[]
    demand=Demand(start_point, end_point, demand, start_time, end_time, used_links)
    Demands_list.append(demand)


for i in range(duration):

    for p in range(len(Demands_list)): 
        if(Demands_list[p].end_time==i and not Demands_list[p].free):
            print(str(message+1)+". igény felszabadítás: "+Demands_list[p].start_point+"<->"+Demands_list[p].end_point+" st:"+str(i))
            message+=1
            for v in range(len(Demands_list[p].used_links)):
                free_links.append(Demands_list[p].used_links[v])
                unavailable_links.remove(Demands_list[p].used_links[v])
                
    
    if counter<len(demands): 
       if Demands_list[counter].start_time==i:
            found_full_link=False
            for j in range(len(circuits)): 
                if circuits[j][0]==Demands_list[counter].start_point and circuits[j][len(circuits[j])-1]==Demands_list[counter].end_point: #jo kezdo es vegpont
                    found_link=True
                    temp_unavailable_links = []
                    for k in range(len(circuits[j])-1): 
                        partlink_free=False
                        for l in range(len(free_links)): 
                            if circuits[j][k]==free_links[l]["points"][0] and circuits[j][k+1]==free_links[l]["points"][1] and Demands_list[counter].demand<=free_links[l]["capacity"]:
                                temp_unavailable_links.append(free_links[l]) 
                                partlink_free=True
                                break 
                        if not partlink_free:
                            temp_unavailable_links.clear() 
                            found_link=False
                            break 
                    if found_link:
                        found_full_link=True
                        break
            if found_full_link:
                for m in range(len(temp_unavailable_links)):
                    free_links.remove(temp_unavailable_links[m])
                    unavailable_links.append(temp_unavailable_links[m])
                    Demands_list[counter].used_links=temp_unavailable_links
                    Demands_list[counter].free=False
                print(str(message+1)+". igény foglalás: "+Demands_list[counter].start_point+"<->"+Demands_list[counter].end_point+" st:"+str(i)+" – sikeres")
                message+=1

            else:
                print(str(message+1)+". igény foglalás: "+Demands_list[counter].start_point+"<->"+Demands_list[counter].end_point+" st:"+str(i)+" – sikertelen")
                message+=1
            counter+=1


#1. igény foglalás: A<->C st:1 – sikeres
#2. igény foglalás: B<->C st:2 – sikeres
#3. igény felszabadítás: A<->C st:5
#4. igény foglalás: D<->C st:6 – sikeres
#5. igény foglalás: A<->C st:7 – sikertelen
#6. igény felszabadítás: B<->C st:10
#7. igény felszabadítás: D<->C st:10