using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBounds : MonoBehaviour
{
    public Bounds camBounds;
    public Camera playerCam;
    public Collider2D boundsCollider;

    void Start()
    {
        //boundsCollider = GetComponent<BoxCollider2D>();
        camBounds = boundsCollider.bounds;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 camPos = playerCam.transform.position;
        float halfSizeX = playerCam.orthographicSize / 9 * 16;
        float halfSizeY = playerCam.orthographicSize;
        float leftCam = camPos.x - halfSizeX;
        float rightCam = camPos.x + halfSizeX;
        float downCam = camPos.y - halfSizeY;
        float upCam = camPos.y + halfSizeY;

        Vector3 min = camBounds.min;
        Vector3 max = camBounds.max;

        if (leftCam < min.x)
        {
            camPos.x = leftCam + halfSizeX;
            print(camPos.x);
        }

        playerCam.transform.position = camPos;
    }
}
