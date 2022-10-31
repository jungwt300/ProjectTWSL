using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss{
    public class bosshp : MonoBehaviour
    {
        public int hp = 100;
        Collider weaponFrom;
        public bool isDamageAvailable;
        private float recoverFrame =15.0f;
        
        // Start is called before the first frame update
        void Start()
        {
            isDamageAvailable = false;

        }

        // Update is called once per frame
        void Update()
        {
            
        }
        public void TakeDamage(int damage){
            this.hp -= damage;
        }
        private void OnTriggerEnter(Collider other) {
            if(other.transform.tag == "Weapon"){
                Debug.Log("Take damaged");
                TakeDamage(30);
                SetInvincible();
            }
        }
        public void SetInvincible(){
            float elapsedFrame = 0.0f;
            isDamageAvailable = false;
            while(recoverFrame == elapsedFrame){
                elapsedFrame += Time.deltaTime;
            }
            isDamageAvailable = true;
        }
    }
}
