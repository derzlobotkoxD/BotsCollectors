using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZone : MonoBehaviour
{
    [SerializeField] private SpawnerResources _spawner;
    [SerializeField] private GeneraterSpawnPoint _generatorSpawnPoint;
    [SerializeField][Range(0.1f, 3f)] private float _delay = 1f;
    [SerializeField] private LayerMask _mask;

    private Vector3 _halfResourceSize = new Vector3(0.2f, 0.2f, 0.2f);
    private List<Vector3> _points;
    private List<Vector3> _occupiedPoints = new List<Vector3>();

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

    private bool TryGetRandomPoint(out Vector3 point)
    {
        point = default(Vector3);

        if (_points.Count == 0)
            return false;

        int currentPointIndex = Random.Range(0, _points.Count);

        while (IsOccupiedPoint(_points[currentPointIndex]))
        {
            _points.Remove(_points[currentPointIndex]);
            currentPointIndex = Random.Range(0, _points.Count);

            if (_points.Count == 0)
                return false;
        }

        point = _points[currentPointIndex];
        _points.Remove(point);
        _occupiedPoints.Add(point);

        return true;
    }

    private bool IsOccupiedPoint(Vector3 point) =>
        Physics.CheckBox(point, _halfResourceSize, Quaternion.identity, _mask, QueryTriggerInteraction.Collide);

    private IEnumerator SpawnWithDelay(float delay)
    {
        WaitForSeconds wait = new WaitForSeconds(delay);

        while (enabled)
        {
            yield return wait;

            if (TryGetRandomPoint(out Vector3 point))
            {
                Resource resource = _spawner.GetInstance(point);
                resource.SetStartPosition(point);
            }
        }
    }

    private void ReturnPointToAvailable(Vector3 point)
    {
        _occupiedPoints.Remove(point);
        _points.Add(point);
    }
}