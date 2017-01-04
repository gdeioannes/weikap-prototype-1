using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerPopUpController : MonoBehaviour
{
    [SerializeField] string okTrigger;
    [SerializeField] string errorTrigger;
    [SerializeField] Animator animator;
	
    public void Show(bool result)
    {
        animator.SetTrigger(result ? okTrigger : errorTrigger);
    }    
}
