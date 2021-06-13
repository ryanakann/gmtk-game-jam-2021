using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AIMovementController : MonoBehaviour
{

    bool passive;
    float cooldown = 0, cooldownMax = 1f;

    Path path;
    Seeker seeker;

    Transform subTarget, target;

    // Start is called before the first frame update
    void Start()
    {
        subTarget = transform.FindDeepChild("subTarget");
        seeker = GetComponent<Seeker>();
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            if (cooldown == 0)
            {
                seeker.StartPath(transform.position, target.position, OnPathComplete);
            }
        }
    }
}
