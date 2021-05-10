using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{

    public float velocidad;

    public SimplePlayer jugador;
    private Vector3 objetivo;
    // Start is called before the first frame update
    void Start()
    {
        objetivo = jugador.transform.position;
        Debug.Log(objetivo);
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.position = Vector3.MoveTowards(transform.position, objetivo, velocidad*Time.deltaTime);
        if(transform.position.x == objetivo.x && transform.position.y == objetivo.y)
        {
            DestruirObjeto();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            DestruirObjeto();
        }
    }

    public void DestruirObjeto()
    {
        Destroy(gameObject);
    }
}
