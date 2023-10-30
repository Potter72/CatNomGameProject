using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Node
{
    public Selector(string name)
    {
        Name = name;
    }

    public override Node.Status Process()
    {
        Status childStatus = Children[CurrentChild].Process();

        if (childStatus == Node.Status.RUNNING)
        {
            return childStatus;
        }

        if (childStatus == Node.Status.SUCCESS)
        {
            CurrentChild = 0;
            return childStatus;
        }

        CurrentChild++;
        if (CurrentChild == Children.Count)
        {
            CurrentChild = 0;
            return childStatus;
        }

        return Node.Status.RUNNING;
    }
}
