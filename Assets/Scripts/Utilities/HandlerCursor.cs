using UnityEngine;

public class HandlerCursor : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private void Update()
    {
        transform.position = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
            Click();
    }

    private void Click() =>
        _animator.SetTrigger(AnimatorData.Parameters.Click);
}