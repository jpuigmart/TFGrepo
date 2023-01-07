using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class altar : MonoBehaviour
{
    public SpriteRenderer[] espelmes;
    public Sprite espelmes_apagades;
    public Sprite espelmes_enceses;
    public bool newSave;
    public GameMan gameMan;
    public void Awake()
    {
        newSave = true;
        gameMan = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameMan>();
    }
    public void Use()
    {
        if (newSave)
        {
            espelmes[0].sprite = espelmes_enceses;
            espelmes[1].sprite = espelmes_enceses;
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
        yield return new WaitForSeconds(5f);
        espelmes[0].sprite = espelmes_apagades;
        espelmes[1].sprite = espelmes_apagades;
        newSave = true;
    }
}
