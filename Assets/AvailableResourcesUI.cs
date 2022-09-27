using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class AvailableResourcesUI : MonoBehaviour
{
    public Image[] resourceImages;
    public TMP_Text[] resourceTexts;

    private GameManager gameManager;
    private FullPlayerDrawer fullPlayerDrawer;
    private GameInfo gameInfo;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameInfo = FindObjectOfType<GameInfo>();
        fullPlayerDrawer = FindObjectOfType<FullPlayerDrawer>();

        for (int i = 0; i < 5; i++)
            resourceImages[i].color = gameInfo.resourcesColor[i];
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.roundState % 7 == 0)
            transform.localScale = Vector3.zero;
        else
        {
            transform.localScale = Vector3.one;

            for (int i = 0; i < 5; i++)
                resourceTexts[i].text = fullPlayerDrawer.player.unusedResources[i].ToString();
        }
    }
}
