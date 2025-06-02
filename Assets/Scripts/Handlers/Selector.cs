using System;
using UnityEngine;

public class Selector : MonoBehaviour
{
    [SerializeField] private LayerMask _mask;
    [SerializeField] private InputReader _inputReader;
    [Range(0, 100)][SerializeField] private float _maxDistence = 70f;

    private bool _isSelected = false;

    private void Update()
    {
        if (_isSelected == false && _inputReader.IsClick)
            TrySelect();
    }

    public void Unselect() =>
        _isSelected = false;

    private void TrySelect()
    {
        Ray ray = _inputReader.GetCursorPositionRay();

        if (Physics.Raycast(ray, out RaycastHit hit, _maxDistence, _mask))
        {
            if (hit.transform.TryGetComponent(out Basehome basehome) && basehome.CanBuildBasehome)
            {
                _isSelected = true;
                basehome.ActivateFlag();
            }
        }
    }
}