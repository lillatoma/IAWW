using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FullPlayerDrawer : MonoBehaviour
{
    public Player player;
    public int selectedMaterial = -1;
    public GameObject cardObject;

    public List<DrawingCard> cards = new List<DrawingCard>();




    private Player lastPlayer;
    private DrawingEmpire empireCard;
    private int lastBuildzone;
    private int lastCardCount;
    private int lastSelected;

    public void DoPlayerTakeOut()
    {
        if (player.selectedCard >= 0)
            player.TakeResourcesOut(player.selectedCard);
    }
    public void DoPlayerPlaceMaterial()
    {
        if (player.selectedCard >= 0 && selectedMaterial >= 0)
            player.AddResourceToBuild(player.selectedCard, selectedMaterial);
    }
    public void DoPlayerChoose()
    {
        if(player.selectedCard >= 0)
            player.DraftCard(player.selectedCard);
    }

    public void DoPlayerRecycle()
    {
        if (player.selectedCard >= 0)
            player.RecycleCard(player.selectedCard);
    }

    public void DoPlayerBuild()
    {
        if (player.selectedCard >= 0)
            player.BuildCard(player.selectedCard);
    }

    public void InactivateCardObjects(int startIndex)
    {
        for (int i = startIndex; i < cards.Count; i++)
            cards[i].gameObject.SetActive(false);
    }

    public int DrawBuiltCards(int startIndex)
    {
        for(int i = 0; i < player.builtCards.Count; i++)
        {
            DrawingCard currentCard = CreateCard(startIndex);
            currentCard.transform.localScale = new Vector3(1f, 1f, 1f);
            currentCard.ForcedStart();
            currentCard.transform.position = empireCard.transform.position + new Vector3(0, 0.825f, 50f) + i * new Vector3(0, 0.1f, -0.25f);
            currentCard.hidden = i < (player.builtCards.Count - 1);
            currentCard.testerCard = false;
            currentCard.refCard = player.builtCards[i];

            currentCard.idAtPlayer = startIndex;
            currentCard.owner = player;
            currentCard.selected = startIndex == player.selectedCard;

            currentCard.SetupCard();
            startIndex++;
        }



        return startIndex;
    }

    public int DrawPlanningCards(int startIndex)
    {
        for (int i = 0; i < player.planningCards.Count; i++)
        {
            DrawingCard currentCard = CreateCard(startIndex);
            currentCard.transform.localScale = new Vector3(1f, 1f, 1f);
            currentCard.ForcedStart();
            currentCard.testerCard = false;
            float xLength = 4.905f / (player.planningCards.Count - 1);
            if (xLength > 0.63f)
                xLength = 0.63f;
            currentCard.transform.position = new Vector3(-1.785f, -1.342f, 0f) + i * new Vector3(xLength, 0, 0.25f);
            currentCard.refCard = player.planningCards[i];


            currentCard.idAtPlayer = startIndex;
            currentCard.owner = player;
            currentCard.selected = startIndex == player.selectedCard;

            currentCard.SetupCard();
            currentCard.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
            startIndex++;
        }

        return startIndex;
    }

    public int DrawBuildzoneCards(int startIndex)
    {
        for (int i = 0; i < player.buildzoneCards.Count; i++)
        {
            DrawingCard currentCard = CreateCard(startIndex);
            currentCard.transform.localScale = new Vector3(1f, 1f, 1f);
       
            currentCard.ForcedStart();
            currentCard.testerCard = false;
            float xLength = 4.905f / (player.buildzoneCards.Count - 1);
            if (xLength > 0.63f)
                xLength = 0.63f;
            currentCard.transform.position = new Vector3(-1.785f, -0.286f, 0f) + i * new Vector3(xLength, 0, 0.25f);
            currentCard.refCard = player.buildzoneCards[i];


            currentCard.idAtPlayer = startIndex;
            currentCard.owner = player;
            currentCard.selected = startIndex == player.selectedCard;

            currentCard.SetupCard();
            currentCard.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
            startIndex++;
        }

        return startIndex;
    }

    public int DrawDraftingCards(int startIndex)
    {
        for (int i = 0; i < player.draftingCards.Count; i++)
        {
            DrawingCard currentCard = CreateCard(startIndex);
            currentCard.transform.localScale = new Vector3(1f, 1f, 1f);

            currentCard.ForcedStart();
            currentCard.testerCard = false;
            float xLength = 4.905f / (player.draftingCards.Count - 1);
            if (xLength > 0.63f)
                xLength = 0.63f;
            currentCard.transform.position = new Vector3(-1.785f, 0.762f, 0f) + i * new Vector3(xLength, 0, 0.25f);
            currentCard.refCard = player.draftingCards[i];


            currentCard.idAtPlayer = startIndex;
            currentCard.owner = player;
            currentCard.selected = startIndex == player.selectedCard;

            currentCard.SetupCard();
            currentCard.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
            startIndex++;
        }

        return startIndex;
    }
    public void DrawCards()
    {
        int index = 0;
        index = DrawBuiltCards(index);
        index = DrawBuildzoneCards(index);
        index = DrawPlanningCards(index);
        index = DrawDraftingCards(index);
        InactivateCardObjects(index);
    }


    public DrawingCard CreateCard(int index)
    {
        if (index < cards.Count)
        {
            cards[index].gameObject.SetActive(true);
            return cards[index];
        }
        GameObject go = Instantiate(cardObject);
        cards.Add(go.GetComponent<DrawingCard>());

        return CreateCard(index);
    }


    // Start is called before the first frame update
    void Start()
    {
        player = lastPlayer = FindObjectOfType<Player>();
        empireCard = FindObjectOfType<DrawingEmpire>();
        lastCardCount = player.builtCards.Count + player.buildzoneCards.Count + player.planningCards.Count;
    }




    // Update is called once per frame
    void Update()
    {
        if (player != lastPlayer)
            Debug.Log("Player is different");

        if (player != lastPlayer || lastCardCount != (player.builtCards.Count + player.buildzoneCards.Count + player.planningCards.Count)
            || player.selectedCard != lastSelected || lastBuildzone != player.buildzoneCards.Count)
        {

            empireCard.currentPlayer = player;
            empireCard.SetupCard();
            lastPlayer = player;
            lastCardCount = player.builtCards.Count + player.buildzoneCards.Count + player.planningCards.Count;
            lastBuildzone = player.buildzoneCards.Count;
            if (lastSelected != player.selectedCard)
                lastSelected = player.selectedCard;
            else player.selectedCard = -1;
            DrawCards();
        }

        if (selectedMaterial != -1)
            {
            if (player.botControlled)
                selectedMaterial = -1;
            else if (selectedMaterial < 5 && player.unusedResources[selectedMaterial] <= 0)
                selectedMaterial = -1;
            else if (selectedMaterial == 5 && player.sergeants <= 0)
                selectedMaterial = -1;
            else if (selectedMaterial == 6 && player.businessmen <= 0)
            
                selectedMaterial = -1;
            else if (selectedMaterial == 7 && player.crystanites <= 0)
                selectedMaterial = -1;
        }
       
    }
}
