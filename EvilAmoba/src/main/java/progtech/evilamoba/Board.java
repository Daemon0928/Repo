package progtech.evilamoba;

/**
 * A board structure, which contains a set of fields (Boardsize x Boardsize)
 * @author Molnár Márk
 */
public class Board {
    private Field[][] board;
    private final int boardSize;

    public Board(int boardSize) {
        this.boardSize = boardSize;
        board = new Field[boardSize][boardSize];
        for(int i = 0; i < boardSize; i++) {
            for(int j = 0; j < boardSize; j++) {
                board[i][j] = new Field();
            }
        }
        GameController.initBoard(this);
    }

    public Field get(int x, int y) { return board[x][y]; }
    
    public int getBoardSize() { return boardSize; }
}
