using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleRect : MonoBehaviour
{
    Vector2 sPos;
    public float speed = 5;
    public float lTime = 0;

    float rot = 0;

    void Start()
    {
        sPos = transform.position;
        rot = Random.Range(-3f, 3f);

        if (Random.Range(0, 2).Equals(0))
            StartCoroutine("scaleBig", Random.Range(0f, 10f));
        else
            StartCoroutine("scaleSmall", Random.Range(0f, 10f));
    }

    void Update()
    {
        transform.position.Scale(Vector2.one * 2);
        transform.Rotate(new Vector3(0, 0, rot));

        lTime += Time.deltaTime;

        transform.position += new Vector3(1.5f, -1) * speed * Time.deltaTime;
        if (transform.position.x > 50 || lTime >= 20)
        {
            transform.GetChild(1).gameObject.SetActive(false);
            transform.position = sPos;
            lTime = 0;
            StartCoroutine("openTail");
        }
    }

    IEnumerator openTail()
    {
        yield return new WaitForSeconds(1);

        transform.GetChild(1).gameObject.SetActive(true);
    }

    IEnumerator scaleBig(float count)
    {
        yield return new WaitForSeconds(count);
        while (transform.localScale.x < 1.4f)
        {
            transform.localScale = transform.localScale + new Vector3(0.02f, 0.02f);
            yield return new WaitForSeconds(0.1f);
        }
        StopCoroutine("scaleBig");
        if (Random.Range(0, 2).Equals(0))
            StartCoroutine("scaleSmall", Random.Range(0f, 10f));
        else
            StartCoroutine("scaleOrigin", Random.Range(0f, 10f));
    }
    IEnumerator scaleOrigin(float count)
    {
        yield return new WaitForSeconds(count);
        while (transform.localScale.x > 1 || transform.localScale.x < 1)
        {
            if (transform.localScale.x > 1)
                transform.localScale = transform.localScale - new Vector3(0.02f, 0.02f);
            else
                transform.localScale = transform.localScale + new Vector3(0.02f, 0.02f);
            yield return new WaitForSeconds(0.1f);
        }
        StopCoroutine("scaleOrigin");
        if (Random.Range(0, 2).Equals(0))
            StartCoroutine("scaleBig", Random.Range(0f, 10f));
        else
            StartCoroutine("scaleSmall", Random.Range(0f, 10f));
    }
    IEnumerator scaleSmall(float count)
    {
        yield return new WaitForSeconds(count);
        while (transform.localScale.x > 0.3f)
        {
            transform.localScale = transform.localScale - new Vector3(0.02f, 0.02f);
            yield return new WaitForSeconds(0.1f);
        }
        StopCoroutine("scaleSmall");
        if (Random.Range(0, 2).Equals(0))
            StartCoroutine("scaleBig", Random.Range(0f, 10f));
        else
            StartCoroutine("scaleOrigin", Random.Range(0f, 10f));
    }
}
