using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public GameMan gameMan;

    private void Awake()
    {
        gameMan = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameMan>();
    }
    public void Reintentar()
    {
        SceneManager.LoadScene(gameMan.lastscene);
    }
    public void Salir()
    {
        SceneManager.LoadScene("Menu");
    }
    
}
