using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public TextMeshProUGUI capcelera;
    public TextMeshProUGUI subcapcelera;
    public TextMeshProUGUI contingut;
    public TextMeshProUGUI final;
    public int state = 0;
    public float timing;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;


    }

    // Update is called once per frame
    void Update()
    {
        timing += Time.deltaTime;
        switch (state)
        {
            case 0: contingut.text = "Productor:\t\tJordi Puig Martinez\r\nDesarrollador:\tJordi Puig Martinez \r\n"; break;
            case 1: capcelera.text = "Arte\r\n";subcapcelera.text = "Sprites\n\r"; contingut.text = "CreativeKind\t\t\tExhitt\nRetrocade Media\t\t\tTotusLotus\nAamatniekss\t\t\tKyrise's\nLyaseek\t\t\t\tGraphictoon\nVnitti\t\t\t\t\tiPixl\nFree Game Assets\t\tBlack Hammer\nCrusenho\t\t\t\tNYKNCK\n\r ";break;
            case 2: capcelera.text = "Arte\r\n"; subcapcelera.text = "Sonido\n\r"; contingut.text = "Alex Ramos\t\t\tMaryagranda\nIvy\t\t\t\tThe Danicon Show\nPixabay\t\t\tNeoSpica\nMendenhall02\t\tstubb\nwjl\t\t\t\tschreibsel\nMrthenoronha\t\tReadeOnly\n\r"; break;
            case 3: capcelera.text = "Agradecimientos\r\n"; subcapcelera.text=""; contingut.text = "Universitat Oberta de Catalunya\nUnity\nItch.io\r"; break;
            case 4: capcelera.text = "¡Gracias por jugar!"; subcapcelera.text = ""; contingut.text = ""; final.text = "Barcelona, Enero 2023"; break;            
            case 5: SceneManager.LoadScene("Start");break;
        }
        if (timing > 10)
        {
            state += 1;
            timing = 0;
        }
    }
}
