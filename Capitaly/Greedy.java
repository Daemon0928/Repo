/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package progtech.capitaly;

/**
 * Greedy strategy player, buys everything
 * @author mark2
 */
public class Greedy extends Player {
    /**
    * Constructor
    * @author Molnár Márk
    */
    public Greedy(String name) {
        super("greedy", name);
    }
    
    /**
     * @return if the greedy player is able to buy this always return true
     * @author Molnár Márk
     */
    @Override
    public boolean buys() {
        return canBuy();
    }
}
