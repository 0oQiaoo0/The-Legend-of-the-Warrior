using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailSkillState : SnailState
{
    public override void OnEnter(Snail enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        currentEnemy.animator.SetBool("isHide", true);
        currentEnemy.animator.SetTrigger("skill");
        currentEnemy.GetComponent<Character>().invulnerable = true;
        currentEnemy.GetComponent<Character>().invulnerableCounter = currentEnemy.lostTime;
    }
    public override void LogicUpdate()
    {
        currentEnemy.GetComponent<Character>().invulnerableCounter = currentEnemy.lostTimeCounter;
    }
    public override void PhysicsUpdate()
    {
        
    }
    public override void OnExit()
    {
        currentEnemy.animator.SetBool("isHide", false);
        currentEnemy.GetComponent<Character>().invulnerable = false;
    }
}
