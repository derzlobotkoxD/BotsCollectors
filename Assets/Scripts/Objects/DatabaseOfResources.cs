using System.Collections.Generic;
using UnityEngine;

public class DatabaseOfResources : MonoBehaviour
{
    private List<Resource> _foundResources = new List<Resource>();
    private List<Resource> _occupiedResources = new List<Resource>();

    public int CountFoundResources => _foundResources.Count;

    private void OnDisable()
    {
        foreach (Resource resource in _occupiedResources)
            resource.Deleted -= ReleaseResource;
    }

    public bool TryGetFoundResource(out Resource resource)
    {
        resource = _foundResources[0];

        while (_occupiedResources.Contains(resource))
        {
            _foundResources.RemoveAt(0);

            if (_foundResources.Count == 0)
            {
                resource = null;
                return false;
            }

            resource = _foundResources[0];
        }

        OccupyResource(resource);
        return true;
    }

    public void SetFoundResources(List<Resource> resources) =>
        _foundResources = resources;

    private void OccupyResource(Resource resource)
    {
        _occupiedResources.Add(resource);
        resource.Deleted += ReleaseResource;
    }

    private void ReleaseResource(Resource resource)
    {
        _occupiedResources.Remove(resource);
        _foundResources.Remove(resource);
        resource.Deleted -= ReleaseResource;
    }
}