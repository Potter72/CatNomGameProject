using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Shroom : BTAgent
{
    [SerializeField] private float _detectionRange = 5f;
    [SerializeField] private float _wanderDistance = 5f;

    private bool _chased = false;

    new void Start()
    {
        base.Start();
        Selector shrooming = new Selector("Shrooming");
        Leaf moveAway = new Leaf("Move Away", MoveAway);
        Leaf wander = new Leaf("Wander", Wander);

        shrooming.AddChild(moveAway);
        shrooming.AddChild(wander);

        Tree.AddChild(shrooming);

        Tree.PrintTree();
    }

    private void Update()
    {
        Vector3 direction = (transform.position - Player.transform.position).normalized * _detectionRange;
        Debug.DrawRay(Player.transform.position, direction, Color.red);
    }

    public Node.Status MoveAway()
    {
        Node.Status status = Node.Status.FAILURE;

        Vector3 playerPos = new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z);
        float distance = Vector3.Magnitude(Player.transform.position - transform.position);

        if (distance > _detectionRange)
        {
            if(_chased)
            {
                Debug.Log("<color=red>Out of range</color>");
                _chased = false;
                status = Node.Status.SUCCESS;
                ChangeDelay(1f);
            }

            return status;
        }

        Vector3 destination = transform.position + (-(playerPos - transform.position).normalized * _wanderDistance);

        NavMeshHit hit;

        if (NavMesh.SamplePosition(destination, out hit, 5f, NavMesh.AllAreas))
        {
            if(!_chased)
            {
                _chased = true;
                ChangeDelay(0.1f);
                Agent.speed = 5f;
            }

            Agent.destination = hit.position;

            return Node.Status.RUNNING;
        }

        return status;
    }

    public Node.Status Wander()
    {
        if (_chased)
        {
            return Node.Status.FAILURE;
        }

        Node.Status status = Node.Status.FAILURE;

        Vector2 rp = Random.insideUnitCircle * _wanderDistance;
        Vector3 randomPoint = new Vector3(rp.x, 0, rp.y);
        Vector3 destination = randomPoint + transform.position;

        NavMeshHit hit;

        if (NavMesh.SamplePosition(destination, out hit, 5f, NavMesh.AllAreas))
        {
            ChangeDelay(1f);
            status = GoToLocation(hit.position);
        }

        return status;
    }
}
