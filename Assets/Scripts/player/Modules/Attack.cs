using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.Modules.Characters;

namespace Player.Modules
{
    public class Attack : MonoBehaviour
    {
        public enum eActionState{
            NONE = 0,
            DELAY_CASTING = 1,
            DELAY_AFTER = 2,
            ROLL = 3
        }
        [Header("Config Parameter")]
        public float duration;  //지속시간
        public float staminaUsage;
        public float afterDelay;    //후 딜레이
        public List<Animation> animations;
        [Header("Action State")]
        public eActionState currentActionState;
        bool isAfterDelayOn;

        public int actionParam;  // 0은 default
        public float elapsedTime;   //경과시간
        public bool isDelayCatchOn;
        public bool isAttackOn;
        public bool isRollOn;
        private CharacterStatus characterStatus;
        private PlayerController playerController;
        private CharacterController characterController;
        private Animator animator;
        private Roll rollSkill;
        void Start(){
            animator = GameObject.Find("player").GetComponentInChildren<Animator>();
            playerController = GameObject.Find("player").GetComponent<PlayerController>();
            characterController = GameObject.Find("player").GetComponent<CharacterController>();
            characterStatus = GameObject.Find("player").GetComponent<CharacterStatus>();
            rollSkill = GameObject.Find("player").GetComponent<Roll>();
            actionParam = 0;
            currentActionState = eActionState.NONE;
            isDelayCatchOn = false;
            isAttackOn = false;
            isAfterDelayOn = false;
        }
        void Update() {

            if(Input.GetMouseButtonDown(0) && (this.isAttackOn == false)){
                StartCoroutine(OnEnter());
            }
            if(playerController.GetActiveState() != PlayerController.eActiveState.ROLL){
                OnAction();
            }
        }
        void OnAction(){
            switch(currentActionState){
                case eActionState.NONE: //평상시
                    if(Input.GetMouseButtonDown(0) && actionParam != 3)
                    {
                        this.isAttackOn = true;
                    }
                    break;
                case eActionState.DELAY_CASTING://시전후
                    break;
                case eActionState.ROLL:
                    break;
                case eActionState.DELAY_AFTER:
                    if(Input.GetMouseButtonDown(0)){
                        this.isAttackOn = true;
                    }
                    if(Input.GetKeyDown(KeyCode.Space)){
                        this.isRollOn = true;
                        this.SetActionState(eActionState.ROLL);
                    }
                    break;
            }
        }
        IEnumerator OnEnter(){
            characterStatus.ReduceStamina(staminaUsage);
            animator.SetBool("isAttackOn",true);
            this.isAttackOn = true;
            this.isDelayCatchOn = false;
            this.elapsedTime = 0.0f;
            Debug.Log("코루틴 시작 공격 애니메이션 재생");
            playerController.SetActiveState(PlayerController.eActiveState.ATTACK);
            PlayAttackAnimation();
            SetActionState(eActionState.DELAY_CASTING);
            yield return new WaitForSeconds(this.duration);
            Debug.Log("캐스팅 끝 후딜레이 시작");
            playerController.SetActiveState(PlayerController.eActiveState.DELAY_ATTACK);
            isAttackOn = false;
            SetActionState(eActionState.DELAY_AFTER);
            while(this.elapsedTime < this.afterDelay){
                this.elapsedTime += Time.deltaTime;
                if(this.isAttackOn == true){
                        Debug.Log("후딜레이 캐치 새로운 코루틴 시작");
                        SetActionState(eActionState.DELAY_CASTING);
                        this.elapsedTime = 0;
                        yield break;
                }
                else if(this.isRollOn == true){
                    Debug.Log("후딜레이 구르기로 캐치");
                    SetActionState(eActionState.ROLL);
                    this.elapsedTime = 0;
                    yield break;

                }
                yield return null;
            }  
                yield return new WaitForSeconds(Time.deltaTime);
                Debug.Log("후딜레이 끝");
                playerController.SetActiveState(PlayerController.eActiveState.DEFAULT);
                animator.SetBool("isAttackOn", false);
                SetActionParam(0);
                this.isAttackOn = false;
                SetActionState(eActionState.NONE);
                //this.isDelayCatchOn = false;
        }
        public void PlayAttackAnimation(){
            switch(this.actionParam){
                case 0:
                    animator.SetInteger("attackCounter", 0);
                    this.actionParam = 1;
                break;
                case 1:
                    animator.SetInteger("attackCounter", 1);
                    this.actionParam = 2;
                break;
                case 2:
                    animator.SetInteger("attackCounter", 2);
                    this.actionParam = 3;
                    break;
                case 3:
                    animator.SetInteger("attackCounter", 3);
                    this.actionParam = 0;
                break;
            }
        }
        public void SetActionState(eActionState actionState){
            this.currentActionState = actionState;
        }
        private void SetActionParam(int parameter)
        {
            this.actionParam = parameter;
        }
        private void SetActionParamToNext(int parameter)
        {
            switch (parameter)
            {
                case 0:
                    this.actionParam = 1;
                    break;
                case 1:
                    this.actionParam = 2;
                    break;
                case 2:
                    this.actionParam = 3;
                    break;
                case 3:
                    this.actionParam = 0;
                    break;
            }
        }
    }
}