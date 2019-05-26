using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurthestBackgroundMover : MonoBehaviour
{
    public GameObject player;
    public GameObject[] backgrounds = new GameObject[3];

    private const float RATE_DIFF   = 0.02f;

    private List<Vector3> offsetList = new List<Vector3>();

    private int nBackgrounds = 0;
    private float playerY = 0f;
    private Vector3 offset;
    private Vector3 newpos;

    void Start()
    {
        int nBackgrounds = backgrounds.Length;
        offset = transform.position - player.transform.position;
        foreach(GameObject bg in backgrounds) {
            offsetList.Add(bg.transform.position);
        }
    }

    void Update()
    {
        updateRootBackground();
        updateChildBackgrounds();
    }

    void updateRootBackground()
    {
        playerY = player.transform.position.y;
        newpos = transform.position;
        newpos[1] = playerY + offset[1];
        transform.position = newpos;
    }

    void updateChildBackgrounds()
    {
        int i = 0;
        foreach(GameObject bg in backgrounds) {
            newpos[1] = playerY * MULTIPLIER(i) + offsetList[i][1];
            bg.transform.position = newpos;
            i++;
        }
    }

    float MULTIPLIER(int n)
    {
        return 1.00f - (RATE_DIFF * (n + 1));
    }
}
