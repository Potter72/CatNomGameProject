using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Node
{
    public Sequence(string name)
    {
        Name = name;
    }

    public override Status Process()
    {
        Status childStatus = Children[CurrentChild].Process();
        if (childStatus == Status.RUNNING)
        {
            return Status.RUNNING;
        }

        if (childStatus == Status.FAILURE)
        {
            CurrentChild = 0;
            foreach(Node node in Children)
            {
                node.Reset();
            }
            return Status.FAILURE;
        }

        CurrentChild++;
        if (CurrentChild >= Children.Count)
        {
            CurrentChild = 0;
            return Status.SUCCESS;
        }

        return Status.RUNNING;
    }
}
