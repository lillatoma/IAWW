using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class PlayerTablet : MonoBehaviour
{
    public Player linkedPlayer;

    public Image[] resourceImages;
    public TMP_Text[] resourceTexts;
    public TMP_Text nameText;
    public TMP_Text scoreText;
    public TMP_Text sergeantText;
    public TMP_Text businessmanText;

    private Image background;

    // Start is called before the first frame update
    void Start()
    {
        background = GetComponent<Image>();

        GameInfo gameInfo = FindObjectOfType<GameInfo>();
        for (int i = 0; i < 5; i++)
            resourceImages[i].color = gameInfo.resourcesColor[i];
    }


    public void SwapPlayer()
    {
        FindObjectOfType<FullPlayerDrawer>().player.selectedCard = -1;
        FindObjectOfType<FullPlayerDrawer>().player = linkedPlayer;
    }

    // Update is called once per frame
    void Update()
    {
        if(linkedPlayer != null)
        {
            background.color = linkedPlayer.empireCard.color;
            nameText.text = linkedPlayer.playerName;
            for (int i = 0; i < 5; i++)
                resourceTexts[i].text = linkedPlayer.production[i].ToString();

            scoreText.text = linkedPlayer.totalScore.ToString();
            sergeantText.text = linkedPlayer.sergeants.ToString();
            businessmanText.text = linkedPlayer.businessmen.ToString();
        }
    }
}
