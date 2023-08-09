using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotMovement : MonoBehaviour
{
    [SerializeField] private Transform[] targetPoints;
    [SerializeField] private Transform productsTransform;
    [SerializeField] private Transform robotArea;
    [SerializeField] private Animator animator;

    private int productCount;
    private int currentCount;
    private NavMeshAgent agent;
    private Transform carriedProduct;
    private bool isIdle = true;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        productCount = productsTransform.childCount;
    }

    private void Update()
    {

        currentCount = 0;
        for (int i = 0; i < targetPoints.Length; i++)
        {
            if (targetPoints[i].childCount > 0)
                currentCount++;
        }

        if (isIdle && currentCount < productCount)
        {
            HandlePickingUpProduct();
        }
        else if (!isIdle)
        {
            HandleDroppingProduct();
        }
        else if (productCount == currentCount)
        {
                agent.SetDestination(robotArea.position);
            if (ReachedDestination())
            {
                animator.SetFloat("Speed", 0);
                agent.isStopped = true; // Hareketi durdur
            }
            else
            {
                //agent.isStopped = false; // Hareketi devam ettir
                animator.SetFloat("Speed", agent.velocity.magnitude);
            }
        }

        Debug.Log("currentCount: " + currentCount + "targetCount: " + productCount);
    }



    private bool ReachedDestination()
    {
        return !agent.pathPending && agent.remainingDistance < 0.3f;
    }

    private void HandlePickingUpProduct()
    {

        if (currentCount < productCount)
        {
            if (productsTransform.childCount > 0)
            {

                agent.SetDestination(productsTransform.GetChild(0).position);


                if (ReachedDestination())
                {
                    animator.SetFloat("Speed", 0);
                    PickUpProduct();
                }
                animator.SetFloat("Speed", agent.velocity.magnitude);
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
                DropProduct();

                //currentCount++;
                //if (currentCount >= targetPoints.Length)
                //{
                //    currentCount = 0;
                //}

                isIdle = true;
            }
        }
    }

    private void PickUpProduct()
    {

        if (productsTransform.childCount < productCount)
        {
            //currentCount++;
        }

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
