using UnityEngine;
using UnityEngine.Events;

public class ScoreCounter : MonoBehaviour
{
    public event UnityAction<int> Changed;

    public int Value { get; private set; }

    public void Add()
    {
        Value++;
        Changed?.Invoke(Value);
    }

    public void Reduce(int value, int currentAmount)
    {
        if (value <= 0 || currentAmount - value < 0)
            return;

        Value -= value;
        Changed?.Invoke(Value);
    }
}