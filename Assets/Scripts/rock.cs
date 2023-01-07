using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rock : MonoBehaviour
{
    public float speed;
    public LayerMask groundLayer;
    public AudioManager audiomanager;
    private Vector2 impactCheckSize = new Vector2(0.05f, 0.05f);
    private void Start()
    {
        audiomanager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
        if (Physics2D.OverlapBox(transform.position, impactCheckSize , 0, groundLayer))
        {
            Destroy(gameObject);
        }

    }
    public void OnDestroy()
    {
        //audiomanager.Play("rock");
    }

}
