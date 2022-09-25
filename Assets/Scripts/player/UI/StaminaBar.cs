using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Player.Modules.Characters;

namespace Player.Modules.UI{
public class StaminaBar : MonoBehaviour
{
        public Slider staminaSlider;
        public CharacterStatus characterStatus;

        private void Start() 
        {
            characterStatus = GameObject.Find("player").GetComponent<CharacterStatus>();
            staminaSlider.minValue = 0;
            staminaSlider.maxValue = characterStatus.GetMaxStamina();
        }

        private void Update() 
        {
            staminaSlider.value = characterStatus.GetCurrentStamina();
        }
    }
}