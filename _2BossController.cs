using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class _2BossController : MonoBehaviour
{
    public enum CurrentState{
        IDLE,       //사거리에 player가 없는 상태
        TRACE,      //사거리 안에 player가 감지
        ATTACK,     //attackRange 안에 player가 감지
        RUSH,       // 돌진
        DEAD        //생사여부
    }
    public CurrentState currentState = CurrentState.IDLE;

    private Transform _transform;       //Boss의 좌표값
    private Transform playerTransform;      //player의 좌표값
    private NavMeshAgent navMeshAgent;      
    private Animator animator;
    public GameObject targetPosition;
    LineRenderer lr;


    public float tranceRange = 15.0f;   //추적 사거리
    public float attackRange = 3.5f;    //공격 사거리
    public float rushRange = 10.0f;      //돌진 사용 가능 사거리

    private bool isDead = false;    //생사 여부
    // Start is called before the first frame update
    void Start()
    {
        _transform = this.gameObject.GetComponent<Transform>();
        playerTransform = GameObject.FindWithTag("player").GetComponent<Transform>();
        
        navMeshAgent = this.gameObject.GetComponent<NavMeshAgent>();
        animator = this.gameObject.GetComponent<Animator>();
        lr = GetComponent<LineRenderer>();

        // navMeshAgent.destination =playerTransform.position;


        StartCoroutine(this.CheckState());
        StartCoroutine(this.CheckStateForAction());
    }
    IEnumerator CheckState()
    {
        while (!isDead)     //Boss가 살아있으면
        {
            yield return new WaitForSeconds(0.2f);

            float range = Vector3.Distance
            (playerTransform.position, _transform.position);    //range = player와 Boss의 거리

            if (range <= attackRange)
            {
                currentState = CurrentState.ATTACK;
            }
            else if (range <= tranceRange)
            {
                currentState = CurrentState.TRACE;
            }
            // else if (range <= rushRange)
            // {
            //     currentState = CurrentState.RUSH;
            // }
            else
            {
                currentState = CurrentState.IDLE;
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
                // navMeshAgent.Stop();
                // animator.SetBool("isTrance", false);
                break;

                case CurrentState.TRACE:
                Debug.Log("BOSS = TRACE");
                navMeshAgent.destination = playerTransform.position;
                // navMeshAgent.Resume();
                // animator.SetBool("isTrance",true);
                break;

                case CurrentState.ATTACK:
                Debug.Log("BOSS = ATTACK");
                yield return new WaitForSeconds(0.2f);
                // animator.SetBool("isAttack",true);
                yield return new WaitForSeconds(0.2f);

                break;

                case CurrentState.RUSH:
                Debug.Log("BOSS = RUSH");
                // yield return new WaitForSeconds(0.5f);
                IsRush();
                // yield return new WaitForSeconds(1.0f);
                
                break;
            }

            yield return null;

        }

    }
    private void IsRush()
    {
        // LineRenderer lr;
        // for (int i = 0; i < lr.positionCount; i++)
        // {
        //     transform.position = Vector3.Slerp(_transform.position, targetPosition.transform.position, i / (float)(lr.positionCount - 1));
        // }
        // float hh = new Vector3.Slerp(_transform, playerTransform, 0.1f);
        // transform.position = Vector3.Slerp(transform.position, targetPosition.transform.position, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        // transform.position = Vector3.Slerp(gameObject.transform.position, targetPosition.transform.position, 0.05f);
                // IsRush();

        // if (Input.GetKeyDown(KeyCode.P))
        // {

        //         IsRush();
                
        // }
    }
}
