using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DrawingEmpire : MonoBehaviour
{
    public Player currentPlayer;
    public GameObject scoreObject;
    public SpriteRenderer scoreLogo;
    public GameObject scoreSergeantLogo;
    public GameObject scoreBusinessLogo;
    public TMP_Text scoreText;
    public TMP_Text text;



    [Header("Summoners")]
    public GameObject nametextObject;
    public GameObject scoreTextObject;
    public GameObject resourceObject;

    private SpriteRenderer spriteRenderer;
    private List<GameObject> addedElements = new List<GameObject>();
    private GameInfo gameInfo;

    private bool begun = false;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentPlayer = FindObjectOfType<Player>();
        gameInfo = FindObjectOfType<GameInfo>();

        GameObject go = GameObject.Instantiate(nametextObject);
        text = go.GetComponent<TMP_Text>();
        go.transform.parent = GameObject.FindGameObjectWithTag("WorldCanvas").transform;
        go.transform.position = transform.position + new Vector3(0, -0.25f, -0.1f);

        GameObject go2 = GameObject.Instantiate(scoreTextObject);
        scoreText = go2.GetComponent<TMP_Text>();
        go2.transform.parent = go.transform.parent;
        go2.transform.position = transform.position + new Vector3(-0.329f, 0.241f, -0.1f);
        scoreText.transform.localScale = new Vector3(1.5f, 1f, 1f);


    }

    void UpdateTextPositions()
    {
        text.transform.position = transform.position + new Vector3(0, -0.25f, -transform.position.z);
        scoreText.transform.position = transform.position + new Vector3(-0.329f, 0.241f, -transform.position.z);
    }

    public void ClearElements()
    {
        for (int i = addedElements.Count - 1; i >= 0; i--)
        {
            Destroy(addedElements[i]);
            addedElements.RemoveAt(i);
        }
    }

    private void OnDestroy()
    {
        Destroy(scoreText.gameObject);
        Destroy(text.gameObject);
    }

    public void SetupCard()
    {
        ClearElements();
        spriteRenderer.color = currentPlayer.empireCard.color;
        text.text = currentPlayer.empireCard.name;


        if(currentPlayer.empireCard.scoreType != -1)
        {
            scoreText.gameObject.SetActive(true);
            scoreObject.gameObject.SetActive(true);
            if (currentPlayer.empireCard.scoreType == 0)
            {
                scoreObject.gameObject.SetActive(false);
                scoreSergeantLogo.gameObject.SetActive(false);
                scoreBusinessLogo.gameObject.SetActive(false);
                scoreText.alignment = TextAlignmentOptions.Center;
                scoreText.text = currentPlayer.empireCard.score.ToString();
            }
            else if (currentPlayer.empireCard.scoreType <= 5)
            {
                scoreLogo.gameObject.SetActive(true);
                scoreSergeantLogo.gameObject.SetActive(false);
                scoreBusinessLogo.gameObject.SetActive(false);
                scoreText.alignment = TextAlignmentOptions.Left;
                scoreText.text = "X" + currentPlayer.empireCard.score;
                scoreLogo.color = gameInfo.cardtypeColor[currentPlayer.empireCard.scoreType - 1];
            }
            else if (currentPlayer.empireCard.scoreType == 6)
            {
                scoreLogo.gameObject.SetActive(false);
                scoreSergeantLogo.gameObject.SetActive(true);
                scoreBusinessLogo.gameObject.SetActive(false);
                scoreText.alignment = TextAlignmentOptions.Left;
                scoreText.text = "X" + currentPlayer.empireCard.score;
            }
            else if (currentPlayer.empireCard.scoreType == 7)
            {
                scoreLogo.gameObject.SetActive(false);
                scoreSergeantLogo.gameObject.SetActive(false);
                scoreBusinessLogo.gameObject.SetActive(true);
                scoreText.alignment = TextAlignmentOptions.Left;
                scoreText.text = "X" + currentPlayer.empireCard.score;
            }
        }
        else
        {
            scoreObject.SetActive(false);
            scoreText.gameObject.SetActive(false);
        }


        if (currentPlayer.empireCard.production.Length != 0)
        {


            float xSpace = 0.0575f;

            for (int i = 0; i < currentPlayer.empireCard.production.Length; i++)
            {
                GameObject go = Instantiate(resourceObject, transform);
                float x = -((float)currentPlayer.empireCard.production.Length - 1f) / 2f * xSpace + i * xSpace;
                go.transform.position = transform.position + new Vector3(x, 0.274f, -0.1f);

                go.GetComponent<SpriteRenderer>().color = gameInfo.resourcesColor[currentPlayer.empireCard.production[i]];


                addedElements.Add(go);
            }

        }

    }



    // Update is called once per frame
    void Update()
    {
        if(!begun)
        {
            begun = true;
            SetupCard();
        }

    }
}
