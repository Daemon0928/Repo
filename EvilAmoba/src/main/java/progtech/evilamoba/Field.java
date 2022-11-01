package progtech.evilamoba;

/**
 *
 * @author Molnár Márk
 */

public class Field {
    private State state;

    public Field() {
        state = State.EMPTY;
    }

    public State getState() { return state; }
    public void setState(State state) { this.state = state; }
}

enum State {
    X, O, EMPTY
}
