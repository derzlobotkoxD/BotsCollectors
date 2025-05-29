using UnityEngine;
using UnityEngine.Events;

public class ScoreCounter : MonoBehaviour
{
    private int _value;

    public event UnityAction<int> Changed;

    public void Add()
    {
        _value++;
        Changed?.Invoke(_value);
    }

    public void Reduce(int value)
    {
        if (value < 0)
            return;

        _value -= value;
        Changed?.Invoke(_value);
    }
}