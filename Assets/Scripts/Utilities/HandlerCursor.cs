using UnityEngine;

public class HandlerCursor : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private InputReader _inputReader;

    private void Update()
    {
        transform.position = _inputReader.GetMousePosition();

        if (Input.GetMouseButtonDown(0))
            Click();
    }

    private void Click() =>
        _animator.SetTrigger(AnimatorData.Parameters.Click);
}