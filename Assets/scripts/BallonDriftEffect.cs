using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallonDriftEffect : MonoBehaviour
{
    /// the balloon will drift around this point.
    Vector2 basePos;

    /// max distance the balloon will float away from its original position
    float driftDistance = 1f;

    /// random number generated when balloon is spawned, to make the drift 
    /// effect look out-of-sync from the other balloons.
    float rando;

    void OnEnable()
    {
        basePos = transform.position;
        rando = Random.Range(-2 * Mathf.PI, 2 * Mathf.PI);
    }

    void FixedUpdate()
    {
        float dy = driftDistance * Mathf.Sin(Time.fixedTime + rando);
        float x = basePos.x;
        float y = basePos.y + dy;
        transform.position = new Vector2(x, y);
    }

}
