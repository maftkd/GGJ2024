using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public Transform bulletPrefab;
    public bool hideGui;
    public Transform gunPivot;
    private Camera _cam;

    void OnGUI() {
        if(hideGui) {
            return;
        }
        float x = 300;
        float width = 120;
        GUI.Box(new Rect(x - 10,10,width + 20,220), "Gun Info");
        /*
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
        */
    }

    // Start is called before the first frame update
    void Awake()
    {
        _cam = Camera.main;
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 delta = new Vector2(mousePos.x, mousePos.y)
            - new Vector2(gunPivot.position.x, gunPivot.position.y);
        delta.Normalize();
        gunPivot.right = new Vector3(delta.x, delta.y, 0);
    }
}
