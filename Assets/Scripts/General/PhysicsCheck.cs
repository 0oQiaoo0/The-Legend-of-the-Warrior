using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    private CapsuleCollider2D capsuleCollider;
    private BoxCollider2D boxCollider;
    [Header("������")]
    public bool manual;
    public Vector2 bottomOffset;
    public Vector2 leftBottomOffset;
    public Vector2 rightBottomOffset;
    public Vector2 leftOffset;
    public Vector2 rightOffset;
    public float checkRadius;
    public LayerMask groundLayer;
    [Header("״̬")]
    public bool isGround;
    public bool leftIsGround;
    public bool rightIsGround;
    public bool touchLeftWall;
    public bool touchRightWall;
    private void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>() ?? null;
        boxCollider = GetComponent<BoxCollider2D>() ?? null;
        if (!manual)
        {
            if(boxCollider)
            {
                bottomOffset = new Vector2(boxCollider.offset.x, boxCollider.offset.y - boxCollider.size.y / 2);
                leftBottomOffset = new Vector2(boxCollider.offset.x - boxCollider.size.x / 2, boxCollider.offset.y - boxCollider.size.y / 2);
                rightBottomOffset = new Vector2(boxCollider.offset.x + boxCollider.size.x / 2, boxCollider.offset.y - boxCollider.size.y / 2);
            }
            if(capsuleCollider)
            {
                rightOffset = new Vector2(capsuleCollider.offset.x + capsuleCollider.bounds.size.x / 2, capsuleCollider.offset.y);
                leftOffset = new Vector2(capsuleCollider.offset.x - capsuleCollider.bounds.size.x / 2, capsuleCollider.offset.y);
            }
        }
        Check();
    }
    private void Update()
    {
        Check();
    }
    public void Check()
    {
        //������
        if (boxCollider)
        {
            isGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, checkRadius, groundLayer);
            leftIsGround = Physics2D.OverlapCircle((Vector2)transform.position + leftBottomOffset, checkRadius, groundLayer);
            rightIsGround = Physics2D.OverlapCircle((Vector2)transform.position + rightBottomOffset, checkRadius, groundLayer);
        }
        //ǽ���ж�
        if (capsuleCollider)
        {
            touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, checkRadius, groundLayer);
            touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, checkRadius, groundLayer);
        }
        
    }

    public void ChangeDirection()
    {
        float tmp;
        if (boxCollider)
        {
            //bottomOffset��ת
            bottomOffset.x = -bottomOffset.x;
            //leftBottomOffset & rightBottomOffset��ת
            tmp = leftBottomOffset.x;
            leftBottomOffset.x = -rightBottomOffset.x;
            rightBottomOffset.x = -tmp;
        }
        if (capsuleCollider)
        {
            //leftOffset & rightOffset��ת
            tmp = leftOffset.x;
            leftOffset.x = -rightOffset.x;
            rightOffset.x = -tmp;
        }
        
    }

    private void OnDrawGizmosSelected()
    {
        if (boxCollider)
        {
            Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, checkRadius);
            Gizmos.DrawWireSphere((Vector2)transform.position + leftBottomOffset, checkRadius);
            Gizmos.DrawWireSphere((Vector2)transform.position + rightBottomOffset, checkRadius);
        }
        if (capsuleCollider)
        {
            Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, checkRadius);
            Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, checkRadius);
        }
    }
}
