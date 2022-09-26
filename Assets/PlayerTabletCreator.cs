using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTabletCreator : MonoBehaviour
{
    public Vector3 startPosition;
    public float xPush;
    public GameObject playerTabletObject;

    // Start is called before the first frame update
    void Start()
    {
        var players = FindObjectsOfType<Player>();

        for(int i = 0; i < players.Length; i++)
        {
            GameObject go = Instantiate(playerTabletObject);

            go.transform.SetParent(GameObject.FindGameObjectWithTag("NormalCanvas").transform, false);

            RectTransform rTrans = (RectTransform)go.transform;
            rTrans.anchoredPosition = startPosition + i * new Vector3(xPush, 0);
            go.GetComponent<PlayerTablet>().linkedPlayer = players[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
