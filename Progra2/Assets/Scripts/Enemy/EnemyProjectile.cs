using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    private Vector3 direction;

    private void Start()
    {
        Invoke("DestroyObject", 3f);
    }

    private void Update()
    {
        transform.position += direction * (speed * Time.deltaTime);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    public void SetMoveDirection(Vector3 dir)
    {
        direction = dir;
    }
}

