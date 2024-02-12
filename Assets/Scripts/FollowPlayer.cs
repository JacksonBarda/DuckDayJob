using Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField]
    private Transform Player;
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private UIManager uiManager;
    private Transform cameraTransform;
    private float yBump;
    private float zBump;
    private float xMin;
    private float xMax;
    // Start is called before the first frame update
    void Awake()
    {
        SetBumps(Locations.LOBBY);
        cameraTransform = mainCamera.transform;
        cameraTransform.position = new Vector3(Player.position.x, Player.position.y + yBump, zBump);
        //cameraTransform.position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        cameraTransform.position = new Vector3(Player.position.x, Player.position.y + yBump, zBump);
        cameraTransform.position = new Vector3(Mathf.Clamp(cameraTransform.position.x, xMin, xMax), cameraTransform.position.y, cameraTransform.position.z);
    }
    public void SetBumps(Locations currentLocation)
    {
        switch (currentLocation)
        {
            case Locations.LOBBY:
                zBump = -8.29f;
                yBump = 1.5f;
                xMin = -7.0f;
                xMax = 16.46f;
                
                break;

            case Locations.CLOSET:
                zBump = -8.29f;
                yBump = 1.0f;
                xMin = -7.0f;
                xMax = 16.46f;
                break;

            case Locations.OFFICE:
                zBump = -8.29f;
                yBump = 1.0f;
                xMin = -7.0f;
                xMax = 16.46f;
                break;

            case Locations.BREAKROOM:
                zBump = -6.99f;
                yBump = 1.0f;
                xMin = 32.55f;
                xMax = 32.76f;
                break;

            case Locations.MEETINGROOM:
                zBump = -8.29f;
                yBump = 1.0f;
                xMin = 82.1f;
                xMax = 106.46f;
                break;

            case Locations.BATHROOM:
                zBump = -7.29f;
                yBump = 1.0f;
                xMin = 39.21f;
                xMax = 40.51f;
                break;

            case Locations.MAZE:
                zBump = -8.29f;
                yBump = 1.0f;
                xMin = -7.0f;
                xMax = 16.46f;
                break;

            case Locations.MANUFACTURING:
                zBump = -8.29f;
                yBump = 1.0f;
                xMin = -7.0f;
                xMax = 16.46f;
                break;

            case Locations.KILLINGFLOOR:
                zBump = -8.29f;
                yBump = 1.0f;
                xMin = -7.0f;
                xMax = 16.46f;
                break;

            case Locations.BOSSROOM:
                zBump = -8.29f;
                yBump = 1.0f;
                xMin = 24.4f;
                xMax = 25.8f;
                break;

            default:

                break;

        }
    }
}
