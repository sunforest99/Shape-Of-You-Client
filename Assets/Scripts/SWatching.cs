using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SWatching : MonoBehaviour
{
    bool bDie;
    void LateUpdate()
    {
        CameraMove();
    }

    public void Move(Transform PlayerTrans)
    {
        transform.localPosition = new Vector3(PlayerTrans.localPosition.x, PlayerTrans.localPosition.y, -10f);
    }

    public void GetLive(bool islive) { islive = bDie; }
    public void CameraMove()
    {
        if (!bDie)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.Translate(Vector3.up * 12f * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.Translate(Vector3.down * 12f * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Translate(Vector3.left * 12f * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Translate(Vector3.right * 12f * Time.deltaTime);
            }
        }
    }
}