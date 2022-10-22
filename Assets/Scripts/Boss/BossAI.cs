
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace Boss.Modules{
public class BossAI : MonoBehaviour
    {
        enum bossState{
            IDLE,
            WALK,
            RUN
        }
        [SerializeField] Transform player;
        public float range;
        public NavMeshAgent bossNav;
        public float speed;
        void Start() {
            bossNav = this.gameObject.GetComponent<NavMeshAgent>();
        }
        void Update(){
            range =  Vector3.Distance(player.position, this.transform.position);
            MoveAround();

        }

        void LookAt(){
            transform.LookAt(player);
        }
        void Move(){
            if(range !> 3.0f){
                bossNav.speed = 2.5f;
                bossNav.destination = player.position;
            }
        }
        void MoveAround(){
            float targetAngle = Mathf.Atan2(this.transform.position.x, this.transform.position.z) * Mathf.Rad2Deg;
            Vector3 objectLeft = Quaternion.Euler(0.0f, targetAngle, 0.0f) * Vector3.left;
            transform.Translate(objectLeft * Time.deltaTime);
            LookAt();
        }
        void Run()
        {
            if (range !> 5.0f)
            {
                bossNav.speed = 5.0f;
                bossNav.destination = player.position;
            }
        }
        IEnumerator OnEnter(){

            yield return null;
        }
        void DebugRay(){
            Debug.DrawRay(this.transform.position, player.position , Color.red);
            
        }
    }
}