/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package progtech.capitaly;

/**
 * Property tpye of field
 * players can buy this field or if an other player already bought
 * it, than they have to pay for the owner
 * A house can also be bought so the price of the service which other
 * players have to pay will be higher
 * @author mark2
 */
public class Property extends Field {
    private int houseCount;
    private Player owner;
    
    /**
    * Constructor
    * @author Molnár Márk
    */
    public Property() {
        super("property", Game.PROPERTY_PRICE);
        houseCount = 0;
        owner = null;
    }
    
    /**
    * Called when a player stepped on this Property field,
    * If the player is the owner, than they might buy a house on the property to
    * increase it's price
    * If the no player owns the property, than the player might buy the property
    * If another player is the owner of the property than the player who stepped on it has to pay to them.
    * @param p the player who stepped on the field
    * @author Molnár Márk
    */
    @Override
    public void steppedOn(Player p) {
        if (owner == null) {
            if (p.buys()) {
                buyProperty(p);
            }
        } else if(owner == p) {
            if (p.buys()) {
                buyHouse(p);
            }
        } else {
            p.petak -= priceIfStepped();
            owner.petak += priceIfStepped();
        }
    }
    
    /**
    * return the price of the property which other players have to pay
    * @return the price of the property if stepped on by a player which is not the owner
    * @author Molnár Márk
    */
    private int priceIfStepped() {
        return houseCount == 0 ? 
                Game.PROPERTY_SERVICEPRICE_WOHOUSE : Game.PROPERTY_SERVICEPRICE_WHOUSE;
    }
    
    /**
    * The owner can buy a house on the property so the other players pay more
    * @param p the owner who buys the house on the property to increase it's value
    * @author Molnár Márk
    */
    private void buyHouse(Player p) {
        p.petak -= Game.PROPERTY_HOUSE_PRICE;
        houseCount++;
    }
    
    /**
    * If no one is the owner of this property, then it can be bought by the player
    * who stepped on it
    * @param p the player who buys the property
    * @author Molnár Márk
    */
    private void buyProperty(Player p) {
        p.petak -= getFieldValue();
        setPropertyOwner(p);
    }
    
    
    public Player getOwner() {return owner;}
    public int getHouseCount() {return houseCount;}
    public void setPropertyOwner(Player p) {owner = p;}
    public void destroyHouses() {houseCount = 0;}
}
