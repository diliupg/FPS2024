using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClickToMove : MonoBehaviour
{
    private NavMeshAgent navAgent;

    private void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //create a ray from the camera to the mouse pos
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            //check if the ray hits the navmesh
            if(Physics.Raycast(ray, out hit, Mathf.Infinity, NavMesh.AllAreas))
            {
                // move agent to the clicked pos
                navAgent.SetDestination(hit.point);
            }
        }
    }
}
