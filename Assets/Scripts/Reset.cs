using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reset : MonoBehaviour
{
    public void ResetFn()
    {
        Debug.Log("RESET !!!!!!");

        SGameMng.I.gameObject.SendMessage("reset");

        for (int i = 0; i < GM.NetworkManager.getInstance.v_user.Count; i++)
        {
            if (GM.NetworkManager.getInstance.v_user[i] != null)
            {
                GM.NetworkManager.getInstance.v_user[i].gameObject.SetActive(true);
                GM.NetworkManager.getInstance.v_user[i].gameObject.tag = "General";
                GM.NetworkManager.getInstance.v_user[i].isLive = true;
                GM.NetworkManager.getInstance.v_user[i].nhp = 1;
                GM.NetworkManager.getInstance.v_user[i].pos = new Vector2(0f, 0f);
                GM.NetworkManager.getInstance.v_user[i].bBlind = false;
                GM.NetworkManager.getInstance.v_user[i].fSpeed = 9f;
                GM.NetworkManager.getInstance.v_user[i].proper = PROPER.GENERAL;
                GM.NetworkManager.getInstance.v_user[i].color = COLOR.WHITE;
                GM.NetworkManager.getInstance.v_user[i].colscrp.gameObject.tag = "col";
            }
        }
        SGameMng.I.uiScrp.SkillUiReset();
        SGameMng.I.InfoGame.SetActive(true);
        SGameMng.I.bStartCheck = false;
        SGameMng.I.thiefCount = 0;
        SGameMng.I.thiefCountTxt.text = "0";
        SGameMng.I.policeCount = 0;
        SGameMng.I.policeCountTxt.text = "0";
        SGameMng.I.ClearMap();
    }
}