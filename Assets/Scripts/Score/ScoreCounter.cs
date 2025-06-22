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

    public void Reduce(int subtrahend)
    {
        if (subtrahend <= 0 || Value > subtrahend)
            return;

        Value -= subtrahend;
        Changed?.Invoke(Value);
    }
}