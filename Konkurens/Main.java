import Beadando.Farm;
import Beadando.SimulationObject;

public class Main {
    public static void main(String[] args) {
        System.out.println("\033[H\033[2J");
        Farm farm1 = new Farm(14, 14,10, 8);
        System.out.println(farm1.toString());

        Thread displayThread = new Thread(){
            @Override public void run(){
                farm1.StartSimulation();
                while (!farm1.isSimOver) {
                    try{
                        //System.out.println("\033[H\033[2J");
                        //System.out.println("\u001B[0;0H");
                        System.out.print(farm1.toString());
                        Thread.sleep(SimulationObject.THREAD_EXECUTION_FREQUENCY);
                    } catch(Exception e) {
                        e.printStackTrace();
                    }
                }
            }
        };

        displayThread.start();
    }
}
