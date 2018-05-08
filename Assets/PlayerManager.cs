﻿using UnityEngine;

public class PlayerManager : MonoBehaviour {

    [HideInInspector] public static float[] playerHeightMultiplier;

    [HideInInspector] public bool isAlive;
    [HideInInspector] public int playerNumber;
    [HideInInspector] public GameObject instance;
    [HideInInspector] public Transform gunPoint;
    public Color playerColor;
    public Transform spawnPoint;

    private Transform gunRotation;
    private Quaternion shiftRotation; // for correcing aiming when player has been rotated
    private PlayerMovement movement;

    private static void SetInitialPleyerHeights()
    {
        playerHeightMultiplier = new float[4] { 0.33f, 0.33f, 0.33f, 0.33f };
    }

    public void Setup()
    {
        isAlive = true;
        
        movement = instance.GetComponent<PlayerMovement>();
        
        gunRotation = instance.transform.Find("Body").transform.Find("GunRotation");
        gunPoint = gunRotation.transform.Find("GunPoint");

        movement.playerNumber = playerNumber;
        movement.gunPoint = gunPoint;

        if (playerHeightMultiplier == null)
        {
            SetInitialPleyerHeights();
        }

        SetPlayerHeight();
        SetColor(playerColor);
    }
    

    private void SetPlayerHeight()
    {
        instance.transform.Find("Body").localScale = new Vector3(1, playerHeightMultiplier[playerNumber], 1);
        instance.transform.Find("Head").localPosition = new Vector3(0, playerHeightMultiplier[playerNumber] + 0.45f, 0);
    }

    void Update()
    {
        if (!movement.isAlive)
        {
            SetColor(Color.white);
            isAlive = false;
        }
    }

    private void FixedUpdate()
    {
        var angle = Vector3.Angle(Vector3.up, instance.transform.up);
        if(angle == 0)
        {
            shiftRotation = Quaternion.Inverse(Quaternion.FromToRotation(Vector3.forward, instance.transform.forward));
        }
        
        var horizontal = Input.GetAxis("RightStickHorizontal" + playerNumber);
        var vertical = Input.GetAxis("RightStickVertical" + playerNumber);

        if (horizontal != 0 || vertical != 0)
        {
            Vector3 lookDirection = new Vector3(horizontal, 0, vertical);

            gunRotation.rotation = instance.transform.rotation * shiftRotation * Quaternion.LookRotation(lookDirection);
        }
    }

    private void SetColor(Color color)
    {
        MeshRenderer[] renderers = instance.GetComponentsInChildren<MeshRenderer>();

        foreach(var renderer in renderers)
        {
            renderer.material.color = color;
        }
    }
}
