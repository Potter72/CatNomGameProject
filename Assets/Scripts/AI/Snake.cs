using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : BTAgent
{
    [SerializeField] private GameObject _plane;

    private List<Item> _itemList;

    private bool _hasItem = false;
    private bool _runningAwayWithItem = false;

    new void Start()
    {
        base.Start();
        _itemList = Player.GetComponent<Ball>().GetItemList();

        Selector doSnake = new Selector("Do Snake");
        Sequence stealFromPlayer = new Sequence("Steal From Player");
        Leaf checkRange = new Leaf("Check Range", CheckRange);
        Leaf chasePlayer = new Leaf("Chase Player", ChasePlayer);
        Leaf snatchFood = new Leaf("Snatch Food", SnatchFood);
        Leaf runAway = new Leaf("Run Away", RunAway);
        Leaf dropFood = new Leaf("Drop Food", DropFood);

        Sequence wander = new Sequence("Wander");
        Leaf decideDestination = new Leaf("Decide Destination", DecideDestination);
        Leaf moveAround = new Leaf("Move Around", MoveAround);

        stealFromPlayer.AddChild(checkRange);
        stealFromPlayer.AddChild(chasePlayer);
        stealFromPlayer.AddChild(snatchFood);
        stealFromPlayer.AddChild(runAway);
        stealFromPlayer.AddChild(dropFood);

        wander.AddChild(decideDestination);
        wander.AddChild(moveAround);

        doSnake.AddChild(stealFromPlayer);
        doSnake.AddChild(wander);

        Tree.AddChild(doSnake);

        Tree.PrintTree();
    }
    
    public Node.Status CheckRange()
    {
        return Node.Status.SUCCESS;
    }

    public Node.Status ChasePlayer()
    {
        return Node.Status.SUCCESS;
    }

    public Node.Status SnatchFood()
    {
        return Node.Status.SUCCESS;
    }

    public Node.Status RunAway()
    {
        return Node.Status.SUCCESS;
    }

    public Node.Status DropFood()
    {
        return Node.Status.SUCCESS;
    }

    public Node.Status DecideDestination()
    {
        return Node.Status.SUCCESS;
    }

    public Node.Status MoveAround()
    {
        return Node.Status.SUCCESS;
    }
}
