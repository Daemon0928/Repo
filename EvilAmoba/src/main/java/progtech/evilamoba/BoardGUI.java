package progtech.evilamoba;

import javax.swing.JButton;
import javax.swing.JPanel;
import javax.swing.JLabel;
import javax.swing.JOptionPane;
import javax.swing.Timer;

import java.awt.GridLayout;
import java.awt.Dimension;
import java.awt.Font;
import java.awt.Color;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

/**
 * The class that is responsible for The gameboards GUI
 * Every new game a new board is initialized
 * @author Molnár Márk
 */
public class BoardGUI {
    private JButton[][] buttons;
    private Board board;
    private JPanel boardPanel;
    private JLabel timeLabel;
    private JLabel playerTurnLabel;

    private long startTime;
    private Timer timer;

    public BoardGUI(int boardSize) {
        this.board = new Board(boardSize);
        this.boardPanel = new JPanel();
        boardPanel.setLayout(new GridLayout(board.getBoardSize(), board.getBoardSize()));
        buttons = new JButton[boardSize][boardSize];
        for (int i = 0; i < boardSize; i++) {
            for (int j = 0; j < board.getBoardSize(); j++) {
                JButton button = new JButton();
                button.addActionListener(new ButtonListener(i, j));
                button.setPreferredSize(new Dimension(50, 50));
                buttons[i][j] = button;
                buttons[i][j].setFont(new Font("Arial", Font.BOLD, 21));
                boardPanel.add(button);
            }
        }

        timeLabel = new JLabel(" ");
        timeLabel.setFont(new Font("Arial", Font.BOLD, 15));
        timeLabel.setHorizontalAlignment(JLabel.RIGHT);
        playerTurnLabel = new JLabel("Player 1");
        playerTurnLabel.setFont(new Font("Arial", Font.BOLD, 15));
        playerTurnLabel.setHorizontalAlignment(JLabel.CENTER);
        timer = new Timer(10, e -> timeLabel.setText(elapsedTime() + " ms"));
        startTime = System.currentTimeMillis();
        timer.start();
        GameController.initBoardGUI(this);
    }

    public void playerMoved(int x, int y, State state) {
        if(state == State.X) {
            buttons[x][y].setText("X");
            buttons[x][y].setForeground(Color.BLACK);
        } else {
            buttons[x][y].setText("O");
            buttons[x][y].setForeground(Color.RED);
        }
    }

    public void playerTurnLabelUpdate() {
        playerTurnLabel.setText(GameController.getPlayerTurn() == GameController.PlayerTurn.PLAYER1 ? "Player 1" : "Player 2");
    }

    public long elapsedTime() {
        return System.currentTimeMillis() - startTime;
    }
    
    public JPanel getBoardPanel() { return boardPanel; }
    public JLabel getTimeLabel() { return timeLabel; }
    public JLabel getPlayerTurnLabel() { return playerTurnLabel; }

    public void setFieldEmpty(int x, int y) {
        buttons[x][y].setText("");
    }

    public void gameOver(GameController.PlayerTurn winner) {
        endTheGame();
        playerTurnLabel.setText((winner == GameController.PlayerTurn.PLAYER1 
        ? "Player 1" : "Player 2") + " won!");
        JOptionPane.showMessageDialog(boardPanel, (winner == GameController.PlayerTurn.PLAYER1 
        ? "Player 1" : "Player 2") + " won!\nCongratulations!", "Game Over!", JOptionPane.PLAIN_MESSAGE);
    }

    public void gameOverDraw() {
        endTheGame();
        playerTurnLabel.setText("Draw!");
        JOptionPane.showMessageDialog(boardPanel, "Draw. Try again!", "Draw!", JOptionPane.PLAIN_MESSAGE);
    }

    private void endTheGame() {
        timer.stop();
        for (int i = 0; i < board.getBoardSize(); i++) {
            for (int j = 0; j < board.getBoardSize(); j++) {
                buttons[i][j].setEnabled(false);
            }
        }
    }

    class ButtonListener implements ActionListener {

        private int x, y;

        public ButtonListener(int x, int y) {
            this.x = x;
            this.y = y;
        }

        @Override
        public void actionPerformed(ActionEvent e) {
            GameController.playerAction(x, y);
        }
    }
}
