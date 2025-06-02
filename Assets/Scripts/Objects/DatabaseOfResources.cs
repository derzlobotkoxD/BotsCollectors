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

    public Resource GetResource()
    {
        Resource resource = _foundResources[0];
        OccupyResource(resource);
        return resource;
    }

    public void SetFoundResources(List<Resource> resources)
    {
        foreach (Resource resource in resources)
        {
            if (_foundResources.Contains(resource) || _occupiedResources.Contains(resource))
                continue;

            _foundResources.Add(resource);
        }
    }

    private void OccupyResource(Resource resource)
    {
        _foundResources.Remove(resource);
        _occupiedResources.Add(resource);
        resource.Deleted += ReleaseResource;
    }

    private void ReleaseResource(Resource resource)
    {
        _occupiedResources.Remove(resource);
        resource.Deleted -= ReleaseResource;
    }
}