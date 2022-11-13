using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameMan : MonoBehaviour
{
    public static bool isGamePause = false;
    // Start is called before the first frame update
    public GameObject menuUI;
    // Update is called once per frame
    private Image[] lifes;
    public GameObject lifeUI;
    public test_moveplayer player;
    public GameObject[] pickups;
    public TextMeshProUGUI quest;
    private int pickupget; 
    private string totalPickups;
    void Start()
    {
        menuUI.SetActive(false);
        lifes = lifeUI.GetComponentsInChildren<Image>();
        pickups = GameObject.FindGameObjectsWithTag("Pickup");

        totalPickups = pickups.Length.ToString();
        pickupget = 0;
    }
    void LateUpdate()
    {
        quest.text = "Monedas recogidas :" + pickupget.ToString() + "/" + totalPickups;
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

    public void Resume()
    {
        menuUI.SetActive(false);
        Time.timeScale = 1;
        isGamePause = false;
    }
    public void Pause()
    {
        menuUI.SetActive(true);
        Time.timeScale = 0;
        isGamePause = true;
    }
    public void takeDamage()
    {
        lifes[player.hp].gameObject.SetActive(false);
    }
    public void deathScene()
    {
        SceneManager.LoadScene("Game Over");
    }
    public void updatePickup()
    {
        pickupget += 1;
    }
}
