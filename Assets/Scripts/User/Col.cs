using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Col : MonoBehaviour
{
    public SPlayerMove playerScrp = null;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (SGameMng.I.bStartCheck)
        {
            if (!playerScrp.proper.Equals(PROPER.POLICE))       // 경찰이 아닐때
            {
                if (col.CompareTag("Pcolider") && col.GetComponent<Col>().playerScrp.color.Equals(playerScrp.color) && !col.GetComponent<Col>().playerScrp.bStartup)
                {
                    playerScrp.nhp = -1;
                    playerScrp.WatchScrp.GetLive(playerScrp.isLive);
                }
                if (col.CompareTag("Skill") && col.GetComponent<Col>().playerScrp.isSkill)
                {
                    playerScrp.nhp = -1;
                    playerScrp.WatchScrp.GetLive(playerScrp.isLive);
                }
                if (playerScrp.nhp <= 0 && (col.CompareTag("Pcolider") || col.CompareTag("Skill")) && playerScrp)
                {
                    playerScrp.isLive = false;
                    GM.NetworkManager.getInstance.SendMsg(string.Format("DIE:{0}:{1}", playerScrp.myIdx, col.GetComponent<Col>().playerScrp.myIdx));
                }
            }
            else // 경찰일때
            {
                if (col.CompareTag("col") && col.GetComponent<Col>().playerScrp.bhold) { gameObject.GetComponent<Collider2D>().isTrigger = false; }
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (SGameMng.I.bStartCheck)
        {
            if (playerScrp.proper.Equals(PROPER.POLICE))     
            {
                if (col.CompareTag("col")) { gameObject.GetComponent<Collider2D>().isTrigger = true; }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (SGameMng.I.bStartCheck)
        {
            if (!playerScrp.proper.Equals(PROPER.POLICE))   // 경찰이 아닐때
            {
                if (col.gameObject.CompareTag("Pcolider") && col.gameObject.GetComponent<Col>().playerScrp.color.Equals(playerScrp.color) && !col.gameObject.GetComponent<Col>().playerScrp.bStartup)
                {
                    playerScrp.nhp = -1;
                    playerScrp.WatchScrp.GetLive(playerScrp.isLive);
                    if (playerScrp.nhp <= 0 && playerScrp && col.gameObject.GetComponent<Col>().playerScrp.bhold)
                    {
                        playerScrp.isLive = false;
                        GM.NetworkManager.getInstance.SendMsg(string.Format("DIE:{0}:{1}", playerScrp.myIdx, col.gameObject.GetComponent<Col>().playerScrp.myIdx));
                    }
                }
            }
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (SGameMng.I.bStartCheck)
        {
            if (playerScrp.proper.Equals(PROPER.POLICE))
            {
                    if (col.gameObject.CompareTag("col")) { gameObject.GetComponent<Collider2D>().isTrigger = true; }
            }
        }
    }
}