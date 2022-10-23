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
    // public int rndPercentage = Random.Range(0,10);
    public float walkRange = 10.0f;      //추적 사거리
    float speed;        //Boss의 속도

    private bool isDead = false;    //생사 여부
    public bool isRushOn;
    public bool isWalkOn;

    // Start is called before the first frame update
    void Start()
    {
        _transform = this.gameObject.GetComponent<Transform>();
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();

        navMeshAgent = this.gameObject.GetComponent<NavMeshAgent>();
        animator = this.gameObject.GetComponent<Animator>();

        StartCoroutine(this.CheckState());
        StartCoroutine(this.CheckStateForAction());
        StartCoroutine("CountTime", 1);

    }
    void Update()
    {
        LookAt();
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

                    break;

                case CurrentState.RUN:

                    IsRun();
                    break;

                case CurrentState.ATTACK:

                    break;

                case CurrentState.WALK:

                    IsWalk();
                    break;

            }

            yield return null;

        }

    }
    IEnumerator CountTime(float delayTime)  //ATTACK 확률 계산
    {
        int rndPercentage = Random.Range(1, 10);     //1~9 더함

        if (percentage >= 10)
        {
            percentage = 0;
            if (currentState == CurrentState.ATTACK)
            {
                isAttack();
            }
        }
        // Debug.Log("Time : " + Time.time);
        Debug.Log("Percentage : " + percentage + "0 % ");
        percentage += rndPercentage;        //percentage = percentage + rndPercentage;
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

    }

    private void isAttack()
    {
        Debug.Log("BOSS = ATTACK");
    }

    private void IsWalk()
    {
        Debug.Log("Walking");
        navMeshAgent.speed = 2.5f;
        navMeshAgent.destination = playerTransform.position;
    }
    private void IsRun()
    {
        navMeshAgent.speed = 5f;
        navMeshAgent.destination = playerTransform.position;
    }

}
