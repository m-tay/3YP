using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBuilder : MonoBehaviour
{
    public NavMeshSurface navMeshSurface;

    public void buildNavMesh() {
        navMeshSurface.BuildNavMesh();
    }

}