using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CameraController : MonoBehaviour
{
    private Transform _player;
    public CharacterController charCon;
    public CameraControllerSettingsSO settings;
    private Camera _cam;

    public bool hideGui;

#if UNITY_EDITOR
    void OnGUI() {
        if(hideGui) {
            return;
        }
        float x = 160;
        float width = 120;
        GUI.Box(new Rect(x - 10,10,width + 20,220), "Camera Info");
        float height = 35;
        GUI.Label (new Rect (x, height, width, 30), "right: " + charCon.goingRight);

        height += 20;
        GUI.Label (new Rect (x, height, width, 30), "Look ahead: " + settings.lookAhead.ToString("0.0"));
        height += 20;
        settings.lookAhead = GUI.HorizontalSlider(new Rect(x, height, width, 30), settings.lookAhead, 0f, 5f);

        height += 20;
        GUI.Label (new Rect (x, height, width, 30), "Speed x: " + settings.speed.ToString("0.0"));
        height += 20;
        settings.speed = GUI.HorizontalSlider(new Rect(x, height, width, 30), settings.speed, 0f, 20f);

        height += 20;
        GUI.Label (new Rect (x, height, width, 30), "Speed y: " + settings.speedVert.ToString("0.0"));
        height += 20;
        settings.speedVert = GUI.HorizontalSlider(new Rect(x, height, width, 30), settings.speedVert, 0f, 20f);

        height += 20;
        GUI.Label (new Rect (x, height, width, 30), "Size: " + settings.size.ToString("0.0"));
        height += 20;
        settings.size = GUI.HorizontalSlider(new Rect(x, height, width, 30), settings.size, 1f, 10f);
    }
#endif

    void Awake() {
        _player = charCon.transform;
        _cam = GetComponent<Camera>();
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
        //lets try something naive at first
        Vector3 pos = transform.position;
        pos.x = _player.position.x;
        pos.y = _player.position.y;
        if(charCon.goingRight) {
            pos.x += settings.lookAhead;
        }
        else{
            pos.x -= settings.lookAhead;
        }
        pos.x = Mathf.Lerp(transform.position.x, pos.x, Time.deltaTime * settings.speed);
        pos.y = Mathf.Lerp(transform.position.y, pos.y, Time.deltaTime * settings.speedVert);
        transform.position = pos;

        _cam.orthographicSize = settings.size;

        if(Input.GetKeyUp(KeyCode.Tab)) {
            hideGui = !hideGui;
        }
    }
}
