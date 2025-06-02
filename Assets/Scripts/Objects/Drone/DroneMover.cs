using System;
using System.Collections;
using UnityEngine;

public class DroneMover : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;

    public event Action StartedMoving;

    public IEnumerator GoTo(Vector3 target)
    {
        yield return RotateTo(target);
        StartedMoving?.Invoke();
        yield return MoveTo(target);
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
        while (transform.position != target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, _moveSpeed * Time.deltaTime);

            yield return null;
        }
    }
}