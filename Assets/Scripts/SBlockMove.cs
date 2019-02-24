using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SBlockMove : MonoBehaviour
{
    public int blockpattern;
    public bool updownCheck;       // true = 위 , false = 아래
    public bool arrowCheck;        // true = 왼 , false = 오른

    private void Start()
    {
        blockpattern = Random.Range(0, 2);
        updownCheck = Random.Range(0, 2) == 0 ? false : true;
        arrowCheck = Random.Range(0, 2) == 0 ? false : true;
    }

void Update()
    {
        blockcontrol();

    }

    void blockcontrol()
    {
        switch (blockpattern)
        {
            case 0:
                if (updownCheck)
                {
                    transform.Translate(Vector3.up * 9f * Time.deltaTime);
                }
                else
                {
                    transform.Translate(Vector3.down * 9f * Time.deltaTime);
                }
                break;
            case 1:
                if (arrowCheck)
                {
                    transform.Translate(Vector3.left * 9f * Time.deltaTime);
                }
                else
                {
                    transform.Translate(Vector3.right * 9f * Time.deltaTime);
                }
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("DEnter") && updownCheck)
        {
            updownCheck = false;
        }
        else if (col.gameObject.CompareTag("DEnter") && !updownCheck)
        {
            updownCheck = true;
        }

        if (col.gameObject.CompareTag("DEnter") && arrowCheck)
        {
            arrowCheck = false;
        }
        else if (col.gameObject.CompareTag("DEnter") && !arrowCheck)
        {
            arrowCheck = true;
        }

    }
}
