using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss{
    public class BossAttack : MonoBehaviour
    {
        [SerializeField] Transform bossBody;
        
        bool isAttackOn;
        float elapsedTime;
        public enum eActionState
        {
            NONE = 0,
            DELAY_CASTING = 1,
            DELAY_AFTER = 2,
        }
        public enum eAttackType{
            NONE,
            ATTACK1,
            ATTACK2,
            ATTACK3,
            ATTACK4,
            ATTACK5
        }
        [SerializeField] private eActionState currentState;
        [SerializeField] public Animator animator;
        void Start() {
            currentState = eActionState.NONE;
            animator = GameObject.Find("boss").GetComponentInChildren<Animator>();
        }
        void OnAction(int type){
            switch(type){
                case 0:
                    break;
                case 1:
                    break;
            }
        }
        public IEnumerator OnEnter()
        {
            animator.SetBool("isAttackOn", true);
            this.isAttackOn = true;
            this.elapsedTime = 0.0f;
            float move_firstDelay = 0.2f;
            float move_secondDelay = 0.4f;
            Vector3 moveDir = transform.forward;
            Vector3 targetPosition = moveDir * 2f * Time.deltaTime;
            animator.SetBool("isAttackOn", true);
            yield return 0;
            while (elapsedTime < move_secondDelay)
            {//선딜레이
                bossBody.Translate(targetPosition);
                Vector3 dir = bossBody.transform.position - this.transform.position;
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 15f);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
        
    }
}
