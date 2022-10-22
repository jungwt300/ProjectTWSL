using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class BossController : MonoBehaviour
{
    public enum CurrentState{
        NONE,       //
        IDLE,       //사거리에 player가 없는 상태
        TRACE,      //사거리 안에 player가 감지
        ATTACK,     //attackRange 안에 player가 감지
        RUSH,       // 돌진
        DAMAGED,    // 피격판정
        DEAD        // 생사여부
    }
    [SerializeField] float range;   //거리
    // [SerializeField] List<Pattern> bosspatterns;
    public CurrentState currentState = CurrentState.IDLE;
    // public Pattern pattern = Pattern._NONE;

    private Transform _transform;       //Boss의 좌표값
    public Transform playerTransform;      //player의 
    private NavMeshAgent navMeshAgent;      //이동 제어를 위한
    public LayerMask whatIsTarget; // 추적 대상 레이어
    private Animator animator;
    public GameObject target;
    // LineRenderer lr;

    public float tranceRange = 30.0f;   //추적 사거리
    public float attackRange = 10f;    //공격 사거리
    public float rushRange = 20.0f;      //돌진 사용 가능 사거리

    private bool isDead = false;    //생사 여부

    public int rushTarget = 0;
    public bool _target = false;
    public bool DamagedTarget = false;

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
    }
    IEnumerator CheckState()
    {
        while (!isDead)     //Boss가 살아있으면
        {
            yield return new WaitForSeconds(0.2f);
            range = Vector3.Distance(playerTransform.position, _transform.position);    //range = player와 Boss의 거리

            if (range <= attackRange)
            {
                currentState = CurrentState.ATTACK;

            }
            else if (range <= rushRange)
            {
                currentState = CurrentState.RUSH;
            }

            else if (range <= tranceRange)
            {

            }
            
            else
            {
                currentState = CurrentState.IDLE;
                rushTarget = 0;
                _target = false;
            }
        }
    }
    IEnumerator CheckStateForAction()
    {
        while (!isDead)     //Boss가 살아있으면
        {
            switch (currentState)
            {
                case CurrentState.IDLE:
                Debug.Log("BOSS = IDLE");
                break;

                case CurrentState.TRACE:
                Debug.Log("BOSS = TRACE");
                IsTrance();
                break;

                case CurrentState.ATTACK:
                Debug.Log("BOSS = ATTACK");
                isAttack();


                break;

                case CurrentState.RUSH:
                Debug.Log("BOSS = RUSH");
                IsRush();

                break;

                case CurrentState.NONE:
                // Stop();
                break;
            }

            yield return null;

        }

    }
    private void IsIdle()
    {
        animator.SetBool("idle", true);

        _transform.position = Vector3.zero;
        rushTarget = 0;
    }

    private void isAttack()
    {

    }

    private void IsRush()
    {
        Debug.Log("Rushing");
    }
    private void IsTrance()
    {
            navMeshAgent.destination = playerTransform.position;    //player를 따라감
    }
    void Update()
    {
    }
}
