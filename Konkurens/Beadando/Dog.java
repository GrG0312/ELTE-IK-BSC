package Beadando;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Random;

public class Dog extends ConsciousObject {
    
    private SimulationObject[][] farmField;
    private IListener listener;

    public Dog(int name, SimulationObject[][] farmField, int x, int y, IListener listener){
        super(x,y);
        this.setName("" + name); /// gigachad
        this.farmField = farmField;
        this.listener = listener;
    }

    @Override public void run() {
        while(!listener.IsOver()){
            try {
                HashMap<Direction, Integer> moveMap = new HashMap<>();
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

    private void DetectImpassables(HashMap<Direction, Integer> moveMap){
        // 0: semmiképpen se menj erre
        // 2: valid move
        synchronized(farmField[myPosX - 1][myPosY]){ // közvetlen felettem
            if(farmField[myPosX - 1][myPosY] instanceof Wall 
            || farmField[myPosX - 1][myPosY] instanceof Sheep 
            || farmField[myPosX - 1][myPosY] instanceof Dog
            || DetermineIfPosInCenter(myPosX - 1, myPosY)
            ){
                PlaceSmallerInHashMap(moveMap, Direction.UP, 0);
            }
        }
        synchronized(farmField[myPosX - 1][myPosY - 1]){ // felettem balra
            if(farmField[myPosX - 1][myPosY - 1] instanceof Wall 
            || farmField[myPosX - 1][myPosY - 1] instanceof Sheep 
            || farmField[myPosX - 1][myPosY - 1] instanceof Dog
            || DetermineIfPosInCenter(myPosX - 1, myPosY - 1)
            ){
                PlaceSmallerInHashMap(moveMap, Direction.UPLEFT, 0);
            }
        }
        synchronized(farmField[myPosX - 1][myPosY + 1]){ // felettem jobbra
            if(farmField[myPosX - 1][myPosY + 1] instanceof Wall 
            || farmField[myPosX - 1][myPosY + 1] instanceof Sheep 
            || farmField[myPosX - 1][myPosY + 1] instanceof Dog
            || DetermineIfPosInCenter(myPosX - 1, myPosY + 1)
            ){
                PlaceSmallerInHashMap(moveMap, Direction.UPRIGHT, 0);
            }
        }
        synchronized(farmField[myPosX + 1][myPosY]){ //közvetlen alattam
            if(farmField[myPosX + 1][myPosY] instanceof Wall 
            || farmField[myPosX + 1][myPosY] instanceof Sheep 
            || farmField[myPosX + 1][myPosY] instanceof Dog
            || DetermineIfPosInCenter(myPosX + 1, myPosY)
            ){
                PlaceSmallerInHashMap(moveMap, Direction.DOWN, 0);
            }
        }
        synchronized(farmField[myPosX + 1][myPosY - 1]){ // alattam balra
            if(farmField[myPosX + 1][myPosY - 1] instanceof Wall 
            || farmField[myPosX + 1][myPosY - 1] instanceof Sheep 
            || farmField[myPosX + 1][myPosY - 1] instanceof Dog
            || DetermineIfPosInCenter(myPosX + 1, myPosY - 1)
            ){
                PlaceSmallerInHashMap(moveMap, Direction.DOWNLEFT, 0);
            }
        }
        synchronized(farmField[myPosX + 1][myPosY + 1]){ // alattam jobbra
            if(farmField[myPosX + 1][myPosY + 1] instanceof Wall 
            || farmField[myPosX + 1][myPosY + 1] instanceof Sheep 
            || farmField[myPosX + 1][myPosY + 1] instanceof Dog
            || DetermineIfPosInCenter(myPosX + 1, myPosY + 1)
            ){
                PlaceSmallerInHashMap(moveMap, Direction.DOWNRIGHT, 0);
            }
        }
        synchronized(farmField[myPosX][myPosY - 1]){ // közvetlen balra
            if(farmField[myPosX][myPosY - 1] instanceof Wall 
            || farmField[myPosX][myPosY - 1] instanceof Sheep 
            || farmField[myPosX][myPosY - 1] instanceof Dog
            || DetermineIfPosInCenter(myPosX, myPosY - 1)
            ){
                PlaceSmallerInHashMap(moveMap, Direction.LEFT, 0);
            }
        }
        synchronized(farmField[myPosX][myPosY + 1]){ // közvetlen jobbra
            if(farmField[myPosX][myPosY + 1] instanceof Wall 
            || farmField[myPosX][myPosY + 1] instanceof Sheep 
            || farmField[myPosX][myPosY + 1] instanceof Dog
            || DetermineIfPosInCenter(myPosX, myPosY + 1)
            ){
                PlaceSmallerInHashMap(moveMap, Direction.RIGHT, 0);
            }
        }
    }

    private boolean DetermineIfPosInCenter(int xPos, int yPos){
        int centerValueX = ((farmField.length - 2) / 3); // 14 - 2 = 12 / 3 = 4
        int centerValueY = ((farmField[0].length - 2) / 3); // 4
        if(xPos > centerValueX && xPos <= 2 * centerValueX){
            if(yPos > centerValueY && yPos <= 2 * centerValueY){
                return true;
            }
        }
        return false;
    }

    private int[] GetMoveVector2D(HashMap<Direction, Integer> moveMap){
        if(moveMap.containsValue(2)){ // ha van legalább egy irány, amerre tudunk mozogni (itt nincs 1-es érték, itt vagy 2 vagy 0 van)
            List<Direction> possibleDirections = new ArrayList<>();
            for (Direction dir : Direction.values()) { // kiválogatom azokat az útvonalakat, amik értéke 2
                if(moveMap.get(dir) == 2){
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
                }
            }
        }
    }

    @Override public String toString(){
        return this.getName();
    }
}
