using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SWatching : MonoBehaviour
{
    void LateUpdate()
    {
        //for (int i = 0; i < GM.NetworkManager.getInstance.v_user.Count; i++)
        //{
        //    if (!GM.NetworkManager.getInstance.v_user[i].isLive)
        //    {
        //        CameraMove();
        //    }
        //}
    }

    void CameraMove()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector3.up * 4f * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(Vector3.down * 4f * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left * 4f * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * 4f * Time.deltaTime);
        }
    }
}