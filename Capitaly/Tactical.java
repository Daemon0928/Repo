/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package progtech.capitaly;

/**
 * Tactical strategy player, skips every other buy
 * @author mark2
 */
public class Tactical extends Player {
    public boolean skipNextBuy;
    /**
    * Constructor
    * @author Molnár Márk
    */
    public Tactical(String name) {
        super("tactical", name);
        skipNextBuy = false;
    }
    
    /**
     * The tactical players skips every other buy.
     * @return if the player can't buy the property or house this return false,
     * else it returns false on every second time the player could buy.
     * @author Molnár Márk
     */
    @Override
    public boolean buys() {
        if (!canBuy()) return false;
        skipNextBuy = !skipNextBuy;
        return !skipNextBuy;
    }
}

