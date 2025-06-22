using System.Collections.Generic;
using UnityEngine;

public class Basehome : MonoBehaviour
{
    [SerializeField] private int _minimumDrones = 1;
    [SerializeField] private int _priceBuildDrone = 3;
    [SerializeField] private int _priceBuildBasehome = 5;
    [SerializeField] private Flag _flag;
    [SerializeField] private DroneBuilder _droneBuilder;
    [SerializeField] private BasehomeBuilder _basehomeBuilder;
    [SerializeField] private ScoreView _scoreView;
    [SerializeField] private Scanner _scanner;
    [SerializeField] private Warehouse _resourceWarehouse;
    [SerializeField] private StateHandler _states;
    [SerializeField] private DroneParking _parking;

    private int _startingDroneCount;
    private DatabaseOfResources _databaseOfResources;
    private RectTransform _positionScore;
    private List<Drone> _drones = new List<Drone>();

    public bool CanBuildBasehome => _drones.Count > _minimumDrones;

    private void Awake()
    {
        _states.AddState(new State(nameof(BuildDrone), BuildDrone, _priceBuildDrone));
        _states.AddState(new State(nameof(SendResourcesToBuildBasehome), SendResourcesToBuildBasehome, _priceBuildBasehome));
        SetStateToBuildDrones();
    }

    private void OnEnable() =>
        _flag.Located += SetStateBuildBasehome;

    private void OnDisable() =>
        _flag.Located -= SetStateBuildBasehome;

    private void Start()
    {
        BuildStarterDrones();
        _scoreView.CreateText(_positionScore);
        _scanner.Activate(_databaseOfResources);
    }

    private void Update()
    {
        if (_resourceWarehouse.ResourcesCount >= _states.CurrentState.Price)
        {
            _resourceWarehouse.Reduce(_states.CurrentState.Price);
            _states.CurrentState.Action();
        }

        if (_databaseOfResources.CountFoundResources > 0 && _parking.AvailableDronesCount > 0)
        {
            Resource resource = _databaseOfResources.GetResource();
            Drone drone = _parking.GetFreeDrone();
            drone.DeliveResource(resource);
        }
    }

    public void Initialize(SpawnerDrone spawnerDrone, SpawnerBasehome spawnerBasehome,
        DatabaseOfResources databaseOfResources, FlagMover flagMover, RectTransform positionScore, int startingDroneCount)
    {
        _droneBuilder.SetSpawner(spawnerDrone);
        _basehomeBuilder.SetSpawner(spawnerBasehome);
        _flag.SetMover(flagMover);
        _positionScore = positionScore;
        _databaseOfResources = databaseOfResources;
        _startingDroneCount = startingDroneCount;
    }

    public void ActivateFlag()
    {
        SetStateToBuildDrones();
        _flag.Activate();
    }

    public void AddDrone(Drone drone)
    {
        drone.ReturnedToBase += ReturnDrone;
        drone.SetBasehomePosition(this);
        _drones.Add(drone);
        _parking.AddDrone(drone);
    }

    private void ReturnDrone(Drone drone)
    {
        _resourceWarehouse.Add(drone.GiveResource());
        _parking.AddDrone(drone);
    }

    private void RemoveDrone(Drone drone)
    {
        if (_drones.Contains(drone))
        {
            drone.ReturnedToBase -= ReturnDrone;
            _drones.Remove(drone);
        }
    }

    private void SendResourcesToBuildBasehome()
    {
        Drone drone = _parking.GetFreeDrone();
        drone.ReachedFlag += _basehomeBuilder.Build;
        _basehomeBuilder.Builded += _flag.Deactivate;

        drone.DeliveResourcesToFlag(_flag);
        RemoveDrone(drone);

        SetStateToBuildDrones();
    }

    private void BuildDrone()
    {
        Vector3 position = _parking.GetSpace();
        Drone drone = _droneBuilder.Build(position);
        AddDrone(drone);
    }

    private void SetStateToBuildDrones() =>
        _states.SetState(nameof(BuildDrone));

    private void SetStateBuildBasehome() =>
        _states.SetState(nameof(SendResourcesToBuildBasehome));

    private void BuildStarterDrones()
    {
        for (int i = 0; i < _startingDroneCount; i++)
            BuildDrone();
    }
}