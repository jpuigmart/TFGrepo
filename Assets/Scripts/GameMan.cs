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
    public bool questPickup;
    public bool questPickupFinished;
    public GameObject startQuest;
    public GameObject questCompleted;
    public DialogueTrigger NPCavi;
    void Start()
    {
        menuUI.SetActive(false);
        lifes = lifeUI.GetComponentsInChildren<Image>();
        pickups = GameObject.FindGameObjectsWithTag("Pickup");
        foreach(GameObject pickup in pickups)
        {
            pickup.gameObject.SetActive(false);
        }
        totalPickups = pickups.Length.ToString();
        quest.gameObject.SetActive(false);
        questPickup = false;
        pickupget = 0;
        startQuest.gameObject.SetActive(true);
        questCompleted.gameObject.SetActive(false);
        questPickupFinished = false;
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
        if (pickupget.ToString() == totalPickups && questPickup)
        {
            questCompleted.gameObject.SetActive(true);
            NPCavi.dialogue.sentences[0] = "Muy bien has conseguido todas las monedas";
            NPCavi.dialogue.sentences[1] = "Gracias por jugar!";
            questPickupFinished = true;
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
    public void QuestActive()
    {
        foreach (GameObject pickup in pickups)
        {
            pickup.gameObject.SetActive(true);
        }
        questPickup = true;
        quest.gameObject.SetActive(true);
        startQuest.gameObject.SetActive(false);
        NPCavi.dialogue.sentences[0] = "Aun te faltan monedas";
        NPCavi.dialogue.sentences[1] = "Vuelve cuando las tengas";

    }
    public void QuestFinish()
    {
        questCompleted.gameObject.SetActive(false);
        quest.gameObject.SetActive(false);
        questPickup = false;
    }

}
