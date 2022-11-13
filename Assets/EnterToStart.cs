using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnterToStart : MonoBehaviour
{
    CutsceneManager cutsceneManager;
    // Start is called before the first frame update
    void Start()
    {
        cutsceneManager = GameObject.Find("SceneManager").GetComponent<CutsceneManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            cutsceneManager.FadeFlow(true);
        }
    }
}
