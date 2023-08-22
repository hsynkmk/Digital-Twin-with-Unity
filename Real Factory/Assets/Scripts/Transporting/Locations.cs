using System.Collections.Generic;
using UnityEngine;

public abstract class Locations
{
    protected Transform locationTransform;
    protected List<bool> availabilityList;

    public abstract void MakeAllAvailable();

    public Transform GetLocation()
    {
        return locationTransform.GetChild(GetAvailableLocation());
    }

    protected abstract int GetAvailableLocation();
    // You can add other common methods here if needed
}
