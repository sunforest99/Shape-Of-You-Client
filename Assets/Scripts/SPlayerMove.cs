using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SPlayerMove : MonoBehaviour
{
    public string nickName;
    public int myIdx = 0;

    public bool isLive = true;
    public int nhp;
    public bool isPlayer = false;
    public Vector3 pos;
    public GameObject colGame = null;
    public GameObject blindGame = null;
    public bool bBlind;
    public bool bStartup;
    public float fSpeed;
    public Col colscrp = null;
    public Color[] colorcls = null;
    public bool isSkill;

    public MOVE_CONTROL myMove = MOVE_CONTROL.STOP;
    public MOVE_CONTROL beforeMove = MOVE_CONTROL.STOP;
    public PROPER proper = PROPER.GENERAL;
    public COLOR color = COLOR.WHITE;

    public SWatching WatchScrp = null;
    [SerializeField]
    SpriteRenderer sprite = null;

    void Start()
    {
        fSpeed = 9f;
        nhp = 1;
        SetUp();
        WatchScrp = Camera.main.GetComponent<SWatching>();
    }

    void Update()
    {
        if (isPlayer && isLive)
        {
            if (proper == PROPER.POLICE) { SGameMng.I.uiScrp.GetSkill(nhp.ToString()); }
            Blind();
            if (!bStartup) KeyDown();
            else KeyDown();

            if (myMove != beforeMove && !bStartup)
            {
                GM.NetworkManager.getInstance.SendMsg(string.Format("MOVE:{0}:{1}:{2}:{3}", myIdx, transform.position.x, transform.position.y, (int)myMove));
                beforeMove = myMove;
            }
            if (Input.GetKeyDown(KeyCode.Space) && proper.Equals(PROPER.POLICE) && !isSkill && !bStartup)
            {
                if (nhp <= 1)
                {
                    GM.NetworkManager.getInstance.SendMsg(string.Format("DIE:{0}:{1}", myIdx, myIdx));
                    Debug.Log("죽은놈" + myIdx); Debug.Log("죽인놈" + myIdx);
                    isLive = false;
                }
                else GM.NetworkManager.getInstance.SendMsg(string.Format("ATTACK:{0}", myIdx));
                //Attack();  
                Debug.Log("attack down");
            }

            WatchScrp.Move(this.transform);
        }

        if (gameObject.tag.Equals("Police") && bStartup) myMove = 0;
        else Move();

    }

    public void SetUp()
    {
        ChangeColor();
        if (proper.Equals(PROPER.POLICE))
        {
            colscrp.gameObject.tag = "Pcolider";
            gameObject.tag = "Police";
            nhp = 10;
        }
        else if (proper.Equals(PROPER.THIEF))
        {
            gameObject.tag = "Player";
            nhp = 1;
        }

        else if (proper.Equals(PROPER.GENERAL))
        {
            gameObject.tag = "General";
            nhp = 1;
        }
    }

    public void Attack()
    {
        nhp -= 1;

        if (nhp >= 0)
            StartCoroutine("Big");
        else nhp = 0;

    }

    IEnumerator Big()
    {
        isSkill = true;
        colscrp.transform.localScale = new Vector2(2f, 2f);
        colscrp.SetColor();
        yield return new WaitForSeconds(1f);
        StartCoroutine("Small");
    }
    IEnumerator Small()
    {
        isSkill = false;
        colscrp.transform.localScale = new Vector2(1f, 1f);
        colscrp.ColorReset();
        yield return null;
    }

    public void ChangeColor()
    {
        for (int i = 0; i < (int)COLOR.E_MAX; i++)
        {
            if (i == (int)color)
                sprite.color = colorcls[i];
        }
    }


    void Blind()
    {
        if (proper == PROPER.POLICE && !bBlind)
        {
            Debug.Log(colscrp.gameObject.tag);
            bStartup = true;
            SGameMng.I.uiScrp.SkillUiActive();
            blindGame.SetActive(true);
            bBlind = true;
        }
        if (SGameMng.I.sTimer.Equals("2:45"))
        {
            bStartup = false;
            blindGame.SetActive(false);
        }
    }

    void KeyDown()
    {
        if (Input.GetKey(KeyCode.UpArrow)) myMove = MOVE_CONTROL.UP;
        else if (Input.GetKey(KeyCode.LeftArrow)) myMove = MOVE_CONTROL.LEFT;
        else if (Input.GetKey(KeyCode.RightArrow)) myMove = MOVE_CONTROL.RIGHT;
        else if (Input.GetKey(KeyCode.DownArrow)) myMove = MOVE_CONTROL.DOWN;
        else myMove = MOVE_CONTROL.STOP;
    }

    void Move()
    {
        if (myMove == MOVE_CONTROL.UP)
        {
            //anim.Play("UP");
            transform.Translate(Vector3.up * fSpeed * Time.deltaTime);
        }
        else if (myMove == MOVE_CONTROL.DOWN)
        {
            //anim.Play("DOWN");
            transform.Translate(Vector3.down * fSpeed * Time.deltaTime);
        }
        else if (myMove == MOVE_CONTROL.LEFT)
        {
            //anim.Play("LEFT");
            transform.Translate(Vector3.left * fSpeed * Time.deltaTime);
        }
        else if (myMove == MOVE_CONTROL.RIGHT)
        {
            //anim.Play("RIGHT");
            transform.Translate(Vector3.right * fSpeed * Time.deltaTime);
        }
    }
}