using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] private float _cooldown;
    [SerializeField] private float _radius;
    [SerializeField] private ParticleSystem _effect;
    [SerializeField] private LayerMask _layerMask;

    private Queue<Resource> _foundResources = new Queue<Resource>();

    public int CountFound => _foundResources.Count;

    private void Start()
    {
        StartCoroutine(Scan());
    }

    private IEnumerator Scan()
    {
        WaitForSeconds wait = new WaitForSeconds(_cooldown);

        while (enabled)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _radius, _layerMask);
            _effect.Play();

            foreach (Collider collider in colliders)
                if (collider.TryGetComponent(out Resource recource) && _foundResources.Contains(recource) == false)
                    _foundResources.Enqueue(recource);

            yield return wait;
        }
    }

    public bool TryGetFoundResource(out Resource resource)
    {
        resource = _foundResources.Dequeue();

        while (resource.IsMarked)
        {
            if (_foundResources.Count == 0)
            {
                resource = null;
                return false;
            }

            resource = _foundResources.Dequeue();
        }

        return true;
    }
}