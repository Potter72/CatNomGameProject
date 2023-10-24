using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BTAgent : MonoBehaviour
{
    [SerializeField] private float _delay = 2f;
    [SerializeField] private float _wanderDistance = 20f;

    public GameObject Player;
    public BehaviorTree Tree;
    public NavMeshAgent Agent;

    public enum ActionState { IDLE, WORKING }
    public ActionState State = ActionState.IDLE;

    public Node.Status TreeStatus = Node.Status.RUNNING;

    private WaitForSeconds waitForSeconds;

    protected Vector3 _wanderDestination;
    

    public void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        Tree = new BehaviorTree();
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

    public Node.Status GoToLocation(Vector3 destination)
    {
        Vector3 pos = new(transform.position.x, 0, transform.position.z);
        float distanceToTarget = Vector3.Distance(destination, pos);
        Debug.Log($"Distance to target is {distanceToTarget}");
        Debug.DrawRay(transform.position, destination - transform.position, Color.blue);
        if (State == ActionState.IDLE)
        {
            Agent.SetDestination(destination);
            State = ActionState.WORKING;
        }

        else if (Vector3.Distance(Agent.pathEndPosition, destination) >= 2f)
        {
            State = ActionState.IDLE;
            return Node.Status.FAILURE;
        }

        else if(distanceToTarget < 2f)
        {
            State = ActionState.IDLE;
            return Node.Status.SUCCESS;
        }

        return Node.Status.RUNNING;
    }

    public void ChangeDelay(float delay)
    {
        _delay = delay;
        waitForSeconds = new WaitForSeconds(_delay);
    }


    public Node.Status SetWanderDestination()
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

        return Node.Status.SUCCESS;
    }

    public virtual Node.Status MoveToWanderDestination()
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
