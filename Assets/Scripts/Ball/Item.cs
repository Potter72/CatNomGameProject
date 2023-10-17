using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;

public class Item : MonoBehaviour
{
    [SerializeField][Range(0.0f, 1.0f)] private float _travelSpeed = 0.02f;

    private Ball _ball;
    private Plate _plate;
    private Vector3 _initialPos;
    private bool _lastItem = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetLastItem()
    {
        _lastItem = true;
    }

    public void SendItem(Plate plate, Ball ball)
    {
        _plate = plate;
        _ball = ball;
        StartCoroutine(GoToPlate2());
    }

    IEnumerator GoToPlate1()
    {
        float timer = 0f;
        _initialPos = transform.position;
        Vector3 newPos = _initialPos + Vector3.up;

        while(timer < 1f)
        {
            timer += 0.02f;

            transform.position = Vector3.Lerp(_initialPos, newPos, timer);

            yield return new WaitForSeconds(0.02f);
        }

        yield return new WaitForSeconds(0.5f);

        timer = 0f;

        _initialPos = transform.position;
        newPos = _plate.transform.position;

        while(timer < 1f)
        {
            timer += 0.02f;

            transform.position = Vector3.Lerp(_initialPos, newPos, timer);

            yield return new WaitForSeconds(0.02f);
        }

        _plate.AddItem(gameObject);
    }

    IEnumerator GoToPlate2()
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

        Vector3 pointB = Vector3.Lerp(pointA, pointC, 0.5f) + ballAngle * 3;

        while (timer < 1f)
        {
            timer += _travelSpeed;
            Vector3 AB = Vector3.Lerp(pointA, pointB, timer);
            Vector3 BC = Vector3.Lerp(pointB, pointC, timer);

            transform.position = Vector3.Lerp(AB, BC, timer);

            yield return new WaitForSeconds(0.02f);
        }

        _plate.AddItem(gameObject);
        
        if(_lastItem)
        {
            _plate.FeedGod();
        }
    }
}
