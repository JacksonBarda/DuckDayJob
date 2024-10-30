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

    private float xRot;
    private float yMin;
    private float yMax;

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
        cameraTransform.position = new Vector3(Mathf.Clamp(cameraTransform.position.x, xMin, xMax), Mathf.Clamp(cameraTransform.position.y, yMin, yMax), cameraTransform.position.z);
        cameraTransform.eulerAngles = new Vector3(xRot, 0, 0);
    }
    public void SetBumps(Locations currentLocation)
    {
        switch (currentLocation)
        {
            case Locations.LOBBY:
                //zBump = -8.29f;
                //yBump = 1.5f;
                //xMin = -6.85f;
                //xMax = 16.46f;
                //yMin = .5f;
                //yMax = 12.3f;
                //xRot = 12.36f;
                zBump = -8.29f;
                yBump = 1.4f;
                xMin = -7.65f;
                xMax = 16.46f;
                yMin = .5f;
                yMax = 12.3f;
                xRot = 12.36f;

                break;

            case Locations.CLOSET:
                zBump = -8.29f;
                yBump = 1.0f;
                xMin = -7.0f;
                xMax = 16.46f;
                yMin = .5f;
                yMax = 12.3f;
                xRot = 12.36f;
                break;

            case Locations.OFFICE:      //should be the same with lobby
                zBump = -8.29f;
                yBump = 1.5f;
                xMin = -7.0f;
                xMax = 16.46f;
                yMin = .5f;
                yMax = 12.3f;
                xRot = 12.36f;
                break;

            case Locations.BREAKROOM:
                zBump = -6.99f;
                yBump = 1.0f;
                xMin = 32.55f;
                xMax = 32.76f;
                yMin = .5f;
                yMax = 12.3f;
                xRot = 12.36f;
                break;

            case Locations.MEETINGROOM:
                zBump = -8.29f;
                yBump = 1.0f;
                xMin = 82.1f;
                xMax = 106.46f;
                yMin = .5f;
                yMax = 12.3f;
                xRot = 12.36f;
                break;

            case Locations.BATHROOM:
                zBump = -7.29f;
                yBump = 1.0f;
                xMin = 39.21f;
                xMax = 40.51f;
                yMin = .5f;
                yMax = 12.3f;
                xRot = 12.36f;
                break;

            case Locations.MAZE:
                zBump = -8.29f;
                yBump = 1.0f;
                xMin = -7.0f;
                xMax = 16.46f;
                yMin = .5f;
                yMax = 12.3f;
                xRot = 12.36f;
                break;

            case Locations.MANUFACTURING:
                zBump = -8.29f;
                yBump = 1.0f;
                xMin = -7.0f;
                xMax = 16.46f;
                yMin = .5f;
                yMax = 12.3f;
                xRot = 12.36f;
                break;

            case Locations.KILLINGFLOOR:
                zBump = -8.29f;
                yBump = 1.0f;
                xMin = -7.0f;
                xMax = 16.46f;
                yMin = .5f;
                yMax = 12.3f;
                xRot = 12.36f;
                break;

            case Locations.BOSSROOM:
                zBump = -8.29f;
                yBump = 1.0f;
                xMin = 24.4f;
                xMax = 25.8f;
                yMin = .5f;
                yMax = 12.3f;
                xRot = 12.36f;
                break;

            case Locations.VENTILATION:
                zBump = -8.29f;
                yBump = 0;
                //xMin = -31.88f;
                //xMax = -21.36f;
                //yMin = .5f;
                //yMax = 12.3f;
                xMin = -41.88f;
                xMax = -11.36f;
                yMin = -10.5f;
                yMax = 22.3f;
                xRot = 0;
                break;

            default:

                break;

        }
        Debug.Log(currentLocation + " bumps set");
    }
}
