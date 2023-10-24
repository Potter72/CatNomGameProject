using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Snake : BTAgent
{
    [SerializeField] private GameObject _plane;
    [SerializeField] private float _snatchRange = 3f; 
    [SerializeField] private float _detectionRange = 20f;

    private List<Item> _itemList;

    private Item _heldItem;

    private bool _hasItem = false;
    private bool _runningAwayWithItem = false;
    private Vector3 _runDestination;

    new void Start()
    {
        base.Start();
        _itemList = Player.GetComponent<Ball>().GetItemList();

        Selector doSnake = new Selector("Do Snake");
        Sequence stealFromPlayer = new Sequence("Steal From Player");
        Leaf checkRange = new Leaf("Check Range", CheckRange);
        Leaf chasePlayer = new Leaf("Chase Player", ChasePlayer);
        Leaf snatchFood = new Leaf("Snatch Food", SnatchFood);
        Leaf setRunDestination = new Leaf("Set Run Destination", SetRunDestination);
        Leaf runAway = new Leaf("Run Away", RunAway);
        Leaf dropFood = new Leaf("Drop Food", DropFood);

        Sequence wander = new Sequence("Wander");
        Leaf setWanderDestination = new Leaf("Wander", SetWanderDestination);
        Leaf moveToWanderDestination = new Leaf("Move To Wander Destination", MoveToWanderDestination);

        stealFromPlayer.AddChild(checkRange);
        stealFromPlayer.AddChild(chasePlayer);
        stealFromPlayer.AddChild(snatchFood);
        stealFromPlayer.AddChild(runAway);
        stealFromPlayer.AddChild(dropFood);

        wander.AddChild(setWanderDestination);
        wander.AddChild(moveToWanderDestination);

        doSnake.AddChild(stealFromPlayer);
        doSnake.AddChild(wander);

        Tree.AddChild(doSnake);

        Tree.PrintTree();
    }
    
    public Node.Status CheckRange()
    {
        if (Vector3.Distance(Player.transform.position, transform.position) < _detectionRange && _itemList.Count > 0)
        {
            ChangeDelay(0.5f);
            Agent.destination = Player.transform.position;
            return Node.Status.SUCCESS;
        }
        
        return Node.Status.FAILURE;
    }

    public Node.Status ChasePlayer()
    {
        if(Vector3.Distance(Player.transform.position, transform.position) < _snatchRange)
        {
            ChangeDelay(0f);
            return Node.Status.SUCCESS;
        }

        return Node.Status.RUNNING;
    }

    public Node.Status SnatchFood()
    {
        _heldItem = _itemList[Random.Range(0, _itemList.Count)];
        _itemList.Remove(_heldItem);
        _heldItem.transform.position = transform.position + transform.forward;
        _heldItem.transform.SetParent(this.transform, true);


        return Node.Status.SUCCESS;
    }

    public Node.Status SetRunDestination()
    {
        Vector2 rp = Random.insideUnitCircle * 20f;
        Vector3 randomPoint = new Vector3(rp.x, 0, rp.y);
        _runDestination = randomPoint + _plane.transform.position;

        NavMeshHit hit;

        if (NavMesh.SamplePosition(_runDestination, out hit, 20f, NavMesh.AllAreas))
        {
            _runDestination = hit.position;
        }

        return Node.Status.SUCCESS;
    }

    public Node.Status RunAway()
    {
        Node.Status status = Node.Status.FAILURE;

        status = GoToLocation(_runDestination);

        return status;
    }

    public Node.Status DropFood()
    {
        _heldItem.transform.parent = null;
        
        GameManager.Instance.GetItemList().AddItem(_heldItem);

        _heldItem = null;

        return Node.Status.SUCCESS;
    }
}
