using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public Image healthBar;
    public float maxHealth;
    private float _health;

    void Awake() {
        _health = maxHealth;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.GetComponent<Bullet>() != null) {
            Bullet bullet = other.GetComponent<Bullet>();
            _health -= bullet.settings.damage;
            if(_health <= 0){
                _health = 0;
                Destroy(gameObject);
            }
            healthBar.fillAmount = _health / maxHealth;
        }
    }
}
