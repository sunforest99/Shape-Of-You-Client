using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    public static string nextScene;

    [SerializeField]
    Image progressBar;

    [SerializeField]
    UnityEngine.UI.Text descTxt;

    private void Awake()
    {
        StartCoroutine(LoadScene());

        switch (Random.Range(0, 6))
        {
            case 0:
                descTxt.text = "알고 계시나요? 도둑과 경찰의 색이 같으면 도둑은 닿기만 해도 죽습니다 !";
                break;
            case 1:
                descTxt.text = "알고 계시나요? 경찰은 10번의 공격을 모두 낭비했을때 사망합니다 !";
                break;
            case 2:
                descTxt.text = "알고 계시나요? UI에 마우스를 가까이 하면 UI가 변한답니다 !";
                break;
            case 3:
                descTxt.text = "도둑 MVP 조건 : 살아남고, 많이 움직일수록 MVP 획득확률이 올라갑니다 !";
                break;
            case 4:
                descTxt.text = "경찰 MVP 조건 : 많이 죽이고 공격을 최소한으로 사용했을 때 MVP 획득확률이 올라갑니다 !";
                break;
            default:
                descTxt.text = "알고 계시나요 ? Shape-Of-You 서버/클라의 제작기간은 고삐리 둘이서 4일 걸렸어요 !!";
                break;
        }
    }

    string nextSceneName;
    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("Loading");
    }

    IEnumerator LoadScene()
    {
        bool once = false;

        yield return null;
        
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;

            timer += Time.deltaTime;

            if (op.progress >= 0.9f)
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1.1f, 0.002f);

                if (progressBar.fillAmount == 1.0f)
                {
                    op.allowSceneActivation = true;
                    
                    GM.NetworkManager.getInstance.Con();
                }
            }
            else
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer);
                if (progressBar.fillAmount >= op.progress)
                {
                    timer = 0f;
                }
            }
        }
    }
}
