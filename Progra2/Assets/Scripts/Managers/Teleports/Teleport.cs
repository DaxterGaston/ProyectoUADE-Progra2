using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField]
    private int _id;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("Es collider");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("Es Trigger");
    }
}
