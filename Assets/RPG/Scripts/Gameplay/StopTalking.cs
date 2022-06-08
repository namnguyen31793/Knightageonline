using KnightAge.Core;
using UnityEngine;

namespace KnightAge.Events
{
    public class StopTalking : Event<StopTalking>
    {
        public Animator animator;

        public override void Execute()
        {
            animator.SetBool("Talk", false);
        }
    }
}