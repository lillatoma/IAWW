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


    public void SelectResource(int id)
    {
        Debug.Log("Select Resource: " + id);
        if (id == fullPlayerDrawer.selectedMaterial)
            fullPlayerDrawer.selectedMaterial = -1;
        else fullPlayerDrawer.selectedMaterial = id;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameInfo = FindObjectOfType<GameInfo>();
        fullPlayerDrawer = FindObjectOfType<FullPlayerDrawer>();

        for (int i = 0; i < 6; i++)
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


        }
        for (int i = 0; i < 8; i++)
        {
            if (i < 5)
                resourceTexts[i].text = fullPlayerDrawer.player.unusedResources[i].ToString();
            else if (i == 5)
                resourceTexts[i].text = fullPlayerDrawer.player.sergeants.ToString();
            else if (i == 6)
                resourceTexts[i].text = fullPlayerDrawer.player.businessmen.ToString();
            else if (i == 7)
                resourceTexts[i].text = fullPlayerDrawer.player.crystanites.ToString();

            if (i == fullPlayerDrawer.selectedMaterial)
                resourceTexts[i].color = new Color(1, 1, 0, 1);
            else
                resourceTexts[i].color = new Color(1, 1, 1, 1);

        }
    }
}
