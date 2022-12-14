using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
namespace Boss
{
    public class BossManage : MonoBehaviour
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
            DELAY_ROLL,
            ATTACK,
            DELAY_ATTACK,
        };
        public enum eDelayState
        {
            ATTACK,
            ROLL,
        }
        [Header("Config Parameter")]
        [SerializeField] Transform playerBody;
        [SerializeField] public  Camera playerCamera;
        [SerializeField] CinemachineFreeLook cinemachineFreeLook;
        [SerializeField] Transform targetBoss;

        [SerializeField] float moveSpeed = 6.0f;
        [SerializeField] float friction = 10.0f;
        [SerializeField] float turnSmoothTime = 0.1f;
        public bool isAttackOn = false;
        public bool isRollOn = false;
        public bool isLockOn = false;
        [Header("INPUT Value")]

        [Header("Debug Value")]
        public eActiveState activeState;

        public float turnSmoothVelocity;
        public float primeTargetAngle;
        CharacterController characterController;
        //Attack attack;
        //CharacterStatus characterStatus;
        Animator animator;
        [Header("Vector Value")]
        public Vector3 ObjectDirection;
        public Vector3 PrimeDirection;
        public Vector3 joystickDirection;
        public Vector3 currentPosition;
        public Vector3 jumpDirection;
        void Start()
        {
            //playerCamera = GetComponent<Camera>();
            characterController = GetComponent<CharacterController>();
            animator = GetComponentInChildren<Animator>();
            //roll = GameObject.Find("player").GetComponent<Roll>();
            //attack = GameObject.Find("player").GetComponent<Attack>();
            ObjectDirection = Vector3.forward;
            PrimeDirection = Vector3.forward;
            // pivotPoint = playerCamera.transform.position;
        }
        void Update()
        {
            debugRay();
            Move();
            AddGravity();
            AttackFront();
            //LockOnTarget();
        }
        private void Move()
        {
            //Debug.DrawRay(playerCamera.position, new Vector3(playerCamera.forward.x, 0f, playerCamera.forward.z).normalized, Color.red);
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            joystickDirection = new Vector3(horizontal, 0.0f, vertical).normalized;   //????????? x y ??? ???????????? ?????? ???????????? 1 0 ?????? ????????????.
            currentPosition = Vector3.Slerp(currentPosition, joystickDirection, Time.deltaTime * friction);    //?????? ?????? ?????? ???????????? ?????????.
            //currentPosition = Vector3.Lerp(currentPosition, joystickDirection, Time.deltaTime * friction);   //?????? ??????
            float currentPositionScala = currentPosition.magnitude;
            currentPositionScala = parseDot(currentPositionScala);
            //Debug.Log("Horizontal : " + horizontal +" vertical : "+ vertical);
            //Debug.Log("joystickDirection.magnitude : " + joystickDirection.magnitude);  //joystickDirection Vector3 ????????? ????????????.
            if (currentPosition.magnitude >= 0.01f)
            { //?????? ????????? ??????!
                float targetAngle = Mathf.Atan2(currentPosition.x, currentPosition.z) * Mathf.Rad2Deg + playerCamera.transform.eulerAngles.y; //????????? ?????? ???????????? ??????
                primeTargetAngle = Mathf.Atan2(joystickDirection.x, joystickDirection.z) * Mathf.Rad2Deg + playerCamera.transform.eulerAngles.y; //???????????? ?????? ?????? ??????
                PrimeDirection = Quaternion.Euler(0.0f, primeTargetAngle, 0.0f) * Vector3.forward;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);   //???????????? ????????? ????????? ?????? ???????????? ??????
                if (activeState == eActiveState.DEFAULT)
                {
                    if(isLockOn != true){
                    transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
                    
                    }
                    ObjectDirection = Quaternion.Euler(0.0f, targetAngle, 0.0f) * Vector3.forward;
                characterController.Move(ObjectDirection.normalized * currentPositionScala * moveSpeed * Time.deltaTime);
                }
            }
            animator.SetFloat("Speed", currentPositionScala);
        }
        public void AttackFront(){
            if(GetActiveState() ==eActiveState.DEFAULT || GetActiveState() == eActiveState.DELAY_ATTACK){
                if (Input.GetMouseButtonDown(0) && (this.isAttackOn == false))
                {
                    SetActiveState(eActiveState.ATTACK);
                    //attack.StartCoroutine(attack.OnEnter());
                }
            }
        }

        //???????????? , Getter Setter
        private float parseDot(float val)
        {
            var str = val.ToString("0.00");
            return float.Parse(str);
        }
        private void AddGravity()
        {
            Vector3 gravityDir = new Vector3(0, -1, 0);
            characterController.Move(gravityDir * 3f * Time.deltaTime);
        }
        public Vector3 GetVector(Vector3 vectorValue)
        {
            return vectorValue;
        }

        public Vector3 GetObjectDirection(int direction)
        {   //?????? ??????????????? ??????
            return this.ObjectDirection * direction;
        }
        public Vector3 GetJoystickDirection()
        {   //??????????????? ?????????
            return joystickDirection;
        }
        public Vector3 GetPrimeDirection(int direction)
        {   // ??????????????? ??????????????? ?????? ??????
            return PrimeDirection;
        }
        public void SetInputDirection()
        {   //??????????????? set ??????.
            transform.rotation = Quaternion.Euler(0f, primeTargetAngle, 0f);
        }
        public void SetActiveState(eActiveState activeState)
        {   //?????? ?????? ??????
            this.activeState = activeState;
        }
        public eActiveState GetActiveState()
        {
            return this.activeState;
        }
        private void debugRay()
        {
            Debug.DrawRay(playerBody.position, ObjectDirection, Color.red);
            Debug.DrawRay(playerBody.position, joystickDirection, Color.blue);
            Debug.DrawRay(playerBody.position, currentPosition, Color.green);
        }
    }
}