using UnityEngine;
using System.Collections;

public class ChangeAnimatorController : MonoBehaviour
{
    public Animator anim;
    public RuntimeAnimatorController combatAnim;
    
    private void OnTriggerEnter(Collider other)
    {
        anim.runtimeAnimatorController = combatAnim;
    }
}
