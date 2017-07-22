using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Col : MonoBehaviour
{
    public SPlayerMove playerScrp = null;
    [SerializeField]
    SpriteRenderer colColor = null;

    public void SetColor() { colColor.color = new Color(0.69f, 0.56f, 0.51f, 1f); }
    public void ColorReset() { colColor.color = new Color(1f, 1f, 1f, 0f); }

    void OnTriggerEnter2D(Collider2D col)
    {
        //if (playerScrp.isPlayer)
        //{
            if (SGameMng.I.bStartCheck)
            {
                if (!playerScrp.proper.Equals(PROPER.POLICE))       // 경찰이 아닐때
                {
                    if (col.CompareTag("Pcolider") && col.GetComponent<Col>().playerScrp.color == playerScrp.color && !col.GetComponent<Col>().playerScrp.bStartup)
                    {
                        playerScrp.nhp = -1;
                        playerScrp.WatchScrp.GetLive(playerScrp.isLive);
                    }
                    else if (col.CompareTag("Pcolider") && col.GetComponent<Col>().playerScrp.isSkill)
                    {
                        playerScrp.nhp = -1;
                        playerScrp.WatchScrp.GetLive(playerScrp.isLive);
                    }
                    if (playerScrp.nhp <= 0 && col.CompareTag("Pcolider") && playerScrp)
                    {
                        playerScrp.isLive = false;
                        GM.NetworkManager.getInstance.SendMsg(string.Format("DIE:{0}:{1}", playerScrp.myIdx, col.GetComponent<Col>().playerScrp.myIdx));
                    }
                }
            }
        //}
    }

    void OnTriggerExit2D(Collider2D col)
    {
        //if (playerScrp.isPlayer)
        //{
            if (SGameMng.I.bStartCheck)
            {
                if (!playerScrp.proper.Equals(PROPER.POLICE))       // 경찰이 아닐때
                {
                    if (col.CompareTag("Pcolider") && col.GetComponent<Col>().playerScrp.isSkill)
                    {
                        playerScrp.nhp = -1;
                        playerScrp.WatchScrp.GetLive(playerScrp.isLive);
                    }
                }
            }
        //}
    }
}