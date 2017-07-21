using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SUi : MonoBehaviour
{
    [SerializeField]
    UnityEngine.UI.Text timer;
    [SerializeField]
    GameObject BtnGame = null;
  
    void Start()
    {
        SGameMng.I.bStartCheck = false;
    }
    
    void Update()
    {
        timer.text = SGameMng.I.sTimer;
        if (timer.text.Equals("1:0"))
            timer.color = new Color(255, 191, 0);
        else if (timer.text.Equals("0:30"))
            timer.color = new Color(255, 69, 83);
        else if (timer.text.Equals(""))
            timer.color = new Color(255, 54, 0);

        if (GM.NetworkManager.getInstance.isAdmin && !SGameMng.I.bStartCheck)
            BtnGame.SetActive(true);
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
