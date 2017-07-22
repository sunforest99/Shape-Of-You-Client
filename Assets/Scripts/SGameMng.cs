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
    public ChatManager _chat;

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
   
    public string sTimer = "READY";
    public bool bStartCheck;
    public int thiefCount = 0;
    public int policeCount = 0;

    public GameObject InfoGame = null;

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
    public void ClearMap()
    {
        for (int i = 0; i < MapGame.Length; i++)
        {
            MapGame[i].SetActive(false);
        }
    }

    public GameObject winnerThiefTxt;
    public GameObject winnerPoliceTxt;
    public UnityEngine.UI.Image p_1;
    public UnityEngine.UI.Image[] p_2 = new UnityEngine.UI.Image[2];
    public UnityEngine.UI.Image[] p_3 = new UnityEngine.UI.Image[3];
    public UnityEngine.UI.Image[] p_4 = new UnityEngine.UI.Image[4];
    public UnityEngine.UI.Image[] p_5 = new UnityEngine.UI.Image[5];
    public UnityEngine.UI.Image[] p_6 = new UnityEngine.UI.Image[6];

    [SerializeField]
    Animator resultAnim;
    
    [SerializeField]
    GameObject mvpObj;

    public void OpenResult(PROPER whoWin, int mvpIdx)
    {
        if (whoWin.Equals(PROPER.POLICE))
        {
            winnerThiefTxt.SetActive(false);
            winnerPoliceTxt.SetActive(true);
        }
        else
        {
            winnerThiefTxt.SetActive(true);
            winnerPoliceTxt.SetActive(false);
        }

        List<string> v_nickList = new List<string>();
        int lIdx = 0;

        for (int i = 0; i < GM.NetworkManager.getInstance.v_user.Count; i++)
            if (GM.NetworkManager.getInstance.v_user[i] != null)
            {
                if (GM.NetworkManager.getInstance.v_user[i].proper.Equals(whoWin))
                    v_nickList.Add(GM.NetworkManager.getInstance.v_user[i].nickName);

                if (GM.NetworkManager.getInstance.v_user[i].myIdx.Equals(mvpIdx))
                    lIdx = v_nickList.Count - 1;
            }


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
            mvpObj.transform.position = p_1.gameObject.transform.position;
            StartCoroutine("mvpOpen", 1.6f);
        }
        else if (v_nickList.Count.Equals(2))
        {
            for (int i = 0; i < 2; i++)
            {
                p_2[i].gameObject.SetActive(true);
                p_2[i].gameObject.GetComponentInChildren<UnityEngine.UI.Text>().text = v_nickList[i];
            }
            mvpObj.transform.position = p_2[lIdx].gameObject.transform.position;
            StartCoroutine("mvpOpen", 1.75f);
        }
        else if (v_nickList.Count.Equals(3))
        {
            for (int i = 0; i < 3; i++)
            {
                p_3[i].gameObject.SetActive(true);
                p_3[i].gameObject.GetComponentInChildren<UnityEngine.UI.Text>().text = v_nickList[i];
            }
            mvpObj.transform.position = p_3[lIdx].gameObject.transform.position;
            StartCoroutine("mvpOpen", 1.9f);
        }
        else if (v_nickList.Count.Equals(4))
        {
            for (int i = 0; i < 4; i++)
            {
                p_4[i].gameObject.SetActive(true);
                p_4[i].gameObject.GetComponentInChildren<UnityEngine.UI.Text>().text = v_nickList[i];
            }
            mvpObj.transform.position = p_4[lIdx].gameObject.transform.position;
            StartCoroutine("mvpOpen", 2.05f);
        }
        else if (v_nickList.Count.Equals(5))
        {
            for (int i = 0; i < 5; i++)
            {
                p_5[i].gameObject.SetActive(true);
                p_5[i].gameObject.GetComponentInChildren<UnityEngine.UI.Text>().text = v_nickList[i];
            }
            mvpObj.transform.position = p_5[lIdx].gameObject.transform.position;
            StartCoroutine("mvpOpen", 2.2f);
        }
        else if (v_nickList.Count.Equals(6))
        {
            for (int i = 0; i < 6; i++)
            {
                p_6[i].gameObject.SetActive(true);
                p_6[i].gameObject.GetComponentInChildren<UnityEngine.UI.Text>().text = v_nickList[i];
            }
            mvpObj.transform.position = p_6[lIdx].gameObject.transform.position;
            StartCoroutine("mvpOpen", 2.35f);
        }
        resultAnim.SetTrigger("RESULT");
    }
    
    IEnumerator mvpOpen(float time)
    {
        yield return new WaitForSeconds(time);

        while (mvpObj.transform.localScale.x < 1.2f)
        {
            mvpObj.transform.localScale += new Vector3(0.05f, 0.05f);
            yield return new WaitForSeconds(0.01f);
        }
        while (mvpObj.transform.localScale.x > 1)
        {
            mvpObj.transform.localScale -= new Vector3(0.05f, 0.05f);
            yield return new WaitForSeconds(0.01f);
        }
        mvpObj.transform.localScale = new Vector3(1, 1);
    }
    IEnumerator mvpClose()
    {
        while (mvpObj.transform.localScale.x > 0)
        {
            mvpObj.transform.localScale -= new Vector3(0.05f, 0.05f);
            yield return new WaitForSeconds(0.01f);
        }
        mvpObj.transform.localScale = new Vector3(0, 0);
    }

    public void reset()
    {
        StartCoroutine("mvpClose");
    }
}