using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public void Reintentar()
    {
        SceneManager.LoadScene("Game");
    }
    public void Salir()
    {
        SceneManager.LoadScene("Start");
    }
    
}