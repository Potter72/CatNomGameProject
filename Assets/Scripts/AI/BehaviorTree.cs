using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTree : Node
{
    public BehaviorTree()
    {
        Name = "Tree";
    }

    public BehaviorTree(string name)
    {
        Name = name;
    }
    
    struct NodeLevel
    {
        public int Level;
        public Node CurrentNode;
    }

    public override Status Process()
    {
        if (Children.Count == 0) return Status.SUCCESS;
        return Children[CurrentChild].Process();
    }

    public void PrintTree()
    {
        // Print the entire tree
    }
}
