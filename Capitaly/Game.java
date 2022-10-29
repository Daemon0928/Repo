/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package progtech.capitaly;

import java.util.ArrayList;
import java.util.Random;
import java.util.concurrent.TimeUnit;

/**
 * The game controller class which only can have one instance
 * @author Molnár Márk
 */
public final class Game {
    public static final int STARTING_PETAK = 10000;
    public static final int PROPERTY_PRICE = 1000;
    public static final int PROPERTY_HOUSE_PRICE = 4000;
    public static final int PROPERTY_SERVICEPRICE_WHOUSE = 2000;
    public static final int PROPERTY_SERVICEPRICE_WOHOUSE = 500;
    public static boolean diceThrowsAreGiven = false; 
    private static final Random r = new Random();
    private static int round = 0;
    private static final Game INSTANCE = null;
    public static ArrayList<Player> players = new ArrayList<>();
    private static ArrayList<Player> losers = new ArrayList<>();
    public static ArrayList<Integer> diceThrows = new ArrayList<>();
    public static ArrayList<Field> board = new ArrayList<>();
    
    private Game() {}
    
    public static Game getInstance() {
        return INSTANCE;
    }
    
    /**
    * Simulation of the game to a certain round
    * @param toRound what round should the simulation stop at and show the game state
    * @author Molnár Márk
    */
    public static void simulate(int toRound) {
        for(int i = 0; i < toRound; i++) {
            round();
        }
        System.out.println(gameState());
    }
    
    /**
    * Simulation of the game until there is a winner
    * @author Molnár Márk
    */
    public static void simulate() throws InterruptedException {
        try {
            while(players.size() > 1) {
                round();
                System.out.println(gameState());
                TimeUnit.MILLISECONDS.sleep(1000);
            }
        } catch(Exception e) {
            System.out.println("Simulation exception: " + e.toString());
        }
    }
    
    /**
    * Simulation of a round
    * If dice throws are not given rolls for each player and than each player step is invoked
    * Calls checkForLosers()
    * @author Molnár Márk
    */
    
    private static void round() {
        round++;
        for(Player p : players) {
            if (diceThrowsAreGiven && !diceThrows.isEmpty()) {
                p.step(diceThrows.get(0));
                diceThrows.remove(0);
            } else {
                p.step(r.nextInt(1,7));
            }
        }
        checkForLosers();
    }
    
    /**
    * Checks if there is any player that has lower than 0 currency
    * If it finds any, those players get put in the losers array
    * and their properties are taken away
    * @author Molnár Márk
    */
    private static void checkForLosers() {
        ArrayList<Player> playersToRemove = new ArrayList<>();
        for(Player p : players) {
            if (p.petak < 0) {
                playersToRemove.add(p);
            }
        }
        for(Player p : playersToRemove) {
            losers.add(p);
            players.remove(p);
            for(Property prop : p.properties) {
                prop.setPropertyOwner(null);
                prop.destroyHouses();
            }
        }
    }
    
    /**
    * Returns the gamestate in string format
    * @return The current gamestate which contain the players names,
    * strategies, amount of currencies, properties, and house counts,
    * and also the losers
    * @author Molnár Márk
    */
    public static String gameState() {
        String gameStateStr = "";
        gameStateStr += "\n\nGame state at round: " + round + "\n" + "Players who lost already: \n";
        for(Player p : losers) {
            gameStateStr += p.getName() + " ";
        }
        
        gameStateStr += "\nPlayers:\n";
        
        for(Player p : players) {
            gameStateStr += "\t" + p.getName() + " (" + p.getStrategy() + ")" + "\nProperties: ";
            for(int i = 0; i < Game.board.size(); i++) {
                if(Game.board.get(i).getFieldType().equals("property")) {
                    if(((Property)Game.board.get(i)).getOwner() == p) {
                        gameStateStr += (i+1) + " ";
                    }
                }
            }
            gameStateStr += "\nPetak: " + p.petak + "\n";
        }
        
        return gameStateStr;
    }
            
    public static int getRound() {return round;}
}
