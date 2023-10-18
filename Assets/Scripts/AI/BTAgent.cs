using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTAgent : MonoBehaviour
{
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
        waitForSeconds = new WaitForSeconds(0.5f);
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

    private void Update()
    {
        Debug.DrawLine(transform.position, Agent.destination, Color.blue);
    }
}
