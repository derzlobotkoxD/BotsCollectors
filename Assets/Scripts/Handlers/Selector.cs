using System;
using UnityEngine;

public class Selector : MonoBehaviour
{
    [SerializeField] private LayerMask _mask;
    [SerializeField] private Camera _camera;
    [Range(0, 100)][SerializeField] private float _maxDistence = 70f;

    private bool _isSelected = false;

    private void Update()
    {
        if (_isSelected == false && Input.GetMouseButtonDown(0))
            TrySelect();
    }

    public void Unselect() =>
        _isSelected = false;

    private void TrySelect()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, _maxDistence, _mask))
        {
            if (hit.transform.TryGetComponent(out Basehome basehome) && basehome.CountDrones > 1)
            {
                _isSelected = true;
                basehome.ActivateFlag();
            }
        }
    }
}