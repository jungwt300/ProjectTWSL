using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.Modules.Characters;    //이거 있어야 PlayerControlelr 작동  

namespace Player.Modules
{
    public class Roll : MonoBehaviour
    {
        private enum eRollType
        {
            ROLL,
            BACKSTEP
        }

[Header("Config Parameter")]
        public float staminaUsage;
        public float duration;  //지속시간
        public float afterDelay;    //후 딜레이
        float force;
[Header("Debug Value")]
        private PlayerController playerController;
        private CharacterController characterController;    //루트모션 적용시 삭제 예정
        private Attack attack;
        private CharacterStatus characterStatus;    
        private Animator animator;
        private Vector3 moveDir;
        public int actionParam = 0; // 0 = 구르기 , 1 = 백스텝
        public bool isRollOn;
        public float elapsedTime;   //경과시간
        void Start()
        {
            animator = GameObject.Find("player").GetComponentInChildren<Animator>();
            playerController = GameObject.Find("player").GetComponent<PlayerController>();
            characterController = GameObject.Find("player").GetComponent<CharacterController>();
            characterStatus = GameObject.Find("player").GetComponent<CharacterStatus>();
            attack = GameObject.Find("player").GetComponent<Attack>();
            isRollOn = false;
        }
        void Update()
        {
//            Debug.Log(staminaUsage + "," + characterStatus.GetCurrentStamina());
            if (Input.GetKeyDown(KeyCode.Space) && (isRollOn ==false))
            {
                if ((characterStatus.GetCurrentStamina() > 0))
                {
                    Debug.Log("버튼 스페이스 입력");
                    switch(playerController.GetActiveState()){
                        case PlayerController.eActiveState.DEFAULT:
                            setDodgeType();
                            break;
                        case PlayerController.eActiveState.DELAY_ATTACK:
                            setDodgeType();
                            break;
                    }
                }
            }
        }

        public void setDodgeType()
        {
            if (playerController.joystickDirection.magnitude >= 0.1)    //이동중일때는 구르기
            {
                playerController.SetActiveState(PlayerController.eActiveState.ROLL);
                moveDir = playerController.GetPrimeDirection(1);
                StartCoroutine(OnEnter(0));
            }
            else
            {
                playerController.SetActiveState(PlayerController.eActiveState.ROLL);    //정지 상태는 백스텝
                moveDir = playerController.GetObjectDirection(-1);
                StartCoroutine(OnEnter(1));
            }
        }
        IEnumerator OnEnter(int actionParam)
        {
            characterStatus.ReduceStamina(staminaUsage);
            attack.SetActionParam(0);
            isRollOn = true;
            playerController.isRollOn = true;
            //afterDelay = 0.1f;
            switch (actionParam)
            {
                case 0:
                    Debug.Log("Dodge Roll");
                    playerController.SetInputDirection();   //방향 재정의
                    animator.SetBool("isSlide", true);
                    duration = 0.8f;
                    force = 0.1f;
                    break;
                case 1:
                    Debug.Log("Back Step");
                    animator.SetBool("isBackStep", true);
                    duration = 0.4f;
                    force = 0.08f;
                    break;  
            }
            Vector3 targetPosition = (moveDir * 0.5f) * force;
            Vector3 currentPosition = Vector3.zero;
            animator.SetBool("isAttackOn", false);
            Debug.Log("구르기 실행");
            while (elapsedTime < duration)
            {//선딜레이
                characterController.Move(targetPosition);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            playerController.SetActiveState(PlayerController.eActiveState.DELAY_ATTACK);
            elapsedTime = 0;
            Debug.Log("Done");
            yield return new WaitForSeconds(0.0f);
            isRollOn = false;
            playerController.isRollOn = false;
            playerController.SetActiveState(PlayerController.eActiveState.DEFAULT);
            animator.SetBool("isBackStep", false);
            animator.SetBool("isSlide", false);
            Debug.Log("IsRollOn = false");
            if (elapsedTime >= duration)
            {   //전환 딜레이
                
            }
        }
    }
}