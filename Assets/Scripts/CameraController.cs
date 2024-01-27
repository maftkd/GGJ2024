using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform _player;
    public CharacterController charCon;
    public CameraControllerSettingsSO settings;

    void OnGUI() {
        GUI.Box(new Rect(140,10,120,120), "Camera Info");
        GUI.Label (new Rect (145, 35, 100, 30), "right: " + charCon.goingRight);

        GUI.Label (new Rect (145, 55, 100, 30), "Look ahead: " + settings.lookAhead.ToString("0.0"));
        settings.lookAhead = GUI.HorizontalSlider(new Rect(145, 75, 100, 30), settings.lookAhead, 0f, 5f);

        GUI.Label (new Rect (145, 95, 100, 30), "Speed: " + settings.speed.ToString("0.0"));
        settings.speed = GUI.HorizontalSlider(new Rect(145, 115, 100, 30), settings.speed, 0f, 20f);
    }

    void Awake() {
        _player = charCon.transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //lets try something naive at first
        Vector3 pos = transform.position;
        pos.x = _player.position.x;
        if(charCon.goingRight) {
            pos.x += settings.lookAhead;
        }
        else{
            pos.x -= settings.lookAhead;
        }
        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * settings.speed);
    }
}
