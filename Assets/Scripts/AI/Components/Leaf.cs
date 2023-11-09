using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : Node
{
    public delegate Status Tick();
    public Tick ProcessMethod;
    public int Index;

    public Leaf() { }

    public Leaf(string name, Tick processMethod)
    {
        Name = name;
        ProcessMethod = processMethod;
    }

    public Leaf(string name, Tick processMethod, int index)
    {
        Name = name;
        ProcessMethod = processMethod;
        Index = index;
    }

    public override Status Process()
    {
        Node.Status status;

        if(ProcessMethod != null)
        {
            status = ProcessMethod();
        }
        else
        {
            status = Status.FAILURE;
        }

        string s = $"{Name} ";

        switch(status)
        {
            case Status.FAILURE:
                s += $"<color=red> {status}</color>";
                break;
            case Status.SUCCESS:
                s += $"<color=blue> {status}</color>";
                break;
            case Status.RUNNING:
                s += $"<color=yellow> {status}</color>";
                break;
        }

        //Debug.Log(s);
        return status;
    }
}
