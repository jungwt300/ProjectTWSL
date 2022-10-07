using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Modules.Characters
{
    public class PlayerController : MonoBehaviour
    {
        public enum eDirection
        {
            FRONT,
            BACK,
            LEFT,
            RIGHT,
            UP,
            DOWN
        }
        public enum eActiveState
        {
            DEFAULT,
            ROLL,
            ATTACK,
            JUMP
        };
        [Header("Config Parameter")]    //이동
        [SerializeField] Transform playerBody;
        [SerializeField] Transform playerCamera;

        [SerializeField] float moveSpeed = 6.0f;
        [SerializeField] float friction = 10.0f;
        [SerializeField] float turnSmoothTime = 0.1f;
        [SerializeField] private bool isJumping;
        // [SerializeField] float gravity = -9.8f;

        // float originalStepOffset = 0.0f;

        [Header("Debug Value")]
        public eActiveState activeState;

        public float turnSmoothVelocity;
        public float primeTargetAngle;
        public bool isGrounded;
        CharacterController characterController;
        //CharacterStatus characterStatus;
        Animator animator;
        [Header("Vector Value")]
        public Vector3 ObjectDirection;
        public Vector3 PrimeDirection;
        public Vector3 joystickDirection;
        public Vector3 currentPosition;
        // public Vector3 moveDirection;
        // public Vector3 jumpDirection;

        // public Vector3 turnVelocity;
        public Vector3 moveVelocity;
        void Start()
        {
            characterController = GetComponent<CharacterController>();
            animator = GetComponentInChildren<Animator>();
            //characterStatus = GetComponent<CharacterStatus>();
            ObjectDirection = Vector3.forward;
            PrimeDirection = Vector3.forward;

        }
        void Update()
        {
            debugRay();
            Move();
            AddGravity();
            Jump();



        }
        private void Move()
        {
            if (activeState == eActiveState.DEFAULT)
            {
                //Debug.DrawRay(playerCamera.position, new Vector3(playerCamera.forward.x, 0f, playerCamera.forward.z).normalized, Color.red);
                float horizontal = Input.GetAxisRaw("Horizontal");
                float vertical = Input.GetAxisRaw("Vertical");

                // joystickDirection = transform.TransformDirection(joystickDirection);

                // joystickDirection = new Vector3(horizontal, joystickDirection.y, vertical).normalized;   //방향은 x z 의 입력값에 따라 정규화된 1 0 값을 반환한다.
                joystickDirection = new Vector3(horizontal, 0, vertical).normalized;   //방향은 x z 의 입력값에 따라 정규화된 1 0 값을 반환한다.
                currentPosition = Vector3.Slerp(currentPosition, joystickDirection, Time.deltaTime * friction);    //선형 보간 으로 스칼라값 구한다.
                //currentPosition = Vector3.Lerp(currentPosition, joystickDirection, Time.deltaTime * friction);   //선형 보간
                float currentPositionScala = currentPosition.magnitude;
                currentPositionScala = parseDot(currentPositionScala);

                //Debug.Log("Horizontal : " + horizontal +" vertical : "+ vertical);
                //Debug.Log("joystickDirection.magnitude : " + joystickDirection.magnitude);  //joystickDirection Vector3 개체의 스칼라값.
                if (currentPosition.magnitude >= 0.01f)
                { //부동 소수점 주의!
                    float targetAngle = Mathf.Atan2(currentPosition.x, currentPosition.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y; //카메라 기준 오브젝트 방향
                    primeTargetAngle = Mathf.Atan2(joystickDirection.x, joystickDirection.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y; //플레이어 기준 인풋 방향
                    PrimeDirection = Quaternion.Euler(0.0f, primeTargetAngle, 0.0f) * Vector3.forward;
                    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);   //플레이어 방향을 카메라 기준 방향으로 회전
                    transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
                    ObjectDirection = Quaternion.Euler(0.0f, targetAngle, 0.0f) * Vector3.forward;
                    characterController.Move(ObjectDirection.normalized * currentPositionScala * moveSpeed * Time.deltaTime);
                }
                animator.SetFloat("Speed", currentPositionScala);
            }

        }
        //지역함수 , Getter Setter
        private float parseDot(float val)
        {
            var str = val.ToString("0.00");
            return float.Parse(str);
        }
        private void AddGravity()
        {
            Vector3 gravityDir = new Vector3(0, -1, 0);
            characterController.Move(gravityDir * StaticValues.GRAVITY_FORCE * Time.deltaTime);
        }
        public Vector3 GetVector(Vector3 vectorValue)
        {
            return vectorValue;
        }

        public Vector3 GetObjectDirection(int direction)
        {   //현제 플레이어의 방향
            return this.ObjectDirection * direction;
        }
        public Vector3 GetJoystickDirection()
        {   //조이스틱의 벡터값
            return joystickDirection;
        }
        public Vector3 GetPrimeDirection(int direction)
        {   // 카메라기준 플레이어의 목표 방향
            return PrimeDirection;
        }
        public void SetInputDirection()
        {   //진행방향을 set 한다.
            transform.rotation = Quaternion.Euler(0f, primeTargetAngle, 0f);
        }
        public void SetActiveState(eActiveState activeState)
        {   //현재 조작 상태
            this.activeState = activeState;
        }
        public eActiveState GetActiveState()
        {
            return this.activeState;
        }
        private void Jump()//eActiveState JUMP
        {
            // var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");         
            // Vector3 StartJumpPos;
            // Vector3 EndJumpPos;
            // // jumpDirection = new Vector3(transform.position.x, transform.position.y, transform.position.z);  //jumpSpeed 만큼 올라감
            // StartJumpPos = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.position.y, gameObject.transform.localPosition.z); //jumpSpeed
            // EndJumpPos = new Vector3(transform.localPosition.x, 4.0f, transform.localPosition.z); //

            float jumpForce = 15.0f;
            // float rotatioSpeed = 90;
            // float speed = 3f;

            if (characterController.isGrounded) //땅에 있으면
            {
                moveVelocity = transform.forward * vertical;
                // moveVelocity = transform.forward * speed * vertical;
                // turnVelocity = transform.up * rotatioSpeed * horizontal;

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    moveVelocity.y = jumpForce;

                    // transform.position = Vector3.Lerp(transform.position, EndJumpPos, 1f);             
                    isJumping = true;
                    Debug.Log("isJumping = true");
                    animator.SetBool("isJump", true);
                    if (characterController.isGrounded)
                    animator.SetBool("isJump", false);
                    
                    // SetActiveState(eActiveState.JUMP);
                    
                }
                
            Debug.Log("isJumping = false");
            isJumping = false;

            // animator.SetBool("isJump", false);
            // SetActiveState(eActiveState.DEFAULT);
            // moveVelocity.y = 0f;
            }

            moveVelocity.y += -StaticValues.GRAVITY_FORCE * Time.deltaTime;     //떨어지는 속력
            characterController.Move(moveVelocity * Time.deltaTime);        //잘 모름

            // transform.Rotate(turnVelocity * Time.deltaTime);

        }

        private void debugRay()
        {
            Debug.DrawRay(playerBody.position, ObjectDirection, Color.red);
            Debug.DrawRay(playerBody.position, joystickDirection, Color.blue);
            Debug.DrawRay(playerBody.position, currentPosition, Color.green);
        }
    }
}