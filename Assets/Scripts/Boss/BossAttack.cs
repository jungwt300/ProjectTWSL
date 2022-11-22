using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss{
    public class BossAttack : MonoBehaviour
    {
        [SerializeField] Transform bossBody;
        BossController bossController;
        public enum eActionState
        {
            NONE = 0,
            DELAY_CASTING = 1,
            DELAY_AFTER = 2,
        }
        public enum eAttackType
        {
            NONE,
            ATTACK1,
            ATTACK2,
            ATTACK3,
            ATTACK4,
            ATTACK5
        }
        eAttackType attackType;
        bool isAttackOn;
        float elapsedTime;
        public float actionTime;
        public float afterDelay;
        [SerializeField] private eActionState currentState;
        [SerializeField] public Animator animator;
        void Start() {
            currentState = eActionState.NONE;
            animator = GameObject.Find("boss").GetComponentInChildren<Animator>();
            bossController = GameObject.Find("boss").GetComponent<BossController>();
        }
        void OnAction(int type){
            switch(type){
              case 0:
                    break;
                case 1:
                    break;  
            }
        }
        public IEnumerator OnEnter(float time, eAttackType act)
        {
            yield return new WaitForSeconds(time);
            PlayAttackAnimation(act);
            this.isAttackOn = true;
            this.elapsedTime = 0.0f;
            float move_firstDelay = 0.2f;
            float move_secondDelay = 0.4f;
            float afterDelay = 0.2f;
            switch(act)
            {
                case eAttackType.ATTACK1:
                move_firstDelay = 1f;
                move_secondDelay = 1f;
                afterDelay = 1f;
                    break;
                case eAttackType.ATTACK2:
                    move_firstDelay = 1f;
                    move_secondDelay = 1f;
                    afterDelay = 0f;
                    break;
                case eAttackType.ATTACK3:
                    move_firstDelay = 1f;
                    move_secondDelay = 1f;
                    afterDelay = 1f;
                    break;
                case eAttackType.ATTACK4:
                    move_firstDelay = 1f;
                    move_secondDelay = 1f;
                    afterDelay = 0f;
                    break;
                case eAttackType.ATTACK5:
                    move_firstDelay = 1f;
                    move_secondDelay = 1f;
                    afterDelay = 0f;
                    break;

            }
            actionTime = move_firstDelay + move_secondDelay+ afterDelay;
            Vector3 moveDir = transform.forward;
            Vector3 targetPosition = moveDir * 2f * Time.deltaTime;
            //animator.SetBool("isAttackOn", true);
            yield return 0;
            while (elapsedTime < move_firstDelay)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            while (elapsedTime < move_secondDelay)
            {//선딜레이
                bossBody.Translate(targetPosition);
                //Vector3 dir = bossBody.transform.position - this.transform.position;
                //this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 15f);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
                        Debug.Log("캐스팅 끝 후딜레이 시작");
            bossController.currentState = BossController.CurrentState.ATTACK;
            isAttackOn = false;
            //bossController.isAttackOn = false;
            //SetActionState(eActionState.DELAY_AFTER);
            //isHitboxOn = false;
            while(this.elapsedTime < afterDelay){
                this.elapsedTime += Time.deltaTime;
                yield return null;
            }
            yield return new WaitForSeconds(Time.deltaTime);
            Debug.Log("후딜레이 끝");
            PlayAttackAnimation(eAttackType.NONE);
            bossController.currentState = BossController.CurrentState.IDLE;
            animator.SetBool("isAttackOn", false);

            this.isAttackOn = false;
            currentState = eActionState.NONE;
        }
        public void PlayAttackAnimation(eAttackType act)
        {
            switch (act)
            {
                case eAttackType.NONE:
                    animator.SetBool("ATTACK1", false);
                    animator.SetBool("ATTACK2", false);
                    animator.SetBool("ATTACK3", false);
                    animator.SetBool("ATTACK4", false);
                    animator.SetBool("ATTACK5", false);
                    break;
                case eAttackType.ATTACK1:
                Debug.Log("ATTACK1 시작 ");
                    animator.SetBool("ATTACK1", true);
                    break;
                case eAttackType.ATTACK2:
                    animator.SetBool("ATTACK2", true);
                    break;
                case eAttackType.ATTACK3:
                    animator.SetBool("ATTACK3", true);
                    break;
                case eAttackType.ATTACK4:
                    animator.SetBool("ATTACK4", true);
                    break;
                case eAttackType.ATTACK5:
                    animator.SetBool("ATTACK5", true);
                    break;
                    
            }
        }
    }
}
