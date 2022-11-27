using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class fadein : MonoBehaviour
{
    public Image blackBoard;
    public bool isFadeIn = false;
    public string nextScene;
    float time = 0f;
    float f_time = 2f;
    private void Start()
    {
        Color alpha = blackBoard.color;
        alpha.a = 0f;
        blackBoard.color = alpha;
        //StartCoroutine(FadeOut());
    }
    void Update()
    {
        if (isFadeIn == true)
        {
            StartCoroutine(FadeIn());
            isFadeIn = false;
        }
    }
    public IEnumerator FadeIn()
    {
        Color alpha = blackBoard.color;
        time = 0;
        blackBoard.gameObject.SetActive(true);
        while (alpha.a < 1f)
        {
            time += Time.deltaTime / f_time;
            alpha.a = Mathf.Lerp(0, 1, time);
            blackBoard.color = alpha;
            yield return null;
        }
    }
    public IEnumerator FadeOut()
    {
        Color alpha = blackBoard.color;
        time = 0f;
        blackBoard.gameObject.SetActive(true);
        while (alpha.a > 0f)
        {
            time += Time.deltaTime / f_time;
            alpha.a = Mathf.Lerp(1, 0, time);
            blackBoard.color = alpha;
            yield return null;
        }
    }
    public void FadeFlow(bool fade)
    {
        switch (fade)
        {
            case true:
                StartCoroutine(FadeIn());
                break;
            case false:
                StartCoroutine(FadeOut());
                break;
        }
    }
}
