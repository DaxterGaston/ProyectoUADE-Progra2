using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    private Health health;

    private void Start()
    {
        health = GetComponent<Health>();

        // Subscribe to death actions
        health.OnDeath += OnDie;
    }

    private void OnDie()
    {
        Destroy(gameObject);
    }
}
