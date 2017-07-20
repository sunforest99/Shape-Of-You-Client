using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum MOVE_CONTROL
{
    STOP,
    UP,
    DOWN,
    LEFT,
    RIGHT,
}
public enum PROPER
{
    GENERAL,        // 일반인
    POLICE,         // 경찰
    THIEF           // 도둑
}
public enum COLOR
{
    WHITE,
    GREEN,
    YELLOW,
    ORANGE,
    BLU,
    PURPLE,
    RED,
    GRAY,
    BLACK,
    E_MAX
}
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
    public int nDieCount;
    public string sTimer;

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