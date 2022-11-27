using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Camera mainCamera;
    Vector3 cameraPos;
    [SerializeField] [Range(0.01f, 0.1f)] float 
        Range = 0.05f;
    [SerializeField] [Range(0.1f, 1f)] float duration =0.5f;
    // Start is called before the first frame update
    public void Shake()
    {
        cameraPos = mainCamera.transform.position;
        InvokeRepeating("StartShake", 0f, 0.005f);
        Invoke("StoptShake", duration);

    }
    void StartShake()
    {
        float cameraPosX = Random.value * shakeRange * 2 - shakeRange;
        float cameraPosY = Random.value * shakeRange * 2 - shakeRange;
        Vector3 cameraPos = mainCamera.transform.position;
        cameraPos.x += cameraPosX;
        cameraPos.y += cameraPosY;
        mainCamera.transform.position = cameraPos;

    }

    void StopShake()
    {
        CancelInvoke("StopShake");
        mainCamera.transform.position = cameraPos;
    }
}