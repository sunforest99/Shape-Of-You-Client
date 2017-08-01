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
    public bool bhold;
    public Rigidbody2D rig2D = null;

    public MOVE_CONTROL myMove = MOVE_CONTROL.STOP;
    public MOVE_CONTROL beforeMove = MOVE_CONTROL.STOP;
    public PROPER proper = PROPER.GENERAL;
    public COLOR color = COLOR.WHITE;

    public SWatching WatchScrp = null;
    public SpriteRenderer sprite = null;

    [SerializeField]
    Animator anim;

    public TextMesh txtMesh;

    void Start()
    {
        fSpeed = 9f;
        nhp = 1;
        SetUp();
        WatchScrp = Camera.main.GetComponent<SWatching>();
        txtMesh.text = nickName;
    }

    void Update()
    {
        if (isPlayer && isLive)
        {
            if (proper.Equals(PROPER.POLICE)) { SGameMng.I.uiScrp.GetSkill(nhp.ToString()); }
            else if (proper.Equals(PROPER.THIEF))
            {
                SGameMng.I.uiScrp.countLife.text = (nhp - 1).ToString();
                if ((Input.GetKeyDown(KeyCode.Space) || SUi.mobileSpace ) && !SGameMng.I.isWritting)
                {
                    SUi.mobileSpace = false;
                    GM.NetworkManager.getInstance.SendMsg(string.Format("KINEMATIC:{0}", myIdx));
                }
            }
            Blind();
            if (!bStartup) KeyDown();

            if (myMove != beforeMove && !bStartup && !bhold)
            {
                GM.NetworkManager.getInstance.SendMsg(string.Format("MOVE:{0}:{1}:{2}:{3}", myIdx, transform.position.x, transform.position.y, (int)myMove));
                beforeMove = myMove;
            }
            if ((Input.GetKeyDown(KeyCode.Space) || SUi.mobileSpace) && proper.Equals(PROPER.POLICE) && !isSkill && !bStartup && !SGameMng.I.isWritting)
            {
                SUi.mobileSpace = false;
                if (nhp < 1)
                {
                    GM.NetworkManager.getInstance.SendMsg(string.Format("DIE:{0}:{1}", myIdx, myIdx));
                    isLive = false;
                }
                else GM.NetworkManager.getInstance.SendMsg(string.Format("ATTACK:{0}", myIdx));
            }

            WatchScrp.Move(this.transform);
        }
        Move();
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
            nhp = 2;
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
        {
            anim.SetTrigger("Attack");
            StartCoroutine("Big");
        }
        else nhp = 0;

    }

    IEnumerator Big()
    {
        isSkill = true;
        //colscrp.transform.localScale = new Vector2(3f, 3f);
        //colscrp.SetColor();
        rig2D.isKinematic = true;
        colGame.SetActive(true);
        yield return new WaitForSeconds(1f);
        colGame.SetActive(false);
        isSkill = false;
        rig2D.isKinematic = false;
        //colscrp.transform.localScale = new Vector2(1f, 1f);
        //colscrp.ColorReset();
    }

    public void ChangeColor()
    {
        for (int i = 0; i < (int)COLOR.E_MAX; i++)
        {
            if (i.Equals((int)color))
                sprite.color = colorcls[i];
        }
    }

    void Blind()
    {
        if (proper.Equals(PROPER.POLICE) && !bBlind)
        {
            sprite.sortingOrder = 1;
            SGameMng.I.uiScrp.SkillUiActive();
            blindGame.SetActive(true);
            bBlind = true;
        }
        else if (proper.Equals(PROPER.THIEF)) SGameMng.I.uiScrp.LifeActive();
        if (SGameMng.I.sTimer.Equals("2:45"))
        {
            bStartup = false;
            blindGame.SetActive(false);
        }
    }

    void KeyDown()
    {
        if ((Input.GetKey(KeyCode.UpArrow) || SUi.mobileMove.Equals(MOVE_CONTROL.UP)) && !SGameMng.I.isWritting) myMove = MOVE_CONTROL.UP;
        else if ((Input.GetKey(KeyCode.LeftArrow) || SUi.mobileMove.Equals(MOVE_CONTROL.LEFT)) && !SGameMng.I.isWritting) myMove = MOVE_CONTROL.LEFT;
        else if ((Input.GetKey(KeyCode.RightArrow) || SUi.mobileMove.Equals(MOVE_CONTROL.RIGHT)) && !SGameMng.I.isWritting) myMove = MOVE_CONTROL.RIGHT;
        else if ((Input.GetKey(KeyCode.DownArrow) || SUi.mobileMove.Equals(MOVE_CONTROL.DOWN)) && !SGameMng.I.isWritting) myMove = MOVE_CONTROL.DOWN;
        else myMove = MOVE_CONTROL.STOP;
    }

    void Move()
    {
        if (myMove.Equals(MOVE_CONTROL.UP) && !bhold && !bStartup)
        {
            //anim.Play("UP");
            transform.Translate(Vector3.up * fSpeed * Time.deltaTime);
        }
        else if (myMove.Equals(MOVE_CONTROL.DOWN) && !bhold && !bStartup)
        {
            //anim.Play("DOWN");
            transform.Translate(Vector3.down * fSpeed * Time.deltaTime);
        }
        else if (myMove.Equals(MOVE_CONTROL.LEFT) && !bhold && !bStartup)
        {
            //anim.Play("LEFT");
            transform.Translate(Vector3.left * fSpeed * Time.deltaTime);
        }
        else if (myMove.Equals(MOVE_CONTROL.RIGHT) && !bhold && !bStartup)
        {
            //anim.Play("RIGHT");
            transform.Translate(Vector3.right * fSpeed * Time.deltaTime);
        }
    }

    public void Hold()
    {
        if (!bhold) { colscrp.GetComponent<Collider2D>().isTrigger = true; }
        else { colscrp.GetComponent<Collider2D>().isTrigger = false; }
    }
}