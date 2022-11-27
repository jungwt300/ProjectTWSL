using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
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
            DELAY_ROLL,
            ATTACK,
            DELAY_ATTACK,
            TAKEDAMAGED
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
        Roll roll;
        Attack attack;
        CharacterStatus characterStatus;
        Animator animator;
        [Header("Vector Value")]
        public Vector3 ObjectDirection;
        public Vector3 PrimeDirection;
        public Vector3 joystickDirection;
        public Vector3 currentPosition;
        public Vector3 jumpDirection;
        [Header("GameOver")]
        public AudioClip audioDied;
        public AudioClip audioDefated;
        private AudioSource audioSource;
        public fadein YouDied;
        public fadein Defeated;
        public CutsceneManager changer;
        public bool isGameOver;


        void Start()
        {
            //playerCamera = GetComponent<Camera>();
            characterController = GetComponent<CharacterController>();
            animator = GetComponentInChildren<Animator>();
            roll = GameObject.Find("player").GetComponent<Roll>();
            attack = GameObject.Find("player").GetComponent<Attack>();
            characterStatus = GetComponent<CharacterStatus>();
            ObjectDirection = Vector3.forward;
            PrimeDirection = Vector3.forward;
            changer = GameObject.Find("SceneManager").GetComponent<CutsceneManager>();
            audioSource = GetComponent<AudioSource>();
            // pivotPoint = playerCamera.transform.position;
        }
        void Update()
        {
            if (characterStatus.GetCurrentHealth() >= 0)
            {
                debugRay();
                AddGravity();
                if (GetActiveState() != eActiveState.TAKEDAMAGED)
                {
                    Move();
                    AttackFront();
                    LockOnTarget();
                }
            }
        
        }
        private void Move()
        {
            //Debug.DrawRay(playerCamera.position, new Vector3(playerCamera.forward.x, 0f, playerCamera.forward.z).normalized, Color.red);
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            joystickDirection = new Vector3(horizontal, 0.0f, vertical).normalized;   //방향은 x y 의 입력값에 따라 정규화된 1 0 값을 반환한다.
            currentPosition = Vector3.Slerp(currentPosition, joystickDirection, Time.deltaTime * friction);    //선형 보간 으로 스칼라값 구한다.
            //currentPosition = Vector3.Lerp(currentPosition, joystickDirection, Time.deltaTime * friction);   //선형 보간
            float currentPositionScala = currentPosition.magnitude;
            currentPositionScala = parseDot(currentPositionScala);
            //Debug.Log("Horizontal : " + horizontal +" vertical : "+ vertical);
            //Debug.Log("joystickDirection.magnitude : " + joystickDirection.magnitude);  //joystickDirection Vector3 개체의 스칼라값.
            if (currentPosition.magnitude >= 0.01f)
            { //부동 소수점 주의!
                float targetAngle = Mathf.Atan2(currentPosition.x, currentPosition.z) * Mathf.Rad2Deg + playerCamera.transform.eulerAngles.y; //카메라 기준 오브젝트 방향
                primeTargetAngle = Mathf.Atan2(joystickDirection.x, joystickDirection.z) * Mathf.Rad2Deg + playerCamera.transform.eulerAngles.y; //플레이어 기준 인풋 방향
                PrimeDirection = Quaternion.Euler(0.0f, primeTargetAngle, 0.0f) * Vector3.forward;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);   //플레이어 방향을 카메라 기준 방향으로 회전
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
        private void LockOnTarget(){
            if (Input.GetMouseButtonDown(2)){
                switch(isLockOn){
                    case false:
                        this.isLockOn = true;
                        Debug.Log("LockOn Target");
                        break;
                    case true:
                        this.isLockOn = false;
                        Debug.Log("LockOn Off");
                        break;
                }
            }
        }
        public void Jump(){
            jumpDirection = new Vector3 (0.0f,1,0.0f);
            float jumpForce = 0.3f;
            characterController.Move(jumpDirection * jumpForce* Time.deltaTime);
        }
        public void AttackFront(){
            if(GetActiveState() ==eActiveState.DEFAULT || GetActiveState() == eActiveState.DELAY_ATTACK){
                if (Input.GetMouseButtonDown(0) && (this.isAttackOn == false))
                {
                    SetActiveState(eActiveState.ATTACK);
                    attack.StartCoroutine(attack.OnEnter());
                }
            }
        }
        public void rollDodge(){
            if (Input.GetKeyDown(KeyCode.Space) && (isRollOn == false))
            {
                if ((characterStatus.GetCurrentStamina() > 0))
                {
                    Debug.Log("버튼 스페이스 입력");
                    switch (GetActiveState())
                    {
                        case PlayerController.eActiveState.DEFAULT:
                            roll.setDodgeType();
                            //attack.SetActionParam(0);
                            break;
                        case PlayerController.eActiveState.DELAY_ATTACK:
                            attack.SetActionParam(0);
                            roll.setDodgeType();
                            break;
                    }
                }
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
        private void debugRay()
        {
            Debug.DrawRay(playerBody.position, ObjectDirection, Color.red);
            Debug.DrawRay(playerBody.position, joystickDirection, Color.blue);
            Debug.DrawRay(playerBody.position, currentPosition, Color.green);
        }
        public void TakeDamaged()
        {
            if(characterStatus.GetCurrentHealth() <= 0f)
            {
                animator.SetTrigger("isDead");
                StartCoroutine(GameOver(true));
            }
            if (characterStatus.GetCurrentHealth() >= 0f)
            {
                SetActiveState(eActiveState.TAKEDAMAGED);
                if (GetActiveState() == eActiveState.TAKEDAMAGED)
                {
                    animator.SetTrigger("TakeDamage");
                    StartCoroutine(DamagedAnimation());
                }
            }
        }
        IEnumerator DamagedAnimation()
        {
            
            yield return new WaitForSeconds(2f);
            if (characterStatus.GetCurrentHealth() >= 0)
            {
                animator.SetBool("Idle", true);
                SetActiveState(eActiveState.DEFAULT);
            }
         }

        IEnumerator GameOver(bool game)
        {
            Debug.Log("YouDied");
            yield return new WaitForSeconds(1f);
            if (game == true)
            {
                audioSource.clip = audioDied;
                audioSource.Play();
                YouDied.FadeFlow(true);
            }
            else
            {
                audioSource.clip = audioDefated;
                audioSource.Play();
                Defeated.FadeFlow(true);
            }
            yield return new WaitForSeconds(2f);
            changer.FadeFlow(true);
        }
    }
}