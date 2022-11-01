/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package progtech.capitaly;

/**
 * Lucky type of field, if a player steps on this they get an addition to their currency
 * @author mark2
 */
public class Lucky extends Field {
    /**
    * Constructor
    * @author Molnár Márk
    */
    public Lucky(int value) {
        super("lucky", value);
    }
    
    /**
    * Called when a player stepped on this lucky field, they get an addition to their currency
    * @param p the player who stepped on the field
    * @author Molnár Márk
    */
    @Override
    public void steppedOn(Player p) {
        if (p.getStrategy().equals("careful")) {
            ((Careful)p).budget += Math.round(getFieldValue() / 2);
        }
        
        p.petak += getFieldValue();
    }
}
