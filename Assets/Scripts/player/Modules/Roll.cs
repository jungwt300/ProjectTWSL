using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.Modules.Characters;

namespace Player.Modules{
    public class Roll : MonoBehaviour{
        private enum eRollType{
            ROLL,
            BACKSTEP
        }
        private bool isRollOn = false;
        public GameObject playerBody;
        public Transform Cam;
        private PlayerController playerController;
        private CharacterController characterController;
        private Animator animator;
        public Vector3 forwardDirection;
        private Vector3 PrePosition;
        private Vector3 moveDir;
        private float turnSmoothVelocity;
        
        void Start() {
            animator = GetComponentInChildren<Animator>();
            playerController = GameObject.Find("player").GetComponent<PlayerController>();
            characterController = GetComponent<CharacterController>();
        }

        void Update() {
                if(Input.GetKeyDown(KeyCode.Space)&&(isRollOn == false))
                {
                    isRollOn = true;
                    setDodgeDirection();
                }
            
        }
        
        private void setDodgeDirection(){
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            forwardDirection = new Vector3(horizontal, 0f, vertical).normalized;
            if(forwardDirection.magnitude >= 0.1){
                float targetAngle = Mathf.Atan2(forwardDirection.x , forwardDirection.z) * Mathf.Rad2Deg + Cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0.00f);
                transform.rotation = Quaternion.Euler(0f,angle, 0f);
                moveDir =Quaternion.Euler(0f,targetAngle,0f)*Vector3.forward;

                GameObject.Find("player").GetComponent<PlayerController>().SetActiveState(PlayerController.eActiveState.ROLL);
            
                StartCoroutine(OnRoll(eRollType.ROLL));
            }
            else{
                moveDir = Quaternion.Euler(0f, transform.eulerAngles.y , 0f) * Vector3.back;
                StartCoroutine(OnRoll(eRollType.BACKSTEP));
            }
        }
        IEnumerator OnRoll(eRollType rollType){
            isRollOn = true;
            float elapsedTime = 0.0f;
            float duration = 0.7f;
            float speed = 0.5f;
            float range = 0.1f;
            switch (rollType){
                case eRollType.ROLL:
                    Debug.Log("Dodge Roll");
                    animator.SetBool("isSlide", true);
                    duration = 0.6f;
                    speed = 0.5f;
                    range = 0.12f;
                    break;
                case eRollType.BACKSTEP:
                    Debug.Log("Back Step");
                    animator.SetBool("isBackStep", true);
                    duration = 0.4f;
                    speed = 0.7f;
                    range = 0.08f;
                    break;
            }
            Vector3 targetPosition = moveDir*range;
            Vector3 currentPosition = Vector3.zero;
                while (elapsedTime < duration){
                    Vector3 rollPosition = Vector3.Slerp(PrePosition, targetPosition, speed);
                    characterController.Move(rollPosition);
                    elapsedTime += Time.deltaTime;
                    Debug.Log(elapsedTime);
                    yield return null;
                }
            //characterController.Move(targetPosition);
            //GameObject.Find("player").GetComponent<PlayerController>().SetActiveState(PlayerController.eActiveState.DEFAULT);
            Debug.Log("Done");
            GameObject.Find("player").GetComponent<PlayerController>().SetActiveState(PlayerController.eActiveState.DEFAULT);
            animator.SetBool("isBackStep", false);
            animator.SetBool("isSlide", false);
            if(elapsedTime >= duration){
                isRollOn = false;
            }
        }
    }
}