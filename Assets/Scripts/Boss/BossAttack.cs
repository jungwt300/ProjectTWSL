using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.Modules.Characters;
using Player.Modules;

namespace Boss
{
    public class BossAttack : MonoBehaviour
    {
        [SerializeField] Transform bossBody;
        BossController bossController;
        public bool isHitboxOn;
        public AudioClip audioSwing;
        public AudioClip audioPownd;
        public AudioSource audioSource;
//CameraShake cameraShake;
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
        CharacterStatus characterStatus;
        PlayerController playerController;
        void Start()
        {
            currentState = eActionState.NONE;
            animator = GameObject.Find("boss").GetComponentInChildren<Animator>();
            bossController = GameObject.Find("boss").GetComponent<BossController>();
            audioSource = GetComponent<AudioSource>();
            characterStatus = GameObject.Find("player").GetComponent<CharacterStatus>();
            playerController = GameObject.Find("player").GetComponent<PlayerController>();
        }
        void OnAction(int type)
        {
            switch (type)
            {
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
            float move_firstDelay = 0.1f;
            float move_secondDelay = 0.4f;
            float afterDelay = 0.2f;
            switch (act)
            {
                case eAttackType.ATTACK1:
                    move_firstDelay = 0.1f;
                    move_secondDelay = 0.6f;
                    afterDelay = 0.8f;
                    audioSource.clip = audioSwing;
                    audioSource.volume = 0.5f;
                    break;
                case eAttackType.ATTACK2:
                    move_firstDelay = 0.3f;
                    move_secondDelay = 0.6f;
                    afterDelay = 0.2f;
                    audioSource.clip = audioSwing;
                    audioSource.volume = 0.5f;
                    break;
                case eAttackType.ATTACK3:
                    move_firstDelay = 0.2f;
                    move_secondDelay = 0.8f;
                    afterDelay = 0.3f;
                    audioSource.clip = audioSwing;
                    audioSource.volume = 0.5f;
                    break;
                case eAttackType.ATTACK4:
                    move_firstDelay = 0.1f;
                    move_secondDelay = 1f;
                    afterDelay = 0.8f;
                    audioSource.clip = audioPownd;
                    audioSource.volume = 1.2f;
                    break;
                case eAttackType.ATTACK5:
                    move_firstDelay = 0.6f;
                    move_secondDelay = 0.8f;
                    afterDelay = 0.8f;
                    audioSource.clip = audioPownd;
                    audioSource.volume = 5f;
                    break;

            }
            actionTime = move_firstDelay + move_secondDelay + afterDelay;
            Vector3 moveDir = transform.forward;
            Vector3 targetPosition = moveDir * 2f * Time.deltaTime;
            //animator.SetBool("isAttackOn", true);
            
            yield return 0;
            while (elapsedTime < move_firstDelay)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            elapsedTime = 0f;
            audioSource.Play();
            if (act == eAttackType.ATTACK4)
            {
                //cameraShake.Shake();
            }
            else if(act == eAttackType.ATTACK5){
                //cameraShake.Shake();
            }
            isHitboxOn = true;
            while (elapsedTime < move_secondDelay)
            {//선딜레이
                bossBody.Translate(targetPosition);
                
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            isHitboxOn = false;
            Debug.Log("캐스팅 끝 후딜레이 시작");
            bossController.currentState = BossController.CurrentState.ATTACK;
            isAttackOn = false;
            //bossController.isAttackOn = false;
            //SetActionState(eActionState.DELAY_AFTER);
            //isHitboxOn = false;
            while (this.elapsedTime < afterDelay)
            {
                this.elapsedTime += Time.deltaTime;
                yield return null;
            }
            yield return new WaitForSeconds(Time.deltaTime);
            Debug.Log("후딜레이 끝");
            PlayAttackAnimation(eAttackType.NONE);
            bossController.currentState = BossController.CurrentState.WALK;
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
                    //OnHit();
                    break;
                case eAttackType.ATTACK2:
                    animator.SetBool("ATTACK2", true);
                    //OnHit();
                    break;
                case eAttackType.ATTACK3:
                    animator.SetBool("ATTACK3", true);
                    //OnHit();
                    break;
                case eAttackType.ATTACK4:
                    animator.SetBool("ATTACK4", true);
                    //OnHit();
                    break;
                case eAttackType.ATTACK5:
                    animator.SetBool("ATTACK5", true);
                    //OnHit();
                    break;

            }
        }
        public void OnHit()
        {
            if (playerController.activeState != PlayerController.eActiveState.ROLL)     //Player != ROLL 때만 20데미지를 준다.
                if (playerController.activeState != PlayerController.eActiveState.TAKEDAMAGED)
                {
                    characterStatus.TakeDamage(60);
                    playerController.TakeDamaged();
                }
        }
    }
}
