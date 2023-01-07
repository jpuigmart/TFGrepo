using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class boss_enemy : MonoBehaviour
{
    public GameObject player;
    public int state;
    public bool change;
    public bool detect;
    public int random_int;
    public float speed;
    public bool isLeft;
    public GameObject right_ground_detect;
    public GameObject left_ground_detect;
    public GameObject attack_point;
    public GameObject arrow;
    public GameMan gamemanager;
    public LayerMask groundLayer;
    private Vector2 impactCheckSize = new Vector2(0.2f, 0.05f);
    public bool attacking1;
    public bool attacking2;
    public bool preattacking1;
    public bool preattacking2;
    public Vector2 direction;
    private Vector3 lastposition;
    public Vector3 player_position;
    public bool postattacking;
    public bool recharge_atac;
    public int hp = 10;
    public int timesattack = 0;
    public int num_arrow = 0;
    public int hp_tmp;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gamemanager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameMan>();
        change = true;
        detect = true;
        attacking1 = false;
        recharge_atac = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (gamemanager.boss_fight)
        {
            animator.SetFloat("speed", Mathf.Abs(speed));
            player = GameObject.FindGameObjectWithTag("Player");
            if (hp < 5)
            {
                if (state == 0)
                {
                    if ((Physics2D.OverlapBox(left_ground_detect.transform.position, impactCheckSize, 0, groundLayer) | Physics2D.OverlapBox(right_ground_detect.transform.position, impactCheckSize, 0, groundLayer)) && detect)
                    {
                        speed *= -1;
                        isLeft = !isLeft;
                        StartCoroutine(detectColision());
                    }
                    if (change && detect)
                    {
                        random_int = UnityEngine.Random.Range(1, 3);

                        if (random_int == 1)
                        {
                            speed = 20f;
                            isLeft = true;
                        }
                        if (random_int == 2)
                        {
                            speed = -20f;
                            isLeft = false;
                        }
                        StartCoroutine(changeDirection());
                    }
                    if (recharge_atac)
                    {
                        StartCoroutine(changestate());
                    }
                }
                if (state == 1)
                {
                    if (!preattacking1)
                    {
                        hp_tmp = hp;
                        preMakeAtac1();
                    }
                }
                if (state == 2)
                {
                    if (!preattacking2)
                    {
                        hp_tmp = hp;
                        preMakeAtac2();
                    }
                }
            }
            else
            {
                if (state == 0)
                {
                    if ((Physics2D.OverlapBox(left_ground_detect.transform.position, impactCheckSize, 0, groundLayer) | Physics2D.OverlapBox(right_ground_detect.transform.position, impactCheckSize, 0, groundLayer)) && detect)
                    {
                        speed *= -1;
                        isLeft = !isLeft;
                        StartCoroutine(detectColision());
                    }
                    if (change && detect)
                    {
                        random_int = UnityEngine.Random.Range(1, 3);

                        if (random_int == 1)
                        {
                            speed = 10f;
                            isLeft = true;
                        }
                        if (random_int == 2)
                        {
                            speed = -10f;
                            isLeft = false;
                        }
                        StartCoroutine(changeDirection());
                    }
                    if (recharge_atac)
                    {
                        StartCoroutine(changestate());
                    }

                }
                if (state == 1)
                {
                    if (!preattacking1)
                    {
                        hp_tmp = hp;
                        preMakeAtac1();
                    }
                }
                if (state == 2)
                {
                    if (!preattacking2)
                    {
                        hp_tmp = hp;
                        preMakeAtac2();
                    }
                }
            }
        }
        else
        {

        }
        if (isLeft)
        {
            transform.localScale = new Vector3(10, 10, 0);
        }
        else
        {
            transform.localScale = new Vector3(-10, 10, 0);
        }
    }
    private void FixedUpdate()
    {
        if (gamemanager.boss_fight)
        {
            if (attacking1)
            {
                transform.position = Vector3.MoveTowards(transform.position, player_position, speed * Time.deltaTime);
            }
            if (postattacking)
            {
                transform.position = Vector3.MoveTowards(transform.position, lastposition, speed * Time.deltaTime);
                if (transform.position == lastposition)
                {
                    if (hp_tmp < 5)
                    {

                        if (timesattack == 1)
                        {
                            postattacking = false;
                            preattacking1 = false;
                            state = 0;
                            recharge_atac = true;
                            timesattack = 0;
                        }
                        else
                        {
                            postattacking = false;
                            preattacking1 = false;
                            timesattack += 1;
                        }
                    }
                    else
                    {
                        postattacking = false;
                        preattacking1 = false;
                        state = 0;
                        recharge_atac = true;
                    }

                }
            }
            else if (!attacking1 && !postattacking)
            {
                transform.Translate(Vector3.right * speed * Time.deltaTime);
            }
        }
        else
        {

        }

    }
    IEnumerator changeDirection()
    {
        change = false;
        yield return new WaitForSeconds(2f);
        change = true;
    }
    IEnumerator detectColision()
    {
        detect = false;
        yield return new WaitForSeconds(0.25f);
        detect = true;
    }
    public void makeAtac1()
    { 
        player_position = player.transform.position;
        speed = 30f;
        StartCoroutine(Atac1());

    }
    public void makeAtac2()
    {
         Instantiate(arrow, attack_point.transform.position, Quaternion.identity);
         StartCoroutine(atac2());
    }
    public void preMakeAtac1()
    {
        preattacking1 = true;
        lastposition = transform.position;
        speed = 0f;
        StartCoroutine(prepAtac1());
    }
    public void preMakeAtac2()
    {
        preattacking2 = true;
        speed = 0f;
        StartCoroutine(prepAtac2());
    }
    IEnumerator prepAtac1()
    {
        yield return new WaitForSeconds(0.6f);
        makeAtac1();
    }
    IEnumerator Atac1()
    {
        attacking1 = true;
        animator.SetTrigger("atac1");
        yield return new WaitForSeconds(1f);

        attacking1 = false;
        postattacking = true;
        if (hp_tmp < 5)
        {
            speed = 20f;
        }
        else
        {
            speed = 10f;
        }

    }
    IEnumerator changestate()
    {
        recharge_atac = false;
        timesattack = 0;
        yield return new WaitForSeconds(5f);
        state = UnityEngine.Random.Range(1, 3);
    }
    IEnumerator prepAtac2()
    {
        yield return new WaitForSeconds(0.6f);
        num_arrow = 0;
        makeAtac2();
    }
    IEnumerator atac2()
    {
        if (hp_tmp < 5)
        {
            attacking2 = true;
            animator.SetTrigger("atac3");
            yield return new WaitForSeconds(0.5f);
            attacking2 = false;
            num_arrow += 1;
            if (num_arrow == 3)
            {
                preattacking2 = false;
                speed = 20f;
                state = 0;
                recharge_atac = true;
            }
            else
            {
                makeAtac2();
            }
        }
        else
        {
            attacking2 = true;
            yield return new WaitForSeconds(0.5f);
            attacking2 = false;
            preattacking2 = false;
            speed = 10f;
            state = 0;
            recharge_atac = true;
        }

    }
    public void takeDamage()
    {
        hp -= 1;
        if (hp == 0)
        {
            GameObject tmp = GameObject.FindGameObjectWithTag("pared_boss");
            tmp.SetActive(false);
            Destroy(gameObject);
        }
    }
}
