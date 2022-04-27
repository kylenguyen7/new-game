

using UnityEngine;

public abstract class FaunaState : IState {
    protected readonly RoamingFaunaController _fauna;
    protected Transform Target {
        get => _fauna.Target;
        set => _fauna.Target = value;
    }

    protected FaunaState(RoamingFaunaController fauna) {
        _fauna = fauna;
    }

    public abstract void Tick();
    public abstract void FixedTick();
    public abstract void OnEnter();
    public abstract void OnExit();
}