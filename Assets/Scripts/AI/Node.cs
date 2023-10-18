        using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public enum Status { SUCCESS, RUNNING, FAILURE }
    public Status NodeStatus;
    public List<Node> Children = new List<Node>();
    public int CurrentChild = 0;
    public string Name;
    public int SortOrder;

    public Node() { }

    public Node (string name)
    {
        Name = name;
    }

    public Node(string name, int sortOrder)
    {
        Name = name;
        SortOrder = sortOrder;
    }

    public void Reset()
    {
        foreach (Node node in Children)
        {
            node.Reset();
        }
        CurrentChild = 0;
    }

    public virtual Status Process()
    {
        return Children[CurrentChild].Process();
    }

    public void AddChild(Node node)
    {
        Children.Add(node);
    }
}
