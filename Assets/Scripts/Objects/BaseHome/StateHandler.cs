using System.Collections.Generic;
using UnityEngine;

public class StateHandler : MonoBehaviour
{
    private Dictionary<string, State> _states = new Dictionary<string, State>();

    public State CurrentState { get; private set; }

    public void AddState(State state)
    {
        if (_states.ContainsKey(state.Name))
            return;

        _states.Add(state.Name, state);
    }

    public void SetState(string name)
    {
        if (CurrentState != null && CurrentState.Name == name)
            return;

        if (_states.TryGetValue(name, out State newState))
            CurrentState = newState;
    }
}