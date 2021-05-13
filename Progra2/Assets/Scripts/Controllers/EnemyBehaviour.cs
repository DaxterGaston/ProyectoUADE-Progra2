using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField]
    private LayerMask _playerLayer;
    [SerializeField]
    private LayerMask _viewObstacle;

    public float velocidad;
    public float DistanciaDeParado;
    public float DistanciaDeFueraDeVision;

    public float tiempoDeDisparo;
    public float tiempoInicialDeDisparo;
    public GameObject bala;

    private Collider2D _collider;
    private Animator _animator;
    private Vector3 _scale;
    private bool _walking;
    //True : derecha - False : izquierda
    private bool _lookingRight;
    // Start is called before the first frame update
    void Start()
    {
        tiempoDeDisparo = tiempoInicialDeDisparo;
        _animator = GetComponent<Animator>();
        _scale = new Vector3(1, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        _collider = Physics2D.OverlapCircle(transform.position, DistanciaDeFueraDeVision, _playerLayer);
        if (_collider)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, _collider.transform.position, Vector2.Distance(transform.position, _collider.transform.position), _viewObstacle);
            Debug.DrawLine((Vector2)transform.position, (Vector2)_collider.transform.position, Color.magenta);

            if (!hit)
            {
                //TODO: Hacer algo, aca ya esta el player en rango de vision para el enemigo, a una distancia que puede verlo y no hay ninguna pared en el medio.
            }

            //if (Vector2.Distance(transform.position, _collider.transform.position) <= DistanciaDeFueraDeVision) { }
            //{
            //    if (Vector2.Distance(transform.position, _collider.transform.position) > DistanciaDeParado)
            //    {

            //        if (hitInfo.collider.CompareTag("Player"))
            //            transform.position = Vector2.MoveTowards(transform.position, _collider.transform.position, velocidad * Time.deltaTime);

            //    }
            //    else if (Vector2.Distance(transform.position, _collider.transform.position) <= DistanciaDeParado)
            //    {

            //        if (hitInfo.collider.CompareTag("Player"))
            //            transform.position = transform.position;

            //    }
            //}
            UpdateAnimations();
        }
    }

    private void UpdateAnimations()
    {
        _animator.SetBool("Walking", _walking);
        if (!_lookingRight) _scale.x = -1;
        else _scale.x = 1;
        transform.localScale = _scale;
    }
}
