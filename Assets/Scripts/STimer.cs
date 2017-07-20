using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class STimer : MonoBehaviour
{
    public float fTimer;        // 타이머 시간
    int nTime;
    int nSeconds;
    float fMillisecond;
    public Text TimerText = null;        // 텍스트 적용 부탁
                                         // Use this for initialization
    void Start()
    {
        fTimer = 60f;
    }

    // Update is called once per frame
    void Update()
    {
        Timer();
    }

    void Timer()
    {
        if (fTimer > 0)
        {
            fTimer -= Time.deltaTime;
            nTime = (int)fTimer;
            nSeconds = nTime % 60;
            fMillisecond = fTimer * 1000;
            fMillisecond = (fMillisecond % 1000);
            TimerText.text = string.Format("{0:00}:{1:00}", nSeconds, fMillisecond);
        }
        if (fTimer <= 0)
        {
            TimerText.text = string.Format("{0:00}:{1:00}", 0, 0);
        }
    }
}
