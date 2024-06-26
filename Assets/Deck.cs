using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Card
{
    public string name;
    public int planningRound;
    public int type; // 0 - gray, 1 - black, 2 - green, 3 - yellow, 4 - blue
    public List<int> cost; // 0 - gray, 1 - black, 2 - green, 3 - yellow, 4 - blue, 5 - sergeant, 6 - businessman, 7 - crystanite
    public List<int> built; //0 - planning, 1,2,3,4,5 - resources, +6/round
    public int specialProduction = -1; // - 1 no, 0 - whitebuild x prod[0], 1 - 
    public int[] productions = new int[5];// 0,1,2,3,4, 
    public int scoreType = -1; // 0 - raw, 1 - gray, 2 - black, 3 - green, 4 - yellow, 5 - blue, 6 - sergeant, 7 - businessman
    public int score;
    public int[] buildReward; //0 - sergeant, 1 - businessman, 2 - crystanite
    public int throwAwayReward; // 0 - gray, 1 - black, 2 - green, 3 - yellow, 4 - blue

    public void Start()
    {
        for (int i = 0; i < cost.Count; i++)
            built.Add(-1);
    }

    public bool NeedsMaterialToBuild(int m)
    {
        for(int i = 0; i < cost.Count; i++)
        {
            if (cost[i] == m && built[i] == -1)
                return true;
        }
        return false;
    }
}
    



public class Deck : MonoBehaviour
{
    public bool shouldShuffle = false;
    public List<Card> cards = new List<Card>();

    public List<Card> throwawayCards = new List<Card>();


    // Start is called before the first frame update
    void Start()
    {
        var expansions = FindObjectsOfType<ExpansionDeck>();

        foreach (var expansion in expansions)
            foreach (Card card in expansion.cards)
                cards.Add(card);



        foreach (Card card in cards)
            card.Start();
        if(shouldShuffle)
            Shuffle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("Shuffle Deck")]
    public void Shuffle()
    {
        List<int> indexes = new List<int>();

        for (int i = 0; i < cards.Count; i++)
            indexes.Add(i);

        List<Card> newCards = new List<Card>();

        for(int i = 0; i < cards.Count; i++)
        {
            int r = Random.Range(0, indexes.Count);
            int idx = indexes[r];
            indexes.RemoveAt(r);

            newCards.Add(cards[idx]);
        }

        cards = newCards;
    }


    public Card TakeCard()
    {
        if (cards.Count == 0 && throwawayCards.Count == 0)
            return null;
        else if (cards.Count == 0)
        {
            for (int i = 0; i < throwawayCards.Count; i++)
                cards.Add(throwawayCards[i]);
            throwawayCards.Clear();
        }
        int r = Random.Range(0, cards.Count);

        Card card = cards[r];
        cards.RemoveAt(r);

        return card;
    }

    public int ThrowCardAway(Card card)
    {
        throwawayCards.Add(card);
        return card.throwAwayReward;
    }
}
