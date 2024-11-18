using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Scanner), typeof(ScoreCounter))]
public class BaseHome : MonoBehaviour
{
    [SerializeField] private int _startingDroneCount = 3;
    [SerializeField] private SpawnerDrone _spawnerDrone;
    [SerializeField] private DatabaseOfResources _databaseOfResources; 
    [SerializeField] private float _radiusSpawnZone;

    private ScoreCounter _counter;
    private Queue<Drone> _drones = new Queue<Drone>();

    public int RecourceCount { get; private set; } = 0;
    public float RadiusSpawnZone => _radiusSpawnZone;

    private void Start()
    {
        _counter = GetComponent<ScoreCounter>();

        for (int i = 0; i < _startingDroneCount; i++)
            CreateDrone();
    }

    private void OnDisable()
    {
        foreach (var drone in _drones)
            drone.DeliveredResource -= AddDrone;
    }

    private void Update()
    {
        if (_databaseOfResources.CountFoundResources > 0 && _drones.Count > 0)
        {
            if (_databaseOfResources.TryGetFoundResource(out Resource resource))
            {
                Drone drone = _drones.Dequeue();
                drone.DeliverResource(resource);
            }
        }
    }

    public void AddResource(Resource resource)
    {
        if (resource != null)
        {
            resource.Delete();
            RecourceCount++;
            _counter.Add();
        }
    }

    private void AddDrone(Drone drone) =>
        _drones.Enqueue(drone);

    private void CreateDrone()
    {
        Vector3 position = GetRandomSpawnPosition();
        Drone drone = _spawnerDrone.GetInstance(position);
        drone.SetBase(this);
        AddDrone(drone);
        drone.DeliveredResource += AddDrone;
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector2 direction = Random.insideUnitCircle.normalized * _radiusSpawnZone;
        Vector3 offset = new Vector3(direction.x, 0, direction.y);

        return transform.position + offset;
    }
}