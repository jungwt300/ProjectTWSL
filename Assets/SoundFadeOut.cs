using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFadeOut : MonoBehaviour
{
    AudioSource clip;
    bool fadeoutOn = false;
    // Start is called before the first frame update
    void Start()
    {
        clip =GameObject.Find("SceneManager").GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            fadeoutOn = true;
        }
        if (fadeoutOn == true)
        {
            clip.volume -= 0.1f * Time.deltaTime;
        }
    }

}
