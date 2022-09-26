using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSCamera : MonoBehaviour
{
    [SerializeField]
    private Transform characterBody;
    [SerializeField]
    private Transform cameraArm;
    Animator animator;

    public float speed;
    float hAxis;
    float vAxis;

    Vector3 moveVec;
    // Start is called before the first frame update
    void Start()
    {
        animator = characterBody.GetComponent<Animator>(); 
    }

    // Update is called once per frame
    void Update()
    {
        LookAround();
        Move();
    }

    private void Move()
    {
        Debug.DrawRay(cameraArm.position, new Vector3(cameraArm.forward.x , 0, cameraArm.forward.z).normalized, Color.red);

        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        bool isMove = moveInput.magnitude != 0;
        // animator.SetBool("isMove", isMove);
        if(isMove)
        {
            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f,cameraArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f ,cameraArm.right.z).normalized;
            Vector3 moveDir = lookForward * moveInput.y + lookRight* moveInput.x;

            characterBody.forward = moveDir;
            transform.position += moveDir * Time.deltaTime * speed;
        }

        // hAxis = Input.GetAxisRaw("Horizontal");
        // vAxis = Input.GetAxisRaw("Vertical");

        // moveVec = new Vector3(hAxis,0,vAxis).normalized;    // Normalize는 대각선으로 이동값도 같게 함

        // transform.position += moveVec * speed * Time.deltaTime;
    }
    private void LookAround()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camAngle = cameraArm.rotation.eulerAngles;
        float x = camAngle.x - mouseDelta.y;

        Debug.Log(camAngle.x);

        if( x < 180f)   //camAngle 최대 최소 값 설정
        {
            x = Mathf.Clamp(x, -1f, 70f);


        }
        else
        {
            x = Mathf.Clamp(x, 335f, 361f);
        }

        cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
    }
}
