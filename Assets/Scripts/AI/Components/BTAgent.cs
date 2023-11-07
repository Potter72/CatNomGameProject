using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BTAgent : MonoBehaviour
{
    [SerializeField] private float _delay = 5f;
    [SerializeField] private float _wanderDistance = 20f;
    [SerializeField] protected Animator _animator;
    
    public NavMeshAgent Agent;
    protected BehaviorTree Tree;
    protected GameObject Player;
    
    
    public enum ActionState { IDLE, WORKING }
    public ActionState State = ActionState.IDLE;

    public Node.Status TreeStatus = Node.Status.RUNNING;

    private WaitForSeconds waitForSeconds;

    protected Vector3 _wanderDestination;
    
    protected void Start()
    {
        Tree = new BehaviorTree();
        Agent = GetComponent<NavMeshAgent>();
        Player = GameManager.Instance.GetPlayer().gameObject;
        waitForSeconds = new WaitForSeconds(_delay);
        StartCoroutine(Behave());
    }

    IEnumerator Behave()
    {
        while(true)
        {
            TreeStatus = Tree.Process();
            yield return waitForSeconds;
        }
    }

    protected Node.Status GoToLocation(Vector3 destination)
    {
        Vector3 pos = transform.position;
        float distanceToTarget = Vector3.Distance(destination, pos);
        // Debug.Log($"Distance to target is {distanceToTarget}");
        // Debug.DrawRay(transform.position, destination - transform.position, Color.blue);
        if (State == ActionState.IDLE)
        {
            Agent.SetDestination(destination);
            State = ActionState.WORKING;
        }

        else if (Vector3.Distance(Agent.pathEndPosition, destination) >= 10f)
        {
            State = ActionState.IDLE;
            return Node.Status.FAILURE;
        }

        else if(distanceToTarget < 5f)
        {
            State = ActionState.IDLE;
            _animator.SetBool("Walking", false);
            _animator.SetBool("Running", false);
            return Node.Status.SUCCESS;
        }

        return Node.Status.RUNNING;
    }

    protected void ChangeDelay(float delay)
    {
        _delay = delay;
        waitForSeconds = new WaitForSeconds(_delay);
    }


    protected Node.Status SetWanderDestination()
    {
        Vector2 rp = Random.insideUnitCircle * _wanderDistance;
        Vector3 randomPoint = new Vector3(rp.x, 0, rp.y);
        _wanderDestination = randomPoint + transform.position;

        NavMeshHit hit;

        // + 1f in third parameter is to ensure that it will hit a spot in navmesh
        if (NavMesh.SamplePosition(_wanderDestination, out hit, _wanderDistance + 1f, NavMesh.AllAreas))
        {
            _wanderDestination = hit.position;
        }

        ChangeDelay(0.5f);

        _animator.SetBool("Walking", true);
        
        return Node.Status.SUCCESS;
    }

    protected virtual Node.Status MoveToWanderDestination()
    {
        Node.Status status = Node.Status.FAILURE;

        status = GoToLocation(_wanderDestination);

        return status;
    }

    //private void Update()
    //{
    //    Debug.DrawLine(transform.position, Agent.destination, Color.blue);
    //}
}
