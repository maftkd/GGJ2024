using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GunController : MonoBehaviour
{
    public Transform bulletPrefab;
    public bool hideGui;
    public Transform gunPivot;
    private Camera _cam;
    public GunSettingsSO settings;

    private float _fireTimer;

#if UNITY_EDITOR
    void OnGUI() {
        if(hideGui) {
            return;
        }
        float x = 300;
        float width = 120;
        GUI.Box(new Rect(x - 10,10,width + 20,220), "Gun Info");

        float height = 35;
        GUI.Label (new Rect (x, height, width, 30), "Fire delay: " + settings.fireDelay.ToString("0.00"));
        height += 20;
        settings.fireDelay = GUI.HorizontalSlider(new Rect(x, height, width, 30), settings.fireDelay, 0f, 1f);

        height += 20;
        GUI.Label (new Rect (x, height, width, 30), "Bullet speed: " + settings.bulletSpeed.ToString("0.0"));
        height += 20;
        settings.bulletSpeed = GUI.HorizontalSlider(new Rect(x, height, width, 30), settings.bulletSpeed, 0f, 50f);

        height += 20;
        GUI.Label (new Rect (x, height, width, 30), "Bullet radius: " + settings.bulletRadius.ToString("0.0"));
        height += 20;
        settings.bulletRadius = GUI.HorizontalSlider(new Rect(x, height, width, 30), settings.bulletRadius, 0f, 2f);

        height += 20;
        GUI.Label (new Rect (x, height, width, 30), "Trail dur: " + settings.trailDur.ToString("0.00"));
        height += 20;
        settings.trailDur = GUI.HorizontalSlider(new Rect(x, height, width, 30), settings.trailDur, 0f, 1f);
    }
#endif

    // Start is called before the first frame update
    void Awake()
    {
        _cam = Camera.main;
    }

    void OnDestroy() {
#if UNITY_EDITOR
        EditorUtility.SetDirty(settings);
#endif
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 delta = new Vector2(mousePos.x, mousePos.y)
            - new Vector2(gunPivot.position.x, gunPivot.position.y);
        delta.Normalize();
        gunPivot.right = new Vector3(delta.x, delta.y, 0);

        if(Input.GetMouseButtonUp(0)) {
            _fireTimer = 0;
        }
        if(_fireTimer >= settings.fireDelay) {
            _fireTimer = 0;
        }
        else if(_fireTimer > 0) {
            _fireTimer += Time.deltaTime;
        }
        if(_fireTimer == 0 && Input.GetMouseButton(0)) {
            _fireTimer += Time.deltaTime;
            Transform bullet = Instantiate(bulletPrefab, gunPivot.position
                    + gunPivot.right, Quaternion.identity);
            Bullet b = bullet.GetComponent<Bullet>();
            b.Init(gunPivot.right * settings.bulletSpeed, settings.bulletRadius, settings.trailDur);
        }

        if(Input.GetKeyUp(KeyCode.Tab)) {
            hideGui = !hideGui;
        }
    }
}
