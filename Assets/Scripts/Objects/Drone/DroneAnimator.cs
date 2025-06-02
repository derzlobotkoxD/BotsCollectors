using System;
using UnityEngine;

public class DroneAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public event Action FinishedGoingDown;
    public event Action FinishedGoingUp;

    public void FinishGoDown() =>
        FinishedGoingDown?.Invoke();

    public void FinishGoUp() =>
        FinishedGoingUp?.Invoke();

    public void Take() =>
        _animator.SetTrigger(AnimatorData.Parameters.Take);

    public void Idle() =>
        _animator.SetTrigger(AnimatorData.Parameters.Idle);
    
    public void Move() =>
        _animator.SetTrigger(AnimatorData.Parameters.Move);
}