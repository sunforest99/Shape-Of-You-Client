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
                if (col.CompareTag("Pcolider") && col.GetComponent<Col>().playerScrp.color.Equals(playerScrp.color) && !col.GetComponent<Col>().playerScrp.bStartup&&gameObject.tag.Equals("Pcollider"))
                {
                    playerScrp.nhp = -1;
                    playerScrp.WatchScrp.GetLive(playerScrp.isLive);
                }
                if (col.CompareTag("Skill") && col.GetComponent<Col>().playerScrp.isSkill && gameObject.tag.Equals("Skill"))
                {
                    playerScrp.nhp = -1;
                    playerScrp.WatchScrp.GetLive(playerScrp.isLive);
                }
                if (playerScrp.nhp <= 0 && col.CompareTag("Pcolider") || col.CompareTag("Skill") && playerScrp)
                {
                    playerScrp.isLive = false;
                    GM.NetworkManager.getInstance.SendMsg(string.Format("DIE:{0}:{1}", playerScrp.myIdx, col.GetComponent<Col>().playerScrp.myIdx));
                }
            }
        }
        //}
    }
}