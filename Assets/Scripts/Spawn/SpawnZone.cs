using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZone : MonoBehaviour
{
    [SerializeField] private SpawnerResources _spawner;
    [SerializeField] private GeneraterSpawnPoint _generatorSpawnPoint;
    [SerializeField][Range(0.1f, 3f)] private float _delay = 1f;
    [SerializeField] private LayerMask _mask;
    
    private float _maxDistanceChecking = 10f;
    private List<Vector3> _points;
    private List<Vector3> _pointsTaken = new List<Vector3>();

    private void Awake()
    {
        float divider = 2f;

        Vector3 offset = transform.position - transform.lossyScale / divider;
        Vector2 zoneSize = new Vector2(transform.localScale.x, transform.localScale.z);
        _points = _generatorSpawnPoint.GeneratePoints(offset, zoneSize);

        StartCoroutine(SpawnWithDelay(_delay));
    }

    private void OnEnable() =>
        _spawner.ResourceReturned += ReturnPointToAvailable;

    private void OnDisable() =>
        _spawner.ResourceReturned -= ReturnPointToAvailable;

    private Vector3 GetRandomPosition()
    {
        int currentPointIndex = Random.Range(0, _points.Count);

        while (IsAvailablePoint(_points[currentPointIndex]))
        {
            _points.Remove(_points[currentPointIndex]);
            currentPointIndex = Random.Range(0, _points.Count);
        }

        Vector3 point = _points[currentPointIndex];
        _pointsTaken.Add(point);
        _points.Remove(point);

        return point;
    }

    private bool IsAvailablePoint(Vector3 point) =>
        Physics.Raycast(point, Vector3.down, _maxDistanceChecking, _mask, QueryTriggerInteraction.Collide);

    private IEnumerator SpawnWithDelay(float delay)
    {
        WaitForSeconds wait = new WaitForSeconds(delay);

        while (enabled)
        {
            yield return wait;

            if (_points.Count > 0)
                _spawner.GetInstance(GetRandomPosition());
        }
    }

    private void ReturnPointToAvailable(Vector3 point)
    {
        _pointsTaken.Remove(point);
        _points.Add(point);
    }
}