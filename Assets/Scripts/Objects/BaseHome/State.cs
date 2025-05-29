using System;

public class State
{
    private Action _action;

    public State(Action action, int price)
    {
        _action = action;
        Price = price;
    }

    public int Price { get; private set; }

    public void Build() =>
        _action();
}