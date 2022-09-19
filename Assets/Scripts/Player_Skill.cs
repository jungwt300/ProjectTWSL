using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Skill : MonoBehaviour
{
    public bool jDown;
    bool isJump;
    public float jumpPower = 15f;
    public Vector3 jumpSpeed = Vector3.zero;
    Rigidbody rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponentInChildren<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        jDown = Input.GetButtonDown("Jump");
        Jump();

    }
    void Awake()
    {
        
    }
    void GetInput()
    {
        jDown = Input.GetButtonDown("Jump");
    }
    private void Jump()
        {
            // jDown = Input.GetButtonDown("Jump");

            if(jDown && !isJump)
            {
                rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
                isJump = true;
            
            }
        }
    void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.tag == "Floor")
            {
                isJump = false;
            }
        }

    private void Dash()
    {

    }
    private void Attack()
    {

    }
}
