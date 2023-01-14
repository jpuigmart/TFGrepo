using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class altar : MonoBehaviour
{
    public SpriteRenderer[] espelmes;
    public Sprite espelmes_apagades;
    public Sprite espelmes_enceses;
    public bool newSave;
    public GameMan gameMan;
    public bool clau_blava = false;
    public bool clau_tronja = false;
    public bool clau_marro = false;
    public bool clau_groga = false;
    public TextMeshProUGUI text;
    private GameObject texto;
    public void Awake()
    {
        newSave = true;
        gameMan = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameMan>();
        text.gameObject.SetActive(false);
    }
    public void Use()
    {
        if (newSave)
        {
            espelmes[0].sprite = espelmes_enceses;
            espelmes[1].sprite = espelmes_enceses;
            gameMan.clau_blava = clau_blava;
            gameMan.clau_tronja = clau_tronja;
            gameMan.clau_marro = clau_marro;
            gameMan.clau_groga = clau_groga;
            gameMan.UpdateCheckpoint(transform.position);
            gameMan.RevivePlayer();
            gameMan.reviveEnemies();
            gameMan.UpdateLlistatEnemies();
            StartCoroutine(espelmes_off());
        }

    }
    IEnumerator espelmes_off()
    {
        newSave = false;
        text.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        text.gameObject.SetActive(false);
        yield return new WaitForSeconds(4f);
        espelmes[0].sprite = espelmes_apagades;
        espelmes[1].sprite = espelmes_apagades;
        newSave = true;
    }
}
