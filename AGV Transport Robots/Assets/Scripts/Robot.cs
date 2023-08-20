using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Robot : MonoBehaviour
{
    private NavMeshAgent agent;
    private bool isBusy = false;

    void Start()
    {
        //agent = GetComponent<NavMeshAgent>();
    }

    public void StartTransport()
    {
        //agent.SetDestination(Product.GetLocation().position);
        //isBusy = true;
    }

    public bool IsBusy()
    {
        return isBusy;
    }
}
