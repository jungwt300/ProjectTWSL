using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player.Modules.Characters{
    public class PlayerController : MonoBehaviour{
        [Header("Config Parameter")]
        [SerializeField] Transform playerBody;
        [SerializeField] Transform playerCamera;

        [SerializeField] float moveSpeed = 6f;
        [SerializeField] float friction = 10f;
        [SerializeField] float turnSmoothTime = 0.1f;
        
        public enum eActiveState{
            DEFAULT,
            ROLL,
            ATTACK,

        };
        [Header("Debug Value")]
        public eActiveState activeState;

        public float turnSmoothVelocity;
        //public Vector3 forwardDirection;
        CharacterController characterController;
        Animator animator;
        public Vector3 moveDir;
        public Vector3 forwardDirection;
        public Vector3 currentDirection;

        
        void Start()
        {
            characterController = GetComponent<CharacterController>();
            animator = GetComponentInChildren<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            debugRay();
            Move();
            AddGravity();
            //rollSKill.GetComponent<Roll>().OnActive();
        }
        private void Move(){
            if(activeState == eActiveState.DEFAULT){
                //Debug.DrawRay(playerCamera.position, new Vector3(playerCamera.forward.x, 0f, playerCamera.forward.z).normalized, Color.red);
                float horizontal = Input.GetAxisRaw("Horizontal");
                float vertical = Input.GetAxisRaw("Vertical");

                forwardDirection = new Vector3(horizontal, 0f, vertical).normalized;   //방향은 x y 의 입력값에 따라 정규화된 1 0 값을 반환한다.
                currentDirection = Vector3.Slerp(currentDirection, forwardDirection, Time.deltaTime * friction);    //구면 선형 보간 으로 스칼라값 구한다.
                //currentDirection = Vector3.Lerp(currentDirection, forwardDirection, Time.deltaTime * friction);   //선형 보간
                float currentDirectionScala = currentDirection.magnitude;
                currentDirectionScala = parseDot(currentDirectionScala);

                //Debug.Log("Horizontal : " + horizontal +" vertical : "+ vertical);
                //Debug.Log("forwardDirection.magnitude : " + forwardDirection.magnitude);  //forwardDirection Vector3 개체의 스칼라값.
                if(currentDirection.magnitude >= 0.01f){ //부동 소수점 주의!
                    float targetAngle = Mathf.Atan2(currentDirection.x, currentDirection.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
                    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y,targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                    transform.rotation = Quaternion.Euler(0f, angle, 0f);
                    moveDir = Quaternion.Euler(0f,targetAngle,0f) * Vector3.forward;
                    characterController.Move(moveDir.normalized * currentDirectionScala * moveSpeed * Time.deltaTime);
                    }
                    animator.SetFloat("Speed", currentDirectionScala);
            }
        }
            
        private float parseDot(float val)
        {
            var str = val.ToString("0.00");
            return float.Parse(str);
        }
            private void AddGravity(){
                Vector3 gravityDir = new Vector3(0,-1,0);
                characterController.Move(gravityDir*StaticValues.GRAVITY_FORCE*Time.deltaTime);
            }
            public Vector3 GetVector(Vector3 vectorValue){
                return vectorValue;
            }
            public void SetActiveState(eActiveState activeState){
                this.activeState = activeState;
            }
            private void debugRay(){
            Debug.DrawRay(playerBody.position, moveDir, Color.red);
            Debug.DrawRay(playerBody.position, forwardDirection, Color.blue);
            Debug.DrawRay(playerBody.position, currentDirection, Color.green);
            }
        }
    }