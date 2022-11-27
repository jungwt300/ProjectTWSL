using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    AudioSource bgm;
    public Image blackBoard;
    public bool isFadeIn = false;
    public string nextScene;
    float time = 0f;
    float f_time = 2f;
    public bool bgmOff;
    private void Start()
    {
        StartCoroutine(FadeOut());
        bgm = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (isFadeIn == true)
        {
            StartCoroutine(FadeIn());
            isFadeIn = false;
        }
        if(bgmOff == true)
        {
            bgm.volume -= 0.1f * Time.deltaTime;
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
        SceneChange();
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
    public void SceneChange()
    {
        SceneManager.LoadScene(nextScene);
    }
}
