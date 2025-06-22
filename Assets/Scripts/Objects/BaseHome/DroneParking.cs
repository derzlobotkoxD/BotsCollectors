using System.Collections.Generic;
using UnityEngine;

public class DroneParking : MonoBehaviour
{
    [SerializeField] private float _radiusSpawnZone = 1.5f;

    private Queue<Drone> _availableDrones = new Queue<Drone>();

    public int AvailableDronesCount => _availableDrones.Count;

    public Vector3 GetSpace()
    {
        Vector2 direction = Random.insideUnitCircle.normalized * _radiusSpawnZone;
        Vector3 offset = new Vector3(direction.x, 0, direction.y);

        return transform.position + offset;
    }

    public void AddDrone(Drone drone) =>
        _availableDrones.Enqueue(drone);

    public Drone GetFreeDrone() =>
        _availableDrones.Dequeue();
}