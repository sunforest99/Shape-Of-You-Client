using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STest : MonoBehaviour
{
    [SerializeField]
    GameObject[] blockGame = new GameObject[50];

    [SerializeField]
    SpriteRenderer[] blockSprite = new SpriteRenderer[50];

    [SerializeField]
    GameObject PGame = null;

    void Start()
    {
        for (int i = 0; i < 50; i++)
        {
            blockGame[i] = PGame.transform.GetChild(i).gameObject;
            blockGame[i].transform.localPosition = new Vector2(Random.Range(-89f, 90f), Random.Range(-47f, 47f));
            blockSprite[i] = PGame.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>();
            blockSprite[i].color = SGameMng.I.rectColor[Random.Range(0, 9)];
        }
    }
}
