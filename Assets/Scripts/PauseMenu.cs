using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool isGamePause = false;
    // Start is called before the first frame update
    public GameObject menuUI;
    // Update is called once per frame

    void Start()
    {
        menuUI.SetActive(false); 
    }
    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    void Resume()
    {
        menuUI.SetActive(false);
        Time.timeScale = 1;
        isGamePause = false;
    }
    void Pause()
    {
        menuUI.SetActive(true);
        Time.timeScale = 0;
        isGamePause = true;
    }
}
