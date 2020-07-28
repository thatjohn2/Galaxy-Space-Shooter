using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8f;

    public string _direction;

    public GameObject _shooter;

    // Start is called before the first frame update
    void Start()
    {
        if (_shooter == null)
        {
            Debug.LogError("The Laser's Shooter is NULL.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_direction == "UP")
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime, Space.World);
        }
        else if (_direction == "DOWN")
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime, Space.World);
        }

        float laserX = transform.position.x;
        float laserY = transform.position.y;
        float laserZ = transform.position.z;

        if (transform.position.y >= 8)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }

        if (transform.position.y <= -8)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }   
}