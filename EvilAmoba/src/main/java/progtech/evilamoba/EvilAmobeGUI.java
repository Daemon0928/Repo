package progtech.evilamoba;

import java.awt.BorderLayout;
import java.awt.event.ActionEvent;
import javax.swing.JFrame;
import javax.swing.JMenu;
import javax.swing.JMenuBar;
import javax.swing.JMenuItem;

/**
 * The class that creates the game window and is responsible for the menu items.
 * @author Molnár Márk
 */
public class EvilAmobeGUI {
    
    private JFrame frame;
    private BoardGUI boardGUI;

    private final int STARTING_BOARDSIZE = 10;
    private final int[] boardSizes = new int[]{6,10,14};

    public EvilAmobeGUI() {
        frame = new JFrame("Evil Amobe");
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        
        boardGUI = new BoardGUI(STARTING_BOARDSIZE);
        frame.getContentPane().add(boardGUI.getBoardPanel(), BorderLayout.CENTER);
        frame.getContentPane().add(boardGUI.getTimeLabel(), BorderLayout.SOUTH);
        frame.getContentPane().add(boardGUI.getPlayerTurnLabel(), BorderLayout.NORTH);
        
        JMenuBar menuBar = new JMenuBar();
        frame.setJMenuBar(menuBar);
        JMenu gameMenu = new JMenu("Game");
        menuBar.add(gameMenu);
        JMenu newMenu = new JMenu("New");
        gameMenu.add(newMenu);
        for(int bSize : boardSizes) {
            JMenuItem sizeMenuItem = new JMenuItem(bSize + "x" + bSize);
            newMenu.add(sizeMenuItem);
            sizeMenuItem.addActionListener((ActionEvent e) -> {
                frame.getContentPane().remove(boardGUI.getBoardPanel());
                frame.getContentPane().remove(boardGUI.getTimeLabel());
                frame.getContentPane().remove(boardGUI.getPlayerTurnLabel());
                boardGUI = new BoardGUI(bSize);
                frame.getContentPane().add(boardGUI.getBoardPanel(), BorderLayout.CENTER);
                frame.getContentPane().add(boardGUI.getTimeLabel(), BorderLayout.SOUTH);
                frame.getContentPane().add(boardGUI.getPlayerTurnLabel(), BorderLayout.NORTH);
                frame.pack();
                GameController.newGame();
            });
        }
        JMenuItem exitMenuItem = new JMenuItem("Exit");
        gameMenu.add(exitMenuItem);
        exitMenuItem.addActionListener((ActionEvent e) -> {
            System.exit(0);
        });
        
        frame.pack();
        frame.setVisible(true);
    }
}
