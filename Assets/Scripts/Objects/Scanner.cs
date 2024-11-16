using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] private DatabaseOfResources _databaseOfResources;
    [SerializeField] private float _cooldown;
    [SerializeField] private float _radius;
    [SerializeField] private ParticleSystem _effect;
    [SerializeField] private LayerMask _layerMask;

    private void Start() =>
        StartCoroutine(Scan());

    private IEnumerator Scan()
    {
        WaitForSeconds wait = new WaitForSeconds(_cooldown);

        while (enabled)
        {
            _databaseOfResources.SetFoundResources(SearchResources());
            yield return wait;
        }
    }

    private List<Resource> SearchResources()
    {
        List<Resource> foundResources = new List<Resource>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, _radius, _layerMask);

        _effect.Play();

        foreach (Collider collider in colliders)
            if (collider.TryGetComponent(out Resource recource))
                foundResources.Add(recource);

        return foundResources;
    }
}