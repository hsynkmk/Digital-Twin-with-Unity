using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Destination
{
    public static Transform destinationTransform;
    public static int destinationCount;
    private static List<bool> availabilityList;

    public static void MakeAllAvailable()
    {
        destinationCount = destinationTransform.childCount;
        availabilityList = new List<bool>();
        for (int i = 0; i < destinationCount; i++)
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
        for (int i = 0; i < destinationCount; i++)
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
