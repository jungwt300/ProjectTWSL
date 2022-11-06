
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace Boss.Modules
{
    public class BossAI : MonoBehaviour
    {
        public enum EBossState
        {
            IDLE,
            WALK,
            ATTACK,
            RUN
        }
        [SerializeField] Transform player;
        public EBossState bossState = EBossState.IDLE;
        public float range;
        public NavMeshAgent bossNav;
        public float speed;
        public float walkRange = 10;
        public float runRange = 20;
        public float attackRange = 5;
        public float walkAroundRange = 0;
        public float percentage = 0;
        public bool isDead = false;
        void Start()
        {
            bossNav = this.gameObject.GetComponent<NavMeshAgent>();
            // StartCoroutine("CountTime", 1);
            // StartCoroutine("OnEnter");

        }
        void Update()
        {
            range = Vector3.Distance(player.position, this.transform.position);
            WalkAround();
            LookAt();

        }
        IEnumerator CountTime(float delayTime)
        {
            if (percentage == 100)
            {
                percentage = 0;
            }
            // Debug.Log("Time : " + Time.time);
            Debug.Log("Percentage : " + percentage + " % ");

            percentage = percentage + 10;
            yield return new WaitForSeconds(delayTime);
            StartCoroutine("CountTime", 1);
        }
        IEnumerator OnEnter()
        {
   
            yield return null;
        }

        public void LookAt()
        {
            // if (bossState != EBossState.IDLE)
            // {
            transform.LookAt(player);
            // }
        }
        public void Walk()
        {
            if (range <= attackRange)
            {
                bossNav.speed = 2.5f;
                bossNav.destination = player.position;
            }

        }
        public void WalkAround()
        {
            // if (bossState == EBossState.WALK)
            // {
                float targetAngle = Mathf.Atan2(this.transform.position.x, this.transform.position.z) * Mathf.Rad2Deg;
                Vector3 objectLeft = Quaternion.Euler(0.0f, targetAngle, 0.0f) * Vector3.left;
                transform.Translate(objectLeft * Time.deltaTime);
                LookAt();
            // }

        }
        public void Run()
        {
            if (range <= attackRange)
            {
                bossNav.speed = 5.0f;
                bossNav.destination = player.position;
            }
            // bossNav.speed = 5.0f;
            // bossNav.destination = player.position;
        }
        public void Attack()
        {
            if (range <= attackRange)
            {
                StartCoroutine("CountTime", 1);

            }
            // StartCoroutine("CountTime", 1);
        }


        void DebugRay()
        {
            Debug.DrawRay(this.transform.position, player.position, Color.red);

        }
    }
}