using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class DepSequence : Node
{
    private BehaviorTree _dependency;
    private NavMeshAgent _agent;
    
    public DepSequence(string name, BehaviorTree d, NavMeshAgent a)
    {
        Name = name;
        _dependency = d;
        _agent = a;
    }

    public override Status Process()
    {
        if (_dependency.Process() == Status.FAILURE)
        {
            _agent.ResetPath();

            foreach (Node n in Children)
            {
                n.Reset();
            }

            return Status.FAILURE;
        }
        
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
