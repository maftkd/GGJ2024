using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//tmp
using UnityEditor;

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
    public GameObject leftArrow;
    public GameObject rightArrow;

    void OnGUI() {
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

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CapsuleCollider2D>();
        //this may be a bit naive, but let's assume the player always spawns on solid ground
        _grounded = true;

        leftArrow.SetActive(false);
        goingRight = true;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    void OnDestroy() {
        //tmp
        EditorUtility.SetDirty(settings);
    }

    // Update is called once per frame
    void Update()
    {
        //horizontal motion
        float horInput = Input.GetAxis("Horizontal");
        /*
        _velocity.x = Mathf.Lerp(_velocity.x, horInput * settings.maxHorVel, 
                Time.deltaTime * settings.horLerp);
        transform.position += Vector3.right * _velocity.x * Time.deltaTime;
        */
        Vector2 vel = _rigidbody.velocity;
        vel.x = horInput * settings.maxHorVel;

        //test position before applying otherwise we get stuck in wall
        Vector2 newPos = new Vector2(transform.position.x, transform.position.y)
            + vel * Time.deltaTime * 1.5f + 
            Vector2.up * transform.localScale.y * _collider.size.y * 0.5f;

        Collider2D hit = Physics2D.OverlapCapsule(newPos, 
                new Vector2(_collider.size.x * transform.localScale.x,
                    _collider.size.y * transform.localScale.y * 0.9f), 
                CapsuleDirection2D.Vertical, 0);//, _collisionLayerMask);

        if(hit != null) {
            vel.x = 0;
        }
        _rigidbody.velocity = vel;

        //vis dir
        if(horInput != 0) {
            goingRight = horInput > 0;
            rightArrow.SetActive(goingRight);
            leftArrow.SetActive(!goingRight);
        }

        //jump
        if(_grounded && Input.GetButtonDown("Jump")) {
            _grounded = false;
            _rigidbody.AddForce(Vector2.up * settings.jumpForce);
            StartCoroutine(CheckForGround());
        }
        else if(!_grounded) {
            //downward force to spice up the jump
            _rigidbody.AddForce(Vector2.down * settings.gravity * Time.deltaTime);
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

    /*
    void OnCollisionEnter2D(Collision2D other){
        if(_grounded == true) {
            //we only use this callback to check if the player is grounded for now...
            return;
        }


        //unity recommends against using this because of Garbage generation
        //but we can worry about that later
        foreach(ContactPoint2D contactPoint in other.contacts) {
            if(Vector2.Dot(contactPoint.normal, Vector2.up) > 0) {
                _grounded = true;
                return;
            }
        }
    }
    */
}
