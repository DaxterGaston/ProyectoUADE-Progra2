using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    private Ammo ammo;
    [SerializeField] private Camera cam;
    private Vector2 mousePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;

    private void Awake()
    {
        ammo = GetComponent<Ammo>();
    }

    private void Update()
    {
        mousePoint = cam.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetButtonDown("Fire1"))
        {
            if (!ammo.IsOutOfAmmo)
            {
                ammo.AmmoUse(1);
                Shoot();
            }
        }
    }
    
    private void Shoot()
    {
        var shootDirection = (mousePoint - (Vector2)firePoint.position).normalized;
        Vector3 diff = (Vector3)mousePoint - transform.position;
        diff.Normalize();
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        
        var go = Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0f, 0f, rot_z + 180));
        var bullet = go.GetComponent<Bullet>();
        bullet.Direction = shootDirection;
    }
}
