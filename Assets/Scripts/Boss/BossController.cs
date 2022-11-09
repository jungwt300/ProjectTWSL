using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossController : MonoBehaviour
{
    public enum CurrentState
    {
        IDLE,       //사거리에 player가 없는 상태
        WALK,       // 걷기
        RUN,      // 뛰기
        ATTACK,     //attackRange 안에 player가 감지
        DAMAGED,    // 피격판정//////////////////////////////// 미구현
        DEAD        // 생사여부
    }
    [SerializeField] float range;   //거리
    // [SerializeField] List<Pattern> bosspatterns;
    public CurrentState currentState = CurrentState.IDLE;
    // public Pattern pattern = Pattern._NONE;

    private Transform _transform;       //Boss의 좌표값
    [SerializeField] Transform playerTransform;      //player의 
    private NavMeshAgent navMeshAgent;      //이동 제어를 위한
    private Animator animator;
    // LineRenderer lr;

    public float runRange = 20.0f;   //추적 사거리
    public float attackRange = 5.0f;    //공격가능 사거리
    public float percentage = 0;        //공격 확률
    public float rushPct = 0;       //돌진확률
    // public int rndPercentage = Random.Range(0,10);
    public float walkRange = 10.0f;      //추적 사거리
    float speed;        //Boss의 속도
    public float bossHealth = 100.0f;   //Boss 체력
    public float bossPower = 20.0f;   //Boss 공격력

    private bool isDead = false;    //생사 여부
    public bool isRushOn;
    public bool isWalkOn;
    public int attacktype;
    public bool isLookAt;
    public bool isToFollow;

    // eActiveState playerActive;

    // Start is called before the first frame update
    void Start()
    {
        //boss
        _transform = this.gameObject.GetComponent<Transform>();
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();

        navMeshAgent = this.gameObject.GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        //player 
        // playerActive = GameObject.Find("player").GetComponent<eActiveState>();

        StartCoroutine(this.CheckState());
        StartCoroutine(this.CheckStateForAction());
        StartCoroutine("CountTime", 1);

    }
    void Update()
    {
        if (isLookAt == true)
        {
            // LookAt();
            FollowTarget();

        }

        if (isToFollow == true)
        {
            navMeshAgent.destination = playerTransform.position;
        }
        // FollowTarget();
        // LookAt();
        // if (range >= attackRange && range <= walkRange)
        // {
        //     navMeshAgent.speed = 5.0f;
        //     // WalkAround();
        // }
        // WalkAround();
        // pathFinder.enabled = false;
        // NavMeshAgent.stop;
    }
    IEnumerator CheckState()        //boss의 조건
    {
        while (!isDead)     //Boss가 살아있으면
        {
            yield return new WaitForSeconds(0.2f);
            range = Vector3.Distance(playerTransform.position, _transform.position);    //range = player와 Boss의 거리

            if (range <= attackRange)
            {
                currentState = CurrentState.ATTACK;
            }
            else if (range <= walkRange)
            {
                currentState = CurrentState.WALK;
            }
            else if (range <= runRange)
            {
                currentState = CurrentState.RUN;
            }
            else
            {
                currentState = CurrentState.IDLE;
            }
        }
    }
    IEnumerator CheckStateForAction()       //boss의 상태의 따른 동작
    {
        while (!isDead)     //Boss가 살아있으면
        {
            switch (currentState)
            {
                case CurrentState.IDLE:
                    // IsIdle();
                    break;

                case CurrentState.RUN:
                    IsRun();
                    isLookAt = true;
                    break;

                case CurrentState.ATTACK:
                    if (percentage >= 10)
                    {
                        attacktype = 0;
                        animator.SetBool("isAttack", false);

                        //     // isAttackOn = true;
                        //     // yield return new WaitForSeconds(delayTime);
                        //     // isAttackOn = false;
                        if (currentState == CurrentState.ATTACK)
                        {
                            isAttack();

                            isLookAt = false;
                            attackRange = 50.0f;
                            yield return new WaitForSeconds(1.0f);
                            attackRange = 5.0f;
                            isLookAt = true;


                            //     isAttackOn = true;
                            //     yield return new WaitForSeconds(delayTime);
                            //     isAttackOn = false;
                        }
                    }


                    // if (percentage >= 10 && currentState == CurrentState.ATTACK)
                    // {
                    // switch (attacktype)
                    // {
                    //     case 0:
                    //         Debug.Log("Attack");
                    //         animator.SetBool("isAttack", false);

                    //         break;
                    //     case 1:
                    //         Debug.Log("Attack2");
                    //         animator.SetBool("isAttack", false);
                    //         yield return new WaitForSeconds(1f);
                    //         animator.SetBool("isAttack2", false);

                    //         break;
                    // }

                    break;

                case CurrentState.WALK:
                    isLookAt = true;
                    IsWalk();
                    break;

            }

            yield return null;

        }

    }
    IEnumerator CountTime(float delayTime)  //ATTACK 확률 계산
    {
        int rndPercentage = Random.Range(1, 10);     //1~9 더함
                                                     // if (percentage >= 10 && percentage <= 12)

        if (rushPct >= 10)
        {
            rushPct = 0;
            // yield return new WaitForSeconds(delayTime);
        }
        if (percentage >= 10)
        {
            percentage = 0;
            yield return new WaitForSeconds(delayTime);
            // attackRange = 5.0f;
            // attacktype = 0;

        }

        // Debug.Log("Time : " + Time.time);
        // Debug.Log("Percentage : " + percentage + "0 % ");
        percentage += rndPercentage;        //percentage = percentage + rndPercentage;
        rushPct += 1;
        yield return new WaitForSeconds(delayTime);
        StartCoroutine("CountTime", 1);
    }
    public void LookAt()
    {
        if (currentState != CurrentState.IDLE)
        {
            transform.LookAt(playerTransform);
        }
    }
    private void IsIdle()
    {
        // animator.SetBool("isIdle",true);
    }

    private void isAttack()
    {
        Debug.Log("BOSS = ATTACK");
        animator.SetBool("isAttack", true);

        navMeshAgent.speed = 0.0f;
        isToFollow = false;

        // if (GameObject.Find("player").GetComponent<PlayerController>)   //플레이어가 ROLL이 아니면 
        // {
        //     // playerHealth -= Damage;
        // }
    }

    private void IsWalk()
    {
        navMeshAgent.speed = 3f;
        isToFollow = true;
        // navMeshAgent.destination = playerTransform.position;
        animator.SetBool("isRun", true);

        // else
        // {
        //     navMeshAgent.speed = 2.5f;
        //     navMeshAgent.destination = playerTransform.position;
        //     // navMeshAgent.destination = playerTransform.position;
        //     // WalkAround();
        // }

    }
    // public void WalkAround()
    // {
    //     if (currentState == CurrentState.WALK)
    //     {
    //         Debug.Log("Boss = WalkAround");
    //         float targetAngle = Mathf.Atan2(this.transform.position.x, this.transform.position.z) * Mathf.Rad2Deg;
    //         Vector3 objectLeft = Quaternion.Euler(0.0f, targetAngle, 0.0f) * Vector3.left;
    //         transform.Translate(objectLeft * Time.deltaTime);
    //         LookAt();
    //     }

    // }
    private void IsRun()
    {
        isToFollow = true;
        navMeshAgent.speed = 10f;

        animator.SetBool("isRun", true);
        // navMeshAgent.destination = playerTransform.position;
    }
    void FollowTarget()
    {
        if (playerTransform != null)
        {
            Vector3 dir = playerTransform.transform.position - this.transform.position;
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 3);
        }
    }

}
