using UnityEngine;

public class SquareButton : MonoBehaviour
{
   [SerializeField] private Animator animator;

    private void Start()
    {
        animator.keepAnimatorControllerStateOnDisable = true;
    }
}
