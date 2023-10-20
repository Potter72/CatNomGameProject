using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.AI;

public class Dog : BTAgent
{
    [SerializeField] private float _detectionRange = 20f;
    [SerializeField] private Color _gizmoColor;
    private Vector3 _guardPosition;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        _guardPosition = transform.position;

        Selector doDog = new Selector("Do Dog");
        Sequence chaseAwayPlayer = new Sequence("Chase Away Player");
        Leaf checkPlayer = new Leaf("Check Player", CheckPlayer);
        Leaf moveToPlayer = new Leaf("Move To Player", MoveToPlayer);

        Leaf wander = new Leaf("Wander", Wander);

        chaseAwayPlayer.AddChild(checkPlayer);
        chaseAwayPlayer.AddChild(moveToPlayer);

        doDog.AddChild(chaseAwayPlayer);
        doDog.AddChild(wander);

        Tree.AddChild(doDog);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmoColor;
        Gizmos.DrawSphere(_guardPosition, _detectionRange);
    }

    public Node.Status CheckPlayer()
    {
        if(Vector3.Distance(Player.transform.position, _guardPosition) < _detectionRange)
        {
            return Node.Status.SUCCESS;
        }

        return Node.Status.FAILURE;
    }

    public Node.Status MoveToPlayer()
    {
        Node.Status status = GoToLocation(Player.transform.position);

        ChangeDelay(0.1f);

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
