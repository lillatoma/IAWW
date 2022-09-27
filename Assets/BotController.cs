using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotController : MonoBehaviour
{
    private Player player;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        gameManager = FindObjectOfType<GameManager>();
        player.botControlled = true;
    }

    void RandomlyDraftCards()
    {
        if(!player.hasValidated && gameManager.roundState % 7 == 0)
        {
            int idMin = player.builtCards.Count + player.buildzoneCards.Count + player.planningCards.Count;
            int idMax = player.builtCards.Count + player.buildzoneCards.Count + player.planningCards.Count + player.draftingCards.Count;

            player.DraftCard(Random.Range(idMin, idMax));
        }
    }


    // Update is called once per frame
    void Update()
    {
        RandomlyDraftCards();
    }
}
