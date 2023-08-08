using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotMovement : MonoBehaviour
{
    [SerializeField] private Transform[] targetPoints;
    [SerializeField] private Transform productsTransform;

    private int targetCount;
    private int currentCount;
    private NavMeshAgent agent;
    private Transform carriedProduct;
    private bool isIdle = true;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        targetCount = productsTransform.childCount;
    }

    private void Update()
    {
        if (isIdle && currentCount < targetCount)
        {
            HandlePickingUpProduct();
        }
        else
        {
            HandleDroppingProduct();
        }
    }

    private bool ReachedDestination()
    {
        return !agent.pathPending && agent.remainingDistance < 0.1f;
    }

    private void HandlePickingUpProduct()
    {
        Debug.Log("currentCount: " +currentCount + "targetCount: " +targetCount);
        if (currentCount < targetCount)
        {
            agent.SetDestination(productsTransform.GetChild(0).position);

            if (ReachedDestination())
            {
                PickUpProduct();
            }
        }
    }

    private void HandleDroppingProduct()
    {
        if (currentCount < targetPoints.Length)
        {
            agent.SetDestination(targetPoints[currentCount].position);

            if (ReachedDestination())
            {
                DropProduct();

                currentCount++;
                if (currentCount >= targetPoints.Length)
                {
                    currentCount = 0;
                }

                isIdle = true;
            }
        }
    }

    private void PickUpProduct()
    {
        carriedProduct = productsTransform.GetChild(0);
        carriedProduct.SetParent(transform);
        //targetCount--;
        isIdle = false;
    }

    private void DropProduct()
    {
        if (carriedProduct != null)
        {
            carriedProduct.SetParent(targetPoints[currentCount]);
            carriedProduct.localPosition = Vector3.zero;
            carriedProduct = null;
        }
    }
}
