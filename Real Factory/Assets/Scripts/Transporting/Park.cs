using System.Collections.Generic;
using UnityEngine;

public static class Park
{
    public static Transform parkTransform;
    private static List<bool> availabilityList;

    public static void MakeAllUnavailable()
    {
        availabilityList = new List<bool>();
        for (int i = 0; i < parkTransform.childCount; i++)
        {
            availabilityList.Add(false);
        }
    }

    public static Transform GetLocation()
    {
        return parkTransform.GetChild(GetAvailablePark());
    }

    public static Transform GetIndex(int index)
    {
        return parkTransform.GetChild(index);
    }

    public static void MakeAvailable(int index)
    {
        availabilityList[index] = true;
    }

    public static void MakeUnAvailable(int index)
    {
        availabilityList[index] = false;
    }

    public static int GetAvailablePark()
    {
        int index = -1;
        for (int i = 0; i < parkTransform.childCount; i++)
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
