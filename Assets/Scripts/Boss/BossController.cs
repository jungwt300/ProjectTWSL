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
    // public enum Pattern{
    //     _NONE,
    //     A, 
    //     B, 
    //     C   //RUSH
    // }
    // public List<string> GachaList = new List<string>(){
    //     "ATTACK1",
    //     "ATTACK2",
    //     "RUSH"
    // };
    // public int RandomInt;
    // public Pattern pattern;
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
    public Transform targetPosition ;
    // LineRenderer lr;

    public float bossHealth = 100f;


    public float tranceRange = 30.0f;   //추적 사거리
    public float attackRange = 10f;    //공격 사거리
    public float rushRange = 20.0f;      //돌진 사용 가능 사거리

    private bool isDead = false;    //생사 여부

    public int rushTarget = 0;
    public bool _target = false;
    public bool DamagedTarget = false;


    // Start is called before the first frame update
    void Start()
    {
        _transform = this.gameObject.GetComponent<Transform>();
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        
        navMeshAgent = this.gameObject.GetComponent<NavMeshAgent>();
        animator = this.gameObject.GetComponent<Animator>();
        // lr = GetComponent<LineRenderer>();

        // navMeshAgent.destination =playerTransform.position;


        StartCoroutine(this.CheckState());
        StartCoroutine(this.CheckStateForAction());
    }
    IEnumerator CheckState()
    {
        while (!isDead)     //Boss가 살아있으면
        {
            yield return new WaitForSeconds(0.2f);

            // float range = Vector3.Distance
            range = Vector3.Distance
            (playerTransform.position, _transform.position);    //range = player와 Boss의 거리

            if (range <= attackRange)
            {
                currentState = CurrentState.ATTACK;
                // rushTarget = 0;

            }
            // else if (range >= attackRange && range <= rushRange)
            else if (range <= rushRange)
            {
                currentState = CurrentState.RUSH;
            }
            //     // currentState = CurrentState.RUSH;
            //     rushTarget = 2;
            //     // _target = true;
            // }
            // else if (currentState == CurrentState.TRACE && pattern == Pattern.A)
            // {
            //     currentState = CurrentState.RUSH;

            // }
            // else if (currentState == CurrentState.TRACE)
            // {
            //     if (Input.GetKeyDown(KeyCode.Alpha1))
            //     {
            //         currentState = CurrentState.RUSH;
            //     }
            // }
            else if (range <= tranceRange)
            {
                currentState = CurrentState.TRACE;
                // playerTarget = true;
                // rushTarget = 1;
                // _target = true;


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
                // navMeshAgent.Stop;
                // NavMeshAgent.stop;
                // IsIdle();
                // animator.SetBool("idle", true);
                // _target = true;

                break;

                case CurrentState.TRACE:
                Debug.Log("BOSS = TRACE");
                IsTrance();
                // animator.SetBool("isTrance",true);

                // navMeshAgent.Resume();
                // _target = true;

                break;

                case CurrentState.ATTACK:
                Debug.Log("BOSS = ATTACK");
                // yield return new WaitForSeconds(0.2f);
                // animator.SetBool("isAttack",true);
                // yield return new WaitForSeconds(0.2f);
                isAttack();


                break;

                case CurrentState.RUSH:
                Debug.Log("BOSS = RUSH");
                // animator.SetBool("isRush",true);
                // yield return new WaitForSeconds(0.5f);
                // IsRush();
                // yield return new WaitForSeconds(0.3f);
                // currentState = CurrentState.IDLE;
                // animator.SetBool("isRush",false);
                // _target = true;
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
        // _target = false;


        // navMeshAgent.destination = playerTransform.position;    //player를 따라감


        // if (target != null)
        // {
        //     // target = new target;
        //     targetPosition = target.transform.position;

        //     // currentState(CurrentState.IDLE);
        // }
    }

    private void isAttack()
    {
        // transform.position = Vector3.forward;

    }

    private void IsRush()
    {
        // for ()
        // {
            // transform.position = Vector3.Slerp(gameObject.transform.position, targetPosition.transform.position, 1f);
            // transform.position = Vector3.lerp(_transform.position, playerTransform.position, 0.03f);
        // if (rushTarget ==2 && _target == true)
        // if (_target == true)

        // {
	    //     transform.position = Vector3.Lerp(transform.position, playerTransform.position, Time.deltaTime * 1f);

        // }
	    transform.position = Vector3.Lerp(transform.position, playerTransform.position, Time.deltaTime * 1f);
        Debug.Log("Rushing");
        // }
        // transform.position = Vector3.Slerp(transform.position, targetPosition.transform.position, 1f);
    }
    private void IsTrance()
    {
        // animator.SetBool("isTrance",true);

        // playerTarget = true;
        // if (rushTarget == 1 && _target == false)
        // if (rushTarget == 0 && rushTarget == 2 && _target == false)

        // {
            navMeshAgent.destination = playerTransform.position;    //player를 따라감
            // if (range >= rushRange)
            // {
            //     // _target = true;
            // }
        // }
        // navMeshAgent.destination = playerTransform.position;    //player를 따라감
        // if (currentState == CurrentState.TRACE)
        // {

        // }

    }
    
    // Update is called once per frame
    void Update()
    {
        targetPosition = playerTransform.transform;

        if (currentState != CurrentState.RUSH)
        {
            
        }
    }
}
