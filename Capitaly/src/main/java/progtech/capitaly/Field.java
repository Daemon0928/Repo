/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package progtech.capitaly;

/**
 * A field on the board, abstract class
 * @author Molnár Márk
 */
public abstract class Field {
    private final String fieldType;
    private final int value;
    
    /**
    * constructor
    * @param fieldType what type of field this is (property, lucky, service)
    * @param value if the field is lucky or service type it needs a custom value
    * else it should be the property price constant
    * @author Molnár Márk
    */
    protected Field(String fieldType, int value) {
        this.fieldType = fieldType;
        this.value = value;
    }
    
    /**
    * Abstract method, what the field does when it's stepped on
    * @param p the player who stepped on the field
    * @author Molnár Márk
    */
    public abstract void steppedOn(Player p);
    
    public int getFieldValue() {return value;}
    public String getFieldType() {return fieldType;}
}
