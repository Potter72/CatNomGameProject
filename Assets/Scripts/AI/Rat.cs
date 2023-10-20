using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rat : BTAgent
{
    [SerializeField] private GameObject _plane;
    [SerializeField] private ItemList _itemList;

    private Vector3 _runAwayPosition;
    private Item _targetItem;
    private bool _hasItem = false;
    private bool _runningAwayWithItem = false;

    new void Start()
    {
        base.Start();
        Selector ratting = new Selector("Ratting");
        Leaf wander = new Leaf("Wander", Wander);

        Sequence snatchFood = new Sequence("Snatch Food");
        Leaf checkForFood = new Leaf("Check For Food", CheckForFood);
        Leaf runToFood = new Leaf("Run To Food", RunToFood);
        Leaf takeFood = new Leaf("TakeFood", TakeFood);
        Leaf runAwayWithFood = new Leaf("Run Away With Food", RunAwayWithFood);
        Leaf dropFood = new Leaf("Drop Food", DropFood);

        snatchFood.AddChild(checkForFood);
        snatchFood.AddChild(runToFood);
        snatchFood.AddChild(takeFood);
        snatchFood.AddChild(runAwayWithFood);
        snatchFood.AddChild(dropFood);

        ratting.AddChild(snatchFood);
        ratting.AddChild(wander);

        Tree.AddChild(ratting);

        Tree.PrintTree();
    }

    public Node.Status CheckForFood()
    {
        Node.Status status = Node.Status.SUCCESS;

        if(_hasItem)
        {
            return Node.Status.FAILURE;
        }

        _targetItem = _itemList.GetRandomItem();

        if (_targetItem == null)
        {
            status = Node.Status.FAILURE;
        }

        return status;
    }

    public Node.Status RunToFood()
    {
        Node.Status status = Node.Status.SUCCESS;

        if(_hasItem)
        {
            status = Node.Status.FAILURE;
        }

        else
        {
            status = GoToLocation(_targetItem.gameObject.transform.position);
        }

        return status;
    }

    public Node.Status TakeFood()
    {
        _targetItem.transform.SetParent(transform);
        _itemList.RemoveItem(_targetItem);

        return Node.Status.SUCCESS;
    }

    public Node.Status RunAwayWithFood()
    {
        Node.Status status = Node.Status.FAILURE;

        Vector2 rp = Random.insideUnitCircle * 20f;
        Vector3 randomPoint = new Vector3(rp.x, 0, rp.y);
        Vector3 destination = randomPoint + _plane.transform.position;

        NavMeshHit hit;

        if(!_runningAwayWithItem && NavMesh.SamplePosition(destination, out hit, 5f, NavMesh.AllAreas))
        {
            _runningAwayWithItem = true;
            Agent.speed = 8f;
            _runAwayPosition = hit.position;
        }

        status = GoToLocation(_runAwayPosition);
        return status;
    }

    public Node.Status DropFood()
    {
        Agent.speed = 2f;
        _targetItem.transform.parent = null;
        _itemList.AddItem(_targetItem);

        _hasItem = false;
        _runningAwayWithItem = false;
        _targetItem = null;
        _runAwayPosition = Vector3.zero;

        return Node.Status.SUCCESS;
    }

    public Node.Status Wander()
    {
        Node.Status status = Node.Status.FAILURE;

        Vector2 rp = Random.insideUnitCircle * 3f;
        Vector3 randomPoint = new Vector3(rp.x, 0, rp.y);
        Vector3 destination = randomPoint + transform.position;

        NavMeshHit hit;

        if (NavMesh.SamplePosition(destination, out hit, 5f, NavMesh.AllAreas))
        {
            status = GoToLocation(hit.position);
        }

        return status;
    }
}
