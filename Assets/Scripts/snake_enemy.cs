using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class snake_enemy : MonoBehaviour
{
    public GameObject center_ground_point;
    public GameObject left_ground_point;
    public GameObject right_ground_point;
    public GameObject right_ground_detect;
    public GameObject left_ground_detect;
    private Vector2 groundCheckSize = new Vector2(0.05f, 0.05f);
    private Vector2 impactCheckSize = new Vector2(0.2f, 0.05f);
    public LayerMask groundLayer;
    private bool isGrounded;
    private bool isLeft;
    public bool isImpact;
    public Rigidbody2D rb_enemy;
    public snake_enemy snake;
    public float speed;
    public Vector3 velocity;
    public bool detect;
    public bool change;
    public int random_int;
    public Animator animator;
    public int hp;
    public GameMan gammemanager;
    public bool hurt;
    public int id;
    public bool death;
    public AudioSource audio;
    public float timing_sound;

    private void Awake()
    {
        snake=transform.GetComponent<snake_enemy>();
        gammemanager = FindObjectOfType<GameMan>();
        audio = transform.GetComponent<AudioSource>();
        snake.detect = true;
        snake.change = true;
        snake.hurt = false;
        death = false;
        snake.hp = 2;
        timing_sound = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        snake.velocity = new Vector3(1 * speed, 0, 0);
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        timing_sound += Time.deltaTime;
        if (!death)
        {
            snake.animator.SetFloat("speed", (float)Mathf.Abs(snake.velocity.x));
            snake.animator.SetBool("hurt", hurt);
            if (timing_sound > 4)
            {
                audio.Play(0);  
                timing_sound = 0;
            }
            if (Physics2D.Raycast(snake.left_ground_point.transform.position, Vector2.down, 0.1f))
            {
                snake.isGrounded = true;
            }
            else
            {
                snake.isGrounded = false;
                if (snake.detect)
                {
                    snake.velocity *= -1;
                    StartCoroutine(snake.detectColision());
                }
            }
            if (Physics2D.OverlapBox(snake.left_ground_detect.transform.position, impactCheckSize, 0, groundLayer) && snake.detect)
            {
                snake.isImpact = true;
                snake.velocity *= -1;
                StartCoroutine(snake.detectColision());
            }
            else
            {
                snake.isImpact = false;
            }
            if (!hurt)
            {
                snake.tag = "Enemy";
                if (snake.change && snake.detect)
                {
                    snake.random_int = UnityEngine.Random.Range(1, 4);

                    if (snake.random_int == 1)
                    {
                        snake.velocity = Vector3.zero;
                    }
                    if (snake.random_int == 2)
                    {
                        snake.velocity = new Vector3(1 * speed, 0, 0);
                    }
                    if (snake.random_int == 3)
                    {
                        snake.velocity = new Vector3(-1 * speed, 0, 0);
                    }
                    StartCoroutine(snake.changeDirection());
                }
            }
            else
            {
                snake.tag = "Untagged";
                snake.velocity = Vector3.zero;
            }
            if (snake.velocity.x > 0)
            {
                transform.localScale = new Vector3(-5, 5, 0);
            }
            if (snake.velocity.x < 0)
            {
                transform.localScale = new Vector3(5, 5, 0);
            }
        }
    }
    private void FixedUpdate()
    {
        if (!death)
        {
            transform.Translate(snake.velocity);
        }
    }
    IEnumerator detectColision()
    {
        snake.detect = false;
        yield return new WaitForSeconds(0.25f);
        snake.detect = true;
    }
    IEnumerator changeDirection()
    {
        snake.change = false;
        yield return new WaitForSeconds(2f);
        snake.change = true;
    }
    IEnumerator hurting()
    {
        snake.hurt = true;
        yield return new WaitForSeconds(1f);
        snake.hurt = false;
    }
    public void takeDamage()
    {
        snake.hp -= 1;
        StartCoroutine(hurting());
        if (snake.hp == 0)
        {
            gammemanager.updateLlistat(snake.id);
            Death();
            if (SceneManager.GetActiveScene().name == "Game")
            {
                gammemanager.updateSnake();
            }

        }
    }
    public void Death()
    {
        death = true;
        animator.SetTrigger("death");
        transform.tag = "Untagged";
        StartCoroutine(animDeath());

    }
    IEnumerator animDeath()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        gammemanager.AddToListDeath(snake.id);
    }

}
