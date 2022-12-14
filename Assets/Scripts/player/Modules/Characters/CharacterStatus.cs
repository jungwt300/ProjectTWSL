using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Player.Modules.Characters
{
    public class CharacterStatus : MonoBehaviour
    {
        [Header("Default Status")]
        public float max_health;
        public float max_stamina;
        public float staminaIncrement;
        [Header("Status")]
        // public byte vitality;
        // public byte endurance;
        // public byte strength;
        [Header("Equipment")]
        [Header("Debug")]
        public PlayerController playerController;
        private float stamina;
        private float health;
        [SerializeField] Slider hpBar;
        [SerializeField] Slider staminaBar;
        void Awake()
        {
            staminaBar.maxValue = max_stamina;
            hpBar.maxValue = max_health;
            playerController = GameObject.Find("player").GetComponent<PlayerController>();
            //max_health = 100.0f;
            health = max_health;
            //max_stamina = 100.0f;
            stamina = max_stamina;
            //staminaIncrement = 0.1f;
        }
        void Update()
        {
            staminaBar.value = stamina;
            hpBar.value = health;
            if (stamina <= max_stamina)
            {
                switch(playerController.GetActiveState()){
                    case PlayerController.eActiveState.DEFAULT:
                        recoveryStamina(staminaIncrement);
                        break;
                    case PlayerController.eActiveState.DELAY_ATTACK:
                        recoveryStamina(staminaIncrement);
                        break;
                    case PlayerController.eActiveState.DELAY_ROLL:
                        recoveryStamina(staminaIncrement);
                        break;
                    default:
                        break;
                }
                
            }
        }

        private void recoveryStamina(float staminaIncrement)
        {
            this.stamina += staminaIncrement;
            stamina = Mathf.Clamp(stamina, -100f, max_stamina);
        }
        //getter setter
        ///HP ??????
        public float GetMaxHealth()
        {
            return this.max_health;
        }
        public void SetMaxHealth(int health)
        {
            this.health = health;
        }
        public float GetCurrentHealth()
        {
            return this.health;
        }
        public void TakeDamage(float value)
        {
            this.health -= value;
        }
        public void HealHealth(float value)
        {
            this.health += value;
        }
        ///???????????? ??????
        public float GetMaxStamina()
        {
            return this.max_stamina;
        }
        public float GetCurrentStamina()
        {
            return stamina;
        }
        public void ReduceStamina(float value)
        {
            this.stamina -= value;
        }
    }
}