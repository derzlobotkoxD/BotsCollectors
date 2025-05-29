using UnityEngine;
using UnityEngine.Events;

public class Resource : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;

    public event UnityAction<Resource> Deleted;

    public Vector3 StartPosition { get; private set; }

    private void OnEnable()
    {
        _rigidbody.isKinematic = false;
        _rigidbody.velocity = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }

    public void Delete()
    {
        transform.SetParent(null);
        Deleted?.Invoke(this);
    }

    public void BecomeKinemetric() =>
        _rigidbody.isKinematic = true;

    public void SetStartPosition(Vector3 position) =>
        StartPosition = position;
}