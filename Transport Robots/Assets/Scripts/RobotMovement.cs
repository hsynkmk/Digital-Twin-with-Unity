using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotMovement : MonoBehaviour
{
    [SerializeField] private Transform[] targetPoints;
    [SerializeField] private Transform productsTransform;
    [SerializeField] private Animator animator;

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
        //Debug.Log("currentCount: " +currentCount + "targetCount: " +targetCount);
        if (currentCount < targetCount)
        {
            if (productsTransform.childCount > 0)
            {
                animator.SetFloat("Speed", agent.velocity.magnitude);
                agent.SetDestination(productsTransform.GetChild(0).position);
                

                if (ReachedDestination())
                {
                    PickUpProduct();
                }
            }

            else
            {
                // If there are no more products to pick up, set isIdle to true to start dropping.
                isIdle = true;
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
                animator.SetFloat("Speed", 0);
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
        carriedProduct.localPosition = new Vector3(0, 1, 0.5f);
        isIdle = false;
    }

    private void DropProduct()
    {
        if (carriedProduct != null)
        {
            carriedProduct.SetParent(targetPoints[currentCount]);
            carriedProduct.localPosition = new Vector3(0, carriedProduct.localScale.y / 2, 0);
            carriedProduct.eulerAngles = Vector3.zero;
            carriedProduct = null;
        }
    }
}
