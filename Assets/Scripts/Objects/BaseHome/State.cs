using System;

public class State
{
    private Action Action;

    public int Price { get; private set; }

    public State(Action action, int price)
    {
        Action = action;
        Price = price;
    }

    public void Build() =>
        Action();
}