using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reset : MonoBehaviour
{
    public SPlayerMove playerScrp;

    public void ResetFn()
    {
        playerScrp.isLive = true;
        playerScrp.nhp = 1;
        playerScrp.pos = new Vector2(0f, 0f);
        playerScrp.bBlind = false;
        playerScrp.fSpeed = 9f;
        playerScrp.SkillGame.SetActive(false);
        playerScrp.proper = PROPER.GENERAL;
        playerScrp.color = COLOR.WHITE;
    }
}