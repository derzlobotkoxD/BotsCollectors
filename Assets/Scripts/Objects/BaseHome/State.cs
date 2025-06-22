using System;

public class State
{
    private Action _action;

    public State(string name, Action action, int price)
    {
        _action = action;
        Price = price;
        Name = name;
    }

    public int Price { get; private set; }
    public string Name { get; private set; }

    public void Action() =>
        _action();
}