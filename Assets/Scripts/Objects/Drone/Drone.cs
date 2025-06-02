using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Drone : MonoBehaviour
{
    [SerializeField] private DroneAnimator _animator;
    [SerializeField] private DroneMover _mover;
    [SerializeField] private DroneBaggage _baggage;
    [SerializeField] private ResourceDetector _resourceDetector;

    private float _resourceTransferDistance = 2;
    private Vector3 _basehomePosition;

    public event UnityAction<Drone> ReturnedToBase;
    public event UnityAction<Drone> ReachedFlag;

    private void OnEnable()
    {
        _animator.FinishedGoingDown += _baggage.AttachResource;
        _animator.FinishedGoingUp += MoveToBase;
        _mover.StartedMoving += _animator.Move;
    }

    private void OnDisable()
    {
        _animator.FinishedGoingDown -= _baggage.AttachResource;
        _animator.FinishedGoingUp -= MoveToBase;
        _mover.StartedMoving -= _animator.Move;
    }

    public void SetBasehomePosition(Basehome basehome) =>
        _basehomePosition = basehome.transform.position;

    public void DeliverResource(Resource resource) =>
        StartCoroutine(GoToAction(resource.transform.position, TakeResource));

    public void DeliverResourcesToFlag(Flag flag) =>
        StartCoroutine(GoToAction(flag.transform.position, ParkAtFlag));

    public Resource GiveResource() =>
        _baggage.GiveResource();

    private void MoveToBase()
    {
        Vector3 direction = (transform.position - _basehomePosition).normalized;
        Vector3 targetPosition = _basehomePosition + direction * _resourceTransferDistance;
        StartCoroutine(GoToAction(targetPosition, ParkAtBasehome));
    }

    private IEnumerator GoToAction(Vector3 target, Action action)
    {
        yield return _mover.GoTo(target);
        action();
    }

    private void TakeResource()
    {
        if (_resourceDetector.IsResourcePresent(out Resource resource))
        {
            _baggage.SetResource(resource);
            _animator.Take();
        }
        else
        {
            MoveToBase();
        }
    }

    private void ParkAtBasehome()
    {
        _animator.Idle();
        ReturnedToBase?.Invoke(this);
    }

    private void ParkAtFlag()
    {
        _animator.Idle();
        ReachedFlag?.Invoke(this);
    }
}