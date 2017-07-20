using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SPlayerMove : MonoBehaviour
{
    public string nickName;
    public int myIdx = 0;

    public bool isLive = true;
    int nhp;
    public bool isPlayer = false;
    public Vector3 pos;
    bool bcol;
    public Color[] colorcls = null;

    public MOVE_CONTROL myMove = MOVE_CONTROL.STOP;
    public MOVE_CONTROL beforeMove = MOVE_CONTROL.STOP;
    public PROPER proper = PROPER.GENERAL;
    public COLOR color = COLOR.WHITE;

    BoxCollider2D boxcol = null;
    [SerializeField]
    SpriteRenderer sprite = null;

    void Start()
    {
        nhp = 1;
        SetUp();
        boxcol = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (isPlayer && isLive)
        {
            transform.localRotation = Quaternion.identity;
            if (Input.GetKey(KeyCode.UpArrow)) myMove = MOVE_CONTROL.UP;
            else if (Input.GetKey(KeyCode.LeftArrow)) myMove = MOVE_CONTROL.LEFT;
            else if (Input.GetKey(KeyCode.RightArrow)) myMove = MOVE_CONTROL.RIGHT;
            else if (Input.GetKey(KeyCode.DownArrow)) myMove = MOVE_CONTROL.DOWN;
            else myMove = MOVE_CONTROL.STOP;

            if (myMove != beforeMove)
            {
                GM.NetworkManager.getInstance.SendMsg(string.Format("MOVE:{0}:{1}:{2}:{3}", myIdx, transform.position.x, transform.position.y, (int)myMove));
                beforeMove = myMove;
            }
        }

        if (myMove == MOVE_CONTROL.UP)
        {
            //anim.Play("UP");
            transform.Translate(Vector3.up * 4f * Time.deltaTime);
        }
        else if (myMove == MOVE_CONTROL.DOWN)
        {
            //anim.Play("DOWN");
            transform.Translate(Vector3.down * 4f * Time.deltaTime);
        }
        else if (myMove == MOVE_CONTROL.LEFT)
        {
            //anim.Play("LEFT");
            transform.Translate(Vector3.left * 4f * Time.deltaTime);
        }
        else if (myMove == MOVE_CONTROL.RIGHT)
        {
            //anim.Play("RIGHT");
            transform.Translate(Vector3.right * 4f * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.Space) && proper.Equals(PROPER.POLICE))
        {
            Attack();
            GM.NetworkManager.getInstance.SendMsg(string.Format("ATTACK:{0}", myIdx));
            Debug.Log("attack down");
        }
        Die();
    }

    /**
     * @brief 바라볼 방향 설정
     * @param direction 방향 
     */
    public void setDirection(MOVE_CONTROL direction)
    {
        //
    }

    /**
     * @brief 채팅한 내용을 보여줌
     * @param text 내용 
     */
    public void SetChatText(string text)
    {
        CancelInvoke("HideChat");
        //chatText.text = text;
        Invoke("HideChat", Mathf.Max(5f, text.Length / 10));
    }

    /**
     * @brief 채팅 숨기기 
     */
    void HideChat()
    {
        //chatText.text = "";
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (!proper.Equals(PROPER.POLICE))       // 경찰이 아닐때
        {
            if (col.CompareTag("Police"))
            {
                nhp = -1;
            }
            if (col.CompareTag("Box"))          // 박스랑 충돌할때
                boxcol.isTrigger = false;
            else
                boxcol.isTrigger = true;
        }

        if (!bcol && proper.Equals(PROPER.POLICE))
        {
            if (col.CompareTag("Box"))          // 박스랑 충돌할때
                boxcol.isTrigger = false;
            else
                boxcol.isTrigger = true;
        }
        else if (bcol && proper.Equals(PROPER.POLICE))
        {
            boxcol.isTrigger = true;
            Debug.Log("asdf");
        }
    }

    public void SetUp()
    {
        for (int i = 0; i < (int)COLOR.E_MAX; i++)
        {
            if (i == (int)color)
                sprite.color = colorcls[i];
        }
        if (proper.Equals(PROPER.POLICE))
        {
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
    }

    IEnumerator Big()
    {
        bcol = true;
        transform.localScale = new Vector2(2f, 2f);
        yield return new WaitForSeconds(1f);
        StartCoroutine("Small");
    }
    IEnumerator Small()
    {
        bcol = false;
        transform.localScale = new Vector2(1f, 1f);
        yield return null;
    }

    void Die()
    {
        if (nhp <= 0)
        {
            GM.NetworkManager.getInstance.SendMsg(string.Format("DIE:{0}", myIdx));
            gameObject.SetActive(false);
            SGameMng.I.bDie = false;
        }
    }
}
