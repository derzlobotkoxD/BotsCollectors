using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Drone : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private DroneAnimator _animator;
    [SerializeField] private Transform _cargoCompartment;

    private float _rayMaxDistance = 5f;
    private float _resourceTransferDistance = 2;
    private Resource _resource;
    private Vector3 _basehomePosition;

    public event UnityAction<Drone> ReturnedToBase;

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

    public void SetBasehomePosition(Basehome basehome) =>
        _basehomePosition = basehome.transform.position;

    public void DeliverResource(Resource resource) =>
        StartCoroutine(GoToAction(resource.transform.position, TakeResource));

    public void DeliverNewBasehome(Flag flag, SpawnerBasehome spawner) =>
        StartCoroutine(GoToFlag(flag, spawner));

    public Resource GiveResource()
    {
        Resource resource = _resource;
        _resource = null;
        return resource;
    }

    private void MoveToBase()
    {
        Vector3 direction = (transform.position - _basehomePosition).normalized;
        Vector3 targetPosition = _basehomePosition + direction * _resourceTransferDistance;
        StartCoroutine(GoToAction(targetPosition, ReturnToBasehome));
    }

    private IEnumerator GoToAction(Vector3 target, Action action)
    {
        yield return GoTo(target);
        action();
    }

    private IEnumerator GoTo(Vector3 target)
    {
        yield return RotateTo(target);
        yield return MoveTo(target);
    }

    private IEnumerator GoToFlag(Flag flag, SpawnerBasehome spawner)
    {
        Vector3 target = flag.transform.position;

        yield return GoTo(target);

        flag.Deactivate();
        Basehome basehome = spawner.GetInstance(target);
        basehome.AddDrone(this);
        _animator.Idle();
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
        Vector3 rayStartPosition = transform.position + Vector3.down;

        if (Physics.Raycast(rayStartPosition, Vector3.up, out RaycastHit hit, _rayMaxDistance, _layerMask))
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

    private void ReturnToBasehome()
    {
        _animator.Idle();
        ReturnedToBase?.Invoke(this);
    }
}