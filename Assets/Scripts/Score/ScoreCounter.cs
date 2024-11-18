using UnityEngine;
using UnityEngine.Events;

public class ScoreCounter : MonoBehaviour
{
    private int _value;

    public event UnityAction<int> ScoreChanged;

    public void Add()
    {
        _value++;
        ScoreChanged?.Invoke(_value);
    }
}