using UnityEngine;
using UnityEngine.Events;

public class ScoreCounter : MonoBehaviour
{
    private int _value;

    public event UnityAction<int> ScoreChanged;

    public void Change(int value)
    {
        if (value < 0)
            return;

        _value = value;
        ScoreChanged?.Invoke(_value);
    }
}