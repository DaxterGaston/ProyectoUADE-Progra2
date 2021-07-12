using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField]
    private int _id;
    [SerializeField]
    private int _target;

    private Vector3 _targetTeleportPosition;

    private AudioSource _audio;

    public int Target { get {return _target; } private set {; } }
    public int Id { get {return _id; } private set {; } }


    // Start is called before the first frame update
    void Start()
    {
        _targetTeleportPosition = TeleportManager.Instance.GetTargetPosition(_target);
        _targetTeleportPosition.z = -10;
        _audio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        { 
            collision.transform.parent.position = (_targetTeleportPosition + new Vector3(1, 1, 0));
            _audio.Play();
        }
        if (collision.CompareTag("Enemy"))
            collision.GetComponent<EnemyController>().Teleport(_targetTeleportPosition);
    }
}
