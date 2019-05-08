using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// WallController should be attached to a wall that will synchronize it's
/// y-position with the main player.  It will follow the player continuously.
public class WallController : MonoBehaviour
{
    public GameObject player;

    void FixedUpdate()
    {
        Vector2 pos = transform.position;
        pos[1] = player.transform.position.y;
        transform.position = pos;
    }
}
