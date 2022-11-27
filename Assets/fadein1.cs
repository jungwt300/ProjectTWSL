using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Player.Modules.Characters;
using Boss;
public class fadein1 : MonoBehaviour
{
    public Image blackBoard;
    public bool isFadeIn = false;
    public string nextScene;
    float time = 0f;
    float f_time = 2f;
    BossController bossController;
    CutsceneManager cutsceneManager;
    private void Start()
    {
        Color alpha1 = blackBoard.color;
        alpha1.a = 0f;
        blackBoard.color = alpha1;
        bossController = GameObject.Find("boss").GetComponent<BossController>();
        // cutsceneManager = GameObject.Find("SceneManager").GetComponent<CutsceneManager>();

        //StartCoroutine(FadeOut());
    }
    void Update()
    {
        if (isFadeIn == true)
        {
            StartCoroutine(FadeIn());
            isFadeIn = false;
        }
        // if (bossController.currentState == BossController.CurrentState.DEAD)     //
        // {
        //     // isFadeIn = 2;
        //     // StartCoroutine(FadeOutIn());
        //     isFadeIn = true;
        // }
    }
    public IEnumerator FadeIn()
    {
        Color alpha1 = blackBoard.color;
        time = 0;
        blackBoard.gameObject.SetActive(true);
        while (alpha1.a < 1f)
        {
            time += Time.deltaTime / f_time;
            alpha1.a = Mathf.Lerp(0, 1, time);
            blackBoard.color = alpha1;
            yield return null;
        }
        // yield return new WaitForSeconds(2.0f);
        // cutsceneManager.FadeFlow(true);
    }
    public IEnumerator FadeOut()
    {
        Color alpha1 = blackBoard.color;
        time = 0f;
        blackBoard.gameObject.SetActive(true);
        while (alpha1.a > 0f)
        {
            time += Time.deltaTime / f_time;
            alpha1.a = Mathf.Lerp(1, 0, time);
            blackBoard.color = alpha1;
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
