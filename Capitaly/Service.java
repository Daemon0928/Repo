/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package progtech.capitaly;

/**
 * Service field
 * If a player steps on it, than they have to pay
 * @author mark2
 */
public class Service extends Field{
    /**
    * Constructor
    * @author Molnár Márk
    */
    public Service(int value) {
        super("-service", value);
    }
    /**
    * Called when a player stepped on this service field, they have to pay a fee
    * @param p the player who stepped on the field
    * @author Molnár Márk
    */
    @Override
    public void steppedOn(Player p) {
        if (p.getStrategy().equals("careful")) {
            ((Careful)p).budget -= getFieldValue();
        }
        
        p.petak -= getFieldValue();
    }
}
