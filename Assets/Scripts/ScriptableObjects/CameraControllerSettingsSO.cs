using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Camera Controller Settings", order = 2)]
public class CameraControllerSettingsSO : ScriptableObject
{
    public float lookAhead;
    public float speed;
    public float speedVert;
}
