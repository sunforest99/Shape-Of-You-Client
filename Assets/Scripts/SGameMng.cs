using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SGameMng : MonoBehaviour
{

    private static SGameMng _Instance = null;

    public static SGameMng I
    {
        get
        {
            if (_Instance == null)
            {
                Debug.Log("instance is null");
            }
            return _Instance;
        }
    }

    void Awake()
    {
        _Instance = this;
    }
    [SerializeField]
    GameObject[] MapGame = null;
    public bool bPause;

    public void MapCtrl(int nMapNum)
    {
        for (int i = 0; i < MapGame.Length; i++)
        {
            if (i.Equals(nMapNum))
                MapGame[i].SetActive(true);
            else
                MapGame[i].SetActive(false);
        }
    }
}