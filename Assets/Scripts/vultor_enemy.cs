using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
public class vultor_enemy : MonoBehaviour
{
    public GameObject right_ground_detect;
    public GameObject left_ground_detect;
    private Vector2 impactCheckSize = new Vector2(0.2f, 0.05f);
    public LayerMask groundLayer;
    private bool isLeft;
    public bool isImpact;
    public Rigidbody2D rb_enemy;
    public vultor_enemy vultor;
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
    public GameObject rock;
    public GameObject pic;
    public int id;
    private void Awake()
    {
        gammemanager = FindObjectOfType<GameMan>();
        vultor = transform.GetComponent<vultor_enemy>();
        vultor.detect = true;
        vultor.change = true;
        vultor.hurt = false;
        attacking = true;
        vultor.hp = 2;
    }
    // Start is called before the first frame update
    void Start()
    {
        vultor.startingX = vultor.transform.position.x;

    }

    // Update is called once per frame
    void Update()
    {
        vultor.animator.SetBool("hurt", hurt);
        if (fixpatrol)
        {

        }
        else
        {

            if (Physics2D.OverlapBox(vultor.left_ground_detect.transform.position, impactCheckSize, 0, groundLayer) && vultor.detect)
            {
                vultor.isImpact = true;
                vultor.dir *= -1;
                StartCoroutine(vultor.detectColision());
            }
            else
            {
                vultor.isImpact = false;
            }
            if (!hurt)
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
                if ((vultor.transform.position.x < startingX || vultor.transform.position.x > startingX + range) && change)
                {
                    dir *= -1;
                    StartCoroutine(vultor.changeDirection());
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

        if (attacking)
        {
            Instantiate(rock, pic.transform.position, Quaternion.identity);
            StartCoroutine(vultor.atac());
        }
    }
    private void FixedUpdate()
    {
        vultor.transform.Translate(Vector2.right * speed * Time.deltaTime * dir);

    }
    IEnumerator detectColision()
    {
        vultor.detect = false;
        yield return new WaitForSeconds(0.25f);
        vultor.detect = true;
    }
    IEnumerator changeDirection()
    {
        vultor.change = false;
        yield return new WaitForSeconds(2f);
        vultor.change = true;
    }
    IEnumerator hurting()
    {
        vultor.hurt = true;
        yield return new WaitForSeconds(0.4f);
        vultor.hurt = false;
    }
    public void takeDamage()
    {
        vultor.hp -= 1;
        StartCoroutine(hurting());
        if (vultor.hp == 0)
        {
            gammemanager.updateLlistat(vultor.id);
            Destroy(vultor.gameObject);

        }
    }
    IEnumerator atac()
    {
        attacking = false;
        yield return new WaitForSeconds(1.5f);
        attacking = true;
    }
    private void OnDestroy()
    {
        gammemanager.AddToListDeath(vultor.id);
    }
}
