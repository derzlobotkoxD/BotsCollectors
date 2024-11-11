using UnityEngine;
using UnityEngine.Events;

public class SpawnerResources : Spawner<Resource>
{
    public event UnityAction<Vector3> ResourceReturned;

    protected override void ReleaseInstance(Resource instance)
    {
        ResourceReturned?.Invoke(instance.StartPosition);
        instance.Deleted -= ReleaseInstance;
        base.ReleaseInstance(instance);
    }

    protected override void ActivateInstance(Resource instance)
    {
        instance.Deleted += ReleaseInstance;
        base.ActivateInstance(instance);
        instance.SetStartPosition(instance.transform.position);
    }
}