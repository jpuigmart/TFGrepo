using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class hyena_enemy : MonoBehaviour
{
    public GameObject center_ground_point;
    public GameObject left_ground_point;
    public GameObject right_ground_point;
    public GameObject right_ground_detect;
    public GameObject left_ground_detect;
    private Vector2 groundCheckSize = new Vector2(0.05f, 0.05f);
    private Vector2 impactCheckSize = new Vector2(0.2f, 0.05f);
    public LayerMask groundLayer;
    public bool isGrounded;
    private bool isLeft;
    public bool isImpact;
    public hyena_enemy hyena;
    public GameObject player;
    public Rigidbody2D rb_hyena;
    public float speed;
    public Vector3 velocity;
    public bool detect;
    public bool change;
    public int random_int;
    public Animator animator;
    public int hp;
    public GameMan gammemanager;
    public bool hurt;
    public bool fixpatrol;
    public int dir;
    public float range = 3;
    float startingX;
    public bool attacking;
    public bool canAtac;
    Vector2 distance;
    public bool checkplayer;
    public float speed_idle = 0.1f;
    public float speed_atac;
    public bool followPj = true;
    public int id;
    public float timing;
    public bool preattacking;
    public bool death;
    public AudioSource audio;
    public float timing_sound;
    private void Awake()
    {
        hyena = transform.GetComponent<hyena_enemy>();
        rb_hyena = transform.GetComponent<Rigidbody2D>();
        hyena.detect = true;
        hyena.change = true;
        hyena.hurt = false;
        hyena.hp = 2;
        hyena.dir = 1;
        hyena.attacking = false;
        hyena.checkplayer = false;
        hyena.canAtac = true;
        hyena.followPj = true;
        hyena.preattacking = false;
        death = false;
        Time.timeScale = 1;
        timing = 0f;
        speed_idle = 0.1f;
        gammemanager = FindObjectOfType<GameMan>();
        audio = transform.GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {

        if (!death)
        {
            timing += Time.deltaTime;

            Debug.Log(hyena.timing);
            hyena.animator.SetFloat("speed", attacking ? Mathf.Abs(speed_atac) : Mathf.Abs(speed));
            player = GameObject.FindGameObjectWithTag("Player");
            hyena.animator.SetBool("hurt", hurt);

            #region Impact
            if (Physics2D.OverlapBox(hyena.left_ground_detect.transform.position, impactCheckSize, 0, groundLayer) && hyena.detect)
            {
                hyena.speed *= -1;
                StartCoroutine(hyena.detectColision());
            }
            #endregion
            #region Grounded
            if (Physics2D.Raycast(hyena.center_ground_point.transform.position, Vector2.down, 0.1f))
            {
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
            #endregion
            #region Fall
            if (!Physics2D.Raycast(hyena.left_ground_point.transform.position, Vector2.down, 0.1f))
            {
                if (timing > 2f)
                {
                    speed *= -1;
                    timing = 0;
                }

            }
            #endregion
            #region checkPlayer
            if (Vector2.Distance(hyena.transform.position, player.transform.position) < 15 && Math.Abs(player.transform.position.y - transform.position.y) < 2)
            {
                checkplayer = true;
            }
            else
            {
                checkplayer = false;
            }

            #endregion
            if (!hurt)
            {
                hyena.tag = "Enemy";
                if (checkplayer)
                {
                    timing = 0;
                    float dist = Vector3.Distance(player.transform.position, transform.position);
                    if (dist > 1f)
                    {
                        if (player.transform.position.x > transform.position.x)
                        {
                            speed = 4f;
                        }
                        else
                        {
                            speed = -4f;
                        }
                    }
                    else
                    {
                        speed = 0f;
                    }


                    if (dist <= 5 && canAtac)
                    {
                        hyena.prepareAtac();
                    }

                }
                else
                {
                    if (hyena.timing > 2f)
                    {
                        hyena.random_int = UnityEngine.Random.Range(1, 4);

                        if (hyena.random_int == 1)
                        {
                            hyena.speed = 0f;
                        }
                        if (hyena.random_int == 2)
                        {
                            hyena.speed = 4f;
                        }
                        if (hyena.random_int == 3)
                        {
                            hyena.speed = -4f;
                        }
                        timing = 0;
                    }
                }
            }
            else
            {
                hyena.speed = 0f;
                hyena.tag = "Untagged";
            }

            #region isLeft
            if (rb_hyena.velocity.x > 0)
            {
                isLeft = true;
                transform.localScale = new Vector3(-5, 5, 0);
            }
            if (rb_hyena.velocity.x < 0)
            {
                isLeft = false;
                transform.localScale = new Vector3(5, 5, 0);
            }
            #endregion
            if (attacking || preattacking)
            {
                transform.Translate(Vector3.right * speed_atac * Time.deltaTime);
            }
        }
    }
    private void FixedUpdate()
    {

        if (!attacking && !preattacking && !death)
        {
            rb_hyena.velocity = new Vector2(speed, 0);
        }


    }
    IEnumerator detectColision()
    {
        hyena.detect = false;
        yield return new WaitForSeconds(0.25f);
        hyena.detect = true;
    }
    IEnumerator changeDirection()
    {
        hyena.change = false;
        yield return new WaitForSeconds(2f);
        hyena.change = true;
    }
    IEnumerator hurting()
    {
        hyena.hurt = true;
        yield return new WaitForSeconds(1f);
        hyena.hurt = false;
    }
    IEnumerator atac()
    {
        yield return new WaitForSeconds(0.4f);
        attacking = false;
    }
    IEnumerator canMakeAtac()
    {
        canAtac = false;
        yield return new WaitForSeconds(2f);
        canAtac = true;
    }
    public void takeDamage()
    {
        hyena.hp -= 1;
        StartCoroutine(hurting());
        if (hyena.hp == 0)
        {
            gammemanager.updateLlistat(hyena.id);
            hyena.Death();
            if (SceneManager.GetActiveScene().name == "Game")
            {
                gammemanager.updateSnake();
            }

        }
    }
    public void Death()
    {
        rb_hyena.velocity = Vector2.zero;
        hyena.death = true;
        animator.SetTrigger("death");
        transform.tag = "Untagged";
        StartCoroutine(animDeath());
    }
    IEnumerator animDeath()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
    public void makeAttack()
    {
        attacking = true;
        StartCoroutine(atac());
        StartCoroutine(canMakeAtac());

    }

    private void OnDestroy()
    {
        gammemanager.AddToListDeath(hyena.id);
    }
    public void prepareAtac()
    {
        audio.Play();
        preattacking = true;
        animator.SetBool("atac", true);
        speed_atac = 0f;
        rb_hyena.velocity = Vector2.zero;
        StartCoroutine(preAtac());

    }
    IEnumerator preAtac()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("atac", false);
        preattacking = false;
        if (isLeft)
        {
            speed_atac = 15f;
        }
        else
        {
            speed_atac = -15f;
        }

        makeAttack();

    }
    public void check_move(float speed)
    {
        if (Physics2D.OverlapBox(hyena.left_ground_detect.transform.position, impactCheckSize, 0, groundLayer) && hyena.detect)
        {
            hyena.isImpact = true;
            hyena.speed *= -1;
            isLeft = !isLeft;
            StartCoroutine(hyena.detectColision());
        }
        else
        {
            hyena.isImpact = false;
        }
        if (Physics2D.Raycast(hyena.center_ground_point.transform.position, Vector2.down, 0.1f))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        if (Physics2D.Raycast(hyena.left_ground_point.transform.position, Vector2.down, 0.1f))
        {
            hyena.isGrounded = true;
        }
        else
        {
            if (hyena.detect)
            {
                hyena.speed *= -1;
                StartCoroutine(hyena.detectColision());
            }
        }
        if (isLeft)
        {
            transform.localScale = new Vector3(-5, 5, 0);
        }
        else
        {
            transform.localScale = new Vector3(5, 5, 0);
        }
    }
    public void move_random()
    {

        hyena.random_int = UnityEngine.Random.Range(1, 4);

        if (hyena.random_int == 1)
        {
            hyena.speed = 0f;
        }
        if (hyena.random_int == 2)
        {
            hyena.speed = 4f;
            isLeft = true;
        }
        if (hyena.random_int == 3)
        {
            hyena.speed = -4f;
            isLeft = false;
        }
        StartCoroutine(hyena.changeDirection());
    }
    public void move_hyena()
    {
        float dist = Vector3.Distance(player.transform.position, transform.position);
        if (dist <= 15)
        {
            if (player.transform.position.x > transform.position.x)
            {
                hyena.isLeft = true;
            }
            else
            {
                hyena.isLeft = false;
            }
            //move to target(player) 

            if (Math.Abs(player.transform.position.y - transform.position.y) > 1)
            {
                checkplayer = false;
            }
            if (hyena.isGrounded)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            }
            if (dist <= 5 && canAtac)
            {
                hyena.prepareAtac();
            }

        }
    }
}


