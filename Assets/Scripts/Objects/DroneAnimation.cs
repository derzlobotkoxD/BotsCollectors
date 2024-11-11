using System;
using UnityEngine;

public class DroneAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private AnimationClip _takeResourceClip;
    [Range(0, 1)]
    [SerializeField] private float _timeEventTakeResource;

    public AnimationClip TakeResourceClip => _takeResourceClip;
    public float IntervalEventTakeResource => _timeEventTakeResource;

    public void AddEventInAnimationClip(string action, float interval, AnimationClip clip)
    {
        AnimationEvent newEvent = new AnimationEvent();
        newEvent.time = Mathf.Lerp(0, clip.length, interval);
        newEvent.functionName = action;

        foreach (AnimationEvent animationEvent in clip.events)
            if (animationEvent.functionName == action)
                return;

        clip.AddEvent(newEvent);
    }

    public void SetTrigger(string trigger) =>
        _animator.SetTrigger(trigger);
}