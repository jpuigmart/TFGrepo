using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class test_moveplayer : MonoBehaviour
{
    
    [Header("References")]
    public Rigidbody2D rb;
    [Header("Movement")]
    public float moveSpeed = 3f;
    public float acceleration;
    public float decceleration;
    public float velPower;
    [Space(10)]
    private float moveInput;
    [Space(10)]
    public float frictionAmount;
    [Header("Jump")]
    public float jumpforce = 10;
    [Range(0, 1)]
    public float jumpCutMultiplier;
    [Space(10)]
    public float jumpInFallTime;
    private float lastGroundedTime;
    public float jumpBufferTime;
    private float lastJumpTime;
    [Space(10)]
    public float fallGravityMultiplier;
    private float gravityScale;
    [Space(10)]
    private bool isJumping;
    private bool jumpInputReleased;
    
    [Header("Checks")]
    public Transform groundCheckPoint;
    public Vector2 groundCheckSize;
    [Space(10)]
    public LayerMask groundLayer;
    public bool Damaged = false;

    public Animator animator;
    public bool isLeft = false;
    public bool _retroceso = false;

    [Header("Stats")]
    public int hp;
    public SpriteRenderer color;

    Vector2 movement;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        hp = 3;
        color = gameObject.GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void Start()
    {
        gravityScale = rb.gravityScale;
        
    }
    void Update()
    {
        animator.SetBool("damaged", _retroceso);
        if (hp == 0)
        {
            SceneManager.LoadScene(1);
        }

 
        #region Inputs
        moveInput = Input.GetAxis("Horizontal");
        animator.SetFloat("Speed", Mathf.Abs(moveInput));
        if (!_retroceso)
        {
            if (Input.GetKey(KeyCode.C))
            {
                lastJumpTime = jumpBufferTime;
            }
            if (Input.GetKeyUp(KeyCode.C))
            {
                OnJumpUp();
            }
        }
        #endregion
        #region Checks
        if (Physics2D.OverlapBox(groundCheckPoint.position,groundCheckSize,0,groundLayer))
        {
            lastGroundedTime = jumpInFallTime;
            animator.SetBool("isJumping", false);
        }
        else
        {
            animator.SetBool("isJumping", true);
        }

        if (rb.velocity.y <= 0 && jumpInputReleased)
        {
            isJumping = false;
        }
        #endregion
        #region Jump
        if (lastGroundedTime > 0 && lastJumpTime > 0 && !isJumping)
        {
            Jump();
        }
        #endregion
        #region Timer
        lastGroundedTime -= Time.deltaTime;
        lastJumpTime -= Time.deltaTime;
        #endregion
        if (moveInput > 0.01f)
        {
            isLeft = false;
            transform.localScale = new Vector3(10,10,1);
        }
        if (moveInput < 0)
        {
            isLeft = true;
            transform.localScale = new Vector3(-10, 10, 1);
        }
    }

    void FixedUpdate()
    {
        if (!_retroceso)
        {
            #region Run
            float targetSpeed = moveInput * moveSpeed;

            float speedDif = targetSpeed - rb.velocity.x;

            float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;

            float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);

            rb.AddForce(movement * Vector2.right);
        }
        #endregion 
        //movement = new Vector2(horizontalMove, 0f);
        //rb.velocity = new Vector2(horizontalMove*speed,rb.velocity.y);
        #region Friction
        if (lastGroundedTime > 0 && Mathf.Abs(moveInput) < 0.01f)
        {
            float amount = Mathf.Min(Mathf.Abs(rb.velocity.x), Mathf.Abs(frictionAmount));

            amount *= Mathf.Sign(rb.velocity.x);
            if (!_retroceso)
            {
                rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
            }
        }
        #endregion
        #region Jump Gravity
        if (rb.velocity.y < 0 && lastGroundedTime <= 0)
        {
            rb.gravityScale = gravityScale * fallGravityMultiplier;
        }
        else
        {
            rb.gravityScale = gravityScale;
        }
        #endregion
    }
    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
        lastGroundedTime = 0;
        lastJumpTime = 0;
        isJumping = true;
        jumpInputReleased = false;

    }
    public void OnJump()
    {
        lastJumpTime = jumpBufferTime;
        jumpInputReleased = false;
    }
    public void OnJumpUp()
    {
        if (rb.velocity.y > 0 && isJumping)
        {
            rb.AddForce(Vector2.down * rb.velocity.y * jumpCutMultiplier, ForceMode2D.Impulse);
        }
        jumpInputReleased = true;
        lastJumpTime = 0;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            if (!Damaged)
            {
                doDamage();
            }
        }
    }
    public void doDamage()
    {
        this.hp -= 1;
        cinemachineShake.Instance.ShakeCamera(5f, 0.1f);
        if (rb.velocity.x < 0)
        {
            rb.AddForce(new Vector2(10, 10), ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(new Vector2(-10, 10), ForceMode2D.Impulse);
        }
        StartCoroutine(retroceso());
        StartCoroutine(cooldownDamage());
        StartCoroutine(damageChangeColor());
    }
    IEnumerator cooldownDamage()
    {
        Damaged = true;
        yield return new WaitForSeconds(3);
        Damaged = false;
    }
    IEnumerator retroceso()
    {
        _retroceso = true;
        yield return new WaitForSeconds(0.5f);
        _retroceso = false;
    }
    IEnumerator damageChangeColor()
    {
        while (Damaged)
        {
            color.color = Color.black;
            yield return new WaitForSeconds(0.5f);
            color.color = Color.white;
            yield return new WaitForSeconds(0.5f);
        }

    }
}
