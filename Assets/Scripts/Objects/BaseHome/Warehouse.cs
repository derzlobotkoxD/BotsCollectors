using UnityEngine;

[RequireComponent(typeof(ScoreCounter))]
public class Warehouse : MonoBehaviour
{
    [SerializeField] private ScoreCounter _counter;

    public int ResourcesCount => _counter.Value;

    private void Awake() =>
        _counter = GetComponent<ScoreCounter>();

    public void Add(Resource resource)
    {
        if (resource == null)
            return;

        resource.Delete();
        _counter.Add();
    }

    public void Reduce(int value) =>
        _counter.Reduce(value);
}