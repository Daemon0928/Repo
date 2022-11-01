/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package progtech.capitaly;

/**
 * Gets thrown when the input file is invalid
 * @author mark2
 */
public class InvalidInputFileException extends Exception {
    public InvalidInputFileException(String errorMessage) {
        super(errorMessage);
    }
}
