using System.Collections;
using System.Collections.Generic;
using General;
using UnityEngine;

[RequireComponent(typeof(PhysicsCheck))]
public abstract class GroundEnemy : Enemy.Enemy
{
    [HideInInspector] public PhysicsCheck physicsCheck;

    [Header("检测玩家")]
    public Vector2 checkSize;

    protected override void Awake()
    {
        base.Awake();
        physicsCheck = GetComponent<PhysicsCheck>();
    }

    public override bool FoundPlayer()
    {
        return Physics2D.BoxCast(transform.position + (Vector3)centerOffset, checkSize, 0, new Vector2(faceDir, 0), checkDistance, attackLayer);
    }

    public override void ChangeDirection()
    {
        base.ChangeDirection();
        physicsCheck.ChangeDirection();
    }

    public bool CheckFrontWall()
    {
        if (physicsCheck.touchLeftWall && faceDir == -1 || physicsCheck.touchRightWall && faceDir == 1)
        {
            return true;
        }
        else return false;
    }

    public bool CheckFrontGround()
    {
        if (physicsCheck.leftIsGround && faceDir == -1 || physicsCheck.rightIsGround && faceDir == 1)
        {
            return true;
        }
        else return false;
    }
}
