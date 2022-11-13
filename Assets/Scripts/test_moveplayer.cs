using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public bool death;
    public bool animationDie;

    public Animator animator;
    public bool isLeft = false;
    public bool _retroceso = false;

    [Header("Stats")]
    public int hp;
    public SpriteRenderer color;

    public GameObject dust;
    private GameObject dustInstance;
    Vector2 movement;
    public GameMan gamemanager;
    public bool newAtac = true;
    public AudioManager audiomanager;
    public GameObject dialogueBox;
    public DialogeManager dialogueManager;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        hp = 3;
        color = gameObject.GetComponent<SpriteRenderer>();
        dialogueBox.SetActive(false);
    }
    // Update is called once per frame
    void Start()
    {
        gravityScale = rb.gravityScale;
        death = false;
        animationDie = false;
        isLeft = true;
    }
    void Update()
    {
        animator.SetBool("damaged", _retroceso);
        animator.SetBool("death", death);
        if (!death)
        {
            if (!Damaged)
            {
                color.color = Color.white;
            }


            #region Inputs
            moveInput = Input.GetAxis("Horizontal");
            animator.SetFloat("Speed", Mathf.Abs(moveInput));
            if (!_retroceso)
            {
                if (Input.GetKey(KeyCode.C) && !isJumping)
                {
                    lastJumpTime = jumpBufferTime;
                    if (isLeft && dustInstance == null)
                    {
                        dustInstance = Instantiate(dust, groundCheckPoint.transform.position + Vector3.right, Quaternion.identity);

                    }
                    else if (dustInstance == null && !isLeft)
                    {
                        dustInstance = Instantiate(dust, groundCheckPoint.transform.position - Vector3.right, Quaternion.identity);
                    }
                    Destroy(dustInstance.gameObject, 0.5f);
                }
                if (Input.GetKeyUp(KeyCode.C))
                {
                    OnJumpUp();
                }
            }
            #endregion
            #region Checks


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
                dust.transform.localScale = new Vector3(5, 5, 0);
                transform.localScale = new Vector3(10, 10, 1);
            }
            if (moveInput < 0)
            {
                isLeft = true;
                dust.transform.localScale = new Vector3(-5, 5, 0);
                transform.localScale = new Vector3(-10, 10, 1);
            }
        }
        if (Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, groundLayer))
        {
            lastGroundedTime = jumpInFallTime;
            animator.SetBool("isJumping", false);
            if (Mathf.Abs(rb.velocity.x) > 0.01)
            {
                audiomanager.Unpause("Run");
            }
            else
            {
                audiomanager.Pause("Run");
            }
        }
        else
        {
            audiomanager.Pause("Run");
            animator.SetBool("isJumping", true);
        }
    }

    void FixedUpdate()
    {
        if (!death)
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

        }
        else
        {

            if (!_retroceso)
            {
                StartCoroutine(animationDeath());

            }
        }
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

            if (!Damaged && !death && newAtac)
            {
                doDamage();
            }
        }
        if (collision.tag == "Pickup")
        {
            gamemanager.updatePickup();
            audiomanager.Play("Coin");
            Destroy(collision.gameObject);
        }
        if (collision.tag == "dialogue")
        {
            if (Input.GetKeyUp(KeyCode.E))
            {

                if (dialogueBox.gameObject.activeInHierarchy)
                {
                    Debug.Log("Hola");
                    dialogueManager.DisplayNextSentence();
                }
                else
                {
                    collision.gameObject.GetComponent<DialogueTrigger>().TriggerDialogue();
                    dialogueBox.SetActive(true);
                }
                
                
            }
        }
    }
    public void doDamage()
    {
        this.hp -= 1;
        gamemanager.takeDamage();
        audiomanager.Play("hurt");
        cinemachineShake.Instance.ShakeCamera(5f, 0.1f);
        if (isLeft)
        {
            rb.AddForce(new Vector2(10, 10), ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(new Vector2(-10, 10), ForceMode2D.Impulse);
        }
        StartCoroutine(retroceso());
        StartCoroutine(cooldownDamage());
        if (hp != 0)
        {
            StartCoroutine(damageChangeColor());
        }
        if (hp == 0)
        {
            death = true;
            audiomanager.Stop("Theme");
            rb.velocity = Vector3.zero;
        }
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
        newAtac = false;
        while (Damaged)
        {
            color.color = Color.black;
            yield return new WaitForSeconds(0.1f);
            color.color = Color.white;
            yield return new WaitForSeconds(0.4f);
        }
        newAtac = true;
    }
    IEnumerator animationDeath()
    {
        yield return new WaitForSeconds(3f);
        gamemanager.deathScene();
    }
}
