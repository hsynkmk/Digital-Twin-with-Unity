using System.Collections.Generic;
using UnityEngine;

public static class Park
{
    // Reference to the park's transform
    public static Transform parkTransform;

    // List to track the availability of park locations
    private static List<bool> availabilityList;

    // Initialize the availability list, marking all locations as unavailable
    public static void Initialize()
    {
        availabilityList = new List<bool>();
        for (int i = 0; i < parkTransform.childCount; i++)
        {
            availabilityList.Add(false);
        }
    }

    // Get a transform representing a park location that is available
    public static Transform GetAvailableLocation()
    {
        int availableIndex = GetAvailableParkIndex();
        if (availableIndex != -1)
        {
            return GetIndex(availableIndex);
        }
        return null; // No available locations
    }

    // Get the transform of a specific park location by index
    public static Transform GetIndex(int index)
    {
        return parkTransform.GetChild(index);
    }

    // Mark a park location as available
    public static void MakeAvailable(int index)
    {
        availabilityList[index] = true;
    }

    // Mark a park location as unavailable
    public static void MakeUnavailable(int index)
    {
        availabilityList[index] = false;
    }

    // Get the index of an available park location, or -1 if none is available
    private static int GetAvailableParkIndex()
    {
        for (int i = 0; i < parkTransform.childCount; i++)
        {
            if (availabilityList[i])
            {
                availabilityList[i] = false; // Mark as unavailable after returning
                return i;
            }
        }
        return -1; // No available locations
    }
}
