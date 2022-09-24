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
        };
        [Header("Config Parameter")]
        [SerializeField] Transform playerBody;
        [SerializeField] Transform playerCamera;

        [SerializeField] float moveSpeed = 6f;
        [SerializeField] float friction = 10f;
        [SerializeField] float turnSmoothTime = 0.1f;
        [Header("Debug Value")]
        public eActiveState activeState;

        public float turnSmoothVelocity;
        public float primeTargetAngle;
        CharacterController characterController;
        CharacterStatus characterStatus;
        Animator animator;
        [Header("Vector Value")]
        public Vector3 ObjectDirection;
        public Vector3 PrimeDirection;
        public Vector3 joystickDirection;
        public Vector3 currentPosition;
        void Start()
        {
            characterController = GetComponent<CharacterController>();
            animator = GetComponentInChildren<Animator>();
            characterStatus = GetComponent<CharacterStatus>();
            ObjectDirection = Vector3.forward;
            PrimeDirection = Vector3.forward;
        }
        void Update()
        {
            debugRay();
            Move();
            AddGravity();
        }
        private void Move()
        {
            if (activeState == eActiveState.DEFAULT)
            {
                //Debug.DrawRay(playerCamera.position, new Vector3(playerCamera.forward.x, 0f, playerCamera.forward.z).normalized, Color.red);
                float horizontal = Input.GetAxisRaw("Horizontal");
                float vertical = Input.GetAxisRaw("Vertical");

                joystickDirection = new Vector3(horizontal, 0f, vertical).normalized;   //방향은 x y 의 입력값에 따라 정규화된 1 0 값을 반환한다.
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
                    PrimeDirection = Quaternion.Euler(0f, primeTargetAngle, 0f) * Vector3.forward;
                    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);   //플레이어 방향을 카메라 기준 방향으로 회전
                    transform.rotation = Quaternion.Euler(0f, angle, 0f);
                    ObjectDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
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
        private void debugRay()
        {
            Debug.DrawRay(playerBody.position, ObjectDirection, Color.red);
            Debug.DrawRay(playerBody.position, joystickDirection, Color.blue);
            Debug.DrawRay(playerBody.position, currentPosition, Color.green);
        }
    }
}