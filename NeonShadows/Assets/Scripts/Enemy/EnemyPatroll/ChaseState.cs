using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { 
        EnemyPatrollAI enemyPatrollAI = animator.gameObject.GetComponent<EnemyPatrollAI>();
        enemyPatrollAI.SetPlayerAsTarget();
        enemyPatrollAI.SetSpeed(15f); // Set chase speed

        animator.SetBool("isPlayerAsTarget", true);
    }
}
