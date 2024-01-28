using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CharacterController : MonoBehaviour
{
    public CharacterControllerSettingsSO settings;
    private Vector2 _velocity;
    private bool _grounded;
    private Rigidbody2D _rigidbody;
    private CapsuleCollider2D _collider;

    //these arrows are for debugging avatar direction
    [HideInInspector]
    public bool goingRight;

    public bool hideGui;

    public float maxHealth;
    private float _health;
    public Image healthBar;
    public LayerMask collisionMask;

    public float invincibilityDur;
    private float _invincibilityTimer;

#if UNITY_EDITOR
    void OnGUI() {
        if(hideGui) {
            return;
        }
        float width = 120;
        GUI.Box(new Rect(10,10,width + 20,240), "Character Info");
        float height = 35;
        GUI.Label (new Rect (15, height, width, 30), "Grounded: " + _grounded);

        height += 20;
        GUI.Label (new Rect (15, height, width, 30), "Max speed: " + settings.maxHorVel.ToString("0.0"));
        height += 20;
        settings.maxHorVel = GUI.HorizontalSlider(new Rect(15, height, width, 30), settings.maxHorVel, 0f, 20f);

        height += 20;
        GUI.Label (new Rect (15, height, width, 30), "Jump Force: " + settings.jumpForce.ToString("0"));
        height += 20;
        settings.jumpForce = GUI.HorizontalSlider(new Rect(15, height, width, 30), settings.jumpForce, 0f, 1000f);

        height += 20;
        GUI.Label (new Rect (15, height, width, 30), "Gravity: " + settings.gravity.ToString("0"));
        height += 20;
        settings.gravity = GUI.HorizontalSlider(new Rect(15, height, width, 30), settings.gravity, 0f, 2000f);
    }
#endif

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CapsuleCollider2D>();
        //this may be a bit naive, but let's assume the player always spawns on solid ground
        _grounded = true;

        goingRight = true;

        _health = maxHealth;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    void OnDestroy() {
        //tmp
#if UNITY_EDITOR
        EditorUtility.SetDirty(settings);
#endif
    }

    // Update is called once per frame
    void Update()
    {
        //horizontal motion
        Vector3 startPos = transform.position;
        float horInput = Input.GetAxis("Horizontal");
        _velocity.x = Mathf.Lerp(_velocity.x, horInput * settings.maxHorVel, 
                Time.deltaTime * settings.horLerp);
        transform.position += Vector3.right * _velocity.x * Time.deltaTime;

        //prevent going into walls
        Collider2D hit = Physics2D.OverlapCapsule(
                new Vector2(transform.position.x, transform.position.y +
                    transform.localScale.y * _collider.size.y * 0.5f),
                new Vector2(_collider.size.x * transform.localScale.x,
                    _collider.size.y * transform.localScale.y * 0.9f), 
                CapsuleDirection2D.Vertical, 0, collisionMask);
        if(hit != null) {
            transform.position = startPos;
        }

        //vis dir
        if(horInput != 0) {
            goingRight = horInput > 0;
        }

        //start jump
        if(_grounded && Input.GetButtonDown("Jump")) {
            if(Input.GetAxis("Vertical") < 0) {
                //down jump
                Debug.Log("Down jump");
                Vector2 newPos = new Vector2(transform.position.x,
                        transform.position.y 
                        + transform.localScale.y * _collider.size.y * 0.5f);
                newPos += Vector2.down * 0.1f;
                //prevent going into walls
                hit = Physics2D.OverlapCapsule(newPos,
                        new Vector2(_collider.size.x * transform.localScale.x,
                            _collider.size.y * transform.localScale.y * 0.9f), 
                        CapsuleDirection2D.Vertical, 0);
                if(hit != null) {
                    if(hit.GetComponent<PlatformEffector2D>() != null) {
                        PlatformEffector2D platformEffector = 
                            hit.GetComponent<PlatformEffector2D>();
                        //tmp
                        hit.enabled = false;
                        _grounded = false;
                        StartCoroutine(CheckForGround());
                        StartCoroutine(ReenableCollider(hit));
                    }
                }
            }
            else{
                _grounded = false;
                _rigidbody.AddForce(Vector2.up * settings.jumpForce);
                StartCoroutine(CheckForGround());
            }
        }
        //fall - having not pressed the jump button
        else if(_grounded) {
            Vector2 newPos = new Vector2(transform.position.x,
                    transform.position.y 
                    + transform.localScale.y * _collider.size.y * 0.5f);
            newPos += Vector2.down * 0.1f;
            //prevent going into walls
            hit = Physics2D.OverlapCapsule(newPos,
                    new Vector2(_collider.size.x * transform.localScale.x,
                        _collider.size.y * transform.localScale.y * 0.9f), 
                    CapsuleDirection2D.Vertical, 0);
            if(hit == null) {
                _grounded = false;
                StartCoroutine(CheckForGround());
            }
        }
        //in air
        else if(!_grounded) {
            //downward force to spice up the jump
            _rigidbody.AddForce(Vector2.down * settings.gravity * Time.deltaTime);
        }

        if(Input.GetKeyUp(KeyCode.Tab)) {
            hideGui = !hideGui;
        }
    }

    IEnumerator CheckForGround() {
        //do we need to wait some frames for velocity to update
        yield return new WaitForSeconds(0.5f);
        while(_rigidbody.velocity.y != 0) {
            yield return null;
        }
        _grounded = true;
    }

    IEnumerator ReenableCollider(Collider2D col) {
        yield return new WaitForSeconds(0.5f);
        col.enabled = true;
    }

    void OnTriggerExit2D(Collider2D other) {
        if(other.GetComponent<EnemyController>() != null) {
            Debug.Log("ouch");
            /*
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
            */
        }
    }

    void OnTriggerStay2D(Collider2D other) {
        if(other.GetComponent<EnemyController>() != null) {
            EnemyController enemy = other.GetComponent<EnemyController>();

            _invincibilityTimer += Time.deltaTime;
            if(_invincibilityTimer > enemy.damageDelay) {
                _invincibilityTimer = 0;

                _health -= enemy.damage;
                if(_health <= 0){
                    _health = 0;
                    SceneManager.LoadScene("TitleScreen");
                }
                healthBar.fillAmount = _health / maxHealth;
            }
            /*
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
            */
        }
    }
}
