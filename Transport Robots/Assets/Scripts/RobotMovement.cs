using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class RobotMovement : MonoBehaviour
{
    // Serialized fields to assign in the Inspector
    [SerializeField] private Transform[] targetPoints; // Array of target points for the robot to move to
    [SerializeField] private Transform productsTransform; // Transform containing the products
    [SerializeField] private Transform robotArea; // Transform representing the robot's starting area
    [SerializeField] private Animator animator; // Animator component for controlling animations
    [SerializeField] private TextMeshProUGUI objectCountText; // Text UI for displaying the count of delivered objects

    // Private variables
    private int productCount; // Total number of products
    private int currentCount; // Current count of delivered products
    private NavMeshAgent agent; // Reference to the NavMeshAgent component
    private Transform carriedProduct; // The product currently carried by the robot
    private bool isIdle = true; // Flag indicating if the robot is idle or not

    private void Awake()
    {
        // Assigning the NavMeshAgent component and calculating the total number of products
        agent = GetComponent<NavMeshAgent>();
        productCount = productsTransform.childCount;
    }

    private void Update()
    {
        // Count the number of delivered objects at target points
        currentCount = CountObjectsAtTargetPoints();

        // If the robot is idle and there are still undelivered products
        if (isIdle && currentCount < productCount)
        {
            HandlePickingUpProduct();
        }
        // If the robot is not idle (carrying a product)
        else if (!isIdle)
        {
            HandleDroppingProduct();
        }
        // If all products are delivered, return to the robot area
        else if (currentCount == productCount)
        {
            HandleReturningToRobotArea();
        }
    }

    private int CountObjectsAtTargetPoints()
    {
        int count = 0; // Start with a count of 0

        // Loop through each target point
        for (int i = 0; i < targetPoints.Length; i++)
        {
            // If a target point has a child (product), increment the count
            if (targetPoints[i].childCount > 0)
            {
                count++;
                objectCountText.text = "Delivered Objects: " + count.ToString();
            }
        }

        return count; // Return the count of delivered products
    }

    private void HandlePickingUpProduct()
    {
        // If there are still undelivered products and productsTransform has children
        if (currentCount < productCount && productsTransform.childCount > 0)
        {
            // Set the robot's destination to the position of the first child product
            agent.SetDestination(productsTransform.GetChild(0).position);

            // If the robot has reached the destination
            if (ReachedDestination())
            {
                animator.SetFloat("Speed", 0); // Stop the animation
                PickUpProduct(); // Pick up the product
            }

            animator.SetFloat("Speed", agent.velocity.magnitude); // Set animation speed based on movement speed
        }
        else
        {
            isIdle = true; // Set the robot to idle if no more undelivered products
        }
    }

    private void HandleDroppingProduct()
    {
        // If there are still undelivered target points
        if (currentCount < targetPoints.Length)
        {
            // Set the robot's destination to the position of the target point for the current count
            agent.SetDestination(targetPoints[currentCount].position);

            // If the robot has reached the destination
            if (ReachedDestination())
            {
                DropProduct(); // Drop the product
                isIdle = true; // Set the robot to idle
            }
        }
    }

    private void HandleReturningToRobotArea()
    {
        // Set the robot's destination to the robot area
        agent.SetDestination(robotArea.position);

        // If the robot has reached the destination
        if (ReachedDestination())
        {
            animator.SetFloat("Speed", 0); // Stop the animation
            agent.isStopped = true; // Stop the NavMeshAgent from moving
        }
        else
        {
            animator.SetFloat("Speed", agent.velocity.magnitude); // Set animation speed based on movement speed
        }
    }

    private bool ReachedDestination()
    {
        // Check if the NavMeshAgent has finished calculating its path and if the remaining distance is less than 0.3 units
        return !agent.pathPending && agent.remainingDistance < 0.3f;
    }

    private void PickUpProduct()
    {
        // Assign the first child product to the carriedProduct variable
        carriedProduct = productsTransform.GetChild(0);
        carriedProduct.SetParent(transform); // Set the robot as the parent of the product
        carriedProduct.localPosition = new Vector3(0, 1, 0.5f); // Set the local position of the carried product
        isIdle = false; // Set the robot to not idle
    }

    private void DropProduct()
    {
        if (carriedProduct != null)
        {
            carriedProduct.SetParent(targetPoints[currentCount]); // Set the target point as the parent of the carried product
            carriedProduct.localPosition = new Vector3(0, carriedProduct.localScale.y / 2, 0); // Set the local position of the dropped product
            carriedProduct.eulerAngles = Vector3.zero; // Reset the rotation of the dropped product
            carriedProduct = null; // Set carriedProduct to null to indicate no product is being carried
        }
    }
}
