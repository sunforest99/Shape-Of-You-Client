using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatManager : MonoBehaviour
{
    bool isOpen = false;

    [SerializeField]
    Animator chatAnim;
    [SerializeField]
    GameObject up;
    [SerializeField]
    GameObject down;

    [SerializeField]
    UnityEngine.UI.Text[] chatTxt = new UnityEngine.UI.Text[7];

    [SerializeField]
    GameObject newMsg;

    public void btClick()
    {
        newMsg.SetActive(false);

        isOpen = isOpen ? false : true;
        chatAnim.SetBool("Chat", isOpen);
        if (isOpen)
        {
            up.SetActive(false);
            down.SetActive(true);
        }
        else
        {
            up.SetActive(true);
            down.SetActive(false);
        }
    }

    public void chat(string msg)
    {
        if (!isOpen)
            newMsg.SetActive(true);

        for (int i = 0; i < 6; i++)
        {
            chatTxt[i].text = chatTxt[i + 1].text;
        }
        chatTxt[6].text = msg;
    }

    public void sendMsg(UnityEngine.UI.InputField msg)
    {
        if (!msg.text.Equals(""))
        {
            GM.NetworkManager.getInstance.SendMsg(string.Format("CHAT:{0}:{1}", GM.NetworkManager.getInstance.nickName, msg.text));
            msg.text = "";
        }
    }
}
