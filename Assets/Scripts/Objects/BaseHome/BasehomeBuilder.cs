using System;
using UnityEngine;

public class BasehomeBuilder : MonoBehaviour
{
    private SpawnerBasehome _spawnerBasehome;

    public event Action<BasehomeBuilder> Builded;

    public void Build(Drone drone)
    {
        drone.ReachedFlag -= Build;
        Basehome basehome = _spawnerBasehome.GetInstance(drone.transform.position);
        basehome.AddDrone(drone);
        Builded?.Invoke(this);
    }

    public void SetSpawner(SpawnerBasehome spawner) =>
        _spawnerBasehome = spawner;
}