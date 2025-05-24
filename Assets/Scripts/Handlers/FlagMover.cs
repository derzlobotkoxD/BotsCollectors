using System;
using System.Collections;
using UnityEngine;

public class FlagMover : MonoBehaviour
{
    [SerializeField] private Selector _selector;
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _layerMask;
    [Range(0, 100)][SerializeField] private float _maxDistence = 70f;

    public event Action<FlagMover> Stopped;
    public event Action Error;

    public void StartMove(Transform flag) =>
        StartCoroutine(Move(flag));

    private IEnumerator Move(Transform flag)
    {
        bool IsEnabled = true;

        while (IsEnabled)
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, _maxDistence, _layerMask))
            {
                flag.transform.position = hit.point;

                if (Input.GetMouseButtonDown(0))
                {
                    if (hit.transform.TryGetComponent(out Ground ground))
                        IsEnabled = false;
                    else
                        Error?.Invoke();
                }
            }

            yield return null;
        }

        Stopped?.Invoke(this);
        _selector.Unselect();
    }
}