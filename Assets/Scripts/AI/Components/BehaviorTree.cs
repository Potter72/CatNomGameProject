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
        string treePrintout = "";
        Stack<NodeLevel> nodeStack = new Stack<NodeLevel>();
        Node currentNode = this;
        nodeStack.Push(new NodeLevel { Level = 0, CurrentNode = currentNode });

        while (nodeStack.Count != 0)
        {
            NodeLevel nextNode = nodeStack.Pop();
            treePrintout += new string('-', nextNode.Level) + nextNode.CurrentNode.Name + "\n";
            for (int i = nextNode.CurrentNode.Children.Count - 1; i >= 0; i--)
            {
                nodeStack.Push(new NodeLevel { Level = nextNode.Level + 1, CurrentNode = nextNode.CurrentNode.Children[i] });
            }
        }

        //Debug.Log(treePrintout);
    }
}
