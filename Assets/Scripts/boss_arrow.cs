using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss_arrow : MonoBehaviour
{
    public test_moveplayer player;
    public Vector3 player_position;
    public float speed = 30f;
    public LayerMask groundLayer;
    private Vector2 impactCheckSize = new Vector2(0.05f, 0.05f);
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<test_moveplayer>();
        player_position = player.groundCheckPoint.transform.position;
        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, player_position, speed * Time.deltaTime);
        if (Physics2D.OverlapBox(transform.position, impactCheckSize, 0, groundLayer))
        {
            Destroy(gameObject);
        }
        if (transform.position == player_position)
        {
            Destroy(gameObject);
        }
    }
}
