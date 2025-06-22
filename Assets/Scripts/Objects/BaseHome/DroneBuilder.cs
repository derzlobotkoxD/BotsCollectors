using UnityEngine;

public class DroneBuilder : MonoBehaviour
{
    private SpawnerDrone _spawnerDrone;

    public Drone Build(Vector3 spawnPosition) =>
        _spawnerDrone.GetInstance(spawnPosition);

    public void SetSpawner(SpawnerDrone spawner) =>
        _spawnerDrone = spawner;
}