using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SUi : MonoBehaviour
{
    [SerializeField]
    UnityEngine.UI.Text timer;
    [SerializeField]
    UnityEngine.UI.Text skill;
    [SerializeField]
    GameObject BtnGame = null;
    [SerializeField]
    GameObject ResultGame = null;
  
    void Start()
    {
        SGameMng.I.bStartCheck = false;
    }
    
    void Update()
    {
        if (SGameMng.I.bStartCheck)
        {
            if (timer.text.Equals("0:00")) Result();
            else if (SGameMng.I.thiefCount < 0) Result();
        }

        timer.text = SGameMng.I.sTimer;
        if (timer.text.Equals("1:0"))
            timer.color = new Color(255, 191, 0);
        else if (timer.text.Equals("0:30"))
            timer.color = new Color(255, 69, 83);
        else if (timer.text.Equals(""))
            timer.color = new Color(255, 255, 255);

        if (GM.NetworkManager.getInstance.isAdmin && !SGameMng.I.bStartCheck)
            BtnGame.SetActive(true);
    }

    void Result()
    {
        ResultGame.SetActive(true);
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
}
