using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    [Header("Dash")]
    public float moveDash = 18f;
    public float accelerationDash;
    public float deccelerationDash;
    public float dashPower;
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
    public bool attacking;
    [Space(10)]
    public LayerMask groundLayer;
    public LayerMask enemiesLayer;
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
    public bool inDialogue;
    public bool canDialogue;
    public bool canDash;
    public bool dashing;
    public bool canAtack;
    public TextMeshProUGUI finalSentence;
    public GameObject hitboxSword;
    public GameObject interactbutton;
    public bool interact;
    public GameObject npcavi;
    public GameObject npctrigger;
    private altar altar;
    public static test_moveplayer instance;
    public interactable interactable;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        rb = GetComponent<Rigidbody2D>();
        hp = 3;
        color = gameObject.GetComponent<SpriteRenderer>();
        audiomanager = FindObjectOfType<AudioManager>();
        gamemanager = FindObjectOfType<GameMan>();
        dialogueBox.SetActive(false);
        inDialogue = false;
        canDialogue = true;
        canDash = true;
        dashing = false;
        attacking = false;
        canAtack = true;
        interact = false;
        interactbutton.SetActive(interact);

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
        interactbutton.SetActive(interact);
        animator.SetBool("damaged", _retroceso);
        animator.SetBool("death", death);
        animator.SetBool("dash", dashing);
        animator.SetFloat("Speed", Mathf.Abs(moveInput));


        if (interact)
        {
            if (Input.GetKeyUp(KeyCode.E))
            {
                interactable.Use();
            }
        }
        if (inDialogue)
        {
            canDialogue = false;
            if (Input.GetKeyDown(KeyCode.E))
            {
                dialogueManager.DisplayNextSentence();
                if (finalSentence.text == "¡Volvamos a casa!")
                {
                    audiomanager.Stop("Theme");
                    interact = false;
                    SceneManager.LoadScene("level1");
                }
                if (!inDialogue && canDialogue)
                {
                    npctrigger.gameObject.GetComponent<DialogueTrigger>().TriggerDialogue();
                }
                if (!gamemanager.questPickup && !gamemanager.questPickupFinished)
                {
                    gamemanager.QuestActive();
                }
                if (gamemanager.questPickupFinished)
                {
                    gamemanager.QuestFinish();
                }
            }

        }

        else
        {
            if (!dashing)
            {
                moveInput = Input.GetAxis("Horizontal");
            }

            if (!death)
            {
                if (!Damaged)
                {
                    color.color = Color.white;
                }

                if (!_retroceso | !dashing)
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
                    if (Input.GetKey(KeyCode.C) && isJumping && gamemanager.learnDobleJump)
                    {
                        lastJumpTime = jumpBufferTime;
                    }
                    if (Input.GetKeyUp(KeyCode.C))
                    {
                        OnJumpUp();
                    }
                }

                #region Checks

                if (Input.GetKeyDown(KeyCode.X) && canAtack)
                {
                    animator.SetTrigger("attack");

                    StartCoroutine(makeAttack());
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
                    dust.transform.localScale = new Vector3(5, 5, 0);
                    transform.localScale = new Vector3(10, 10, 1);
                }
                if (moveInput < 0)
                {
                    isLeft = true;
                    dust.transform.localScale = new Vector3(-5, 5, 0);
                    transform.localScale = new Vector3(-10, 10, 1);
                }
                if (canDash)
                {
                    if (Input.GetKeyDown(KeyCode.Z))
                    {
                        StartCoroutine(makeDash());
                    }
                }
            }
            if (Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, groundLayer))
            {
                lastGroundedTime = jumpInFallTime;
                animator.SetBool("isJumping", false);
                if (!dashing)
                {
                    canDash = true;
                }
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
    }

    void FixedUpdate()
    {
        if (inDialogue)
        {
            rb.velocity = Vector2.zero;
        }
        else
        {
            if (!death)
            {
                if (!_retroceso && !dashing)
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
            if (dashing)
            {
                rb.gravityScale = 0f;
                float modifier;
                if (isLeft)
                {
                    modifier = -1f;
                    
                }
                else
                {
                    modifier = 1f;
                }

                float targetSpeed = modifier * moveDash;

                float speedDif = targetSpeed - rb.velocity.x;

                float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? accelerationDash : deccelerationDash;

                float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, dashPower) * Mathf.Sign(speedDif);

                rb.AddForce(movement * Vector2.right);
            }
        }
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "trigger_boss")
        {
            gamemanager.boss_fight = true;
            GameObject gameObject = GameObject.FindGameObjectWithTag("pared_boss");
            gameObject.GetComponent<Collider2D>().isTrigger = false;
            Destroy(collision.gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" || collision.tag == "boss")
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
        if (collision.tag == "Potion")
        {
            if (this.hp < 3)
            {
                this.hp += 1;
                gamemanager.takeHeal();
                Destroy(collision.gameObject);
            }

        }
        if (collision.tag == "PotionExtra")
        {
            gamemanager.TakeExtraLife();
            Destroy(collision.gameObject);
        }
        if (collision.tag == "rock")
        {
            if (!Damaged && !death && newAtac)
            {
                doDamage();
            }
            Destroy(collision.gameObject);
        }
        if (collision.tag == "traps")
        {
            if (!Damaged && !death && newAtac)
            {
                doDamage();
            }
        }
        if (collision.tag == "interact")
        {
            interact = true;
            interactable = collision.GetComponent<interactable>();
        }
        if (collision.tag == "clau_blau")
        {
            Destroy(collision.gameObject);
            GameObject tmp = GameObject.FindGameObjectWithTag("bloc_blau");
            Destroy(tmp.gameObject);
        }
        if (collision.tag == "clau_groc")
        {
            Destroy(collision.gameObject);
            GameObject tmp = GameObject.FindGameObjectWithTag("bloc_groc");
            Destroy(tmp.gameObject);
        }
        if (collision.tag == "clau_marro")
        {
            Destroy(collision.gameObject);
            GameObject tmp = GameObject.FindGameObjectWithTag("bloc_marro");
            Destroy(tmp.gameObject);
        }
        if (collision.tag == "clau_tronja")
        {
            Destroy(collision.gameObject);
            GameObject tmp = GameObject.FindGameObjectWithTag("bloc_tronja");
            Destroy(tmp.gameObject);
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "interact")
        {
            interact = false;
        }
    }
    public void doDamage()
    {
        if (dashing)
        {
            dashing = false;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        gamemanager.takeDamage();
        audiomanager.Play("hurt");
        cinemachineShake.Instance.ShakeCamera(5f, 0.1f);
        rb.velocity = Vector2.zero;
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
            gamemanager.boss_fight = true;
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
    IEnumerator makeDash()
    {
        dashing = true;
        canDash = false;
        rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        yield return new WaitForSeconds(0.5f);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        dashing = false;

    }
    public IEnumerator canStartDialogue()
    {
        yield return new WaitForSeconds(1f);
        canDialogue = true;
    }
    void attack()
    {

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(hitboxSword.transform.position, 1f, enemiesLayer);
        foreach (Collider2D hit in hitEnemies)
        {
            if (hit.tag == "Enemy")
            {
                if (hit.gameObject.TryGetComponent(out snake_enemy enem))
                {
                    snake_enemy enemy = enem;
                    enem.takeDamage();
                }
                if (hit.gameObject.TryGetComponent(out hyena_enemy enem1))
                {
                    hyena_enemy enemy = enem1;
                    enem1.takeDamage();
                }
                if (hit.gameObject.TryGetComponent(out vultor_enemy enem2))
                {
                    vultor_enemy enemy = enem2;
                    enem2.takeDamage();
                }

                cinemachineShake.Instance.ShakeCamera(5f, 0.1f);
            }
            if (hit.tag == "boss")
            {
                if (hit.gameObject.TryGetComponent(out boss_enemy enem4))
                {
                    boss_enemy enemy = enem4;
                    enem4.takeDamage();
                }
            }

        }
    }
    IEnumerator makeAttack()
    {
        canAtack = false;
        yield return new WaitForSeconds(0.2f);
        attack();
        yield return new WaitForSeconds(0.2f);
        canAtack = true;
    }
}
