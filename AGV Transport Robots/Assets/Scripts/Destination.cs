using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Destination
{
    public static Transform destinationTransform;
    private static List<bool> availabilityList;

    public static void MakeAllAvailable()
    {
        availabilityList = new List<bool>();
        for (int i = 0; i < destinationTransform.childCount; i++)
        {
            availabilityList.Add(true);
        }
    }

    public static Transform GetLocation()
    {
        return destinationTransform.GetChild(GetAvailableDestination());
    }


    public static int GetAvailableDestination()
    {
        int index = -1;
        for (int i = 0; i < destinationTransform.childCount; i++)
        {
            if (availabilityList[i])
            {
                index = i;
                availabilityList[i] = false;
                break;
            }
        }

        return index;
    }
}
