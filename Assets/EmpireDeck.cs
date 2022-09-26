using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class EmpireCard
{
    public string name;
    public Color color;
    public int side;
    public int[] production; //0 - gray, 1 - black, 2 - green, 3 - yellow, 4 - blue
    public int scoreType = -1; //-1 none, 0 - raw, 1 - gray, 2 - black, 3 - green, 4 - yellow, 5 - blue, 6 - sergeant, 7 - businessman
    public int score;
}



public class EmpireDeck : MonoBehaviour
{
    public List<EmpireCard> cards = new List<EmpireCard>();

    
    public EmpireCard PickEmpireCard()
    {
        int side = FindObjectOfType<GameInfo>().side;

        while (true)
        {
            int r = Random.Range(0, cards.Count);

            EmpireCard empCard = cards[r];

            if (empCard.side == side)
            {
                cards.RemoveAt(r);

                return empCard;
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
