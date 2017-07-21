using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleRect : MonoBehaviour
{
    Vector2 sPos;
    public float speed = 5;
    float lTime = 0;

    float rot = 0;

    void Start()
    {
        sPos = transform.position;
        //StartCoroutine(scaleBig());
        //StartCoroutine(scaleSmall());
        rot = Random.Range(-3f, 3f);
    }

    void Update()
    {
        transform.position.Scale(Vector2.one * 2);
        transform.Rotate(new Vector3(0, 0, rot));

        lTime += Time.deltaTime;

        transform.position += new Vector3(1.5f, -1) * speed * Time.deltaTime;
        if (transform.position.x > 50 || lTime >= 15)
        {
            transform.position = sPos;
            lTime = 0;
        }
    }

    IEnumerator scaleBig()
    {
        while (transform.localScale.x < 2)
        {
            transform.localScale = transform.localScale + new Vector3(0.02f, 0.02f);
            yield return new WaitForSeconds(0.1f);
        }
    }
    IEnumerator scaleSmall()
    {
        while (transform.localScale.x > 0)
        {
            transform.localScale = transform.localScale - new Vector3(0.02f, 0.02f);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
