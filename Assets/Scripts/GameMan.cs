using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Newtonsoft.Json.Serialization;

public class GameMan : MonoBehaviour
{
    public List<string> enemytags = new List<string>();        
    public static bool isGamePause = false;
    // Start is called before the first frame update
    public GameObject menuUI;
    // Update is called once per frame
    private Image[] lifes;
    public GameObject lifeUI;
    public test_moveplayer player;
    public GameObject[] pickups;
    public GameObject[] snakes;
    public TextMeshProUGUI quest;
    private int pickupget;
    private int snakekill;
    private string totalPickups;
    private string totalSnakes;
    public bool questPickup;
    public bool questPickupFinished;
    public GameObject startQuest;
    public GameObject questCompleted;
    public DialogueTrigger NPCavi;
    void Start()
    {
        quest.gameObject.SetActive(false);
        if (SceneManager.GetActiveScene().name == "Game")
        {
            pickups = GameObject.FindGameObjectsWithTag("Pickup");
            snakes = GameObject.FindGameObjectsWithTag("Snake");
            foreach (GameObject snk in snakes)
            {
                snk.gameObject.SetActive(false);
            }
            foreach (GameObject pickup in pickups)
            {
                pickup.gameObject.SetActive(false);
            }
            totalPickups = pickups.Length.ToString();
            totalSnakes = snakes.Length.ToString();

            questPickup = false;
            snakekill = 0;
            pickupget = 0;
            startQuest.gameObject.SetActive(true);
            questCompleted.gameObject.SetActive(false);
            questPickupFinished = false;
        }
        menuUI.SetActive(false);
        lifes = lifeUI.GetComponentsInChildren<Image>();

        enemytags.Add("Snake");
        enemytags.Add("Hyena");
        enemytags.Add("Voltor");
    }
    void LateUpdate()
    {

        if (SceneManager.GetActiveScene().name == "Game")
        {
            quest.text = "Monedas recogidas :" + pickupget.ToString() + "/" + totalPickups + "\n Serpientes eliminadas :" + snakekill.ToString() + "/" + totalSnakes;
            if (pickupget.ToString() == totalPickups && snakekill.ToString() == totalSnakes && questPickup)
            {
                questCompleted.gameObject.SetActive(true);
                NPCavi.dialogue.sentences[0] = "Muy bien has conseguido todas las monedas y has matado a todas las serpientes.";
                NPCavi.dialogue.sentences[1] = "¡Gracias por jugar!";
                questPickupFinished = true;
            }
        }

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
    public void takeHeal()
    {
        Debug.Log(player.hp);
        lifes[player.hp-1].gameObject.SetActive(true);
    }
    public void deathScene()
    {
        SceneManager.LoadScene("Game Over");
    }
    public void updatePickup()
    {
        pickupget += 1;
    }
    public void updateSnake()
    {
        snakekill += 1;
    }
    public void QuestActive()
    {
        foreach (GameObject pickup in pickups)
        {
            pickup.gameObject.SetActive(true);
        }
        foreach (GameObject snk in snakes)
        {
            snk.gameObject.SetActive(true);
        }
        questPickup = true;
        quest.gameObject.SetActive(true);
        startQuest.gameObject.SetActive(false);
        NPCavi.dialogue.sentences[0] = "Aún te faltan monedas por recoger y serpientes que matar. Vuelve cuando lo hayas conseguido.";
        NPCavi.dialogue.sentences[1] = "Busca en cada rincón del mapa, algunas pueden estar escondidas.";

    }
    public void QuestFinish()
    {
        questCompleted.gameObject.SetActive(false);
        quest.gameObject.SetActive(false);
        questPickup = false;
    }
}
