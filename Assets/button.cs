using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class button : MonoBehaviour
{
    GameMan gameman;
    Button btt;
    // Start is called before the first frame update
    void Start()
    {
        gameman = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameMan>();
        btt = transform.GetComponent<Button>();
        btt.onClick.AddListener(gameman.Salir);
    }

}
