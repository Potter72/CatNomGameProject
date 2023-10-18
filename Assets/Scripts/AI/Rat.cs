using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rat : BTAgent
{
    [SerializeField] private ItemList _itemList;

    public GameObject Player;

    private GameObject _food;
    private Item _targetItem;
    private bool _chasingItem = false;
    private bool _hasItem = false;

    new void Start()
    {
        base.Start();
        Selector ratting = new Selector("Ratting");
        Leaf wander = new Leaf("Wander", Wander);

        Sequence snatchFood = new Sequence("Snatch Food");
        Leaf checkForFood = new Leaf("Check For Food", CheckForFood);
        Leaf runToFood = new Leaf("Snatch Food", RunToFood);
        Leaf runAwayWithFood = new Leaf("Run Away With Food", RunAwayWithFood);
        Leaf dropFood = new Leaf("Drop Food", DropFood);

        snatchFood.AddChild(checkForFood);
        snatchFood.AddChild(runToFood);

        ratting.AddChild(snatchFood);

        Tree.AddChild(ratting);
    }

    public Node.Status CheckForFood()
    {
        return Node.Status.SUCCESS;
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
            if(_targetItem == null)
            {
                _targetItem = _itemList.GetRandomItem();
            }

            status = GoToLocation(_targetItem.gameObject.transform.position);
        }

        return status;
    }

    public Node.Status RunAwayWithFood()
    {
        return Node.Status.SUCCESS;
    }

    public Node.Status DropFood()
    {
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
