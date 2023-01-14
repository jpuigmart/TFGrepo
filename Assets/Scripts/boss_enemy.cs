using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


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
    public bool animChangeState;
    private bool Damaged;
    public SpriteRenderer color;
    public int numatacs;
    public bool preidle;
    public bool death;
    public AudioManager audiomanager;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gamemanager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameMan>();
        audiomanager = GameObject.FindObjectOfType<AudioManager>();
        change = true;
        detect = true;
        attacking1 = false;
        recharge_atac = true;
        animChangeState = false;
        Damaged = false;
        preidle = false;
        color = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gamemanager.boss_fight)
        {
            if (!death)
            {

                if (animChangeState)
                {

                }
                else
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
                        if (state == 3)
                        {
                            if (!preidle)
                            {
                                numatacs = 0;
                                speed = 0f;
                                StartCoroutine(idleState());
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
                        if (state == 3)
                        {
                            if (!preidle)
                            {
                                numatacs = 0;
                                speed = 0f;
                                StartCoroutine(idleState());
                            }
                        }
                    }
                }
                if (!attacking2 && !attacking1)
                {
                    if (isLeft)
                    {
                        transform.localScale = new Vector3(10, 10, 0);
                    }
                    else
                    {
                        transform.localScale = new Vector3(-10, 10, 0);
                    }
                }
            }
        }

    }
    private void FixedUpdate()
    {
        if (!death)
        {

            if (!animChangeState)
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
         audiomanager.Play("arrow_boss");
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
        if (player.transform.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(10, 10, 0);
        }
        else
        {
            transform.localScale = new Vector3(-10, 10, 0);
        }
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
        if (hp_tmp < 5)
        {
            if (timesattack == 1)
            {
                numatacs += 1;
            }
        }
        else
        {
            numatacs += 1;
        }

    }
    IEnumerator changestate()
    {
        recharge_atac = false;
        timesattack = 0;
        yield return new WaitForSeconds(5f);

        if (numatacs == 3)
        {
            state = 3;
        }
        else
        {
            state = UnityEngine.Random.Range(1, 3);
        }

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
            if (player.transform.position.x > transform.position.x)
            {
                transform.localScale = new Vector3(10, 10, 0);
            }
            else
            {
                transform.localScale = new Vector3(-10, 10, 0);
            }
            animator.SetTrigger("atac2");
            yield return new WaitForSeconds(0.5f);
            attacking2 = false;
            num_arrow += 1;
            if (num_arrow == 3)
            {
                preattacking2 = false;
                speed = 20f;
                state = 0;
                numatacs += 1;
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
            if (player.transform.position.x > transform.position.x)
            {
                transform.localScale = new Vector3(10, 10, 0);
            }
            else
            {
                transform.localScale = new Vector3(-10, 10, 0);
            }
            animator.SetTrigger("atac2");
            yield return new WaitForSeconds(0.5f);
            attacking2 = false;
            preattacking2 = false;
            speed = 10f;
            numatacs += 1;
            state = 0;
            recharge_atac = true;
        }

    }
    IEnumerator animChangeStates()
    {
        animChangeState = true;
        animator.SetTrigger("power_up");
        yield return new WaitForSeconds(2.5f);
        animChangeState = false;
    }
    public void takeDamage()
    {
        hp -= 1;
        StartCoroutine(cooldownDamage());
        StartCoroutine(damageChangeColor());
        if (hp == 4)
        {
            StartCoroutine(animChangeStates());
        }
        if (hp == 0)
        {
            GameObject tmp = GameObject.FindGameObjectWithTag("pared_boss");
            tmp.SetActive(false);
            Death();

        }

    }
    public void Death()
    {
        speed = 0;
        death = true;
        audiomanager.Stop("boss_fight");
        audiomanager.Stop("run_concrete");
        animator.SetTrigger("death");
        transform.tag = "Untagged";
        StartCoroutine(animDeath());
    }
    IEnumerator animDeath()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Credits");
        Destroy(gameObject);

    }
    IEnumerator damageChangeColor()
    {
        while (Damaged)
        {
            color.color = Color.blue;
            yield return new WaitForSeconds(0.2f);
            color.color = Color.white;
            yield return new WaitForSeconds(0.4f);
        }
    }
    IEnumerator cooldownDamage()
    {
        Damaged = true;
        yield return new WaitForSeconds(2f);
        Damaged = false;
    }
    IEnumerator idleState()
    {
        preidle = true;
        animator.SetTrigger("recharge");
        yield return new WaitForSeconds(3f);
        state = 0;
        recharge_atac = true;
        preidle = false;

    }
}
