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
        public float staminaUsage = 2f;
        private bool isRollOn = false;
        private PlayerController playerController;
        private CharacterController characterController;
        private CharacterStatus characterStatus;
        private Animator animator;
        private Vector3 moveDir;
        void Start()
        {
            animator = GameObject.Find("player").GetComponentInChildren<Animator>();
            playerController = GameObject.Find("player").GetComponent<PlayerController>();
            characterController = GameObject.Find("player").GetComponent<CharacterController>();
            characterStatus = GameObject.Find("player").GetComponent<CharacterStatus>();
        }
        void Update()
        {
            Debug.Log(staminaUsage + "," + characterStatus.GetCurrentStamina());
            if (characterStatus.GetCurrentStamina() > 0)
            {
                if (Input.GetKeyDown(KeyCode.Space) && (isRollOn == false))
                {
                    characterStatus.ReduceStamina(staminaUsage);
                    isRollOn = true;
                    setDodgeDirection();
                }
            }

        }
        private void setDodgeDirection()
        {
            if (playerController.joystickDirection.magnitude >= 0.1)
            {
                playerController.SetActiveState(PlayerController.eActiveState.ROLL);
                moveDir = playerController.GetPrimeDirection(1);
                StartCoroutine(OnRoll(eRollType.ROLL));
            }
            else
            {
                playerController.SetActiveState(PlayerController.eActiveState.ROLL);
                moveDir = playerController.GetObjectDirection(-1);
                StartCoroutine(OnRoll(eRollType.BACKSTEP));
            }
        }
        IEnumerator OnRoll(eRollType rollType)
        {
            isRollOn = true;
            float elapsedTime = 0.0f;
            float duration = 0.7f;
            float force = 0.1f;
            switch (rollType)
            {
                case eRollType.ROLL:
                    Debug.Log("Dodge Roll");
                    playerController.SetInputDirection();   //방향 재정의
                    animator.SetBool("isSlide", true);
                    duration = 0.6f;
                    force = 0.8f;
                    break;
                case eRollType.BACKSTEP:
                    Debug.Log("Back Step");
                    animator.SetBool("isBackStep", true);
                    duration = 0.4f;
                    force = 0.6f;
                    break;
            }
            Vector3 targetPosition = (moveDir * 0.05f) * force;
            Vector3 currentPosition = Vector3.zero;
            while (elapsedTime < duration)
            {
                characterController.Move(targetPosition);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            Debug.Log("Done");
            GameObject.Find("player").GetComponent<PlayerController>().SetActiveState(PlayerController.eActiveState.DEFAULT);
            animator.SetBool("isBackStep", false);
            animator.SetBool("isSlide", false);
            if (elapsedTime >= duration)
            {
                yield return new WaitForSeconds(0.2f);
                Debug.Log("IsOllOn = false");
                isRollOn = false;
            }
        }
    }
}