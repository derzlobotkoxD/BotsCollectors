using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Scanner), typeof(ScoreCounter), typeof(ScoreView))]
public class Basehome : MonoBehaviour
{
    [SerializeField] private int _startingDroneCount = 3;
    [SerializeField] private int _minimumDrones = 1;
    [SerializeField] private int _priceBuildDrone = 3;
    [SerializeField] private int _priceBuildBasehome = 5;
    [SerializeField] private float _radiusSpawnZone = 1.5f;
    [SerializeField] private SpawnerDrone _spawnerDrone;
    [SerializeField] private SpawnerBasehome _spawnerBasehome;
    [SerializeField] private DatabaseOfResources _databaseOfResources;
    [SerializeField] private RectTransform _positionScore;
    [SerializeField] private FlagMover _flagMover;
    [SerializeField] private Flag _flag;

    private ScoreCounter _counter;
    private ScoreView _scoreView;
    private Scanner _scanner;
    private List<Drone> _drones = new List<Drone>();
    private Queue<Drone> _availableDrones = new Queue<Drone>();

    private State _currentState;
    private State _stateBuildDrones;
    private State _stateBuildBasehome;

    public int ResourceCount { get; private set; } = 0;
    public bool CanBuildBasehome => _drones.Count > _minimumDrones;

    private void Awake()
    {
        _stateBuildDrones = new State(BuildDrone, _priceBuildDrone);
        _stateBuildBasehome = new State(SendResourcesToBuildBasehome, _priceBuildBasehome);
        _currentState = _stateBuildDrones;
    }

    private void OnEnable() =>
        _flag.Located += SetStateBuildBasehome;

    private void OnDisable() =>
        _flag.Located -= SetStateBuildBasehome;

    private void Start()
    {
        _scanner = GetComponent<Scanner>();
        _counter = GetComponent<ScoreCounter>();
        _scoreView = GetComponent<ScoreView>();

        for (int i = 0; i < _startingDroneCount; i++)
            BuildDrone();

        _scanner.SetDatabaseOfResources(_databaseOfResources);
        _scoreView.CreateText(_positionScore);
        _scanner.Activate();
    }

    private void Update()
    {
        if (ResourceCount >= _currentState.Price)
        {
            ReduceResources(_currentState.Price);
            _currentState.Build();
        }

        if (_databaseOfResources.CountFoundResources > 0 && _availableDrones.Count > 0)
        {
            Resource resource = _databaseOfResources.GetResource();
            Drone drone = _availableDrones.Dequeue();
            drone.DeliverResource(resource);
        }
    }

    public void Initialize(SpawnerDrone spawnerDrone, DatabaseOfResources databaseOfResources, SpawnerBasehome spawnerBasehome, FlagMover flagMover, RectTransform positionScore)
    {
        _spawnerDrone = spawnerDrone;
        _databaseOfResources = databaseOfResources;
        _spawnerBasehome = spawnerBasehome;
        _flagMover = flagMover;
        _positionScore = positionScore;
        _startingDroneCount = 0;
    }

    public void ActivateFlag()
    {
        SetStateToBuildDrones();
        _flagMover.StartMove(_flag.transform);
        _flag.Activate(_flagMover);
    }

    public void AddDrone(Drone drone)
    {
        drone.ReturnedToBase += ReturnDrone;
        drone.SetBasehomePosition(this);
        _drones.Add(drone);
        _availableDrones.Enqueue(drone);
    }

    public void ReturnDrone(Drone drone)
    {
        AddResource(drone.GiveResource());
        _availableDrones.Enqueue(drone);
    }

    public void AddResource(Resource resource)
    {
        if (resource == null)
            return;

        resource.Delete();
        _counter.Add();
        ResourceCount = _counter.Value;
    }

    private void ReduceResources(int value)
    {
        _counter.Reduce(value, ResourceCount);
        ResourceCount = _counter.Value;
    }

    private void BuildDrone()
    {
        Vector3 position = GetRandomSpawnPosition();
        Drone drone = _spawnerDrone.GetInstance(position);
        AddDrone(drone);
    }

    private void SendResourcesToBuildBasehome()
    {
        Drone drone = _availableDrones.Dequeue();
        drone.ReachedFlag += BuildBaseHome;

        drone.DeliverResourcesToFlag(_flag);

        if (_drones.Contains(drone))
        {
            drone.ReturnedToBase -= ReturnDrone;
            _drones.Remove(drone);
        }

        SetStateToBuildDrones();
    }

    private void BuildBaseHome(Drone drone)
    {
        drone.ReachedFlag -= BuildBaseHome;
        Basehome basehome = _spawnerBasehome.GetInstance(_flag.transform.position);
        basehome.AddDrone(drone);
        _flag.Deactivate();
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector2 direction = Random.insideUnitCircle.normalized * _radiusSpawnZone;
        Vector3 offset = new Vector3(direction.x, 0, direction.y);

        return transform.position + offset;
    }

    private void SetStateToBuildDrones() =>
        _currentState = _stateBuildDrones;

    private void SetStateBuildBasehome() =>
        _currentState = _stateBuildBasehome;
}