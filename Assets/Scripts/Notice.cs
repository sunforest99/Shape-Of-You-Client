using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notice : MonoBehaviour
{

    public UnityEngine.UI.Text descTxt;
    [SerializeField]
    Animator anim;

    void OnEnable()
    {
        anim.SetTrigger("On");
        StartCoroutine("move");
    }

    IEnumerator move()
    {
        float t = 0;
        while (t <= 1)
        {
            t += Time.deltaTime;
            transform.position += new Vector3(0, 5) * Time.deltaTime;
            yield return null;
        }

        transform.localPosition = new Vector3(0, 500);
        yield return new WaitForSeconds(0.5f);
        this.gameObject.SetActive(false);
    }
}
