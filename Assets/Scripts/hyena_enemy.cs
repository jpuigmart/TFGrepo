using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Rendering.HybridV2;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
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
    public bool attacking;
    public bool canAtac;
    Vector2 distance;
    public bool checkplayer;
    public float speed_idle = 0.1f;
    public float speed_atac;
    public bool followPj = true;
    public int id;
    private void Awake()
    {
        hyena = transform.GetComponent<hyena_enemy>();
        hyena.detect = true;
        hyena.change = true;
        hyena.hurt = false;
        hyena.hp = 2;
        hyena.dir = 1;
        hyena.attacking = false;
        hyena.checkplayer = false;
        hyena.canAtac = true;
        hyena.followPj = true;
        Time.timeScale = 1;
        speed_idle = 0.1f;
        gammemanager = FindObjectOfType<GameMan>(); 
    }
    // Start is called before the first frame update
    void Start()
    {
        hyena.startingX = hyena.transform.position.x;
        hyena.velocity = new Vector3(1 * speed_idle, 0, 0);
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        hyena.animator.SetFloat("speed", attacking ?  Mathf.Abs(speed_atac) : Mathf.Abs(speed));
        player = GameObject.FindGameObjectWithTag("Player");

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
        hyena.animator.SetBool("hurt", hurt);
        if (Physics2D.Raycast(hyena.center_ground_point.transform.position, Vector2.down, 0.1f))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        if (Vector2.Distance(hyena.transform.position,player.transform.position) < 15 && Math.Abs(player.transform.position.y - transform.position.y) < 3 )
        {
            checkplayer = true;
        }
        else
        {
            checkplayer = false;
        }
        if (!attacking && !hurt && hyena.followPj)
        {
            if (checkplayer)
            {
                float dist = Vector3.Distance(player.transform.position, transform.position);
                if (dist <= 15)
                {
                    if (player.transform.position.x > transform.position.x)
                    {
                        isLeft = true;
                    }
                    else
                    {
                        isLeft = false;
                    }
                    //move to target(player) 

                    if (Math.Abs(player.transform.position.y - transform.position.y) > 3)
                    {
                        checkplayer = false;
                    }
                    if (isGrounded)
                    {
                        Debug.Log("Update");
                        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                    }
                    if (dist <= 5 && canAtac)
                    {
                        hyena.prepareAtac();
                    }

                }

            }
            else
            {

                if (hyena.change && hyena.detect)
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
            }
        }
        if (hurt)
        {
            transform.Translate(Vector3.zero);
        }
        if (attacking)
        {
            Debug.Log("HOLA");
            if (isLeft)
            {
                transform.Translate(Vector3.right  * speed_atac * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector3.right * -1 * speed_atac * Time.deltaTime);
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
    private void FixedUpdate()
    {
        if (!checkplayer && !attacking)
        {
            Debug.Log("FixedUpdate1");
            transform.Translate(Vector3.right * hyena.speed * Time.deltaTime);
        }
        if (!isGrounded && !attacking)
        {
            Debug.Log("FixedUpdate2");
            transform.Translate(Vector3.down * speed * Time.deltaTime);
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
        yield return new WaitForSeconds(0.4f);
        hyena.hurt = false;
    }
    IEnumerator atac()
    {
        yield return new WaitForSeconds(0.4f);
        attacking = false;
        yield return new WaitForSeconds(1.5f);
        followPj = true;
        speed = 4f;
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
             Debug.Log("Hola");
             gammemanager.updateSnake();
             gammemanager.updateLlistat(hyena.id);
             Destroy(hyena.gameObject);

        }
   }
    public void makeAttack()
    {
        checkplayer = false;
        followPj = false;
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
        speed = 0f;
        animator.SetBool("atac", true);
        StartCoroutine(preAtac());

    }
    IEnumerator preAtac()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("atac", false);
        speed_atac = 15f;
        makeAttack();

    }
}
