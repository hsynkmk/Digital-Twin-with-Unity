using System.Collections.Generic;
using UnityEngine;

public static class ResourceManager
{
    // Queue to store available resource transforms
    public static Queue<Transform> availableResources = new Queue<Transform>();

    // Get an available resource transform from the queue
    public static Transform GetAvailableResource()
    {
        if (availableResources.Count > 0)
        {
            return availableResources.Dequeue();
        }
        return null;
    }

    // Check if there are available resources
    public static bool HasAvailableResources()
    {
        return availableResources.Count > 0;
    }
}
