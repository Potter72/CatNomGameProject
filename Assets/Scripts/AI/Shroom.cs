using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Shroom : BTAgent
{
    [SerializeField] private float _detectionRange = 5f;
    [SerializeField] private float _runDistance = 10f;

    private bool _chased = false;

    new void Start()
    {
        base.Start();
        Sequence wander = new Sequence("Wander");
        Leaf setWanderDestination = new Leaf("Wander", SetWanderDestination);
        Leaf moveToWanderDestination = new Leaf("Move To Wander Destination", MoveToWanderDestination);

        Selector shrooming = new Selector("Shrooming");
        Leaf moveAway = new Leaf("Move Away", MoveAway);

        wander.AddChild(setWanderDestination);
        wander.AddChild(moveToWanderDestination);

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
                ChangeDelay(0.5f);
            }

            return status;
        }

        Vector3 destination = transform.position + (-(playerPos - transform.position).normalized * _runDistance);

        if (!_chased)
        {
            _chased = true;
            ChangeDelay(0.3f);
            Agent.speed = 5f;
        }

        Agent.destination = destination;

        return Node.Status.RUNNING;
    }

    public override Node.Status MoveToWanderDestination()
    {
        if (Vector3.Magnitude(Player.transform.position - transform.position) < _detectionRange)
        {
            _chased = true;
        }

        if (_chased)
        {
            ChangeDelay(0.1f);
            return Node.Status.FAILURE;
        }

        Node.Status status = Node.Status.FAILURE;

        status = GoToLocation(_wanderDestination);

        return status;
    }
}
