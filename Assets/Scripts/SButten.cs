using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class SButten : MonoBehaviour
{
    [SerializeField]
    GameObject ServerGame = null;

    private void Start()
    {
#if UNITY_EDITOR_OSX
        ServerGame.SetActive(false);
#elif UNITY_STANDALONE_OSX
         ServerGame.SetActive(false);
#elif UNITY_ANDROID
        ServerGame.SetActive(false);        
#elif UNITY_IPHONE
        ServerGame.SetActive(false);  
#endif
    }
    public void OpenServer()        // 테스트
    {
        Process.Start(System.IO.Directory.GetCurrentDirectory() + "/Shape-Of-You-Server.exe");
    }
}
