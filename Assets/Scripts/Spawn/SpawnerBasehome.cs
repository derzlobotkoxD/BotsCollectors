using UnityEngine;

public class SpawnerBasehome : Spawner<Basehome>
{
    [SerializeField] private SpawnerDrone _spawnerDrone;
    [SerializeField] private DatabaseOfResources _databaseOfResources;
    [SerializeField] private FlagMover _flagMover;
    [SerializeField] private RectTransform _positionScore;
    [SerializeField] private Basehome _firstBasehome;
    [SerializeField] private int _startingDroneCount = 0;
    [SerializeField] private int _startingDroneCountFirst = 3;


    protected override void Awake()
    {
        _firstBasehome.Initialize(_spawnerDrone, this, _databaseOfResources, _flagMover, _positionScore, _startingDroneCountFirst);
        base.Awake();
    }

    protected override void ActivateInstance(Basehome instance)
    {
        instance.Initialize(_spawnerDrone, this, _databaseOfResources, _flagMover, _positionScore, _startingDroneCount);
        base.ActivateInstance(instance);
    }
}