using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSetTagger : MonoBehaviour
{
    [SerializeField]
    GameObject[] PlayerGame = null;

    // Use this for initialization
    void Start()
    {
        SGameMng.I.Init();
        for (int i = 0; i < PlayerGame.Length; i++)
        {
            if (i == SGameMng.I.nPlayerRand)
                PlayerGame[i].tag = "Tagger";
            else
                PlayerGame[i].tag = "Player";
        }
    }
}
