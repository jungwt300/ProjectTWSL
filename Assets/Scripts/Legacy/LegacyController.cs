using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegacyController : MonoBehaviour
{
    [SerializeField] float sensitivity = 1f;
    [SerializeField] Transform playerBody;
    [SerializeField] Transform playerCamera;
    //[SerializeField] float cameraRange = 3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rotateCamera();
    }

    private void rotateCamera()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X")*sensitivity, Input.GetAxis("Mouse Y") * sensitivity);
        Vector3 camAngle = playerCamera.rotation.eulerAngles;
        float x = camAngle.x - mouseDelta.y;
        if(x < 180f)
        {
            x = Mathf.Clamp(x, -1, 70f);
        }
        else
        {
            x = Mathf.Clamp(x, 300f, 361f);
        }
        playerCamera.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
    }
    public void SetCameraRange()
    {
        
    }
}
