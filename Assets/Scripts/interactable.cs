using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactable : MonoBehaviour
{
    private altar altar;
    private test_moveplayer player;
    private GameMan gamemanager;
    private DialogueTrigger npctrigger;

    private void Start()
    {

        if (gameObject.TryGetComponent(out DialogueTrigger comp))
        {
            npctrigger = comp;
        }
        if (gameObject.TryGetComponent(out altar comp1))
        {
            altar = comp1;
        }
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<test_moveplayer>();
        gamemanager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameMan>();
    }
    public void Use()
    {
        if (npctrigger)
        {
            if (!player.inDialogue && player.canDialogue)
            {
                npctrigger.gameObject.GetComponent<DialogueTrigger>().TriggerDialogue();
            }
            if (!gamemanager.questPickup && !gamemanager.questPickupFinished)
            {
                gamemanager.QuestActive();
            }
            if (gamemanager.questPickupFinished)
            {
                gamemanager.QuestFinish();
            }
        }
        if (altar)
        {
            altar.Use();
        }
    }
}
