using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    #region Tracking

    [SerializeField] private Transform targetObj;
    [SerializeField] private float speed;
    [SerializeField] private float range;
    [SerializeField] private float angle;
    [SerializeField] private LayerMask mask;
    private bool isInShootingRange;
    public bool DijkstraMove => (!IsInSight(targetObj));

    #endregion

    #region Shooting

    private float currentShootingCooldown;
    [SerializeField] private float maxShootingCooldown;
    [SerializeField] private EnemyProjectile bulletPrefab;
    [SerializeField] private Transform firePoint;

    #endregion

    [SerializeField] private int currLifePoints;
    [SerializeField] private int maxLifePoints;
    
    private void Start()
    {
        targetObj = FindObjectOfType<SimplePlayer>().transform;
        currLifePoints = maxLifePoints;
        currentShootingCooldown = maxShootingCooldown;
    }

    private bool IsInSight(Transform target)
    {
        var diff = (target.position - transform.position);
        var distance = diff.magnitude;
        if (distance > range) return false;
        var angleToTarget = Vector2.Angle(transform.right, diff.normalized);
        if (angleToTarget > angle / 2) return false;
        if (Physics.Raycast(transform.position, diff.normalized, distance, mask))
        {
            return false;
        }
        isInShootingRange = false;
        if (distance <= range / 2)
        {
            isInShootingRange = true;
        }
        return true;
    }

    private void Update()
    {
        if (IsInSight(targetObj))
        {
            // Dispara al objetivo
            if (isInShootingRange)
            {
                currentShootingCooldown -= Time.deltaTime;
                if (currentShootingCooldown <= 0)
                {
                    var go = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                    go.SetMoveDirection((targetObj.position - transform.position).normalized);
                    currentShootingCooldown = maxShootingCooldown;
                }
            }
            if (!isInShootingRange)
            {
                // Persigue al objetivo
                transform.position = Vector3.MoveTowards(transform.position, targetObj.transform.position, speed * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            if (currLifePoints > 0)
            {
                currLifePoints--;
            }

            if (currLifePoints <= 0)
            {
                DestroyEnemy();
            }
        }
    }

    private void DestroyEnemy()
    {
        SpawnController.killedAmount++;
        print(SpawnController.killedAmount);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector2.right * range);
        Gizmos.DrawWireSphere(transform.position, range);
        Gizmos.DrawWireSphere(transform.position, range / 2);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0,0, angle / 2) * transform.right * range);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0,0, -angle / 2) * transform.right * range);
    }
}
