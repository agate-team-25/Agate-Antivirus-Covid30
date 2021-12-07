using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveController : MonoBehaviour
{
    [Header("Position")]
    public Transform player;
    public float horizontalOffset;

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            Vector3 newPosition = transform.position;
            newPosition.x = player.position.x + horizontalOffset;
            newPosition.y = player.position.y;
            transform.position = newPosition;
        }
    }
}
