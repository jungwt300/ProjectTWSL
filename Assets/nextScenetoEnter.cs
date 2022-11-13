using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class nextScenetoEnter : MonoBehaviour
{
    public CutsceneManager cutscene;
    // Start is called before the first frame update
    void Start()
    {
        cutscene =GameObject.Find("SceneManager").GetComponent<CutsceneManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("ÄÆ½Å Àç¼Ä On");
            cutscene.FadeFlow(true);

        }
    }
}
