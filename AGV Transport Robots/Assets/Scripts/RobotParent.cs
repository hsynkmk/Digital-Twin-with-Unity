using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;

public class RobotManager : MonoBehaviour
{
    enum RobotState
    {
        OnPark,
        OnProduct,
        OnDestination,
    }

    [SerializeField] private Button startButton;
    [SerializeField] private TextMeshProUGUI infoText;
    private List<Transform> robotList = new List<Transform>();
    private List<RobotState> robotStates = new List<RobotState>();
    private int currentRobotIndex = 0;
    private int destIndex = 0;

    private int deliveredObjects = 0; // Track the number of delivered objects
    private int remainingObjects;

    private void Start()
    {
        Button btn = startButton.GetComponent<Button>();
        btn.onClick.AddListener(StartNextRobot);

        foreach (Transform child in transform)
        {
            robotList.Add(child);
            robotStates.Add(RobotState.OnPark); // Initialize each robot's state
        }

        remainingObjects = Product.productTransform.childCount; // Initialize remaining objects count
    }

    private void Update()
    {
        int workingRobots = 0;
        int fullParks = 0;

        for (int i = 0; i < robotStates.Count; i++)
        {
            if (robotStates[i] == RobotState.OnPark)
            {
                currentRobotIndex = i;
                fullParks++;
            }
            else if (robotStates[i] == RobotState.OnProduct || robotStates[i] == RobotState.OnDestination)
            {
                workingRobots++;
            }
        }

        infoText.text = $"Working robots: {workingRobots}/ {transform.childCount}\r\nFull parks: {fullParks}/{Park.parkTransform.childCount}\r\n\nRemaining objects: {remainingObjects}\r\nDelivered objects: {deliveredObjects}";

        for (int i = 0; i < robotStates.Count; i++)
        {
            if (robotStates[i] != RobotState.OnPark)
            {
                Park.MakeAvailable(i);
            }
        }

        for (int i = 0; i < robotStates.Count; i++)
        {
            if (robotStates[i] == RobotState.OnPark)
            {
                currentRobotIndex = i;
                break;
            }
        }

        for (int i = 0; i < robotStates.Count; i++)
        {
            Transform robot = robotList[i];
            NavMeshAgent agent = robot.GetComponent<NavMeshAgent>();
            RobotState currentState = robotStates[i];

            switch (currentState)
            {
                case RobotState.OnProduct:
                    if (!agent.pathPending && agent.remainingDistance < 0.1f)
                    {

                        Product.productTransform.GetChild(0).SetParent(robot.transform); // Set the robot as the parent of the product
                        MoveRobotToDestination(robot, agent);
                        robotStates[i] = RobotState.OnDestination; // Update state
                    }
                    break;
                case RobotState.OnDestination:
                    if (!agent.pathPending && agent.remainingDistance < 0.1f)
                    {

                        robot.GetChild(1).SetParent(Destination.destinationTransform.GetChild(destIndex)); // Set the target point as the parent of the carried product
                        destIndex++;
                        MoveRobotToPark(robot, agent);
                        robotStates[i] = RobotState.OnPark; // Reset state for the next cycle
                    }
                    break;
            }
        }
    }

    private void StartNextRobot()
    {
        if (robotStates[currentRobotIndex] == RobotState.OnPark && Product.IsAvailable())
        {
            Transform robot = robotList[currentRobotIndex];
            NavMeshAgent agent = robot.GetComponent<NavMeshAgent>();
            MoveRobotToProductLocation(robot, agent);
            robotStates[currentRobotIndex] = RobotState.OnProduct; // Update state
        }
    }

    // Implement the methods to move the robot to the product and destination locations
    private void MoveRobotToProductLocation(Transform robot, NavMeshAgent agent)
    {
        agent.SetDestination(Product.GetAvailableProduct().position);
        


    }

    private void MoveRobotToDestination(Transform robot, NavMeshAgent agent)
    {
        agent.SetDestination(Destination.GetLocation().position);
    }

    private void MoveRobotToPark(Transform robot, NavMeshAgent agent)
    {
        remainingObjects--;
        deliveredObjects++;
        agent.SetDestination(Park.GetLocation().position);
    }
}
