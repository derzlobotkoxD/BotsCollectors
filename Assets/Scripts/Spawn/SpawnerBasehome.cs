using UnityEngine;

public class SpawnerBasehome : Spawner<Basehome> 
{
    [SerializeField] private SpawnerDrone _spawnerDrone;
    [SerializeField] private DatabaseOfResources _databaseOfResources;
    [SerializeField] private FlagMover _flagMover;
    [SerializeField] private RectTransform _positionScore;

    protected override void ActivateInstance(Basehome instance)
    {
        instance.Initialize(_spawnerDrone, _databaseOfResources, this, _flagMover, _positionScore);
        base.ActivateInstance(instance);
    }
}