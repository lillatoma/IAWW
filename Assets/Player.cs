using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public EmpireCard empireCard;
    public List<Card> builtCards = new List<Card>();
    public List<Card> buildzoneCards = new List<Card>();
    public List<Card> planningCards = new List<Card>();




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
    public int[] reservedResources;
    public int crystanites;
    public int sergeants;
    public int businessmen;




    // Start is called before the first frame update
    void Start()
    {
        empireCard = FindObjectOfType<EmpireDeck>().PickEmpireCard();
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


    // Update is called once per frame
    void Update()
    {
        
    }
}
