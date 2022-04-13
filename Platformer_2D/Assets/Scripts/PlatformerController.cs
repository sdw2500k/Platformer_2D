using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerController : MonoBehaviour
{
    //공격 시퀀스 정의 - 1타, 2타
    enum AttackSequence { None = 0, Attack_A, Attack_B }

    Rigidbody2D rigidBody;

    public SpriteRenderer body;
    public Animator bodyAnimator;

    public float maxWalkSpeed;
    public float maxDashSpeed;

    public float jumpScale;

    //공중 조작
    bool isFall;
    bool isJump;

    //추가이동 조작
    bool isDash;

    //공격 시퀀스
    AttackSequence attackSequence;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float horizontalAxis = Input.GetAxisRaw("Horizontal");
        CheckHorizontalAxis(horizontalAxis);

        RaycastHit2D rayHit = Physics2D.Raycast(rigidBody.position, Vector3.down, 1, LayerMask.GetMask("Map"));

        if (rigidBody.velocity.y < 0)
        {
            if (isFall == false)
            {
                isFall = true;
                bodyAnimator.SetBool("isFall", true);
            }
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 0.2f)
                {
                    isFall = false;
                    isJump = false;
                    bodyAnimator.SetBool("isJump", false);
                    bodyAnimator.SetBool("isFall", false);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckSequence_Walk();
        CheckSequence_Jump();

        CheckSequence_Attack();
        CheckSequence_Dash();
    }

    void CheckHorizontalAxis(float horizontalAxis)
    {
        if (horizontalAxis == 0 || isDash == true)
        {
            return;
        }
        body.flipX = (horizontalAxis < 0);
        rigidBody.AddForce(Vector2.right * horizontalAxis, ForceMode2D.Impulse);

       if (rigidBody.velocity.x > maxWalkSpeed)
       {
            rigidBody.velocity = new Vector2(maxWalkSpeed, rigidBody.velocity.y);
       }

       if (rigidBody.velocity.x < (-1) * maxWalkSpeed)
       {
            rigidBody.velocity = new Vector2((-1) * maxWalkSpeed, rigidBody.velocity.y);
       }
    }
    
    void CheckSequence_Walk()
    {
        bool isHorizontal = Input.GetButtonUp("Horizontal");
        if (isHorizontal == true)
        {
            rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
        }


        bodyAnimator.SetBool("isWalk", (Mathf.Abs(rigidBody.velocity.x) > 0.01f));
    }

    void CheckSequence_Jump()
    {
        if(isJump == true)
        {
            return;
        }

        isJump = Input.GetButtonDown("Jump");
        if (isJump == true)
        {
            rigidBody.AddForce(Vector2.up * jumpScale, ForceMode2D.Impulse);

            bodyAnimator.SetBool("isJump", true);
        }
    }

    void CheckSequence_Attack()
    {
        bool isAttack = Input.GetButtonDown("Attack");

        if(isAttack == true)
        {
            switch(attackSequence)
            {
                case AttackSequence.None:
                    attackSequence = AttackSequence.Attack_A;
                    bodyAnimator.SetInteger("attackSequence", 1);
                    break;

                case AttackSequence.Attack_A:
                    if(isJump || isFall)
                    {
                        break;
                    }
                    attackSequence = AttackSequence.Attack_B;
                    break;

                default:
                    break;
            }
        }
    }

    void CheckSequence_Dash()
    {
        if(isDash == false)
        {
            isDash = Input.GetButtonDown("Dash");

            if(isDash == true)
            {
                float horizontalAxis = (body.flipX == true) ? -1.0f : 1.0f;
                rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
                rigidBody.AddForce(Vector2.right * horizontalAxis * maxDashSpeed, ForceMode2D.Impulse);
                bodyAnimator.SetBool("isDash", true);
            }
        }

        if(isDash == true)
        {
            if(Mathf.Abs(rigidBody.velocity.x) < 3)
            {
                isDash = false;
                bodyAnimator.SetBool("isDash", false);
            }
        }
    }

    public void AttackEnd(int seq)
    {
        if(seq == (int)AttackSequence.Attack_A)
        {
            if(attackSequence == AttackSequence.Attack_A || isJump || isFall)
            {
                attackSequence = AttackSequence.None;
            }

            bodyAnimator.SetInteger("attackSequence", (int)attackSequence);
            return;
        }

        if(seq == 2)
        {
            if (attackSequence != AttackSequence.Attack_A)
            {
                attackSequence = AttackSequence.None;
            }
            bodyAnimator.SetInteger("attackSequence", (int)attackSequence);
            return;
        }
    }
}
