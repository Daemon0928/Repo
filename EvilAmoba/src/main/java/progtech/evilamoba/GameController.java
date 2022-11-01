package progtech.evilamoba;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.Random;


/**
 * Game controller singleton class, the game's main logic is here.
 * @author Molnár Márk
 */
public class GameController {

    private static final GameController INSTANCE = new GameController();
    private GameController() {};

    public static GameController getInstance() { return INSTANCE; }

    private static PlayerTurn playerTurn = PlayerTurn.PLAYER1;
    private static Board board;
    private static BoardGUI boardGUI;
    private static boolean gameOverBool = false; 

    public static void initBoard(Board board) { GameController.board = board; }
    public static void initBoardGUI(BoardGUI boardGUI) { GameController.boardGUI = boardGUI; }

    public static PlayerTurn getPlayerTurn() { return playerTurn; }
    public static Board getBoard() { return board; }
    public static boolean isGameOver() { return gameOverBool; }

    public static void startGame() {
        new EvilAmobeGUI();
    }
    
    public static void newGame() {
        gameOverBool = false;
        playerTurn = PlayerTurn.PLAYER1;
    }

    public static void playerAction(int x, int y) {
        if(board.get(x,y).getState() == State.EMPTY)
        {
            if (playerTurn == PlayerTurn.PLAYER1) {
                board.get(x,y).setState(State.X);
                boardGUI.playerMoved(x, y, State.X);
            } else {
                board.get(x,y).setState(State.O);
                boardGUI.playerMoved(x, y, State.O);
            }

            int consec = checkForConsecutive(x, y, board.get(x,y).getState());
            
            if(consec == 3) {
                deleteFromRandomPosition(1, board.get(x,y).getState());
            } else if(consec == 4) {
                deleteFromRandomPosition(2, board.get(x, y).getState());
            } else if(consec > 4) {
                gameOver(playerTurn);
            }
            
            if(!gameOverBool) {
                if(!isDraw()) {
                    if(playerTurn == PlayerTurn.PLAYER1) {
                        playerTurn = PlayerTurn.PLAYER2;
                    } else {
                        playerTurn = PlayerTurn.PLAYER1;
                    }
                    boardGUI.playerTurnLabelUpdate();
                } else {
                    gameOverDraw();
                }
            }
        }
    }

    private static boolean isDraw() {
        boolean b = true;
        int i = 0;
        while(b && i < board.getBoardSize() * board.getBoardSize()) {
            b = board.get(i / board.getBoardSize(), i % board.getBoardSize()).getState() != State.EMPTY;
            i++;
        }
        return b;
    }

    private static void deleteFromRandomPosition(int n, State state) {
        Random r = new Random();
        ArrayList<Integer> indexes = new ArrayList<Integer>();
        for(int i = 0; i < board.getBoardSize(); i++) {
            for(int j = 0; j < board.getBoardSize(); j++) {
                if(board.get(i,j).getState() == state) {
                    indexes.add(i*board.getBoardSize() + j);
                }
            }
        }
        while(n > 0) {
            int ind = indexes.get(r.nextInt(indexes.size()));
            board.get(ind / board.getBoardSize(), ind % board.getBoardSize()).setState(State.EMPTY);
            boardGUI.setFieldEmpty(ind / board.getBoardSize(), ind % board.getBoardSize());
            n--;
        }
    }

    private static int checkForConsecutive(int x, int y, State state)
    {
        Integer[] consec = new Integer[] {1,1,1,1};
        int current = x-1;
        boolean checking = true;

        //vertical checking
        while(current >= 0 && checking)
        {
            if(board.get(current,y).getState() == state) {
                consec[0]++;
            } else {
                checking = false;
            }
            current--;
        }
        checking = true;
        current = x+1; 

        while(current < board.getBoardSize() && checking)
        {
            if(board.get(current,y).getState() == state) {
                consec[0]++;
            } else {
                checking = false;
            }
            current++;
        }

        //horizontal checking
        checking = true;
        current = y-1;

        while(current >= 0 && checking)
        {
            if(board.get(x,current).getState() == state) {
                consec[1]++;
            } else {
                checking = false;
            }
            current--;
        }
        checking = true;
        current = y+1; 

        while(current < board.getBoardSize() && checking)
        {
            if(board.get(x,current).getState() == state) {
                consec[1]++;
            } else {
                    checking = false;
            }
            current++;
        }

        //cross checking
        checking = true;
        int currentX = x-1;
        int currentY = y+1;

        while(currentX >= 0 && currentY < board.getBoardSize() && checking) {
            if(board.get(currentX,currentY).getState() == state) {
                consec[2]++;
            } else {
                checking = false;
            }
            currentX--;
            currentY++;
        }
        checking = true;
        currentX = x+1;
        currentY = y-1;

        while(currentX < board.getBoardSize() && currentY >= 0 && checking) {
            if(board.get(currentX,currentY).getState() == state) {
                consec[2]++;
            } else {
                checking = false;
            }
            currentX++;
            currentY--;
        }

        //cross negative checking
        checking = true;
        currentX = x-1;
        currentY = y-1;

        while(currentX >= 0 && currentY >= 0 && checking) {
            if(board.get(currentX,currentY).getState() == state) {
                consec[3]++;
            } else {
                checking = false;
            }
            currentX--;
            currentY--;
        }
        checking = true;
        currentX = x+1;
        currentY = y+1;

        while(currentX < board.getBoardSize() && currentY < board.getBoardSize() && checking) {
            if(board.get(currentX,currentY).getState() == state) {
                consec[3]++;
            } else {
                checking = false;
            }
            currentX++;
            currentY++;
        }

        return Collections.max(Arrays.asList(consec));
    }

    private static void gameOver(PlayerTurn winner) {
        gameOverBool = true;
        boardGUI.gameOver(winner);
    }

    private static void gameOverDraw() {
        gameOverBool = true;
        boardGUI.gameOverDraw();
    }


    public static enum PlayerTurn{PLAYER1, PLAYER2};
}