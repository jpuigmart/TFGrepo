using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startMenu : MonoBehaviour
{
    public void Jugar()
    {
        SceneManager.LoadScene("Game");
    }
    public void Sortir()
    {
        Application.Quit();
    }
    
}
