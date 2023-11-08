using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Snake : BTAgent
{
    [SerializeField] private Color _gizmoColor;
    [SerializeField] private GameObject _foodPlacementArea;
    [SerializeField] private Transform _mouth;
    [SerializeField] private float _snatchRange = 3f; 
    [SerializeField] private float _detectionRange = 20f;



    private List<Item> _itemList;

    private Item _heldItem;

    private Vector3 _runDestination;

    new void Start()
    {
        base.Start();
        _itemList = Player.GetComponent<Ball>().GetItemList();

        Selector doSnake = new Selector("Do Snake");
        Sequence stealFromPlayer = new Sequence("Steal From Player");
        Leaf checkRange = new Leaf("Check Range", CheckRange);
        Leaf setWalking = new Leaf("SetWalking", SetWalking);
        Leaf chasePlayer = new Leaf("Chase Player", ChasePlayer);
        Leaf snatchFood = new Leaf("Snatch Food", SnatchFood);
        Leaf setRunDestination = new Leaf("Set Run Destination", SetRunDestination);
        Leaf runAway = new Leaf("Run Away", RunAway);
        Leaf dropFood = new Leaf("Drop Food", DropFood);

        BehaviorTree seePlayer = new BehaviorTree("SeePlayer");
        Inverter invertRangeCheck = new Inverter("InvertRangeCheck");
        invertRangeCheck.AddChild(checkRange);

        Selector wanderWithCheck = new Selector("WanderWithCheck");
        DepSequence wander = new DepSequence("Wander", seePlayer, Agent);
        Leaf setWanderDestination = new Leaf("Wander", SetWanderDestination);
        Leaf moveToWanderDestination = new Leaf("Move To Wander Destination", MoveToWanderDestination);
        Leaf stopWalk = new Leaf("Stop Walk", StopWalk);

        stealFromPlayer.AddChild(checkRange);
        stealFromPlayer.AddChild(setWalking);
        stealFromPlayer.AddChild(chasePlayer);
        stealFromPlayer.AddChild(snatchFood);
        stealFromPlayer.AddChild(setRunDestination);
        stealFromPlayer.AddChild(runAway);
        stealFromPlayer.AddChild(dropFood);

        wander.AddChild(setWanderDestination);
        wander.AddChild(setWalking);
        wander.AddChild(moveToWanderDestination);
        wander.AddChild(stopWalk);

        doSnake.AddChild(stealFromPlayer);
        doSnake.AddChild(wander);

        Tree.AddChild(doSnake);

        Tree.PrintTree();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmoColor;
        Gizmos.DrawSphere(transform.position, _detectionRange);
    }

    public Node.Status CheckRange()
    {
        if (Vector3.Distance(Player.transform.position, transform.position) < _detectionRange && _itemList.Count > 0)
        {
            ChangeDelay(0.2f);
            return Node.Status.SUCCESS;
        }
        
        return Node.Status.FAILURE;
    }

    public Node.Status SetWalking()
    {
        _animator.SetBool("Walking", true);
        ChangeDelay(0f);
        return Node.Status.SUCCESS;
    }

    public Node.Status StopWalk()
    {
        _animator.SetBool("Walking", false);
        ChangeDelay(0.3f);
        return Node.Status.SUCCESS;
    }
    
    public Node.Status ChasePlayer()
    {
        ChangeDelay(0.2f);
        Agent.destination = Player.transform.position;
        Agent.speed = 8f;

        if (Vector3.Distance(Player.transform.position, transform.position) < _snatchRange)
        {
            ChangeDelay(0.2f);
            _animator.SetBool("Walking", false);
            return Node.Status.SUCCESS;
        }

        return Node.Status.RUNNING;
    }

    public Node.Status SnatchFood()
    {
        _heldItem = _itemList[Random.Range(0, _itemList.Count)];
        _itemList.Remove(_heldItem);
        _heldItem.transform.position = _mouth.position;
        GameObject oldParent = _heldItem.transform.parent.gameObject;
        _heldItem.transform.SetParent(_mouth, true);
        Destroy(oldParent);

        return Node.Status.SUCCESS;
    }

    public Node.Status SetRunDestination()
    {
        Vector2 rp = Random.insideUnitCircle * 20f;
        Vector3 randomPoint = new Vector3(rp.x, 0, rp.y);
        _runDestination = randomPoint + _foodPlacementArea.transform.position;

        NavMeshHit hit;

        if (NavMesh.SamplePosition(_runDestination, out hit, 75f, NavMesh.AllAreas))
        {
            _runDestination = hit.position;
            Agent.speed = 15f;
            ChangeDelay(0.1f);
            _animator.SetBool("Walking", false);
            _animator.SetBool("Running", true);
            return Node.Status.SUCCESS;
        }

        return Node.Status.FAILURE;
    }

    public Node.Status RunAway()
    {
        Node.Status status = Node.Status.FAILURE;

        status = GoToLocation(_runDestination);

        if (status == Node.Status.SUCCESS)
        {        
            _animator.SetBool("Running", false);
        }
        
        return status;
    }
    
    public Node.Status DropFood()
    {
        GameManager.Instance.GetItemList().AddItem(_heldItem);
        _heldItem.EnableItem();
        _heldItem.RemoveItem();
        _heldItem.transform.parent = null;
        _heldItem = null;

        Agent.speed = 3.5f;
        
        return Node.Status.SUCCESS;
    }
}
