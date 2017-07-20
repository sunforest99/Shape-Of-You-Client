using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum MOVE_CONTROL
{
    STOP,
    UP,
    DOWN,
    LEFT,
    RIGHT,
}
public enum PROPER
{
    GENERAL,        // 일반인
    POLICE,         // 경찰
    THIEF           // 도둑
}

public class SPlayerMove : MonoBehaviour
{
    public string nickName;
    public int myIdx = 0;

    public bool isPlayer = false;
    public Vector3 pos;

    public MOVE_CONTROL myMove = MOVE_CONTROL.STOP;
    public MOVE_CONTROL beforeMove = MOVE_CONTROL.STOP;
    public PROPER proper = PROPER.GENERAL;

    BoxCollider2D boxcol = null;

    void Start()
    {
        SetUp();
        boxcol = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (isPlayer)
        {
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
        if (Input.GetKey(KeyCode.Space) && proper.Equals(PROPER.POLICE))
        {
            GM.NetworkManager.getInstance.SendMsg(string.Format("ATTACK:{0}", myIdx));
            Debug.Log("attack down");
        }
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
        if (col.CompareTag("Box"))
            boxcol.isTrigger = false;
        else
            boxcol.isTrigger = true;

    }

    public void SetUp()
    {
        if (proper.Equals(PROPER.POLICE))
        {
            gameObject.tag = "Police";
        }
        else if (proper.Equals(PROPER.THIEF))
            gameObject.tag = "Player";

        else if (proper.Equals(PROPER.GENERAL))
        {
            gameObject.tag = "General";
        }
    }

    public void Attack()
    {
        StartCoroutine("Big");
    }

    IEnumerator Big()
    {
        transform.localScale = new Vector2(2f, 2f);
        yield return new WaitForSeconds(1f);
        StartCoroutine("Small");
    }
    IEnumerator Small()
    {
        transform.localScale = new Vector2(1f, 1f);
        yield return null;
    }
}
