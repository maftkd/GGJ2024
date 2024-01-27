using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Gun Settings", order = 2)]
public class GunSettingsSO : ScriptableObject
{
    public float fireDelay;
    public float bulletSpeed;
    public float bulletRadius;
}
