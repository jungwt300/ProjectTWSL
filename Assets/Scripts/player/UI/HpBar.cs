using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Player.UI{
    public class HpBar : MonoBehaviour
    {
        public Slider healthBar;
        public BossController bossController;

        void Start()
        {
            bossController = GameObject.Find("boss").GetComponent<BossController>();
        }
        private void Update()
        {
            healthBar.value = bossController.hp;
        }
    }
}