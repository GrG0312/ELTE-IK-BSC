package Beadando;

import java.security.InvalidParameterException;
import java.util.Random;

public class Farm implements IListener {

    public static final int DEFAULT_SHEEP_AMOUNT = 10;
    public static final int DEFAULT_DOG_AMOUNT = 5;

    public boolean isSimOver;
    public SimulationObject[][] field; // sadly itt nincs olyan hogy egy csak read-only változatot adok vissza egy getterrel, szóval marad a public
    /*     0          1          2
     * 0 1 2 3 4   5 6 7 8   9 0 1 2 3
     * # # # # # | # # # # | # # # # # 0
     * #         |         |         # 1
     * #   1     |   2     |   3     # 2    0
     * #   0-0   |   1-0   |   2-0   # 3
     * #         |         |         # 4
     * ----------+---------+----------
     * #         | A B C D |         # 5
     * #   4     | E F G H |   5     # 6
     * #   0-1   | I J     |         # 7    1
     * #         |   1-1   |   2-1   # 8
     * ----------+---------+----------
     * #         |         |         # 9 
     * #   6     |   7     |   8     # 10
     * #   0-2   |   1-2   |   2-2   # 11   2
     * #         |         |         # 12
     * # # # # # | # # # # | # # # # # 13
     * 
     */

//#region Constructors
    public Farm(){
        isSimOver = false;
        field = new SimulationObject[14][14];
        GenerateObjects(14,14);
        GenerateWalls(14,14);
        GenerateGate(14,14);
        int innerFieldLength = 14 - 2; // 12
        innerFieldLength = innerFieldLength / 3; // jelen esetben: 4
        GenerateSheep(innerFieldLength, innerFieldLength, DEFAULT_SHEEP_AMOUNT);
        GenerateDog(innerFieldLength, innerFieldLength, DEFAULT_DOG_AMOUNT);
    }
    public Farm(int xLength, int yLength, int sheepAmount, int dogAmount){
        if((xLength - 2) % 3 != 0 || (yLength - 2) % 3 != 0){
            throw new InvalidParameterException("A size parameter is not divisible by 3");
        }
        isSimOver = false;
        field = new SimulationObject[xLength][yLength];
        GenerateObjects(xLength, yLength);
        GenerateWalls(xLength,yLength);
        GenerateGate(xLength,yLength);
        int centerValueX = xLength - 2; // pl 12
        int centerValueY = yLength - 2; // pl 12
        centerValueX = centerValueX / 3; // pl 4
        centerValueY = centerValueY / 3; // pl 4
        GenerateSheep(centerValueX, centerValueY, sheepAmount); // kell majd egy +1 mert igazából 1től indexelünk, nem nullától
        GenerateDog(centerValueX, centerValueY, dogAmount);
    }
//#endregion Contructors

//#region Listen Methods
    public boolean IsOver(){
        return isSimOver;
    }
    public void Listen(){
        isSimOver = true;
        System.out.println("\n" + this.toString());
        System.out.println("A sheep has escaped!");
    }
//#endregion Listen Methods

//#region Generation Methods
    private void GenerateObjects(int xLength, int yLength){
        for(int i = 0; i < xLength; i++){
            for(int j = 0; j < yLength; j++){
                field[i][j] = new EmptyObject();
            }
        }
    }
    private void GenerateWalls(int xLength, int yLength){
        for (int i = 0; i < xLength; i++){
            field[i][0] = new Wall();
            field[i][yLength - 1] = new Wall();
        }
        for(int j = 0; j < yLength; j++){
            field[0][j] = new Wall();
            field[xLength - 1][j] = new Wall();
        }
    }
    private void GenerateGate(int xLength, int yLength){
        Random r = new Random();

        int pos = r.nextInt(xLength - 2) + 1; // 14 - 2 = 12; 0-11 --- +1 ---> 1-12
        field[pos][0] = new Gate();

        pos = r.nextInt(xLength - 2) + 1;
        field[pos][yLength - 1] = new Gate();

        pos = r.nextInt(yLength - 2) + 1;
        field[0][pos] = new Gate();

        pos = r.nextInt(yLength - 2) + 1;
        field[xLength - 1][pos] = new Gate();
    }
    private void GenerateSheep(int centerX, int centerY, int sheepAmount){
        int xindex = centerX + 1; // centerX = 4, xindex = 5
        int yindex = centerY + 1; // same...
        int name = 65;
        for(int i = 0; i < sheepAmount; i++){
            field[xindex][yindex] = new Sheep(((char)name) + "", this, this.field, xindex, yindex); //ganggang
            name++;
            xindex++;
            if(xindex >= ((2*centerX) + 1)){ // amennyiben 2*4+1 = 9-nél nagyobb vagy egyenlő, tehát kifutnánk a középső négyzetből
                yindex++;
                xindex = centerX + 1;
                if(yindex >= ((2*centerY) + 1)){
                    break; // megállítjuk a generálást
                }
            }
        }
    }
    private void GenerateDog(int innerFieldLengthX, int innerFieldLengthY, int dogAmount){ //alap esetben X és Y = 4
        //xIndex, yIndex --> aszerint mozog, hogy hány kilencedbe generáltunk már kutyákat
        int yIndex = 0;
        int xIndex = 0;
        while(dogAmount > 0){
            //Feltételezem nem adnak meg egy akkora számot, hogy több kutyát kelljen egy kilencedbe generálni (dogAmount <= 8)
            if(xIndex < 3){
                //Ha NOT(xindex = 1 AND yindex = 1)
                if(xIndex != 1 || yIndex != 1){ // ilyenkor a középső kilencedbe generálnánk, azt nyem szabad 
                    Random r = new Random();
                    int xPos = r.nextInt(innerFieldLengthX) + 1; // 0-3 --- +1 ---> 1-4
                    int yPos = r.nextInt(innerFieldLengthY) + 1;
                    //System.out.println(dogAmount + ": " + "\n\tRandom pozíció: (" + xPos + " " + yPos + ")");
                    
                    xPos = xPos + (xIndex * innerFieldLengthX);
                    yPos = yPos + (yIndex * innerFieldLengthY);
                    //System.out.println("\tTényleges pozíció: (" + xPos + " " + yPos + ")");

                    field[xPos][yPos] = new Dog(dogAmount, this.field, xPos, yPos, this);
                    dogAmount = dogAmount - 1;
                }
                xIndex = xIndex + 1;
            } else {
                xIndex = 0;
                yIndex = yIndex + 1;
            }
        }
    }
//#endregion Generation Methods

public void StartSimulation(){
    for (SimulationObject[] simulationObjects : field) {
        for (SimulationObject simulationObject : simulationObjects) {
            if(simulationObject instanceof Sheep || simulationObject instanceof Dog){
                simulationObject.start();
                try{
                } catch(IllegalThreadStateException e){
                    e.printStackTrace();
                    System.out.println(simulationObject.getState());
                }
            }
        }
    }
}

@Override public String toString(){
        String retval = "";
        for (int i = 0; i < field.length; i++) {
            for (int j = 0; j < field[0].length; j++) {
                synchronized(field[i][j]){
                    String addition = field[i][j] != null ? (field[i][j]).toString() : " ";
                    retval = retval + addition + " ";
                }
            }
            retval = retval + "\n";
        }
        return retval;
    }
}
