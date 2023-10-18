using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Shroom : BTAgent
{
    public GameObject Player;

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

    public Node.Status MoveAway()
    {
        Node.Status status = Node.Status.FAILURE;

        if(Vector3.Magnitude(Player.transform.position - transform.position) > 3f)
        {
            Debug.Log("Out of range");

            if(_chased)
            {
                _chased = false;
                status = Node.Status.SUCCESS;
            }

            return status;
        }

        Vector3 destination = transform.position + (-(Player.transform.position - transform.position).normalized * 3f);

        NavMeshHit hit;

        if (NavMesh.SamplePosition(destination, out hit, 5f, NavMesh.AllAreas))
        {
            _chased = true;
            status = GoToLocation(hit.position);
        }

        return status;
    }

    public Node.Status Wander()
    {
        Node.Status status = Node.Status.FAILURE;

        Vector2 rp = Random.insideUnitCircle * 3f;
        Vector3 randomPoint = new Vector3(rp.x, 0, rp.y);
        Vector3 destination = randomPoint + transform.position;

        NavMeshHit hit;

        if (NavMesh.SamplePosition(destination, out hit, 5f, NavMesh.AllAreas))
        {
            status = GoToLocation(hit.position);
        }

        return status;
    }
}
