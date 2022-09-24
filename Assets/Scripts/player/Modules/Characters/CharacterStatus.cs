using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Modules.Characters
{
    public class CharacterStatus : MonoBehaviour
    {
        [Header("Status")]
        private int max_health;
        private int health;
        private int max_stamina;
        private int stamina;
        private int staminaIncrement;

        void Start() {
            max_health = 100;
            health = max_health;
            max_stamina = 1000;
            stamina = max_stamina;
            staminaIncrement = 10;
        }
        void update() {
            recoveryStamina(staminaIncrement);
        }

        private void recoveryStamina(int staminaIncrement){
            this.stamina += staminaIncrement;
            stamina =Mathf.Clamp(stamina,-1000, max_stamina);
        }
        //getter setter
        ///HP 관련
        public int GetMaxHealth(){
            return this.max_health;
        }
        public void SetMaxHealth(int health){
            this.health = health;
        }
        public int GetCurrentHealth(){
            return this.health;
        }
        public void TakeDamage(int value){
            this.health -= value;
        }
        public void HealHealth(int value){
            this.health += value;
        }
        ///스테미나 관련
        public int GetMaxStamina(){
            return this.max_stamina;
        }
        public void ReduceStamina(int value){
            this.stamina -= value;
        }
    }
}