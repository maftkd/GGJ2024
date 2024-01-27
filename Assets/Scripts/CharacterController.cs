using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float maxHorVel;
    public float horLerp;
    private Vector2 _velocity;

    void OnGUI() {
        GUI.Box(new Rect(10,10,120,90), "Character Settings");
        horLerp = GUI.HorizontalSlider (new Rect (15, 35, 100, 30), horLerp, 0.0f, 400.0f);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float horInput = Input.GetAxis("Horizontal");
        _velocity.x = Mathf.Lerp(_velocity.x, horInput * maxHorVel, Time.deltaTime * horLerp);

        transform.position += Vector3.right * _velocity.x * Time.deltaTime;
    }

}
