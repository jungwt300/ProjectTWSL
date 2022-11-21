using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    public float hp = 1000;
    public float stamina = 100f;
    public float recoveryStamina = 1f;
    public Slider hpGauge;
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

    private Transform boss;       //Boss의 좌표값
    public Vector3 bossV;
    [SerializeField] Transform player;      //player의 
    // public Vector3 playerV;

    private NavMeshAgent navMeshAgent;      //이동 제어를 위한
    private Animator animator;
    // LineRenderer lr;

    public float runRange = 20.0f;   //추적 사거리
    public float attackRange = 7.0f;    //공격가능 사거리
    public float percentage = 0;        //공격 확률
    // public float rushPct = 0;       //돌진확률
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
        hpGauge.maxValue = hp;
        //boss
        boss = this.gameObject.GetComponent<Transform>();
        bossV = this.gameObject.transform.localPosition;

        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        // playerV = GameObject.FindWithTag("Player").GetComponent<Vector3>();

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
        hpGauge.value = hp;
        Mathf.Clamp(stamina, 0, 100);
        if (currentState != CurrentState.RUN)
        {
            stamina += recoveryStamina;
        }
        if (isLookAt == true)
        {
            // LookAt();
            FollowTarget();

        }

        if (isToFollow == true)
        {
            navMeshAgent.destination = player.position;
            // animator.SetBool("isIDLE", true);
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
    public void ReduceHp(int damage)
    {
        this.hp -= damage;
    }
    IEnumerator CheckState()        //boss의 조건
    {
        while (!isDead)     //Boss가 살아있으면
        {
            yield return new WaitForSeconds(Time.deltaTime);    //프레임 단위로 체크를 함
            range = Vector3.Distance(player.position, boss.position);    //range = player와 Boss의 거리

            // if (range <= attackRange)       //공격이 가능한 사거리
            // {
            //     currentState = CurrentState.ATTACK;
            // }
            // else if (range <= walkRange)
            // {
            //     currentState = CurrentState.WALK;
            // }
            // else if (range <= runRange)
            // {
            //     currentState = CurrentState.RUN;
            // }
            // else
            // {
            //     currentState = CurrentState.IDLE;
            // }

            // if (percentage == 0 && currentState == CurrentState.WALK && range <= attackRange)       //공격이 가능한 사거리
            // {
            //     currentState = CurrentState.ATTACK;
            //     Debug.Log("Boss = ATTACK");
            //     // yield return new WaitForSeconds(1.0f);

            // }
            if (currentState == CurrentState.ATTACK && percentage == 0)    //CurrentState.ATTACK 이면 1초 동안 유지
            {
                isLookAt = false;
                isToFollow = false;
                yield return new WaitForSeconds(1.0f);
            }
            if (range <= walkRange)
            {
                currentState = CurrentState.WALK;
                // Debug.Log("Boss = WALK");

            }
            else if (range <= runRange)
            {
                currentState = CurrentState.RUN;
                // Debug.Log("Boss = RUN");

            }
            else
            {
                currentState = CurrentState.IDLE;
                // currentState = CurrentState.ATTACK;
                // yield return new WaitForSeconds(1.0f);
                
            }
        }
    }
    IEnumerator CheckStateForAction()       //boss의 상태의 따른 동작 프레임 단위
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
                    // if (percentage == 0)
                    // {

                    // }
                    transform.position = Vector3.MoveTowards(boss.transform.position, player.transform.position, 0.05f);
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

        // if (rushPct >= 10)
        // {
        //     rushPct = 0;
        //     // yield return new WaitForSeconds(delayTime);
        // }
        if (percentage >= 10 && range <= attackRange)   //percentage 가 10 이상, 거리가 7안에 있으면
        {
            percentage = 0;
            
            // attackRange = 5.0f;
            // attacktype = 0;
            // animator.SetBool("isRUN", true);
            if (percentage == 0 && currentState == CurrentState.WALK)   //percentage == 0, boss가 WALK 상태일 때
            {
                isAttack();
                currentState = CurrentState.ATTACK;
            // yield return new WaitForSeconds(1.0f);

            }
        }

        // Debug.Log("Time : " + Time.time);
        // Debug.Log("Percentage : " + percentage + "0 % ");
        if (currentState == CurrentState.WALK) //player 가 attackRange 안에 있을때만 올라감
        {
            percentage += rndPercentage;        //percentage = percentage + rndPercentage;

        }
        // else if (currentState == CurrentState.WALK)
        // {
        //     percentage += 0;
        // }
        // rushPct += 1;
        yield return new WaitForSeconds(delayTime);
        StartCoroutine("CountTime", 1);
        // Vector3 player = new Vector3(player.transform.position);     //player 전 위치
    }
    public void LookAt()
    {
        if (currentState != CurrentState.IDLE)
        {
            transform.LookAt(player);
        }
    }
    private void IsIdle()
    {
        // animator.SetBool("isIDLE",true);
    }

    private void isAttack()
    {
        
            Debug.Log("BOSS = ATTACK");
            animator.SetBool("isATTACK", true);
            animator.SetBool("isWALK", false);
            animator.SetBool("isRUN", false);

            // currentState = CurrentState.ATTACK;
            

        

        // animator.SetBool("isAttack", false);

        // navMeshAgent.speed = 0.0f;
    }

    IEnumerator AttackType(int type)
    {
        Debug.Log("AttackType 코루틴 실행");
        yield return 0;
    }
    private void IsWalk()
    {
        if (range <= 6 && range >= 8)
        {
            WalkAround();
        }
        Debug.Log("Boss = WALK");
        navMeshAgent.speed = 2f;
        isToFollow = true;
        // navMeshAgent.destination = playerTransform.position;
        animator.SetBool("isWALK", true);
        animator.SetBool("isRUN", false);
        animator.SetBool("isATTACK", false);
        animator.SetBool("isIDLE", false);

    }
    public void WalkAround()
    {
        if (currentState == CurrentState.WALK)
        {
            Debug.Log("Boss = WalkAround");
            float targetAngle = Mathf.Atan2(this.transform.position.x, this.transform.position.z) * Mathf.Rad2Deg;
            Vector3 objectLeft = Quaternion.Euler(0.0f, targetAngle, 0.0f) * Vector3.left;
            transform.Translate(objectLeft * Time.deltaTime);
            LookAt();
            isToFollow = false;
        }

    }
    private void IsRun()
    {
        Debug.Log("Boss = RUN");   
        isToFollow = true;
        navMeshAgent.speed = 15f;

        animator.SetBool("isRUN", true);
        animator.SetBool("isWALK", false);
        // navMeshAgent.destination = playerTransform.position;
    }
    void FollowTarget()
    {
        if (player != null)
        {
            Vector3 dir = player.transform.position - this.transform.position;
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 3);
        }
    }

}
