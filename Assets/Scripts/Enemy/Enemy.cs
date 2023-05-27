using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D),typeof(Animator))]
public abstract class Enemy : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Animator animator;
    
    [Header("��������")]
    public float normalSpeed;
    public float chaseSpeed;
    [HideInInspector] public float currentSpeed;
    protected int faceDir;// right 1 left -1;
    /// <summary>
    /// ��������ʱ�ܵ�����
    /// </summary>
    public float hurtForce;
    /// <summary>
    /// �������˵ĳ���ʱ��
    /// </summary>
    public float hurtDuration;
    [HideInInspector] public Transform attacker;
    [Header("������")]
    public Vector2 centerOffset;
    public float checkDistance;
    public LayerMask attackLayer;

    [Header("��ʱ��")]
    public float waitTime;
    [HideInInspector] public float waitTimeCounter;

    public float lostTime;
    [HideInInspector] public float lostTimeCounter;
    [Header("״̬")]
    public bool isWait;
    public bool isHurt;
    public bool isDead;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentSpeed = normalSpeed;
        waitTimeCounter = waitTime;
        lostTimeCounter = lostTime;
        faceDir = -1 * (int)transform.localScale.x;
    }
    protected abstract void OnEnable();

    protected abstract void Update();

    protected abstract void FixedUpdate();

    protected abstract void OnDisable();
    
    public virtual void Move()
    {
        rb.velocity = new Vector2(currentSpeed * faceDir * Time.deltaTime, rb.velocity.y);
    }
    public virtual void ChangeDirection()
    {
        //change faceDir
        faceDir = -faceDir;
        //change scale
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
    /// <summary>
    /// ��ʱ��
    /// </summary>
    protected virtual void TimeCounter()
    {
        //waitTimeCounter
        if (isWait)
        {
            waitTimeCounter -= Time.deltaTime;
            //after wait
            if(waitTimeCounter < 0)
            {
                ChangeDirectionWithJudge();
                isWait = false;
                AfterWaitSetAnimator();
                waitTimeCounter = waitTime;
            }
        }

        //lostTimeCounter
        if(currentSpeed == chaseSpeed) // is chasing
        if (!FoundPlayer())
        {
            lostTimeCounter -= Time.deltaTime;
            // losted
            if(lostTimeCounter <= 0)
            {
                lostTimeCounter = lostTime;
                SwitchState(NPCState.Patrol);
            }
        }
        else
        {
            lostTimeCounter = lostTime;
        }
    }
    public virtual void ChangeDirectionWithJudge()
    {
        ChangeDirection();
    }

    protected virtual void AfterWaitSetAnimator()
    {
        animator.SetBool("isWalk", true);
    }

    public abstract bool FoundPlayer();
    

    public abstract void SwitchState(NPCState state);
    #region �¼�ִ�з���
    public void OnTakeDamage(Transform attackTrans)
    {
        attacker = attackTrans;

        //ת��
        Vector2 dir = new Vector2(transform.position.x - attackTrans.position.x,0).normalized;
        if (dir.x < 0 && faceDir == -1 || dir.x > 0 && faceDir == 1) 
        {
            ChangeDirection();
        }

        //���˱�����
        isHurt = true;
        animator.SetTrigger("hurt");
        StartCoroutine(OnHurt(dir));
    }
    /// <summary>
    /// Unable to move for a period of time after being hit.
    /// </summary>
    /// <param name="dir">Defensive direction.</param>
    /// <returns></returns>
    IEnumerator OnHurt(Vector2 dir)
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(hurtDuration);
        isHurt = false;
    }

    /// <summary>
    /// execute when enemy die.
    /// Used by OnDie event in character.
    /// </summary>
    public void OnDie()
    {
        gameObject.layer = 2;
        animator.SetBool("isDead", true);
        isDead = true;
    }
    /// <summary>
    /// Destroy this gameObject.
    /// Used by dead animation.
    /// </summary>
    public void DestroyAfterAnimation()
    {
        Destroy(this.gameObject);
    }
    #endregion

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + (Vector3)centerOffset + new Vector3(checkDistance * -transform.localScale.x, 0), 0.2f);
    }
}
