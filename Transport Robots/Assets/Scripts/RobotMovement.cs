using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class RobotMovement : MonoBehaviour
{
    [SerializeField] private Transform[] targetPoints;
    [SerializeField] private Transform productsTransform;
    [SerializeField] private Transform robotArea;
    [SerializeField] private Animator animator;
    [SerializeField] private TextMeshProUGUI objectCountText;

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
        currentCount = CountObjectsAtTargetPoints();

        if (isIdle && currentCount < productCount)
        {
            HandlePickingUpProduct();
        }
        else if (!isIdle)
        {
            HandleDroppingProduct();
        }
        else if (currentCount == productCount)
        {
            HandleReturningToRobotArea();
        }
    }

    private int CountObjectsAtTargetPoints()
    {
        int count = 0;
        for (int i = 0; i < targetPoints.Length; i++)
        {
            if (targetPoints[i].childCount > 0)
            {
                count++;
                objectCountText.text = "Delivered Objects: " + count.ToString();
            }
        }
        return count;
    }

    private void HandlePickingUpProduct()
    {
        if (currentCount < productCount && productsTransform.childCount > 0)
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
            isIdle = true;
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
                isIdle = true;
            }
        }
    }

    private void HandleReturningToRobotArea()
    {
        agent.SetDestination(robotArea.position);

        if (ReachedDestination())
        {
            animator.SetFloat("Speed", 0);
            agent.isStopped = true;
        }
        else
        {
            animator.SetFloat("Speed", agent.velocity.magnitude);
        }
    }

    private bool ReachedDestination()
    {
        return !agent.pathPending && agent.remainingDistance < 0.3f;
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
