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
    private Collider2D _collider;
    public GameObject bala;

    private Vector3 _test;

    // Start is called before the first frame update
    void Start()
    {
        tiempoDeDisparo = tiempoInicialDeDisparo;
        _test = transform.position;
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
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, DistanciaDeFueraDeVision);
        Gizmos.DrawLine((Vector2)transform.position, (Vector2)_test);
    }
}
