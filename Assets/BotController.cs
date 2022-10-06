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

    void RandomlyPlanCards()
    {
        if (!player.hasValidated && gameManager.roundState % 7 == 1)
        {
            for (int i = player.planningCards.Count - 1; i >= 0; i--)
            {
                if (Random.value < 0.5f)
                    player.RecycleCard(player.builtCards.Count + player.buildzoneCards.Count + i);
                else
                    player.BuildCard(player.builtCards.Count + player.buildzoneCards.Count + i);
            }

            player.ReplaceUnusableMaterial();

            for(int i = 0; i < 5; i++)
            {
                for(int j = player.unusedResources[i] - 1; j >= 0; j--)
                {
                    List<int> buildzoneIndexes = new List<int>();
                    for(int k = 0; k < player.buildzoneCards.Count; k++)
                        if (player.buildzoneCards[k].NeedsMaterialToBuild(i))
                            buildzoneIndexes.Add(k);

                    int r = Random.Range(0, buildzoneIndexes.Count);
                    player.AddResourceToBuild(player.builtCards.Count + buildzoneIndexes[r], i);

                }
            }

        }

    }

    void RandomlySpendMaterials()
    {

        if (!player.hasValidated && gameManager.roundState % 7 > 1)
        {
            player.ReplaceUnusableMaterial();

            for (int i = 0; i < 5; i++)
            {
                for (int j = player.unusedResources[i] - 1; j >= 0; j--)
                {
                    List<int> buildzoneIndexes = new List<int>();
                    for (int k = 0; k < player.buildzoneCards.Count; k++)
                        if (player.buildzoneCards[k].NeedsMaterialToBuild(i))
                            buildzoneIndexes.Add(k);

                    int r = Random.Range(0, buildzoneIndexes.Count);
                    player.AddResourceToBuild(player.builtCards.Count + buildzoneIndexes[r], i);

                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        RandomlyDraftCards();
        RandomlyPlanCards();
        RandomlySpendMaterials();
    }
}
