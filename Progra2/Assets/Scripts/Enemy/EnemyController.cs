using System;
using System.Diagnostics;
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

    #region Stats

    [SerializeField] private int currLifePoints;
    [SerializeField] private int maxLifePoints;

    #endregion

    #region Teleportation

    public bool CanTeleport { get; private set; }
    [SerializeField] private int _milisecondsToTeleport = 2;
    private Stopwatch _sw;
    private TimeSpan _ts;

    #endregion

    #region Animation

    private Animator _animator;
    private Vector3 _scale;
    private bool _walking;
    private bool _lookingRight;

    #endregion
    
    private void Start()
    {
        targetObj = FindObjectOfType<SimplePlayer>().transform;
        currLifePoints = maxLifePoints;
        currentShootingCooldown = maxShootingCooldown;
        _sw = new Stopwatch();
        _ts = new TimeSpan(0, 0, 0, _milisecondsToTeleport);
        _animator = GetComponent<Animator>();
        _scale = transform.localScale;
    }

    private bool IsInSight(Transform target)
    {
        var diff = (target.position - transform.position);
        if (Vector3.Dot(diff, transform.right) > 0)
        {
            _lookingRight = true;
        }
        if (Vector3.Dot(diff, transform.right) < 0)
        {
            _lookingRight = false;
        }
        var distance = diff.magnitude;
        if (distance > range) return false;
        var angleToTarget = Vector2.Angle(transform.right, diff.normalized);
        if (angleToTarget > angle / 2) return false;
        if (Physics2D.Raycast(transform.position,diff.normalized,distance,mask))
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
                _walking = false;
                currentShootingCooldown -= Time.deltaTime;
                if (currentShootingCooldown <= 0)
                {
                    var go = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                    go.SetMoveDirection((targetObj.position - transform.position).normalized);
                    currentShootingCooldown = maxShootingCooldown;
                }
            }
            //if (!isInShootingRange)
            //{
            //    _walking = true;
            //    // Persigue al objetivo
            //    transform.position = Vector3.MoveTowards(transform.position, targetObj.transform.position, speed * Time.deltaTime);
            //}
        }
        UpdateAnimations();
        if (!CanTeleport)
            if (_sw.Elapsed >= _ts) CanTeleport = true;
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
        SpawnController.Instance.KillUpdate();
        Destroy(gameObject);
    }
    
    public void Teleport(Vector3 position)
    {
        if (CanTeleport)
        {
            transform.position = position;
            _sw.Start();
            CanTeleport = false;
        }
    }
    
    private void UpdateAnimations()
    {
        _animator.SetBool("Walking", _walking);
        if (_lookingRight)
        {
            _scale.x = Mathf.Abs(_scale.x);
        }

        if (!_lookingRight)
        {
            _scale.x = Mathf.Abs(_scale.x) * -1;
        }
        transform.localScale = _scale;
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
