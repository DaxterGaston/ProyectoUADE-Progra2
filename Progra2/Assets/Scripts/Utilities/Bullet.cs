using System;
using System.Diagnostics;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private int _lifeInSeconds;


    private TimeSpan _ts;
    private Stopwatch _sw;
    public Vector3 Direction;
    
    // Start is called before the first frame update
    void Start()
    {
        _ts = new TimeSpan(0, 0, _lifeInSeconds);
        _sw = new Stopwatch();
        _sw.Start();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Direction * _speed * Time.deltaTime;
        if (_sw.Elapsed >= _ts)
        {
            Destroy(gameObject);
        }
    }
}
