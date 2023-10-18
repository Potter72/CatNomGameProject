using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTAgent : MonoBehaviour
{
    [SerializeField] private float _delay = 2f;

    public BehaviorTree Tree;
    public NavMeshAgent Agent;

    public enum ActionState { IDLE, WORKING }
    public ActionState State = ActionState.IDLE;

    public Node.Status TreeStatus = Node.Status.RUNNING;

    private WaitForSeconds waitForSeconds;

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
        float distanceToTarget = Vector3.Distance(destination, transform.position);

        if (State == ActionState.IDLE)
        {
            Agent.SetDestination(destination);
            State = ActionState.WORKING;
        }

        else if (Vector3.Distance(Agent.pathEndPosition, destination) >= 2)
        {
            State = ActionState.IDLE;
            return Node.Status.FAILURE;
        }

        else if(distanceToTarget < 5)
        {
            State = ActionState.IDLE;
            return Node.Status.SUCCESS;
        }

        return Node.Status.RUNNING;
    }

    private void Update()
    {
        Debug.DrawLine(transform.position, Agent.destination, Color.blue);
    }
}
