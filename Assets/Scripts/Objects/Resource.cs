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

    public void Take(Transform parent)
    {
        _rigidbody.isKinematic = true;
        transform.parent = parent;
        transform.position = parent.position;
        transform.rotation = parent.rotation;
    }

    public void SetStartPosition(Vector3 position) =>
        StartPosition = position;
}