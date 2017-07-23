using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SUi : MonoBehaviour
{
    [SerializeField]
    UnityEngine.UI.Text timer;
    [SerializeField]
    UnityEngine.UI.Text skillui;
    [SerializeField]
    GameObject BtnGame = null;
    [SerializeField]
    GameObject KillGame = null;

    [SerializeField]
    Animator infoAnim;
    [SerializeField]
    GameObject b_bg;

    bool isInfoOpen = false;

    [SerializeField]
    Animator contentAnim;
    [SerializeField]
    UnityEngine.UI.Text contentTitle;
    [SerializeField]
    UnityEngine.UI.Text contentSubTitle;
    [SerializeField]
    UnityEngine.UI.Text contentDesc;

    void Start()
    {
        SGameMng.I.bStartCheck = false;
    }

    void Update()
    {
        timer.text = SGameMng.I.sTimer;
        if (timer.text.Equals("1:0"))
            timer.color = new Color(1f, 0.74f, 0, 1);
        else if (timer.text.Equals("0:30"))
            timer.color = new Color(1f, 0.27f, 0.32f, 1f);
        else if (timer.text.Equals("READY"))
            timer.color = new Color(1f, 1f, 1f, 1f);

        if (GM.NetworkManager.getInstance.isAdmin && !SGameMng.I.bStartCheck)
            BtnGame.SetActive(true);
    }

    public void infoBT()
    {
        isInfoOpen = isInfoOpen ? false : true;
        infoAnim.SetBool("Open", isInfoOpen);
    }

    public void gameStart()
    {
        if (GM.NetworkManager.getInstance.isAdmin)
        {
            SGameMng.I.bStartCheck = true;
            GM.NetworkManager.getInstance.SendMsg(string.Format("START"));
            BtnGame.SetActive(false);
        }
    }

    public void imdie(string name)
    {
        contentTitle.text = "- 사망 - ";
        contentSubTitle.text = string.Format("[{0}] 님으로 부터 사망하셨습니다.", name);
        contentDesc.text = "관전모드로 설정됩니다.";

        contentAnim.SetTrigger("Content");
    }

    public void start()
    {
        contentTitle.text = "- GAME START -";
        contentSubTitle.text = "15초후 경찰이 움직입니다.";
        contentDesc.text = "경찰로부터 도망가십시오.";

        contentAnim.SetTrigger("Content");
    }

    public void GetSkill(string sKill) { skillui.text = sKill; }
    public void SkillUiActive() { KillGame.SetActive(true); }
    public void SkillUiReset() { KillGame.SetActive(false); }
}
