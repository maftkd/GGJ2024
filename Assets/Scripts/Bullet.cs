using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 _velocity;
    private float _life;

    public void Init(Vector3 vel, float radius, float dmg = 0) {
        _velocity = vel;
        //outjog so trail is visible
        transform.position += Vector3.back;
        transform.localScale = Vector3.one * radius * 2;
    }

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += _velocity * Time.deltaTime;

        _life += Time.deltaTime;
        if(_life > 7f) {
            Destroy(gameObject);
        }
    }
}
