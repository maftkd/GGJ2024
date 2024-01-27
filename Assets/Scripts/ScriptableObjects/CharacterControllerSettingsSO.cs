using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Char Controller Settings", order = 1)]
public class CharacterControllerSettingsSO : ScriptableObject
{
    public float maxHorVel;
    public float horLerp;
    public float jumpForce;
    public float jumpOverTime;
    public float jumpWindow;
}
