using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Product
{
    public static Transform productTransform;
    public static int productCount;
    private static List<bool> availabilityList;
    

    public static void MakeAllAvailable()
    {
        productCount = productTransform.childCount;
        availabilityList = new List<bool>();
        for (int i = 0; i < productCount; i++)
        {
            availabilityList.Add(true);
        }
    }

    public static Transform GetLocation()
    {
        return productTransform.GetChild(GetAvailableProduct());
    }

    public static int GetAvailableProduct()
    {
        int index = -1;
        for (int i = 0; i < productCount; i++)
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
