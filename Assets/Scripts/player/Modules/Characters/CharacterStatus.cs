using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Modules.Characters
{
    public class CharacterStatus : MonoBehaviour
    {
        [Header("Default Status")]
        public float max_health;
        public float max_stamina;
        public float staminaIncrement;
        public float movementSpeed;
        [Header("Status")]
        // public byte vitality;
        // public byte endurance;
        // public byte strength;
        [Header("Equipment")]
        [Header("Debug")]
        public PlayerController playerController;
        private float stamina;
        private float health;
        void Awake()
        {
            playerController = GameObject.Find("player").GetComponent<PlayerController>();
            max_health = 100.0f;
            health = max_health;
            max_stamina = 100.0f;
            stamina = max_stamina;
            staminaIncrement = 0.1f;
        }
        void Update()
        {
            if ((playerController.GetActiveState() == PlayerController.eActiveState.DEFAULT) && (stamina <= max_stamina))
            {
                recoveryStamina(staminaIncrement);
            }
        }

        private void recoveryStamina(float staminaIncrement)
        {
            this.stamina += staminaIncrement;
            stamina = Mathf.Clamp(stamina, -100f, max_stamina);
        }
        //getter setter
        ///HP 관련
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
        ///스테미나 관련
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