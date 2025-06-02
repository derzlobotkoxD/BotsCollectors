using UnityEngine;

public static class AnimatorData
{
    public class Parameters
    {
        public static readonly int Move = Animator.StringToHash(nameof(Move));
        public static readonly int Take = Animator.StringToHash(nameof(Take));
        public static readonly int Idle = Animator.StringToHash(nameof(Idle));
        public static readonly int Error = Animator.StringToHash(nameof(Error));
        public static readonly int Click = Animator.StringToHash(nameof(Click));
    }
}