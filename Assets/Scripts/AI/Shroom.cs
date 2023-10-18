using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Shroom : BTAgent
{
    public GameObject Player;

    new void Start()
    {
        base.Start();
        Leaf moveAway = new Leaf("Move Away", MoveAway);
    }

    public Node.Status MoveAway()
    {
        Node.Status status = Node.Status.SUCCESS;
        return status;
    }
}
