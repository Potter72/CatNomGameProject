using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
//using UnityEditor.ShaderGraph.Internal;
using Unity.VisualScripting;
using UnityEngine.XR;

//bigangryenemy by Elias Fredriksson
//summary:
//AI for the big angry enemy who rushes towards the player after spotting them and preparing for a little bit
//Base classes are made by Danny Bui
public class BigAngryEnemy : BTAgent
{
    //patrol area
    [Header("Patrol Area")]
    [SerializeField] private Transform _patrolAreaCenter;
    [SerializeField] private float _patrolRadius;
    [SerializeField] private float _detectionRange;

    //speed 
    [Header("Speed attributes")]
    [SerializeField] private float _chargeSpeed = 10f;
    [SerializeField] private float _baseSpeed = 3.5f;

    [Header("Attack Range")]
    [SerializeField] private float _chargeRange = 9;

    private Vector3 _chargePosition;
    private float _dirAngle;

    Coroutine _lookAtPlayerRoutine;

    new void Start()
    {
        base.Start();

        //make new selector
        Selector bigGuy = new("BigGuy");

        //setup behaviour nodes
        Leaf wander = new("wander", Wander);

        //setup behaviour sequence aimAndChargeTowardsPlayer
        Sequence aimAndChargeTowardsPlayer = new("aimAndChargeTowardsPlayer");
        Leaf pointTowardsPlayer = new("pointTowardsPlayer", PointTowardsPlayer);
        Leaf chargeTowardsPlayer = new("chargeTowardsPlayer", ChargeTowardsPlayer);
        Leaf recoverAfterCharge = new("Recover After Charging Player", RecoverAfterCharging);
        
        //add behaviours to sequence
        aimAndChargeTowardsPlayer.AddChild(pointTowardsPlayer);
        aimAndChargeTowardsPlayer.AddChild(chargeTowardsPlayer);
        aimAndChargeTowardsPlayer.AddChild(recoverAfterCharge);

        //add nodes to selector
        bigGuy.AddChild(aimAndChargeTowardsPlayer);
        bigGuy.AddChild(wander);

        //add selector to tree
        Tree.AddChild(bigGuy);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(_patrolAreaCenter.position, _patrolRadius);

        Gizmos.DrawLine(this.transform.position, _chargePosition);
    }

    IEnumerator LookAtPlayer()
    {
        while (true)
        {
            transform.LookAt(new Vector3(Player.transform.position.x,this.transform.position.y, Player.transform.position.z));
            //_dirAngle = Vector3.SignedAngle(Vector3.forward,Player.transform.position - transform.position, Vector3.up);
            //_chargePosition = (Player.transform.position - this.transform.position).normalized * _chargeRange;
            _chargePosition = (new Vector3(Player.transform.position.x, 0, Player.transform.position.z));
            yield return null;
        }
    }

    public Node.Status PointTowardsPlayer()
    {
        Node.Status status = Node.Status.SUCCESS;
        ChangeDelay(1.2f);
        //turn towards player quickly, then follow the player with an everso slight delay
        _lookAtPlayerRoutine = StartCoroutine(LookAtPlayer());
        //_chargePosition = (Player.transform.position - Agent.transform.position).normalized * _chargeRange;
        return status;
    }

    public Node.Status ChargeTowardsPlayer()
    {
        StopCoroutine(_lookAtPlayerRoutine);

        //_chargePosition = Quaternion.AngleAxis(_dirAngle, Vector3.up) * Vector3.forward * _chargeRange;

        Node.Status status = Node.Status.FAILURE;
        Agent.autoBraking = false;
        Agent.speed = _chargeSpeed;
        Agent.acceleration = 100;

        NavMeshHit hit;

        if (NavMesh.SamplePosition(_chargePosition, out hit, 5f, NavMesh.AllAreas))
        {
            status = GoToLocation(_chargePosition);
        }

        ChangeDelay(0.1f);
        return status;
    }

    public Node.Status RecoverAfterCharging()
    {
        Node.Status status = Node.Status.FAILURE;

        Agent.acceleration = 8;
        Agent.autoBraking = true;
        Agent.speed = _baseSpeed;

        ChangeDelay(2);

        return status;
    }

    public Node.Status Wander()
    {
        Agent.speed = _baseSpeed;
        ChangeDelay(2);
        Node.Status status = Node.Status.FAILURE;

        Vector2 rp = Random.insideUnitCircle * _patrolRadius;

        Vector3 destination = new(rp.x + _patrolAreaCenter.position.x, 0, rp.y + _patrolAreaCenter.position.z);

        Debug.Log(destination);

        NavMeshHit hit;

        if (Vector3.Distance(Player.transform.position, this.transform.position) < _detectionRange)
        {
            return Node.Status.FAILURE;
        }

        if (NavMesh.SamplePosition(destination, out hit, 5f, NavMesh.AllAreas))
        {
            status = GoToLocation(hit.position);
        }


        return status;
    }
}
