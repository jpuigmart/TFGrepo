using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class enemys : MonoBehaviour
{
    public GameObject center_ground_point;
    public GameObject left_ground_point;
    public GameObject right_ground_point;
    public GameObject right_ground_detect;
    public GameObject left_ground_detect;
    private Vector2 groundCheckSize = new Vector2(0.05f, 0.05f);
    private Vector2 impactCheckSize = new Vector2(0.2f, 0.05f);
    private bool isGrounded;
    private bool isLeft;
    public bool isImpact;
    public float speed;
    public bool detect;
    public bool change;
    public int random_int;
    public Animator animator;
    public bool hurt;
    public int id;
    public enemys enemy;
    public int hp;
    public GameMan gamemanager;
    // Start is called before the first frame update
    void Start()
    {
        enemy = transform.GetComponent<enemys>();
        enemy.detect = true;
        enemy.change = true;
        enemy.hurt = false;
        enemy.gamemanager = FindObjectOfType<GameMan>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void move_random()
    {

        enemy.random_int = UnityEngine.Random.Range(1, 4);

        if (enemy.random_int == 1)
        {
            enemy.speed = 0f;
        }
        if (enemy.random_int == 2)
        {
            enemy.speed = 4f;
            enemy.isLeft = true;
        }
        if (enemy.random_int == 3)
        {
            enemy.speed = -4f;
            isLeft = false;
        }
        StartCoroutine(enemy.changeDirection());
    }
    IEnumerator detectColision()
    {
        enemy.detect = false;
        yield return new WaitForSeconds(0.25f);
        enemy.detect = true;
    }
    IEnumerator changeDirection()
    {
        enemy.change = false;
        yield return new WaitForSeconds(2f);
        enemy.change = true;
    }
    IEnumerator hurting()
    {
        enemy.hurt = true;
        yield return new WaitForSeconds(0.4f);
        enemy.hurt = false;
    }
}
