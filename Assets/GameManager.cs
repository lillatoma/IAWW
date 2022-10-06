using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int roundState = 0;
    public int subState = 0;
    public bool stateSatisfied = false;


    private Player[] players = null;
    private Deck deck;
    private Ruleset ruleset;


    Player[] GetPlayers()
    {
        if (players == null)
            players = FindObjectsOfType<Player>();
        return players;
    }

    // Start is called before the first frame update
    void Start()
    {
        deck = FindObjectOfType<Deck>();
        ruleset = FindObjectOfType<Ruleset>();
    }



    void TrySatisfy()
    {
        if (stateSatisfied)
            return;

        if (roundState % 7 == 0)
        {
            var players = GetPlayers();
            if (players.Length == 1)
            {

            }
            else if (players.Length == 2)
            {
                for (int i = 0; i < players.Length; i++)
                {
                    for (int j = 0; j < ruleset.cardsHandedRoundStartTwo; j++)
                    {
                        var card = deck.TakeCard();
                        if (card == null)
                            return;
                        players[i].draftingCards.Add(card);
                    }
                }
            }
            else
            {
                for (int i = 0; i < players.Length; i++)
                {
                    for (int j = 0; j < ruleset.cardsHandedRoundStart; j++)
                    {
                        var card = deck.TakeCard();
                        if (card == null)
                            return;
                        players[i].draftingCards.Add(card);
                    }
                }
            }
        }
        else if (roundState % 7 == 1)
        {
            var players = GetPlayers();
            for (int i = 0; i < players.Length; i++)
                for (int j = 0; j < players[i].planningCards.Count; j++)
                    players[i].planningCards[j].planningRound = roundState;
        }
        else
        {
            var players = GetPlayers();
            for (int i = 0; i < players.Length; i++)
            {
                players[i].hasValidated = false;
                players[i].hasToChoosePerson = false;
                players[i].unusedResources[roundState % 7 - 2] = players[i].production[roundState % 7- 2];
            }

            bool shouldGive = true;
            int mostProductionID = 0;

            for (int i = 1; i < players.Length; i++)
            {
                if (players[i].production[roundState % 7 - 2] > players[mostProductionID].production[roundState % 7 - 2])
                {
                    mostProductionID = i;
                    shouldGive = true;
                }
                else if (players[i].production[roundState % 7- 2] == players[mostProductionID].production[roundState % 7- 2])
                    shouldGive = false;
            }

            if (shouldGive && (roundState == 2 || roundState == 5))
                players[mostProductionID].businessmen++;
            else if (shouldGive && (roundState == 3 || roundState == 6))
                players[mostProductionID].sergeants++;
            else players[mostProductionID].hasToChoosePerson = true;




        }

        stateSatisfied = true;
    }

    void GoToNextStep()
    {
        bool allPlayersReady = true;
        if(players.Length > 0)
        {
            for (int i = 0; i < players.Length; i++)
                allPlayersReady = allPlayersReady && players[i].hasValidated;

            if(allPlayersReady)
            {
                if(roundState % 7 == 0)
                {
                    if(subState < ruleset.cardsDraftablePerRound - 1)
                    {
                        subState++;
                        for (int i = 0; i < players.Length; i++)
                            players[i].hasValidated = false;
                        if((roundState / 7) % 2 == 0)
                        {
                            List<List<Card>> draftDecks = new List<List<Card>>();
                            for (int i = 0; i < players.Length; i++)
                                draftDecks.Add(players[i].draftingCards);
                            for(int i = 1; i < players.Length + 1; i++)
                            {
                                int j = i % players.Length;
                                players[j].draftingCards = draftDecks[0];
                                draftDecks.RemoveAt(0);
                            }
                        }
                        else
                        {
                            List<List<Card>> draftDecks = new List<List<Card>>();
                            for (int i = 0; i < players.Length; i++)
                                draftDecks.Add(players[i].draftingCards);
                            for (int i = players.Length - 1; i < players.Length * 2 - 1; i++)
                            {
                                int j = i % players.Length;
                                players[j].draftingCards = draftDecks[0];
                                draftDecks.RemoveAt(0);
                            }
                        }
                    }    
                    else
                    {
                        subState = 0;
                        roundState++;
                        for (int i = 0; i < players.Length; i++)
                            players[i].hasValidated = false;
                        for (int i = 0; i < players.Length; i++)
                        {
                            for (int j = 0; j < players[i].draftingCards.Count; j++)
                                deck.throwawayCards.Add(players[i].draftingCards[j]);
                            players[i].draftingCards.Clear();
                        }
                        stateSatisfied = false;
                    }
                    
                }
                else if (roundState % 7 > 0)
                {
                    for (int i = 0; i < players.Length; i++)
                        players[i].hasValidated = false;
                    roundState++;
                    subState = 0;
                    stateSatisfied = false;
                }
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        TrySatisfy();
        GoToNextStep();
    }
}
