using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class Item : MonoBehaviour
{
    [SerializeField][Range(0.0f, 1.0f)] private float _travelSpeed = 0.02f;
    
    public ItemType FoodType;
    
    private Ball _ball;
    private Plate _plate;
    
    private Transform _mouth;
    private Vector3 _randomPos;
    
    private Vector3 _initialPos;
    private bool _lastItem = false;

    private WaitForSeconds _waitForSeconds;

    public enum ItemType
    {
        Fish,
        Ham, 
        Potato,
        Corn
    }

    private void Awake()
    {
        RandomizeItemType();
        _waitForSeconds = new WaitForSeconds(0.02f);

    }

    public void AddBall(Ball ball)
    {
        _ball = ball;
    }
    
    public void RemoveItem()
    {
        _ball.RemoveItem(this);
    }
    
    public void RandomizeItemType()
    {
        FoodType = (ItemType)Random.Range(0, 4);
    }

    // Used for the ball script to indicate when the last food from the
    // ball has been sent
    public void SetLastItem()
    {
        _lastItem = true;
    }

    public void SendItem(Plate plate, Ball ball)
    {
        _plate = plate;
        _ball = ball;
        StartCoroutine(GoToPlate());
    }

    public void SendToMouth(Transform mouth, Vector3 randomPos)
    {
        _mouth = mouth;
        _randomPos = randomPos;
        _initialPos = transform.position;
        StartCoroutine(GoToMouth());
    }
    
    IEnumerator GoToPlate()
    {
        float timer = 0f;

        Vector3 pointA = transform.position;
        Vector3 pointC = _plate.transform.position;

        Vector3 ballAngle = (transform.position - _ball.transform.position).normalized;
        
        if(ballAngle.y < 0f)
        {
            ballAngle.y = 0f;
            ballAngle = ballAngle.normalized;
        }

        Vector3 pointB = Vector3.Lerp(pointA, pointC, 0.5f) + ballAngle * (Vector3.Distance(pointA, pointC) / 3f);

        while (timer < 1f)
        {
            timer += _travelSpeed;
            Vector3 AB = Vector3.Lerp(pointA, pointB, timer);
            Vector3 BC = Vector3.Lerp(pointB, pointC, timer);

            transform.position = Vector3.Lerp(AB, BC, timer);

            yield return _waitForSeconds;
        }

        _plate.AddItem(this);
        
        if(_lastItem)
        {
            _plate.FeedGod();
        }
    }

    IEnumerator GoToMouth()
    {
        float timer = 0f;
        
        while (timer < 1f)
        {
            timer += 0.02f;

            transform.position = Vector3.Lerp(_initialPos, _randomPos, timer);

            yield return _waitForSeconds;
        }

        yield return new WaitForSeconds(0.5f);

        timer = 0f;
        
        while (timer < 1f)
        {
            timer += 0.02f;

            transform.position = Vector3.Lerp(_randomPos, _mouth.position, timer);

            yield return _waitForSeconds;
        }
        
        Destroy(gameObject);
        
        yield return null;
    }
    
    // Not used
    IEnumerator GoToPlate2()
    {
        float timer = 0f;
        _initialPos = transform.position;
        Vector3 newPos = _initialPos + Vector3.up;

        while (timer < 1f)
        {
            timer += 0.02f;

            transform.position = Vector3.Lerp(_initialPos, newPos, timer);

            yield return new WaitForSeconds(0.02f);
        }

        yield return new WaitForSeconds(0.5f);

        timer = 0f;

        _initialPos = transform.position;
        newPos = _plate.transform.position;

        while (timer < 1f)
        {
            timer += 0.02f;

            transform.position = Vector3.Lerp(_initialPos, newPos, timer);

            yield return new WaitForSeconds(0.02f);
        }

        _plate.AddItem(this);
    }
}
