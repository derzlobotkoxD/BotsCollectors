using System;
using System.Collections;
using UnityEngine;

public class Drone : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private DroneAnimation _animation;
    [SerializeField] private Transform _cargoCompartment;

    private float _rayMaxDistance = 5f;
    private Resource _resource;
    private BaseHome _base;

    private void Awake()
    {
        _animation.AddEventInAnimationClip(nameof(AddResourceToCargo), _animation.IntervalEventTakeResource, _animation.TakeResourceClip);
        _animation.AddEventInAnimationClip(nameof(MoveToBase), 1, _animation.TakeResourceClip);
    }

    public void SetBase(BaseHome baseHome) =>
        _base = baseHome;

    public void DeliverResource(Resource resource) =>
        StartCoroutine(RotateToTarget(resource.transform.position, TakeResource));

    private void MoveToBase()
    {
        Vector3 direction = (transform.position - _base.transform.position).normalized;
        Vector3 targetPosition = _base.transform.position + direction * _base.RadiusSpawnZone;
        StartCoroutine(RotateToTarget(targetPosition, PutIntoBaseHome));
    }

    private IEnumerator RotateToTarget(Vector3 target, Action action)
    {
        Vector3 direction = target - transform.position;
        direction.y = 0f;

        while ((direction.normalized - transform.forward.normalized).magnitude > 0.01f)
        {
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, _rotateSpeed * Time.deltaTime, 0f);
            transform.rotation = Quaternion.LookRotation(newDirection);

            yield return null;
        }

        StartCoroutine(MoveToTarget(target, action));
    }

    private IEnumerator MoveToTarget(Vector3 target, Action action)
    {
        _animation.SetTrigger(Constants.Animation.Move);

        while (transform.position != target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, _moveSpeed * Time.deltaTime);

            yield return null;
        }

        action();
    }

    private void TakeResource()
    {
        if (Physics.Raycast(transform.position + Vector3.down, Vector3.up, out RaycastHit hit, _rayMaxDistance, _layerMask))
        {
            _resource = hit.collider.GetComponent<Resource>();
            _animation.SetTrigger(Constants.Animation.Take);
        }
        else
        {
            MoveToBase();
        }
    }

    private void AddResourceToCargo()
    {
        _resource.BecomeKinemetric();
        _resource.transform.position = _cargoCompartment.position;
        _resource.transform.rotation = transform.rotation;
        _resource.transform.parent = _cargoCompartment;
    }

    private void PutIntoBaseHome()
    {
        if (_resource != null)
        {
            _resource.transform.SetParent(null);
            _base.AddResource(_resource);
            _resource = null;
        }

        ReturnToBaseHome();
    }

    private void ReturnToBaseHome()
    {
        _base.AddDrone(this);
        _animation.SetTrigger(Constants.Animation.Idle);
    }
}