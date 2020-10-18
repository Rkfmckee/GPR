using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PathfindingValues
{
    private static int nextPathfindingId = 0;

    public static int GetNextPathfindingId() {
        int nextId = nextPathfindingId;
        nextPathfindingId++;

        return nextId;
    }
}
