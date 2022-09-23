using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class DrawingCard : MonoBehaviour
{
    public int idFromList;
    public GameObject scoreObject;
    public SpriteRenderer scoreLogo;
    public GameObject scoreSergeantLogo;
    public GameObject scoreBusinessLogo;
    public TMP_Text scoreText;
    
    public SpriteRenderer throwawayObject;
    public TMP_Text text;



    public SpriteRenderer specialType;
    public SpriteRenderer specialResource;
    public TMP_Text specialText;




    [Header("Summoners")]
    public GameObject nametextObject;
    public GameObject scoreTextObject;
    public GameObject resourceObject;
    public GameObject costObject;
    public GameObject costSatisfiedObject;
    public GameObject costSergeantObject;
    public GameObject costSergeantSatisfiedObject;
    public GameObject costBusinessmanObject;
    public GameObject costBusinessmanSatisfiedObject;
    public GameObject sergeantRewardObject;
    public GameObject businessmanRewardObject;
    public GameObject resourceRewardObject;





    private SpriteRenderer spriteRenderer;
    private List<GameObject> addedElements = new List<GameObject>();
    private GameInfo gameInfo;
    private Deck deck;

    private int lastId = -1;

    public void ClearElements()
    {
        for(int i = addedElements.Count - 1; i >= 0; i--)
        {
            Destroy(addedElements[i]);
            addedElements.RemoveAt(i);
        }
    }


    private void OnDestroy()
    {
        Destroy(scoreText.gameObject);
        Destroy(text.gameObject);
        Destroy(specialText.gameObject);
    }

    public void SetupCard()
    {
        ClearElements();
        spriteRenderer.color = gameInfo.cardtypeColor[deck.cards[idFromList].type];
        if (deck.cards[idFromList].scoreType != -1)
        {
            scoreObject.SetActive(true);
            scoreText.gameObject.SetActive(true);

            if (deck.cards[idFromList].scoreType == 0)
            {
                scoreLogo.gameObject.SetActive(false);
                scoreSergeantLogo.gameObject.SetActive(false);
                scoreBusinessLogo.gameObject.SetActive(false);
                scoreText.alignment = TextAlignmentOptions.Center;
                scoreText.text = deck.cards[idFromList].score.ToString();
            }
            else if (deck.cards[idFromList].scoreType <= 5)
            {
                scoreLogo.gameObject.SetActive(true);
                scoreSergeantLogo.gameObject.SetActive(false);
                scoreBusinessLogo.gameObject.SetActive(false);
                scoreText.alignment = TextAlignmentOptions.Left;
                scoreText.text = "X" + deck.cards[idFromList].score;
                scoreLogo.color = gameInfo.cardtypeColor[deck.cards[idFromList].scoreType - 1];
            }
            else if (deck.cards[idFromList].scoreType == 6)
            {
                scoreLogo.gameObject.SetActive(false);
                scoreSergeantLogo.gameObject.SetActive(true);
                scoreBusinessLogo.gameObject.SetActive(false);
                scoreText.alignment = TextAlignmentOptions.Left;
                scoreText.text = "X" + deck.cards[idFromList].score;
            }
            else if (deck.cards[idFromList].scoreType == 7)
            {
                scoreLogo.gameObject.SetActive(false);
                scoreSergeantLogo.gameObject.SetActive(false);
                scoreBusinessLogo.gameObject.SetActive(true);
                scoreText.alignment = TextAlignmentOptions.Left;
                scoreText.text = "X" + deck.cards[idFromList].score;
            }
        }
        else
        {
            scoreObject.SetActive(false);
            scoreText.gameObject.SetActive(false);
        }
        throwawayObject.color = gameInfo.resourcesColor[deck.cards[idFromList].throwAwayReward];

        text.text = deck.cards[idFromList].name;
       

        if (deck.cards[idFromList].specialProduction == -1)
        {
            specialResource.gameObject.SetActive(false);
            specialType.gameObject.SetActive(false);
            specialText.gameObject.SetActive(false);

            float xSpace = 0.0575f;

            for(int i = 0; i < deck.cards[idFromList].productions.Length; i++)
            {
                GameObject go = Instantiate(resourceObject,transform);
                float x = -((float)deck.cards[idFromList].productions.Length - 1f) / 2f * xSpace + i * xSpace;
                go.transform.position = transform.position + new Vector3(x, -0.448f, -0.1f);

                go.GetComponent<SpriteRenderer>().color = gameInfo.resourcesColor[deck.cards[idFromList].productions[i]];


                addedElements.Add(go);
            }

        }
        else
        {
            specialResource.gameObject.SetActive(true);
            specialType.gameObject.SetActive(true);
            specialText.gameObject.SetActive(true);
            specialResource.color = gameInfo.resourcesColor[deck.cards[idFromList].productions[0]];
            specialType.color = gameInfo.cardtypeColor[deck.cards[idFromList].specialProduction];
        }

        float yDistance = 0.103f;

        for(int i = 0; i < deck.cards[idFromList].cost.Count; i++)
        {
            bool satisfied = deck.cards[idFromList].built[i] != -1;
            float y = 0.4f - i * yDistance;

            if (deck.cards[idFromList].cost[i] < 5)
            {
                GameObject go;
                if(satisfied)
                    go = Instantiate(costSatisfiedObject,transform);
                else
                    go = Instantiate(costObject,transform);

                go.transform.position = transform.position + new Vector3(-0.275f, y, -0.05f);

                go.GetComponent<SpriteRenderer>().color = gameInfo.resourcesColor[deck.cards[idFromList].cost[i]];

                addedElements.Add(go);
            }
            else if (deck.cards[idFromList].cost[i] == 5)
            {
                GameObject go;
                if (satisfied)
                    go = Instantiate(costSergeantSatisfiedObject, transform);
                else
                    go = Instantiate(costSergeantObject, transform);

                go.transform.position = transform.position + new Vector3(-0.275f, y, -0.05f);

                addedElements.Add(go);
            }
            else if (deck.cards[idFromList].cost[i] == 6)
            {
                GameObject go;
                if (satisfied)
                    go = Instantiate(costBusinessmanSatisfiedObject, transform);
                else
                    go = Instantiate(costBusinessmanObject, transform);

                go.transform.position = transform.position + new Vector3(-0.275f, y, -0.05f);

                addedElements.Add(go);
            }
            else if (deck.cards[idFromList].cost[i] == 7)
            {
                GameObject go;
                if (satisfied)
                    go = Instantiate(costSatisfiedObject, transform);
                else
                    go = Instantiate(costObject, transform);

                go.transform.position = transform.position + new Vector3(-0.275f, y, -0.05f);

                go.GetComponent<SpriteRenderer>().color = gameInfo.resourcesColor[5];

                addedElements.Add(go);
            }
        }


        float rewardX = 0.1f;

        for(int i = 0; i < deck.cards[idFromList].buildReward.Length; i++)
        {
            float x = -((float)deck.cards[idFromList].buildReward.Length - 1f) / 2 * rewardX + i * rewardX;
            if(deck.cards[idFromList].buildReward[i] == 0)
            {
                GameObject go = Instantiate(sergeantRewardObject, transform);

                go.transform.position = transform.position + new Vector3(x, -0.31f, -0.05f);

                addedElements.Add(go);
            }
            else if (deck.cards[idFromList].buildReward[i] == 1)
            {
                GameObject go = Instantiate(businessmanRewardObject, transform);

                go.transform.position = transform.position + new Vector3(x, -0.31f, -0.05f);

                addedElements.Add(go);
            }
            else if (deck.cards[idFromList].buildReward[i] == 2)
            {
                GameObject go = Instantiate(resourceRewardObject, transform);

                go.transform.position = transform.position + new Vector3(x, -0.31f, -0.05f);
                go.GetComponent<SpriteRenderer>().color = gameInfo.resourcesColor[5];
                addedElements.Add(go);
            }
        }


    }

    
    // Start is called before the first frame update
    void Start()
    {
        gameInfo = FindObjectOfType<GameInfo>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        deck = FindObjectOfType<Deck>();

        idFromList = 0;


        GameObject go = GameObject.Instantiate(nametextObject);
        text = go.GetComponent<TMP_Text>();
        go.transform.parent = GameObject.FindGameObjectWithTag("WorldCanvas").transform;
        go.transform.position = transform.position + new Vector3(0, 0.393f, -0.1f);

        GameObject go2 = GameObject.Instantiate(scoreTextObject);
        scoreText = go2.GetComponent<TMP_Text>();
        go2.transform.parent = go.transform.parent;
        go2.transform.position = transform.position + new Vector3(-0.25f, -0.453f, -0.1f);
        scoreText.transform.localScale = new Vector3(1.5f, 1f, 1f);

        GameObject go3 = GameObject.Instantiate(scoreTextObject);
        specialText = go3.GetComponent<TMP_Text>();
        go3.transform.parent = go.transform.parent;
        go3.transform.position = transform.position + new Vector3(0, -0.455f, -0.1f);
        specialText.text = "X";
        specialText.alignment = TextAlignmentOptions.Center;
        go3.transform.localScale = new Vector3(2, 1, 1);
        //((RectTransform)text.transform).sizeDelta = Vector3.zero;
        //text.alignment = TextAlignmentOptions.Center;
        //text.enableWordWrapping = false;
        //text.fontSize = 0.1f;

    }

    void UpdateTextPositions()
    {
        if(text)
            text.transform.position = transform.position + new Vector3(0.02f, 0.393f, -0.1f);
        if(scoreText)
            scoreText.transform.position = transform.position + new Vector3(-0.25f, -0.453f, -0.1f);
        if (specialText)
            specialText.transform.position = transform.position + new Vector3(0, -0.455f, -0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTextPositions();
        if(idFromList != lastId)
        {
            SetupCard();
            lastId = idFromList;
        }
    }
}
