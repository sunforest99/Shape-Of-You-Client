using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SUi : MonoBehaviour
{
    [SerializeField]
    UnityEngine.UI.Text timer;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer.text = SGameMng.I.sTimer;
    }
}
