using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] private Vector3[] points;

    public Vector3[] Points => points;
    public Vector3 CurrentPosition => currentPosition;

    private Vector3 currentPosition;
    private bool gameStarted;

    // Start is called before the first frame update
    void Start()
    {
        gameStarted = true;
        currentPosition = transform.position;
    }

    public Vector3 GetWaypointPosition(int index)
    {
        return CurrentPosition + Points[index];
    }

    private void OnDrawGizmos()
    {
        if (!gameStarted && transform.hasChanged)
        {
            currentPosition = transform.position;
        }

        for (int i = 0; i < points.Length; i++)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(points[i] + currentPosition, 0.5f);

            if (i < points.Length - 1)
            {
                Gizmos.color = Color.grey;
                Gizmos.DrawLine(points[i] + currentPosition, points[i + 1] + currentPosition);
            }
        }
    }
}
