using UnityEngine;

public class InputReader : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    public bool IsClick { get; private set; }

    private void Update() =>
        IsClick = Input.GetMouseButtonDown(0);

    public Ray GetCursorPositionRay() =>
        _camera.ScreenPointToRay(Input.mousePosition);
}