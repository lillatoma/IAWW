using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool hasValidated = false;

    public string playerName;
    public bool botControlled = false;
    public bool hasToChoosePerson = false;


    public EmpireCard empireCard;
    public List<Card> builtCards = new List<Card>();
    public List<Card> buildzoneCards = new List<Card>();
    public List<Card> planningCards = new List<Card>();
    public List<Card> draftingCards = new List<Card>();




    public int selectedCard = -1;


    public int totalScore;
    public int rawScore;
    public int grayScore;
    public int blackScore;
    public int greenScore;
    public int yellowScore;
    public int blueScore;
    public int sergeantScore;
    public int businessmenScore;
    public int[] production = new int[5];
    public int[] unusedResources = new int[5];
    public List<int> conversionResources = new List<int>();
    public int crystanites;
    public int sergeants;
    public int businessmen;


    public FullPlayerDrawer drawer;

    private int lastBuilt = 0;
    private GameManager gameManager;
    private Deck deck;
    private Ruleset ruleset;


    public void AutoValidate()
    {
        if (gameManager.roundState % 7 == 1 && planningCards.Count == 0)
        {
            hasValidated = true;
            for (int i = 0; i < 5; i++)
                if (unusedResources[i] > 0)
                    hasValidated = false;
        }

        else if (gameManager.roundState % 7 > 1)
        {
            hasValidated = true;
            for (int i = 0; i < 5; i++)
                if (unusedResources[i] > 0)
                    hasValidated = false;
        }
    }

    public void AddToConversion(int res)
    {
        conversionResources.Add(res);

        if(conversionResources.Count == ruleset.crystaniteConversionReq)
        {
            conversionResources.Clear();
            crystanites++;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        drawer = FindObjectOfType<FullPlayerDrawer>();
        empireCard = FindObjectOfType<EmpireDeck>().PickEmpireCard();
        gameManager = FindObjectOfType<GameManager>();
        deck = FindObjectOfType<Deck>();
        ruleset = FindObjectOfType<Ruleset>();

        CalculateProduction();
        CalculateScore();
    }

    public void DraftCard(int id)
    {
        int idx = id - (builtCards.Count + buildzoneCards.Count + planningCards.Count);
        if (idx >= 0 && idx < draftingCards.Count)
        {
            planningCards.Add(draftingCards[idx]);
            draftingCards.RemoveAt(idx);
            hasValidated = true;
        }
    }

    public void RecycleCard(int id)
    {
        int idx = id - builtCards.Count;
        if(idx >= 0 && idx < buildzoneCards.Count)
        {
            if(buildzoneCards[idx].planningRound == gameManager.roundState)
            {
                int res = deck.ThrowCardAway(buildzoneCards[idx]);
                unusedResources[res]++;
                buildzoneCards.RemoveAt(idx);
            }
            else
            {
                int res = deck.ThrowCardAway(buildzoneCards[idx]);
                AddToConversion(res);
                buildzoneCards.RemoveAt(idx);
            }
        }
       
        else if (idx >= buildzoneCards.Count && idx < buildzoneCards.Count+planningCards.Count)
        {
            idx = idx - buildzoneCards.Count;
            int res = deck.ThrowCardAway(planningCards[idx]);
            unusedResources[res]++;
            planningCards.RemoveAt(idx);
        }

    }

    void FinishBuilding(int idx)
    {
        for(int i = 0; i < buildzoneCards[idx].buildReward.Length; i++)
        {
            if (buildzoneCards[idx].buildReward[i] == 0)
                sergeants++;
            else if (buildzoneCards[idx].buildReward[i] == 1)
                businessmen++;
            else if (buildzoneCards[idx].buildReward[i] == 2)
                crystanites++;
        }

        for (int i = 0; i < buildzoneCards[idx].cost.Count; i++)
            if (buildzoneCards[idx].built[i] == -1)
                buildzoneCards[idx].built[i] = 1;

        builtCards.Add(buildzoneCards[idx]);
        buildzoneCards.RemoveAt(idx);
    }

    public void BuildCard(int id)
    {
        int idx = id - builtCards.Count;
        if(idx >= 0 && idx < buildzoneCards.Count)
        {
            int[] cost = new int[5] { 0, 0, 0, 0, 0 };
            int sergeantCost = 0;
            int businessmenCost = 0;
            int crystaniteCost = 0;
            for(int i = 0; i < buildzoneCards[idx].cost.Count; i++)
            {
                if (buildzoneCards[idx].cost[i] < 5)
                    cost[buildzoneCards[idx].cost[i]]++;
                else if (buildzoneCards[idx].cost[i] == 5)
                    sergeantCost++;
                else if (buildzoneCards[idx].cost[i] == 6)
                    businessmenCost++;
                else crystaniteCost++;
            }

            bool buildable = true;
            for (int i = 0; i < 5; i++)
                if (cost[i] > unusedResources[i])
                    buildable = false;
            if (sergeantCost > sergeants || businessmenCost > businessmen || crystaniteCost > crystanites)
                buildable = false;

            if(buildable)
            {
                for (int i = 0; i < 5; i++)
                    unusedResources[i] -= cost[i];
                sergeants -= sergeantCost;
                businessmen -= businessmenCost;
                crystanites -= crystaniteCost;

                FinishBuilding(idx);
            }

        }

        else if (idx >= buildzoneCards.Count && idx < buildzoneCards.Count + planningCards.Count)
        {
            idx = idx - buildzoneCards.Count;
            buildzoneCards.Add(planningCards[idx]);
            planningCards.RemoveAt(idx);
        }
    }

    public void TakeResourcesOut(int id)
    {
        if(id < builtCards.Count)
        {
            bool taken = false;
            for(int i = 0; i < builtCards[id].cost.Count; i++)
            {
                Debug.Log("Cost " + i + ": " + builtCards[id].cost[i]);
                if(builtCards[id].cost[i] < 5 && builtCards[id].built[i] == gameManager.roundState)
                {
                    taken = true;
                    builtCards[id].built[i] = -1;
                    unusedResources[builtCards[id].cost[i]]++;

                }
                if (builtCards[id].cost[i] < 5 && (builtCards[id].built[i] == 10000 + gameManager.roundState || taken))
                {
                    taken = true;
                    builtCards[id].built[i] = -1;
                    crystanites++;

                }
                else if (builtCards[id].cost[i] == 5 && (builtCards[id].built[i] == gameManager.roundState || taken))
                {
                    taken = true;
                    builtCards[id].built[i] = -1;
                    sergeants++;
                }
                else if (builtCards[id].cost[i] == 6 && (builtCards[id].built[i] == gameManager.roundState || taken))
                {
                    taken = true;
                    builtCards[id].built[i] = -1;
                    businessmen++;
                }
                else if (builtCards[id].cost[i] == 7 && (builtCards[id].built[i] == 10000 + gameManager.roundState || taken))
                {
                    taken = true;
                    builtCards[id].built[i] = -1;
                    crystanites++;
                }


            }

            if (taken)
            {
                buildzoneCards.Add(builtCards[id]);
                builtCards.RemoveAt(id);
            }
        }
        else
        {
            int idx = id - builtCards.Count;
            if(idx < buildzoneCards.Count)
            {
                for (int i = 0; i < buildzoneCards[idx].cost.Count; i++)
                {
                    if (buildzoneCards[idx].cost[i] < 5 && buildzoneCards[idx].built[i] == gameManager.roundState)
                    {
                        buildzoneCards[idx].built[i] = -1;
                        unusedResources[buildzoneCards[idx].cost[i]]++;

                    }
                    if (buildzoneCards[idx].cost[i] < 5 && buildzoneCards[idx].built[i] >= 10000)
                    {
                        buildzoneCards[idx].built[i] = -1;
                        crystanites++;

                    }
                    else if (buildzoneCards[idx].cost[i] == 5)
                    {
                        buildzoneCards[idx].built[i] = -1;
                        sergeants++;
                    }
                    else if (buildzoneCards[idx].cost[i] == 6)
                    {
                        buildzoneCards[idx].built[i] = -1;
                        businessmen++;
                    }
                    else if (buildzoneCards[idx].cost[i] == 7)
                    {
                        buildzoneCards[idx].built[i] = -1;
                        crystanites++;
                    }

                }
            }

        }
    }

    public void AddResourceToBuild(int id, int type)
    {
        int idx = id - builtCards.Count;
        
        if (idx >= 0 && idx < buildzoneCards.Count)
        {
            Debug.Log("Type is :" + type);
            if (type == 7 && crystanites > 0)
            {
                Debug.Log("Type is 7");
                for (int i = 0; i < buildzoneCards[idx].cost.Count; i++)
                {
                    Debug.Log("Cost " + i + " is " + buildzoneCards[idx].cost[i]);
                    if ((buildzoneCards[idx].cost[i] == type && buildzoneCards[idx].built[i] == -1)
                        || (buildzoneCards[idx].cost[i] < 5 && buildzoneCards[idx].built[i] == -1))
                    {
                        buildzoneCards[idx].built[i] = 10000 + gameManager.roundState;
                        crystanites--;
                        break;
                    }
                }
            }
            else
                for (int i = 0; i < buildzoneCards[idx].cost.Count; i++)
            {
                if (buildzoneCards[idx].cost[i] == type && buildzoneCards[idx].built[i] == -1)
                {

                    if (type < 5 && unusedResources[type] > 0)
                    {
                        buildzoneCards[idx].built[i] = gameManager.roundState;
                        unusedResources[type]--;
                        break;
                    }
                    else if (type == 5 && sergeants > 0)
                    {
                        buildzoneCards[idx].built[i] = gameManager.roundState;
                        sergeants--;
                        break;
                    }
                    else if (type == 6 && businessmen > 0)
                    {
                        buildzoneCards[idx].built[i] = gameManager.roundState;
                        businessmen--;
                        break;
                    }
                    else if (type == 7 && crystanites > 0)
                    {
                        buildzoneCards[idx].built[i] = gameManager.roundState;
                        businessmen--;
                        break;
                    }
                }

            }
            bool allSatisfied = true;
            for (int i = 0; i < buildzoneCards[idx].cost.Count; i++)
                if (buildzoneCards[idx].built[i] == -1)
                    allSatisfied = false;
            if(allSatisfied)
            {
                FinishBuilding(idx);
            }
        }
    }



    public int GetCardTypeCount(int type)
    {
        int count = 0;

        for (int i = 0; i < builtCards.Count; i++)
            if (builtCards[i].type == type)
                count++;

        return count;
    }

    public int[] GetCardProduction(int idx)
    {
        if (builtCards[idx].specialProduction == -1)
        {
            int[] prod = new int[5] { 0, 0, 0, 0, 0 };

            for (int i = 0; i < builtCards[idx].productions.Length;i++)
                prod[builtCards[idx].productions[i]]++;

            return prod;
        }

        else
        {
            int[] prod = new int[5] { 0, 0, 0, 0, 0 };

            int count = GetCardTypeCount(builtCards[idx].specialProduction);

            prod[builtCards[idx].productions[0]] = count;

            return prod;
        }


    }

    [ContextMenu("Recalculate Productions")]
    public void CalculateProduction()
    {
        for (int i = 0; i < 5; i++)
            production[i] = 0;

        for (int i = 0; i < empireCard.production.Length; i++)
        {
            production[empireCard.production[i]]++;
        }
        for(int j = 0; j < builtCards.Count; j++)
        {
            int[] cardProd = GetCardProduction(j);

            for (int i = 0; i < 5; i++)
                production[i] += cardProd[i];
        }

    }

    [ContextMenu("Recalculate Score")]
    public void CalculateScore()
    {
        rawScore = 0;
        grayScore = 0;
        blackScore = 0;
        greenScore = 0;
        yellowScore = 0;
        blueScore = 0;
        sergeantScore = sergeants;
        businessmenScore = businessmen;

        if (empireCard.scoreType == 0)
            rawScore += empireCard.score;
        else if (empireCard.scoreType == 1)
            grayScore += empireCard.score * GetCardTypeCount(0);
        else if (empireCard.scoreType == 2)
            blackScore += empireCard.score * GetCardTypeCount(1);
        else if (empireCard.scoreType == 3)
            greenScore += empireCard.score * GetCardTypeCount(2);
        else if (empireCard.scoreType == 4)
            yellowScore += empireCard.score * GetCardTypeCount(3);
        else if (empireCard.scoreType == 5)
            blueScore += empireCard.score * GetCardTypeCount(4);
        else if (empireCard.scoreType == 6)
            sergeantScore += empireCard.score * sergeants;
        else if (empireCard.scoreType == 7)
            businessmenScore += empireCard.score * businessmen;

        for(int i = 0; i < builtCards.Count; i++)
        {
            if (builtCards[i].scoreType == 0)
                rawScore += builtCards[i].score;
            else if (builtCards[i].scoreType == 1)
                grayScore += builtCards[i].score * GetCardTypeCount(0);
            else if (builtCards[i].scoreType == 2)
                blackScore += builtCards[i].score * GetCardTypeCount(1);
            else if (builtCards[i].scoreType == 3)
                greenScore += builtCards[i].score * GetCardTypeCount(2);
            else if (builtCards[i].scoreType == 4)
                yellowScore += builtCards[i].score * GetCardTypeCount(3);
            else if (builtCards[i].scoreType == 5)
                blueScore += builtCards[i].score * GetCardTypeCount(4);
            else if (builtCards[i].scoreType == 6)
                sergeantScore += builtCards[i].score * sergeants;
            else if (builtCards[i].scoreType == 7)
                businessmenScore += builtCards[i].score * businessmen;
        }

        totalScore = rawScore + grayScore + blackScore + greenScore + yellowScore + blueScore + sergeantScore + businessmenScore;
    }


    public void ReplaceUnusableMaterial()
    {
        for(int i = 0; i < 5; i++)
        {
            if (unusedResources[i] > 0)
            {
                bool foundCard = false;
                for(int j = 0; j < buildzoneCards.Count; j++)
                {
                    for (int k = 0; k < buildzoneCards[j].cost.Count; k++)
                        if (buildzoneCards[j].cost[k] == i && buildzoneCards[j].built[k] == -1)
                            foundCard = true;
                }
                if(!foundCard)
                    for (int j = 0; j < planningCards.Count; j++)
                    {
                        for (int k = 0; k < planningCards[j].cost.Count; k++)
                            if (planningCards[j].cost[k] == i && planningCards[j].built[k] == -1)
                                foundCard = true;
                    }
                if (!foundCard)
                    for (int j = unusedResources[i] - 1; j >= 0; j--)
                    {
                        AddToConversion(i);
                        unusedResources[i]--;
                    }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(lastBuilt != builtCards.Count)
        {
            CalculateProduction();
            CalculateScore();
            lastBuilt = builtCards.Count;
        }
        AutoValidate();
        ReplaceUnusableMaterial();
    }
}
