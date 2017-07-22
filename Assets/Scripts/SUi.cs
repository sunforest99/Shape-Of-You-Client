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
        else if (timer.text.Equals("2:59"))
            timer.color = new Color(1f, 1f, 1f,1f);

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
    
    public void GetSkill(string sKill) { skillui.text = sKill; }
    public void SkillUiActive() { KillGame.SetActive(true); }
    public void SkillUiReset() { KillGame.SetActive(false); }
}
