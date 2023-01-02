using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Rendering.HybridV2;
using UnityEditor.Build;
using UnityEngine;
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
    private bool isGrounded;
    private bool isLeft;
    public bool isImpact;
    public Rigidbody2D rb_enemy;
    public hyena_enemy hyena;
    public GameObject player;
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
    bool attacking;
    private void Awake()
    {
        hyena = transform.GetComponent<hyena_enemy>();
        player = GameObject.FindGameObjectWithTag("Player");
        hyena.detect = true;
        hyena.change = true;
        hyena.hurt = false;
        hyena.hp = 2;
        hyena.dir = 1;
        hyena.attacking = false;
        Time.timeScale = 1;
    }
    // Start is called before the first frame update
    void Start()
    {
        hyena.startingX = hyena.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        hyena.animator.SetFloat("speed", 1);
        hyena.animator.SetBool("hurt", hurt);
        if (fixpatrol)
        {
            if (Physics2D.Raycast(hyena.left_ground_point.transform.position, Vector2.down, 0.1f))
            {
                hyena.dir *= 1;
            }
            else
            {
                hyena.dir *= -1;
                if (hyena.detect)
                {
                    hyena.dir *= -1;
                    StartCoroutine(hyena.detectColision());
                }
            }
           
        }
        else
        {

            if (Physics2D.OverlapBox(hyena.left_ground_detect.transform.position, impactCheckSize, 0, groundLayer) && hyena.detect)
            {
                hyena.isImpact = true;
                hyena.dir *= -1;
                StartCoroutine(hyena.detectColision());
            }
            else
            {
                hyena.isImpact = false;
            }
            if (!hurt)
            {
                if (Math.Abs(player.transform.position.x - hyena.transform.position.x) < 15)
                {
                    if (player.transform.position.x > hyena.transform.position.x)
                    {
                        dir = 1;
                    }
                    else
                    {
                        dir = -1;
                    }
                    if (Math.Abs(player.transform.position.x - hyena.transform.position.x) < 5 && !attacking)
                    {
                        rb_enemy.AddForce(Vector2.right * dir * 10,ForceMode2D.Impulse);
                        StartCoroutine(atac());
                    }
                }
                else
                {
                    if (dir == 0)
                    {
                        if (isLeft)
                        {
                            dir = 1;
                        }
                        else
                        {
                            dir = -1;
                        }
                    }
                    if ((hyena.transform.position.x < startingX || hyena.transform.position.x > startingX + range) && change)
                    {
                        dir *= -1;
                        StartCoroutine(hyena.changeDirection());
                    }
                }
                if (!Physics2D.OverlapBox(hyena.center_ground_point.transform.position, impactCheckSize, 0, groundLayer))
                {
                    hyena.transform.Translate(Vector2.down * speed * Time.deltaTime);
                }
            }
            else
            {
                dir *= 0;
            }

        }
        if (dir == 1)
        {
            transform.localScale = new Vector3(-5, 5, 0);
            isLeft = true;
        }
        if(dir == -1)
        {
            isLeft = false;
            transform.localScale = new Vector3(5, 5, 0);
        }
    }
    private void FixedUpdate()
    {
        hyena.transform.Translate(Vector2.right * speed * Time.deltaTime * dir);

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
        yield return new WaitForSeconds(0.5f);
        hyena.change = true;
    }
    IEnumerator hurting()
    {
        hyena.hurt = true;
        yield return new WaitForSeconds(0.4f);
        hyena.hurt = false;
    }
    IEnumerator atac()
    {
        attacking = true;
        yield return new WaitForSeconds(3f);
        attacking = false;
    }
    public void takeDamage()
    {
        hyena.hp -= 1;
        StartCoroutine(hurting());
        if (hyena.hp == 0)
        {
            gammemanager.updateSnake();
            Destroy(hyena.gameObject);

        }
    }
}
