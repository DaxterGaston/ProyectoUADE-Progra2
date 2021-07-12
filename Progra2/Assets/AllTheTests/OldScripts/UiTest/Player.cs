using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Health health;
    private Armor armor;
    private Ammo ammo;
    private Rigidbody2D rb;

    private void Start()
    {
        health = GetComponent<Health>();
        armor = GetComponent<Armor>();
        ammo = GetComponent<Ammo>();
        rb = GetComponent<Rigidbody2D>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyBullet"))
        {
            armor.TakeArmorDamage(25f);            
        }
        if (other.CompareTag("PickUp"))
        {
            health.Heal(10f);            
        }
        if (other.CompareTag("Bullet"))
        {
            armor.ArmorUp(10f);
        }
    }
    
    private void Update()
    {
        // Debug
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ammo.AmmoUse(1);
        }
    }
}
