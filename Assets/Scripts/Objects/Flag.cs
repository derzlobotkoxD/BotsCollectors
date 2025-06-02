using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Flag : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Collider _noSpawnZone;

    private bool _isMoving = false;

    public event Action Located;

    public void Activate(FlagMover flagMover)
    {
        flagMover.Error += ChangeColor;
        flagMover.Stopped += Set;
        gameObject.SetActive(true);
        _isMoving = true;
        _noSpawnZone.enabled = false;
    }

    public void ChangeColor() =>
        _animator.SetTrigger(AnimatorData.Parameters.Error);

    public void Deactivate()
    {
        if (_isMoving == false)
            gameObject.SetActive(false);
    }

    private void Set(FlagMover flagMover)
    {
        _noSpawnZone.enabled = true;
        flagMover.Error -= ChangeColor;
        flagMover.Stopped -= Set;
        Located?.Invoke();
        _isMoving = false;
    }
}