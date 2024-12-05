package Beadando;

import java.util.*;

public class Sheep extends ConsciousObject {

    private Object[][] farmField;
    private IListener listener;

    public Sheep(String name, IListener myFarmObject, SimulationObject[][] fieldParam, int x, int y){
        super(x,y);
        this.setName(name);
        this.listener = myFarmObject;
        this.farmField = fieldParam;
    }

    @Override public void run(){
        while (!listener.IsOver()) {
            try {
                HashMap<Direction, Integer> moveMap = new HashMap<>();
                DetectDogs(moveMap);
                DetectImpassables(moveMap);
                FillRemainingDirections(moveMap);
                int[] moveVector = GetMoveVector2D(moveMap);
                ExecuteMove(moveVector);
                sleep(THREAD_EXECUTION_FREQUENCY);
            } catch (Exception e) {
                e.printStackTrace();
            }
        }
    }

    private void DetectDogs(HashMap<Direction, Integer> moveMap){
        // Leírásban annyi volt, hogy mező: ez most nem tudom hogy a kilencedre akar-e utalni, vagy konkrétan a mellette lévő [i][j]-edik pozícióra
        // Esetleg az oldalamon lévő 3 pozícióra? Annyira rosszul van megfogalmazva
        // Nem tudom ki írta de kérem legközelebb kérjen róla véleményt, és ha valami kérdéses írja át mert ez borzalom

        // Megjegyzés: a feladat szerint az irány nem lehet mindkét dimenzióban nulla, tehát lehet olyan hogy mindkét irányban egy és mozoghatnak átlósan???
        // Újfent borzalmas fogalmazás

        // 0: semmiképpen se menj erre
        // 1: erre mehetsz, de nem ajánlott
        // 2: valid move
        synchronized(farmField[myPosX - 1][myPosY]){ // közvetlen felettem
            if(farmField[myPosX - 1][myPosY] instanceof Dog){
                PlaceSmallerInHashMap(moveMap, Direction.UP, 0);
                PlaceSmallerInHashMap(moveMap, Direction.UPLEFT, 1);
                PlaceSmallerInHashMap(moveMap, Direction.UPRIGHT, 1);
                PlaceSmallerInHashMap(moveMap, Direction.DOWN, 3);
            }
        }
        synchronized(farmField[myPosX - 1][myPosY - 1]){ // felettem balra
            if(farmField[myPosX - 1][myPosY - 1] instanceof Dog){
                PlaceSmallerInHashMap(moveMap, Direction.UPLEFT, 0);
                PlaceSmallerInHashMap(moveMap, Direction.UP, 1);
                PlaceSmallerInHashMap(moveMap, Direction.LEFT, 1);
                PlaceSmallerInHashMap(moveMap, Direction.DOWNRIGHT, 3);
            }
        }
        synchronized(farmField[myPosX - 1][myPosY + 1]){ // felettem jobbra
            if(farmField[myPosX - 1][myPosY + 1] instanceof Dog){
                PlaceSmallerInHashMap(moveMap, Direction.UPRIGHT, 0);
                PlaceSmallerInHashMap(moveMap, Direction.UP, 1);
                PlaceSmallerInHashMap(moveMap, Direction.RIGHT, 1);
                PlaceSmallerInHashMap(moveMap, Direction.DOWNLEFT, 3);
            }
        }
        synchronized(farmField[myPosX + 1][myPosY]){ //közvetlen alattam
            if(farmField[myPosX + 1][myPosY] instanceof Dog){
                PlaceSmallerInHashMap(moveMap, Direction.DOWN, 0);
                PlaceSmallerInHashMap(moveMap, Direction.DOWNLEFT, 1);
                PlaceSmallerInHashMap(moveMap, Direction.DOWNRIGHT, 1);
                PlaceSmallerInHashMap(moveMap, Direction.UP, 3);
            }
        }
        synchronized(farmField[myPosX + 1][myPosY - 1]){ // alattam balra
            if(farmField[myPosX + 1][myPosY - 1] instanceof Dog){
                PlaceSmallerInHashMap(moveMap, Direction.DOWNLEFT, 0);
                PlaceSmallerInHashMap(moveMap, Direction.DOWN, 1);
                PlaceSmallerInHashMap(moveMap, Direction.LEFT, 1);
                PlaceSmallerInHashMap(moveMap, Direction.UPRIGHT, 3);
            }
        }
        synchronized(farmField[myPosX + 1][myPosY + 1]){ // alattam jobbra
            if(farmField[myPosX + 1][myPosY + 1] instanceof Dog){
                PlaceSmallerInHashMap(moveMap, Direction.DOWNRIGHT, 0);
                PlaceSmallerInHashMap(moveMap, Direction.DOWN, 1);
                PlaceSmallerInHashMap(moveMap, Direction.RIGHT, 1);
                PlaceSmallerInHashMap(moveMap, Direction.UPLEFT, 3);
            }
        }
        synchronized(farmField[myPosX][myPosY - 1]){ // közvetlen balra
            if(farmField[myPosX][myPosY - 1] instanceof Dog){
                PlaceSmallerInHashMap(moveMap, Direction.LEFT, 0);
                PlaceSmallerInHashMap(moveMap, Direction.UPLEFT, 1);
                PlaceSmallerInHashMap(moveMap, Direction.DOWNLEFT, 1);
                PlaceSmallerInHashMap(moveMap, Direction.RIGHT, 3);
            }
        }
        synchronized(farmField[myPosX][myPosY + 1]){ // közvetlen jobbra
            if(farmField[myPosX][myPosY + 1] instanceof Dog){
                PlaceSmallerInHashMap(moveMap, Direction.RIGHT, 0);
                PlaceSmallerInHashMap(moveMap, Direction.UPRIGHT, 1);
                PlaceSmallerInHashMap(moveMap, Direction.DOWNRIGHT, 1);
                PlaceSmallerInHashMap(moveMap, Direction.LEFT, 3);
            }
        }
    }

    private void DetectImpassables(HashMap<Direction, Integer> moveMap){
        // 0: semmiképpen se menj erre
        // 1: erre mehetsz, de nem ajánlott
        // 2: valid move
        synchronized(farmField[myPosX - 1][myPosY]){ // közvetlen felettem
            if(farmField[myPosX - 1][myPosY] instanceof Wall || farmField[myPosX - 1][myPosY] instanceof Sheep){
                PlaceSmallerInHashMap(moveMap, Direction.UP, 0);
            }
        }
        synchronized(farmField[myPosX - 1][myPosY - 1]){ // felettem balra
            if(farmField[myPosX - 1][myPosY - 1] instanceof Wall || farmField[myPosX - 1][myPosY - 1] instanceof Sheep){
                PlaceSmallerInHashMap(moveMap, Direction.UPLEFT, 0);
            }
        }
        synchronized(farmField[myPosX - 1][myPosY + 1]){ // felettem jobbra
            if(farmField[myPosX - 1][myPosY + 1] instanceof Wall || farmField[myPosX - 1][myPosY + 1] instanceof Sheep){
                PlaceSmallerInHashMap(moveMap, Direction.UPRIGHT, 0);
            }
        }
        synchronized(farmField[myPosX + 1][myPosY]){ //közvetlen alattam
            if(farmField[myPosX + 1][myPosY] instanceof Wall || farmField[myPosX + 1][myPosY] instanceof Sheep){
                PlaceSmallerInHashMap(moveMap, Direction.DOWN, 0);
            }
        }
        synchronized(farmField[myPosX + 1][myPosY - 1]){ // alattam balra
            if(farmField[myPosX + 1][myPosY - 1] instanceof Wall || farmField[myPosX + 1][myPosY - 1] instanceof Sheep){
                PlaceSmallerInHashMap(moveMap, Direction.DOWNLEFT, 0);
            }
        }
        synchronized(farmField[myPosX + 1][myPosY + 1]){ // alattam jobbra
            if(farmField[myPosX + 1][myPosY + 1] instanceof Wall || farmField[myPosX + 1][myPosY + 1] instanceof Sheep){
                PlaceSmallerInHashMap(moveMap, Direction.DOWNRIGHT, 0);
            }
        }
        synchronized(farmField[myPosX][myPosY - 1]){ // közvetlen balra
            if(farmField[myPosX][myPosY - 1] instanceof Wall || farmField[myPosX][myPosY - 1] instanceof Sheep){
                PlaceSmallerInHashMap(moveMap, Direction.LEFT, 0);
            }
        }
        synchronized(farmField[myPosX][myPosY + 1]){ // közvetlen jobbra
            if(farmField[myPosX][myPosY + 1] instanceof Wall || farmField[myPosX][myPosY + 1] instanceof Sheep){
                PlaceSmallerInHashMap(moveMap, Direction.RIGHT, 0);
            }
        }
    }

    private int[] GetMoveVector2D(HashMap<Direction, Integer> moveMap){
        int mostPromising = 0;
        for (Direction dir : Direction.values()) { // megnézem, mi a legnagyobb érték ami útvonalnak szerepel (0: semmiképp se jó, 1: nem rossz de lehetne jobb, 2: király)
            Integer value = moveMap.get(dir);
            if(value != null && value > mostPromising){
                mostPromising = value;
            }
        }
        if(mostPromising != 0){ // ha nem találtunk egyetlen minimálisan jó útvonalat sem, akkor nem tudunk mozogni, better luck next time
            List<Direction> possibleDirections = new ArrayList<>();
            for (Direction dir : Direction.values()) { // kiválogatom azokat az útvonalakat, amik értéke a maximummal megegyezik
                if(moveMap.get(dir) == mostPromising){
                    possibleDirections.add(dir);
                }
            }

            Random r = new Random();
            int posDirLen = possibleDirections.size();
            int selectedDirIndex = r.nextInt(posDirLen);
            int[] vector2D = ConvertDirectionToVector(possibleDirections.get(selectedDirIndex));
            return vector2D;
        }
        return new int[]{0, 0}; // ez akkor van ha nem tudunk semerre mozogni
    }

    private void ExecuteMove(int[] vector){
        synchronized(farmField[myPosX][myPosY]){
            synchronized(farmField[myPosX + vector[0]][myPosY + vector[1]]){
                if(farmField[myPosX + vector[0]][myPosY + vector[1]] instanceof EmptyObject){
                    farmField[myPosX + vector[0]][myPosY + vector[1]] = this;
                    farmField[myPosX][myPosY] = new EmptyObject();
                    myPosX = myPosX + vector[0];
                    myPosY = myPosY + vector[1];
                } else if(farmField[myPosX + vector[0]][myPosY + vector[1]] instanceof Gate){
                    farmField[myPosX + vector[0]][myPosY + vector[1]] = this;
                    farmField[myPosX][myPosY] = new EmptyObject();
                    myPosX = myPosX + vector[0];
                    myPosY = myPosY + vector[1];
                    listener.Listen(); // ezzel jelzem hogy ez a bárány kijutott
                }
            }
        }
    }

    @Override public String toString(){
        return this.getName();
    }
}