using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Modules{
    public class Weapon : MonoBehaviour
    {
        public bool hitAvailable;
        private Attack attack;
        BossController controller;
        // Start is called before the first frame update
        void Start()
        {
            hitAvailable = true;
            attack = GameObject.Find("player").GetComponent<Attack>();
            controller = GameObject.Find("boss").GetComponent<BossController>();
        }

        // Update is called once per frame
        void Update()
        {
            if(attack.GetStateHitboxOn() == false){
                hitAvailable = true;
            }
        }
        void OnTriggerEnter(Collider other) {
            if(other.gameObject.tag == "Enemy"){
                if(attack.GetStateHitboxOn() == true){
                    if(hitAvailable == true){
                        Debug.Log("Weapons hitbox Hitted");
                        hitAvailable = false;
                        controller.ReduceHp(45);
                    }
                }
            }
        }
    }
}