using UnityEngine;

public class ResourceDetector : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;

    private float _rayMaxDistance = 5f;

    public bool IsResourcePresent(out Resource resource)
    {
        Vector3 rayStartPosition = transform.position + Vector3.down;

        if (Physics.Raycast(rayStartPosition, Vector3.up, out RaycastHit hit, _rayMaxDistance, _layerMask))
            if (hit.collider.TryGetComponent(out resource))
                return true;

        resource = null;
        return false;
    }
}