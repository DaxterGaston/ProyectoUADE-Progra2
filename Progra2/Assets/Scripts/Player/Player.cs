using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private Armor armor;
    private Health health;

    private void Start()
    {
        armor = GetComponent<Armor>();
        health = GetComponent<Health>();

        health.OnDeath += GameOver;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyBullet"))
        {
            armor.TakeArmorDamage(25f);            
        }
    }

    private void GameOver()
    {
        SceneManager.LoadScene("Defeat");
    }
}
