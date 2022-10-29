/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package progtech.capitaly;

/**
 * Careful strategy player
 * Has a budget (currency/2) until it goes around, than it's reseted
 * @author mark2
 */
public class Careful extends Player {
    public int budget;
    
    /**
    * Constructor
    * @author Molnár Márk
    */
    public Careful(String name) {
        super("careful", name);
    }
    
    /**
    * Step on the thrown field and call the fields steppedOn method
    * Called every round until the player haven't lost
    * @param diceThrow the dice throw (1-6) preferably
    * @author Molnár Márk
    */
    @Override
    public void step(int diceThrow) {
        if (position + diceThrow <= Game.board.size()) {
            position += diceThrow;
        } else {
            newBudget();
            position = (position + diceThrow) - Game.board.size();
        }
        
        Game.board.get(position-1).steppedOn(this); 
    }
    
    /**
     * @return if the careful player is able to buy, and the buy is in it's budget
     * it return true, otherwise false
     * @author Molnár Márk
     */
    @Override
    public boolean buys() {
        if (((Property)Game.board.get(position-1)).getOwner() == this) {
            if (budget - Game.PROPERTY_HOUSE_PRICE > 0
                    && ((Property)Game.board.get(position-1)).getHouseCount() == 0) {
                budget -= Game.PROPERTY_HOUSE_PRICE;
                return true;
            }
        } else if(((Property)Game.board.get(position-1)).getOwner() == null){
            if (budget > Game.PROPERTY_PRICE) {
                budget-= Game.PROPERTY_PRICE;
                return true;
            }
        }
        
        return false;
    }
        
    /**
    * If this careful player goes around the board it gets a new budget
    * @author Molnár Márk
    */
    private void newBudget() {
         budget = Math.round(petak / 2);
    }
}
