using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Park
{
    public static Transform parkTransform;
    public static int parkCount;
    private static List<bool> availabilityList;

    public static void MakeAllAvailable()
    {
        parkCount = parkTransform.childCount;
        availabilityList = new List<bool>();
        for (int i = 0; i < parkCount; i++)
        {
            availabilityList.Add(true);
        }
    }

    public static Transform GetLocation()
    {
        return parkTransform.GetChild(GetAvailableProduct());
    }

    public static int GetAvailableProduct()
    {
        int index = -1;
        for (int i = 0; i < parkCount; i++)
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
