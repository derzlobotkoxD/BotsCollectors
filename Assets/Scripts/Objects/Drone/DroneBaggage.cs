using UnityEngine;

public class DroneBaggage : MonoBehaviour
{
    private Resource _resource;

    public Resource GiveResource()
    {
        Resource resource = _resource;
        _resource = null;
        return resource;
    }

    public void SetResource(Resource resource) =>
        _resource = resource;

    public void AttachResource() =>
        _resource.Take(transform);
}