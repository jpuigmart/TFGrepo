using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.Burst;
using System.Diagnostics.Tracing;

public class mitad : MonoBehaviour
{
    public Image background;
    public Sprite background1;
    public Sprite background2;
    public TextMeshProUGUI text;
    public int state = 0;
    // Start is called before the first frame update
    void Start()
    {
        background = GetComponent<Image>();
        background.sprite = background1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            state += 1;
        }
        switch (state)
        {
            case 0: text.text = "Durante la noche, te despiertas por culpa del viento, que azota la casa.  Al mirar por la ventana observas como se acerca una gran tormenta.\r\n"; break;
            case 1: text.text = "A medida que avanza, se va volviendo mas violenta. A los pocos minutos se vuelve un autentico temporal, que comienza a destrozar las casas del pueblo.\r\n";break;
            case 2: text.text = "De golpe, un trozo del techo cae sobre tu cabeza y quedas inconsciente en el suelo...\r\n"; background.color = Color.black;break;
            case 3: text.text = "Al levantarte ya es de dia, toda tu casa esta en ruinas. Al observarla observas a tu abuelo sin vida en el suelo. Nadie en la aldea ha sobrevivido, excepto tu.\r\n"; background.sprite = background2; background.color = Color.white; break;
            case 4: text.text = "Esto solo puede haber sido asunto de las grandes bestias. Decides ir en busca de respuestas al reino de Ivalon.\r\n"; break;
            case 5: SceneManager.LoadScene("level1");break;
            default:
                break;
        }
    }
}
