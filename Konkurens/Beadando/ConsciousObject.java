package Beadando;

import java.util.HashMap;

public abstract class ConsciousObject extends SimulationObject {

    protected enum Direction { UP, DOWN, LEFT, RIGHT, UPLEFT, UPRIGHT, DOWNLEFT, DOWNRIGHT }

    protected int myPosX;
    protected int myPosY;
    
    public ConsciousObject(int x, int y){
        this.myPosX = x;
        this.myPosY = y;
    }

    protected int[] ConvertDirectionToVector(Direction dir){
        switch (dir) {
            case UP:
                return new int[]{-1, 0};
            case UPLEFT:
                return new int[]{-1,-1};
            case UPRIGHT:
                return new int[]{-1, 1};
            case LEFT:
                return new int[]{0, -1};
            case RIGHT:
                return new int[]{0, 1};
            case DOWNLEFT:
                return new int[]{1, -1};
            case DOWN:
                return new int[]{1, 0};
            default:
                return new int[]{1, 1};
        }
    }

    protected void PlaceSmallerInHashMap(HashMap<Direction, Integer> hm, Direction dir, Integer value){
        Integer old = hm.get(dir); // integer ha létezik hozzá, null ha nincs key VAGY null értéke van
        if(old == null || old > value){ // ha null (ergo nincs érték) VAGY az érték nagyobb mint a megadott
            hm.put(dir, value);
        }
    }

    protected void FillRemainingDirections(HashMap<Direction, Integer> moveMap){
        for (Direction dir : Direction.values()) {
            if(!moveMap.containsKey(dir)){
                moveMap.put(dir, 2);
            }
        }
    }
}