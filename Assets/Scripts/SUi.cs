using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SUi : MonoBehaviour
{
    [SerializeField]
    UnityEngine.UI.Text timer;
    [SerializeField]
    GameObject BtnGame = null;
  
    // Use this for initialization
    void Start()
    {
        SGameMng.I.bStartCheck = false;
    }

    // Update is called once per frame
    void Update()
    {
        timer.text = SGameMng.I.sTimer;
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
