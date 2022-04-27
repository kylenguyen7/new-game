
public abstract class BitletState : IState {
    private BitletController _bitlet;
    protected BitletController Bitlet => _bitlet;
    
    public BitletState(BitletController bitlet) {
        _bitlet = bitlet;
    }

    public abstract void Tick();

    public abstract void FixedTick();

    public abstract void OnEnter();

    public abstract void OnExit();
}