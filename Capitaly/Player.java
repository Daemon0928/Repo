/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package progtech.capitaly;

import java.util.ArrayList;

/**
 * There are three types of players; Greedy, Careful, Tactical
 * @author mark2
 */
public class Player {
    public int petak;
    public ArrayList<Property> properties;
    private final String name;
    private final String strategy;
    protected int position;
    
    /**
    * Constructor
    * @author Molnár Márk
    */
    protected Player(String strategy, String name) {
        this.name = name;
        this.strategy = strategy;
        petak = Game.STARTING_PETAK;
        position = 0;
        properties = new ArrayList<>();
    }
    
    /**
    * Step on the thrown field and call the fields steppedOn method
    * Called every round until the player haven't lost
    * @param diceThrow the dice throw (1-6) preferably
    * @author Molnár Márk
    */
    public void step(int diceThrow) {
        if ((position) + diceThrow <= Game.board.size()) {
            position += diceThrow;
        } else {
            position = (position + diceThrow) - Game.board.size();
        }
        
        Game.board.get(position-1).steppedOn(this); 
    }
    
    /**
     * @return returns true if the player can make the buy, and if the players
     * strategy is to buy the property or the house on the turn that this is called
     * @author Molnár Márk
     */
    public boolean buys() {return false;}
    
    /**
     * @return returns true if the player is able to make the buy.
     * @author Molnár Márk
     */
    protected boolean canBuy() {
        if (((Property)Game.board.get(position-1)).getOwner() == this) {
            if(((Property)Game.board.get(position-1)).getHouseCount() == 1
                    || petak < Game.PROPERTY_HOUSE_PRICE){
                return false;
            }
        } else if(((Property)Game.board.get(position-1)).getOwner() == null 
                && petak < Game.PROPERTY_PRICE) {
            return false;
        }
        return true;
    }
    
    public String getName() {return name;}
    public String getStrategy() {return strategy;}
}
