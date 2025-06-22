using System;
using UnityEngine;

public class BasehomeBuilder : MonoBehaviour
{
    private SpawnerBasehome _spawnerBasehome;

    public event Action Builded;

    public void Build(Drone drone)
    {
        drone.ReachedFlag -= Build;
        Basehome basehome = _spawnerBasehome.GetInstance(drone.transform.position);
        basehome.AddDrone(drone);
        Builded?.Invoke();
    }

    public void SetSpawner(SpawnerBasehome spawner) =>
        _spawnerBasehome = spawner;
}