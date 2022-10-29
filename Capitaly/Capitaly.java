/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Project/Maven2/JavaApp/src/main/java/${packagePath}/${mainClassName}.java to edit this template
 */

package progtech.capitaly;

import java.io.BufferedReader;
import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.util.Scanner;

/**
 *
 * @author mark2
 */
public class Capitaly {

    public static void main(String[] args) throws IOException, FileNotFoundException, InvalidInputFileException, InterruptedException {
        try {
            BufferedReader reader = new BufferedReader(new InputStreamReader(System.in));
            String fname;
            do  {
                System.out.println("Input a valid file.");
                fname = reader.readLine();
            } while(!read(fname));
     
            Game.simulate();

            } catch(Exception e) {
                System.out.println(e.getMessage());
            }
    }
    
    /**
     * 
     * @param fileName name of the input file
     * @return returns if the file was successfully read or not
     * @throws FileNotFoundException thrown if file was not found
     * @throws InvalidInputFileException thrown if the input file is invalid
     */
    public static boolean read(String fileName) throws FileNotFoundException, InvalidInputFileException {
        try {
            Scanner sc = new Scanner(new BufferedReader(new FileReader(fileName)));
            int n = sc.nextInt();
            while(n > 0) {
                switch(sc.next()) {
                    case "Property" -> Game.board.add(new Property());
                    case "Service" -> Game.board.add(new Service(sc.nextInt()));
                    case "Lucky" -> Game.board.add(new Lucky(sc.nextInt()));
                    default -> throw new InvalidInputFileException("Invalid input file");
                }
                n--;
            }   
            n = sc.nextInt();
            while(n > 0)
            {
                String name = sc.next();
                switch(sc.next()) {
                    case "Greedy" -> Game.players.add(new Greedy(name));
                    case "Tactical" -> Game.players.add(new Tactical(name));
                    case "Careful" -> Game.players.add(new Careful(name));
                }
                n--;
            }
        
            if (sc.hasNext()) {
                Game.diceThrowsAreGiven = true;
                while(sc.hasNext())
                {
                    Game.diceThrows.add(sc.nextInt());
                }
            }
        } catch(Exception e) {
            System.out.println(e.getMessage() + "\n");
            return false;
        }
        return true;
    }
}
