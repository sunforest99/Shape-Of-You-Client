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

    public SUi uiScrp;

    public bool bPause;
    public string sTimer;
    public bool bStartCheck;
    public int thiefCount = 0;
    public int policeCount = 0;
    
    public UnityEngine.UI.Text thiefCountTxt;
    public UnityEngine.UI.Text policeCountTxt;

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

    public UnityEngine.UI.Image p_1;
    public UnityEngine.UI.Image[] p_2 = new UnityEngine.UI.Image[2];
    public UnityEngine.UI.Image[] p_3 = new UnityEngine.UI.Image[3];
    public UnityEngine.UI.Image[] p_4 = new UnityEngine.UI.Image[4];
    public UnityEngine.UI.Image[] p_5 = new UnityEngine.UI.Image[5];
    public UnityEngine.UI.Image[] p_6 = new UnityEngine.UI.Image[6];

    [SerializeField]
    Animator resultAnim;

    public void OpenResult(PROPER whoWin)
    {
        List<string> v_nickList = new List<string>();

        for (int i = 0; i < GM.NetworkManager.getInstance.v_user.Count; i++)
            if (GM.NetworkManager.getInstance.v_user[i] != null)
                if (GM.NetworkManager.getInstance.v_user[i].proper.Equals(whoWin))
                    v_nickList.Add(GM.NetworkManager.getInstance.v_user[i].nickName);


        p_1.gameObject.SetActive(false);
        for (int i = 0; i < 2; i++)
            p_2[i].gameObject.SetActive(false);
        for (int i = 0; i < 3; i++)
            p_3[i].gameObject.SetActive(false);
        for (int i = 0; i < 4; i++)
            p_4[i].gameObject.SetActive(false);
        for (int i = 0; i < 5; i++)
            p_5[i].gameObject.SetActive(false);
        for (int i = 0; i < 6; i++)
            p_6[i].gameObject.SetActive(false);

        if (v_nickList.Count.Equals(1))
        {
            p_1.gameObject.SetActive(true);
            p_1.gameObject.GetComponentInChildren<UnityEngine.UI.Text>().text = v_nickList[0];
        }
        else if (v_nickList.Count.Equals(2))
        {
            for (int i = 0; i < 2; i++)
            {
                p_2[i].gameObject.SetActive(true);
                p_2[i].gameObject.GetComponentInChildren<UnityEngine.UI.Text>().text = v_nickList[i];
            }
        }
        else if (v_nickList.Count.Equals(3))
        {
            for (int i = 0; i < 3; i++)
            {
                p_3[i].gameObject.SetActive(true);
                p_3[i].gameObject.GetComponentInChildren<UnityEngine.UI.Text>().text = v_nickList[i];
            }
        }
        else if (v_nickList.Count.Equals(4))
        {
            for (int i = 0; i < 4; i++)
            {
                p_4[i].gameObject.SetActive(true);
                p_4[i].gameObject.GetComponentInChildren<UnityEngine.UI.Text>().text = v_nickList[i];
            }
        }
        else if (v_nickList.Count.Equals(5))
        {
            for (int i = 0; i < 5; i++)
            {
                p_5[i].gameObject.SetActive(true);
                p_5[i].gameObject.GetComponentInChildren<UnityEngine.UI.Text>().text = v_nickList[i];
            }
        }
        else if (v_nickList.Count.Equals(6))
        {
            for (int i = 0; i < 6; i++)
            {
                p_6[i].gameObject.SetActive(true);
                p_6[i].gameObject.GetComponentInChildren<UnityEngine.UI.Text>().text = v_nickList[i];
            }
        }
        resultAnim.SetTrigger("RESULT");
    }
}