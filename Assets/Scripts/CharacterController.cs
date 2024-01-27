using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        GUI.Box(new Rect(10,10,120,120), "Character Info");
        GUI.Label (new Rect (15, 35, 100, 30), "Grounded: " + _grounded);

        GUI.Label (new Rect (15, 55, 100, 30), "Max speed: " + settings.maxHorVel.ToString("0.0"));
        settings.maxHorVel = GUI.HorizontalSlider(new Rect(15, 75, 100, 30), settings.maxHorVel, 0f, 20f);

        GUI.Label (new Rect (15, 95, 100, 30), "Jump Force: " + settings.jumpForce.ToString("0"));
        settings.jumpForce = GUI.HorizontalSlider(new Rect(15, 115, 100, 30), settings.jumpForce, 0f, 1000f);
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
