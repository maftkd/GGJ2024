using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private Vector2 _velocity;
    public AnimationCurve horAccelCurve;
    public float horAccelDur;
    private float _horAccelTimer;
    public float maxHorVel;

    public enum HorMoveState {
        Idle,
        Accel,
        FullSpeed,
        Decel
    }
    public HorMoveState horMoveState;
    // Start is called before the first frame update
    void Start()
    {
        horMoveState = HorMoveState.Idle;
        
    }

    // Update is called once per frame
    void Update()
    {
        float horInput = Input.GetAxis("Horizontal");

        switch(horMoveState) {
            case HorMoveState.Idle:
                if(horInput != 0) {
                    horMoveState = HorMoveState.Accel;
                    _horAccelTimer += Time.deltaTime;
                    _velocity.x = horAccelCurve.Evaluate(_horAccelTimer / horAccelDur)
                        * maxHorVel;
                }
                break;
            case HorMoveState.Accel:
                if(horInput != 0) {
                    horMoveState = HorMoveState.Accel;
                    _horAccelTimer += Time.deltaTime;
                    if(_horAccelTimer >= horAccelDur) {
                        _velocity.x = maxHorVel;
                        horMoveState = HorMoveState.FullSpeed;
                    }
                    else {
                        _velocity.x = horAccelCurve.Evaluate(_horAccelTimer / horAccelDur)
                            * maxHorVel;
                    }
                }
                break;
            case HorMoveState.FullSpeed:
                break;
        }

        transform.position += Vector3.right * _velocity.x * Time.deltaTime;
    }
}
