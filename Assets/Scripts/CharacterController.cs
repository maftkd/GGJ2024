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
        _velocity.x = Mathf.Lerp(_velocity.x, horInput * settings.maxHorVel, 
                Time.deltaTime * settings.horLerp);
        transform.position += Vector3.right * _velocity.x * Time.deltaTime;
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
        }
        else if(!_grounded) {
            //downward force to spice up the jump
            _rigidbody.AddForce(Vector2.down * settings.gravity * Time.deltaTime);
        }
    }

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
}
