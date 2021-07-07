using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region SetUp

    #region Serializables

    [SerializeField] 
    private float speed = 5f;
    [SerializeField]
    private int _maxBullets = 6;
    [SerializeField] 
    private Camera _camera;
    [SerializeField] 
    private ViewCone _viewCone;
    [SerializeField] 
    private Image _hpImage;
    [SerializeField]
    private Rigidbody2D _rigidBody;
    [SerializeField]
    private Transform _firePoint;
    [SerializeField]
    private GameObject _bullet;

    #endregion

    private int _currentBullets;
    private Animator _animator;
    private int _hp;
    private Vector2 _mouse;
    private Vector3 _scale;
    private Vector3 _initialLocalScale;
    private float _h, _v;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _hpImage = GameObject.Find("HPBar").GetComponent<Image>();
        _hp = 5;
        _hpImage.fillAmount = _hp / 5;
    }

    private void Start()
    {
        _currentBullets = _maxBullets;
        _scale = transform.localScale;
        _initialLocalScale = transform.localScale;
        Debug.Log(transform.localScale);
    }
    
    #endregion

    private void UpdateHpBars()
    {
        _hpImage.fillAmount = _hp / 5;
    }

    private void Update()
    {
        GetInputs();
        UpdateAnimations();
        Debug.Log(transform.localScale);
    }

    private void UpdateAnimations()
    {
        _animator.SetBool("Walking", _rigidBody.velocity.magnitude > 0.1f);
        _animator.SetFloat("X", _h);

        if (_mouse.x > transform.position.x)
            _scale.x = -_initialLocalScale.x;
        else
            _scale.x = _initialLocalScale.x;
        transform.localScale = _scale;
    }

    private void FixedUpdate() 
    {
        print(($"h: {1} v: {2} speed: {3}", _h, _v, speed));
        _rigidBody.velocity = new Vector2(_h * speed, _v * speed);
        print("rb: " + _rigidBody.velocity);
    }

    private void GetInputs()
    {
        _mouse = _camera.ScreenToWorldPoint(Input.mousePosition);
        _h = Input.GetAxisRaw("Horizontal");
        _v = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Fire1"))
        {
            if (_currentBullets <= 0)
                Invoke("Reload", 2);
            else
            {
                _currentBullets--;
                Shoot();
            }
        }
    }

    public void Shoot()
    {
        var shootDirection = (_mouse - (Vector2)_firePoint.position).normalized;
        var go = Instantiate(_bullet, _firePoint.position, Quaternion.identity);
        go.GetComponent<Bullet>().Direction = shootDirection;
    }

    private void Reload() { _currentBullets = _maxBullets; }

    public void TakeDamage(int amount)
    {
        _hp -= amount;
        UpdateHpBars();

        if (_hp <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        throw new NotImplementedException();
    }

    public void RestoreHealth(int amount)
    {
        if (_hp != 5)
            _hp += amount;
        if (_hp > 5)
            _hp = 5;

        UpdateHpBars();
    }
}