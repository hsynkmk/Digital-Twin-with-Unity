using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Objects
{
    public static Transform productTransform;
    private static Queue<Transform> availableProducts = new Queue<Transform>();

    public static void MakeAllAvailable()
    {
        foreach (Transform child in productTransform)
        {
            availableProducts.Enqueue(child);
        }
    }

    public static Transform GetAvailableProduct()
    {
        if (availableProducts.Count > 0)
        {
            return availableProducts.Dequeue();
        }
        return null;
    }

    public static bool IsAvailable()
    {
        return availableProducts.Count > 0;
    }
}
