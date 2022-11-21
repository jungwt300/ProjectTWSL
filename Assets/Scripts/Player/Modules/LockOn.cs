using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
public class LockOn : MonoBehaviour
{

[SerializeField] private Camera mainCamera;
[SerializeField] private CinemachineFreeLook cinemachineFreeLook;

[SerializeField] private Vector2 targetLockOffset;
[SerializeField] private KeyCode _Input;
[SerializeField] private float minDistance;
[SerializeField] private float maxDistance;
[SerializeField] private string enemyTag;

public bool isLockOn;

private float maxAngle;
private Transform currentTarget;
private Transform target;
private float mouseX;
private float mouseY;

private void Start() {
    maxAngle = 90f;
        cinemachineFreeLook.m_XAxis.m_InputAxisName = "";
        cinemachineFreeLook.m_YAxis.m_InputAxisName = "";
}
private void Update() {
    if(!isLockOn){
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
    }
    else{
        NewInputTarget(currentTarget);
    }
        cinemachineFreeLook.m_XAxis.m_InputAxisValue = mouseX;
        cinemachineFreeLook.m_YAxis.m_InputAxisValue = mouseY;
    if(Input.GetKeyDown(_Input)){
        AssignTarget();
    }
}
private void AssignTarget(){
    if(isLockOn){
        isLockOn = false;
        currentTarget = null;
        return;
    }
    if (ClosestTarget())
    {
        currentTarget = ClosestTarget().transform;
        isLockOn = true;
    }
}
    private void NewInputTarget(Transform target) // sets new input value.
    {
        if (!currentTarget) return;

        Vector3 viewPos = mainCamera.WorldToViewportPoint(target.position);

        if ((target.position - transform.position).magnitude < minDistance) return;
        mouseX = (viewPos.x - 0.5f + targetLockOffset.x) * 10f;              // you can change the [ 3f ] value to make it faster or  slower
        mouseY = (viewPos.y - 0.5f + targetLockOffset.y) * 10f;              // don't use delta time here.
    }

    private GameObject ClosestTarget() // this is modified func from unity Docs ( Gets Closest Object with Tag ). 
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag(enemyTag);
        GameObject closest = null;
        float distance = maxDistance;
        float currAngle = maxAngle;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.magnitude;
            if (curDistance < distance)
            {
                Vector3 viewPos = mainCamera.WorldToViewportPoint(go.transform.position);
                Vector2 newPos = new Vector3(viewPos.x - 0.5f, viewPos.y - 0.5f);
                if (Vector3.Angle(diff.normalized, mainCamera.transform.forward) < maxAngle)
                {
                    closest = go;
                    currAngle = Vector3.Angle(diff.normalized, mainCamera.transform.forward.normalized);
                    distance = curDistance;
                }
            }
        }
        return closest;
    }
    }
