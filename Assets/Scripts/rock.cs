using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rock : MonoBehaviour
{
    public float speed;
    public LayerMask groundLayer;
    public AudioSource audio1;
    private Vector2 impactCheckSize = new Vector2(0.05f, 0.05f);
    private void Start()
    {
        audio1 = transform.GetComponent<AudioSource>();

    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
        if (Physics2D.OverlapBox(transform.position, impactCheckSize , 0, groundLayer))
        {
            AudioSource.PlayClipAtPoint(audio1.clip, transform.position);
            Destroy(gameObject);
        }

    }

}
