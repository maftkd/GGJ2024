using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    public GameObject Map;

    private GameObject Player;
    private GameObject Camera;

    private void Start()
    {
        Player = GameObject.Find("Player");
        Camera = Camera.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Map.SetActive(!Map.activeSelf);
        }
    }

    public void TeleportToCheckpoint(GameObject location)
    {
        Player.transform.position = location.transform.position;
        Camera.transform.position = location.transform.position;
    }
}
