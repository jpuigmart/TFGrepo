using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;



public class GameMan : MonoBehaviour
{
    [System.Serializable]
    public struct info_enemies
    {
        public int id;
        public string name;
        public Vector3 position;
        public bool alive;
        public int hp_total;
        public GameObject prefab;

        public info_enemies(int id, string name, Vector3 position, bool alive, int hp, GameObject prefab)
        {
            this.id = id;
            this.name = name;
            this.position = position;
            this.alive = alive;
            this.hp_total = hp;
            this.prefab = prefab;
        }
    }
    public static GameMan instance;
    public List<string> enemytags = new List<string>();
    public static bool isGamePause = false;
    // Start is called before the first frame update
    public GameObject menuUI;
    // Update is called once per frame
    private Image[] lifes;
    public GameObject lifeUI;
    public GameObject dialogueBox;
    public test_moveplayer player;
    public GameObject[] pickups;
    public GameObject[] snakes;
    public TextMeshProUGUI quest;
    private int pickupget;
    private int snakekill;
    private string totalPickups;
    private string totalSnakes;
    public bool questPickup;
    public bool learnDobleJump;
    public bool questPickupFinished;
    public GameObject startQuest;
    public GameObject questCompleted;
    public DialogueTrigger NPCavi;
    //private sceneManager sceneManager;
    public Vector3 Initialcheckpoint;
    public Vector3 checkpointNew;
    public cinemachineShake cinemachineShakev;
    public string lastscene;
    public GameObject Canvas;
    private bool firstTimeLevel = true;
    public DialogeManager dialogueManager;
    public bool firstTimeGame = true;
    public List<info_enemies> llistat_enemies;
    public List<info_enemies> llistat_enemies_morts;
    public GameObject[] enemys;
    public GameObject prefab_snake;
    public GameObject prefab_hyena;
    public GameObject prefab_vultor;
    public Dictionary<string, GameObject> enemies_prefabs;
    public bool boss_fight;
    public Image life_extra;
    public bool clau_blava;
    public bool clau_groga;
    public bool clau_marro;
    public bool clau_tronja;
    public AudioManager audioManager;
    public GameObject image_quest;
    public GameObject cartel;
    public GameObject controlsUI;
    private void Awake()
    {
        enemies_prefabs = new Dictionary<string, GameObject>();
        llistat_enemies = new List<info_enemies>();
        llistat_enemies_morts = new List<info_enemies>();
        enemies_prefabs.Add("snake", prefab_snake);
        enemies_prefabs.Add("hyena", prefab_hyena);
        enemies_prefabs.Add("vultor", prefab_vultor);
        learnDobleJump = false;
        clau_blava = false;
        clau_groga = false;
        clau_marro = false;
        clau_tronja = false;
        boss_fight = false;
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
        Initialcheckpoint = GameObject.FindGameObjectWithTag("InitialCheckPoint").transform.position;
        audioManager = GameObject.FindObjectOfType<AudioManager>();
        Canvas = GameObject.FindGameObjectWithTag("Canvas").gameObject;
        dialogueManager = FindObjectOfType<DialogeManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<test_moveplayer>();
        player.transform.position = Initialcheckpoint;

        lastscene = SceneManager.GetActiveScene().name;
        checkpointNew = Initialcheckpoint;
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(Canvas);
    }
    void Start()
    {
        Debug.Log("NewCheckPoint");
        player.transform.position = checkpointNew;
        quest.gameObject.SetActive(false);
        controlsUI.gameObject.SetActive(false); 
        cartel.gameObject.SetActive(false);
        image_quest.gameObject.SetActive(false);
        if (SceneManager.GetActiveScene().name == "Game")
        {
            pickups = GameObject.FindGameObjectsWithTag("Pickup");
            snakes = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject snk in snakes)
            {
                if (snk.gameObject.TryGetComponent(out snake_enemy item))
                {
                    snk.gameObject.SetActive(false);
                }
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
        life_extra.gameObject.SetActive(false);

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
                NPCavi.dialogue.sentences[1] = "¡Volvamos a casa!";
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
        UnhideHealth();
        audioManager.Unpause("Run");
        audioManager.Unpause("Theme");
        if (SceneManager.GetActiveScene().name == "Game" && questPickup)
        {
            quest.gameObject.SetActive(true);
            cartel.gameObject.SetActive(true);
            image_quest.gameObject.SetActive(true);
        }
        if (SceneManager.GetActiveScene().name == "level1" && !boss_fight)
        {
            audioManager.Unpause("Theme2");
            audioManager.Unpause("run_concrete");
        }
        if (SceneManager.GetActiveScene().name == "level1" && boss_fight)
        {
            audioManager.Unpause("boss_fight");
            audioManager.Unpause("run_concrete");
        }
        Time.timeScale = 1;
        isGamePause = false;
    }
    public void Pause()
    {
        menuUI.SetActive(true);
        HideHealt();
        audioManager.Pause("Run");
        audioManager.Pause("Theme");
        if (SceneManager.GetActiveScene().name == "Game" && questPickup)
        {
            quest.gameObject.SetActive(false);
            cartel.gameObject.SetActive(false);
            image_quest.gameObject.SetActive(false);
        }
        if (SceneManager.GetActiveScene().name == "level1" && !boss_fight)
        {
            audioManager.Pause("Theme2");
            audioManager.Pause("run_concrete");
        }
        if (SceneManager.GetActiveScene().name == "level1" && boss_fight)
        {
            audioManager.Pause("boss_fight");
            audioManager.Pause("run_concrete");
        }
        Time.timeScale = 0;
        isGamePause = true;
    }
    
    public void takeDamage()
    {
        if (life_extra.isActiveAndEnabled)
        {
            life_extra.gameObject.SetActive(false);
            player.extralife = false;
        }
        else
        {
            player.hp -= 1;
            lifes[player.hp].gameObject.SetActive(false);
        }

    }
    public void takeHeal()
    {
        Debug.Log(player.hp);
        lifes[player.hp - 1].gameObject.SetActive(true);
    }
    public void TakeExtraLife()
    {
        life_extra.gameObject.SetActive(true);
        player.extralife = true;
    }
    public void deathScene()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            Destroy(player.gameObject);
            Destroy(dialogueManager.gameObject);
            Destroy(Canvas.gameObject);
            Time.timeScale = 1;
            SceneManager.LoadScene("Game Over");
        }
        else
        {
            if (boss_fight)
            {
                audioManager.Stop("boss_fight");
            }
            else
            {
                audioManager.Stop("Theme2");
            }
            player.gameObject.SetActive(false);
            SceneManager.LoadScene("Game Over");
        }


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
        UpdateLlistatEnemies();
        questPickup = true;
        quest.gameObject.SetActive(true);
        cartel.gameObject.SetActive(true);
        image_quest.gameObject.SetActive(true);
        startQuest.gameObject.SetActive(false);
        NPCavi.dialogue.sentences[0] = "Aun te faltan monedas por recoger y serpientes que matar. Vuelve cuando lo hayas conseguido.";
        NPCavi.dialogue.sentences[1] = "Busca en cada rincón del mapa, algunas pueden estar escondidas.";

    }
    public void QuestFinish()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            questCompleted.gameObject.SetActive(false);
            cartel.gameObject.SetActive(false);
            image_quest.gameObject.SetActive(false);
            quest.gameObject.SetActive(false);
            questPickup = false;
            learnDobleJump = true;
        }

    }
    public void RevivePlayer()
    {
        player.hp = 3;
        foreach (Image life in lifes)
        {
            life.gameObject.SetActive(true);
        }
    }
    public void UpdateCheckpoint(Vector3 checkpoint)
    {
        audioManager.Play("altar");
        checkpointNew = checkpoint;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        menuUI.SetActive(false);
        controlsUI.gameObject.SetActive(false);
        quest.gameObject.SetActive(false);
        cartel.gameObject.SetActive(false);
        image_quest.gameObject.SetActive(false);
        llistat_enemies.Clear();
        llistat_enemies_morts.Clear();
        UpdateLlistatEnemies();
        if (scene.name == "Game")
        {
            audioManager.Play("Run");
            audioManager.Play("Theme");
        }
        if (scene.name == "Credits")
        {
            audioManager.Play("Theme");
        }
        if ((scene.name == "Game" | scene.name == "level1") && !firstTimeGame)
        {
            
            lastscene = SceneManager.GetActiveScene().name;
            dialogueManager.gameObject.SetActive(false);
            dialogueBox.gameObject.SetActive(false);
            player.inDialogue = false;
            cinemachineShakev = FindObjectOfType<cinemachineShake>();
            cinemachineShakev.cinemachineVirtualCamera.Follow = player.transform;
            player.gameObject.SetActive(true);

            if (scene.name == "level1")
            {
                audioManager.Play("run_concrete");
                audioManager.Play("Theme2");
                if (clau_blava)
                {
                    GameObject porta_blava = GameObject.FindGameObjectWithTag("bloc_blau");
                    GameObject clau_blava1 = GameObject.FindGameObjectWithTag("clau_blau");
                    Destroy(porta_blava.gameObject);
                    Destroy(clau_blava1.gameObject);
                }
                if (clau_marro)
                {
                    GameObject porta_marro = GameObject.FindGameObjectWithTag("bloc_marro");
                    GameObject clau_marro1 = GameObject.FindGameObjectWithTag("clau_marro");
                    Destroy(porta_marro.gameObject);
                    Destroy(clau_marro1.gameObject);
                }
                if (clau_groga)
                {
                    GameObject porta_groga = GameObject.FindGameObjectWithTag("bloc_groc");
                    GameObject clau_groga1 = GameObject.FindGameObjectWithTag("clau_groc");
                    Destroy(porta_groga.gameObject);
                    Destroy(clau_groga1.gameObject);
                }
                if (clau_tronja)
                {
                    GameObject porta_tronja = GameObject.FindGameObjectWithTag("bloc_tronja");
                    GameObject clau_tronja1 = GameObject.FindGameObjectWithTag("clau_tronja");
                    Destroy(porta_tronja.gameObject);
                    Destroy(clau_tronja1.gameObject);
                }


            }
            if (firstTimeLevel)
            {
                audioManager.Play("Theme2");
                audioManager.Play("run_concrete");
                Initialcheckpoint = GameObject.FindGameObjectWithTag("InitialCheckPoint").transform.position;
                checkpointNew = Initialcheckpoint;
                firstTimeLevel = false;
            }
            if (player.death)
            {
                if (scene.name == "Game")
                {

                }
                else
                {
                    RevivePlayer();
                    player.death = false;
                }

            }
            
            player.transform.position = checkpointNew;
        }
        if (scene.name == "mitad")
        {
            HideHealt();
        }
        if (scene.name != "mitad" && !firstTimeGame)
        {
            UnhideHealth();
        }
        firstTimeGame = false;
    }
    void OnDisable()
    {
        
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public void updateLlistat(int id)
    {
        foreach(info_enemies var in llistat_enemies)
        {
            if (var.id == id)
            {
                UpdateAlive(var, false);
            }
        } 
    }
    public void reviveEnemies()
    {
        foreach (info_enemies item in llistat_enemies_morts)
        {
            Instantiate(item.prefab, item.position, Quaternion.identity);
        }
        llistat_enemies_morts.Clear();
    }
    public void UpdateAlive(info_enemies info,bool alive)
    {
        info_enemies tmp;
        foreach (info_enemies item in llistat_enemies)
        {
            if (item.id == info.id)
            {
                tmp = item;
                break;
            }
        }
        tmp.alive = alive;
    }
    public void AddToListDeath(int id)
    {
        foreach (info_enemies item in llistat_enemies)
        {
            if (item.id == id)
            {
                llistat_enemies_morts.Add(item);
                llistat_enemies.Remove(item);
                break;
            }
        }
    }
    public void UpdateLlistatEnemies()
    {
        enemys = GameObject.FindGameObjectsWithTag("Enemy");
        snake_enemy tmp_snake;
        hyena_enemy tmp_hyena;
        vultor_enemy tmp_vultor;
        string name_enmi = "";
        int hp_enmi = 0;
        int id_enmi = 0;
        foreach (GameObject enmi in enemys)
        {
            if (!findEnemyId(id_enmi))
            {
                if (enmi.gameObject.TryGetComponent(out snake_enemy cmp1))
                {
                    name_enmi = "snake";
                    hp_enmi = 2;
                    tmp_snake = cmp1;
                    cmp1.id = id_enmi;
                }
                if (enmi.gameObject.TryGetComponent(out hyena_enemy cmp2))
                {
                    name_enmi = "hyena";
                    hp_enmi = 2;
                    tmp_hyena = cmp2;
                    cmp2.id = id_enmi;
                }
                if (enmi.gameObject.TryGetComponent(out vultor_enemy cmp3))
                {
                    name_enmi = "vultor";
                    hp_enmi = 2;
                    tmp_vultor = cmp3;
                    cmp3.id = id_enmi;
                }

                info_enemies tmp = new info_enemies(id_enmi, name_enmi, enmi.transform.position, true, hp_enmi, enemies_prefabs[name_enmi]);
                llistat_enemies.Add(tmp);
            }
            id_enmi++;
        }
    }
    public bool findEnemyId(int id)
    {
        bool exist = false;
        foreach (info_enemies item in llistat_enemies)
        {
            if (id == item.id)
            {
                exist = true;
                break;
            }
        }
        return exist;
    }
    public void Salir()
    {
        Destroy(player.gameObject);
        Destroy(dialogueManager.gameObject);
        Destroy(Canvas.gameObject);
        Time.timeScale = 1;
        SceneManager.LoadScene("Start");
        Destroy(gameObject);
    }
    public void HideHealt()
    {
        foreach(Image life in lifes)
        {
            life.gameObject.SetActive(false);
        }
        if (life_extra.IsActive())
        {
            life_extra.gameObject.SetActive(false);
        }
    }
    public void UnhideHealth()
    {
        for (int i = 0; i < player.hp; i++)
        {
            lifes[i].gameObject.SetActive(true);
        }
        if (player.extralife)
        {
            life_extra.gameObject.SetActive(true);
        }
    }
    public void goMenuControls()
    {
        menuUI.gameObject.SetActive(false);
        controlsUI.gameObject.SetActive(true);
    }
    public void exitMenuControls()
    {
        controlsUI.gameObject.SetActive(false);
        menuUI.gameObject.SetActive(true);
    }
}
