using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public class StateMachine {
    private IState _currentState;
    
    // Dict: state -> transitions out of that state
    private Dictionary<Type, List<Transition>> _allTransitions = new Dictionary<Type, List<Transition>>();
    
    // Transitions out of current state
    private List<Transition> _currentTransitions = new List<Transition>();
    
    // Transitions out of any state
    private List<Transition> _anyTransitions = new List<Transition>();

    private static List<Transition> EmptyTransitions = new List<Transition>(0);

    /**
     * Initialize the state machine by setting its initial state.
     * If you have anyTransitions, you can skip this init. But the state machine
     * won't do anything until one of the anyTransition conditions is met.
     */
    public void Init(IState initialState) {
        SetState(initialState);
    }

    public void Tick() {
        // Check if transitions happened
        Transition transition = getTransition();
        if (transition != null) {
            SetState(transition.To);
        }
        
        // Tick current state
        _currentState.Tick();
    }
    
    public void FixedTick() {
        _currentState.FixedTick();
    }

    private Transition getTransition() {
        // Any transitions are checked before state-specific transitions
        foreach (Transition t in _anyTransitions) {
            if (t.Condition()) {
                return t;
            }
        }
        
        foreach (Transition t in _currentTransitions) {
            if (t.Condition()) {
                return t;
            }
        }

        return null;
    }

    public void AddTransition(IState from, IState to, Func<bool> condition) {
        if (!_allTransitions.TryGetValue(from.GetType(), out List<Transition> transitions)) {
            transitions = new List<Transition>();
            _allTransitions[from.GetType()] = transitions;
        }
        
        transitions.Add(new Transition(to, condition));
    }

    public void AddAnyTransition(IState to, Func<bool> condition) {
        _anyTransitions.Add(new Transition(to, condition));
    }
    
    private void SetState(IState newState) {
        // Cannot transition from a state to itself
        if (newState == _currentState) return;
        
        _currentState?.OnExit();
        _currentState = newState;

        _allTransitions.TryGetValue(_currentState.GetType(), out _currentTransitions);
        if (_currentTransitions == null) {
            _currentTransitions = EmptyTransitions;
        }
        
        _currentState.OnEnter();
    }

    public IState getCurrentState() {
        return _currentState;
    }
}

public class Transition {
    public Func<bool> Condition;
    public IState To;
    
    public Transition(IState to, Func<bool> condition) {
        To = to;
        Condition = condition;
    }
}
