using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startMenu : MonoBehaviour
{
    public AudioManager audiomanager;
    private void Awake()
    {
        audiomanager = FindObjectOfType<AudioManager>();
        audiomanager.Stop("Run");
    }
    public void Jugar()
    {
        audiomanager.Play("Theme");
        audiomanager.Play("Run");
        SceneManager.LoadScene("Game");
    }
    public void Sortir()
    {
        Application.Quit();
    }
    public void ReturnMenu()
    {
        SceneManager.LoadScene("Start");
    }
    public void Controles()
    {
        SceneManager.LoadScene("Controls");
    }
}
