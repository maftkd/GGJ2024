using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public Image healthBar;
    public float maxHealth;
    private float _health;

    //nearby physics checks
    private Vector2 _groundLeftPos;
    private Collider2D _groundLeft;
    private Vector2 _airLeftPos;
    private Collider2D _airLeft;
    private Vector2 _groundRightPos;
    private Collider2D _groundRight;
    private Vector2 _airRightPos;
    private Collider2D _airRight;

    private bool _goingLeft = true;
    [Range(0.01f, 2f)]
    public float moveSpeed;

    public AudioClip[] popSounds;

    public float damage;
    public float damageDelay;

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
        if(_goingLeft) {
            _groundLeftPos = new Vector2(
                        transform.position.x - 1f,
                        transform.position.y - 1);
            _groundLeft = Physics2D.OverlapPoint(_groundLeftPos);
            _airLeftPos = new Vector2(
                        transform.position.x - 1f,
                        transform.position.y);
            _airLeft = Physics2D.OverlapPoint(_airLeftPos);
            if(_groundLeft != null && _airLeft == null) {
                transform.position += Vector3.left * Time.deltaTime * moveSpeed;
            }
            else{
                _goingLeft = !_goingLeft;
            }
        }
        else{
            _groundRightPos = new Vector2(
                        transform.position.x + 1f,
                        transform.position.y - 1);
            _groundRight = Physics2D.OverlapPoint(_groundRightPos);
            _airRightPos = new Vector2(
                        transform.position.x + 1f,
                        transform.position.y);
            _airRight = Physics2D.OverlapPoint(_airRightPos);
            if(_groundRight != null && _airRight == null) {
                transform.position += Vector3.right * Time.deltaTime * moveSpeed;
            }
            else{
                _goingLeft = !_goingLeft;
            }
        }
    }

    void OnDrawGizmos() {
        if(_goingLeft) {
            if(_groundLeft != null) {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(new Vector3(_groundLeftPos.x, _groundLeftPos.y, 0), 0.15f);
            }
            if(_airLeft != null) {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(new Vector3(_airLeftPos.x, _airLeftPos.y, 0), 0.15f);
            }
        }
        else{
            if(_groundRight != null) {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(new Vector3(_groundRightPos.x, _groundRightPos.y, 0), 0.15f);
            }
            if(_airRight != null) {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(new Vector3(_airRightPos.x, _airRightPos.y, 0), 0.15f);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.GetComponent<Bullet>() != null) {
            Bullet bullet = other.GetComponent<Bullet>();
            _health -= bullet.settings.damage;
            if(_health <= 0){
                _health = 0;
                Destroy(gameObject);
            }
            else{
                int index = Random.Range(0, popSounds.Length);
                AudioController.Instance.PlayOneShot(popSounds[index], transform.position);
            }
            healthBar.fillAmount = _health / maxHealth;
        }
    }
}
