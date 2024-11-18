using System;
using System.Collections;
using UnityEngine;

public class Drone : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private DroneAnimator _animator;
    [SerializeField] private Transform _cargoCompartment;

    private float _rayMaxDistance = 5f;
    private Resource _resource;
    private BaseHome _base;

    public event Action<Drone> DeliveredResource;

    private void OnEnable()
    {
        _animator.FinishedGoingDown += AddResourceToCargo;
        _animator.FinishedGoingUp += MoveToBase;
    }

    private void OnDisable()
    {
        _animator.FinishedGoingDown -= TakeResource;
        _animator.FinishedGoingUp -= MoveToBase;
    }

    public void SetBase(BaseHome baseHome) =>
        _base = baseHome;

    public void DeliverResource(Resource resource) =>
        StartCoroutine(GoToAction(resource.transform.position, TakeResource));


    private void MoveToBase()
    {
        Vector3 direction = (transform.position - _base.transform.position).normalized;
        Vector3 targetPosition = _base.transform.position + direction * _base.RadiusSpawnZone;
        StartCoroutine(GoToAction(targetPosition, PutIntoBaseHome));
    }

    private IEnumerator GoToAction(Vector3 target, Action action)
    {
        yield return RotateTo(target);
        yield return MoveTo(target);
        action();
    }

    private IEnumerator RotateTo(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        direction.y = 0f;

        while ((direction.normalized - transform.forward.normalized).sqrMagnitude > 0.01f)
        {
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, _rotateSpeed * Time.deltaTime, 0f);
            transform.rotation = Quaternion.LookRotation(newDirection);

            yield return null;
        }
    }

    private IEnumerator MoveTo(Vector3 target)
    {
        _animator.Move();

        while (transform.position != target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, _moveSpeed * Time.deltaTime);

            yield return null;
        }
    }

    private void TakeResource()
    {
        if (Physics.Raycast(transform.position + Vector3.down, Vector3.up, out RaycastHit hit, _rayMaxDistance, _layerMask))
        {
            if (hit.collider.TryGetComponent(out Resource resource))
            {
                _resource = resource;
                _animator.Take();
            }
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

        _animator.Idle();
        DeliveredResource?.Invoke(this);
    }
}