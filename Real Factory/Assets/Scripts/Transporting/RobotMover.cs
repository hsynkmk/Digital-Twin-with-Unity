using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class RobotManager : MonoBehaviour
{
    // Enum to represent the different states of the robot
    enum RobotState
    {
        OnPark,
        OnResource,
        OnDelivery,
    }

    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private Transform mineDelivery;
    [SerializeField] private Transform siliconDelivery;
    [SerializeField] private Transform phoneDelivery;
    [SerializeField] private Transform productTransform;
    [SerializeField] private Transform parkTransform;

    private List<Transform> robotList = new List<Transform>();
    private List<Transform> robotTarget = new List<Transform>();
    private List<RobotState> robotStates = new List<RobotState>();

    private int currentRobotIndex;
    private int deliveredObjects;
    private int remainingObjects;

    private void Awake()
    {   // Initialize the resource manager and park
        ResourceManager.resourceTransform = productTransform;
        ResourceManager.InitializeAvailableResources();

        Park.parkTransform = parkTransform;
        Park.Initialize();
    }

    private void Start()
    {
        // Initialize robots and update remaining objects
        InitializeRobots();
    }

    private void Update()
    {
        // Start the next robot if there are available resources
        if (ResourceManager.HasAvailableResources())
            StartNextRobot();

        // Update the information text on each frame
        UpdateInfoText();

        // Update robot states and move them
        UpdateParkStates();
        MoveRobots();
    }

    private void UpdateInfoText()
    {
        // Update the information text
        remainingObjects = ResourceManager.resourceTransform.childCount;
        int fullParks = 0;
        string text = "";

        // Count the number of robots in different states
        for (int i = 0; i < robotStates.Count; i++)
        {
            if (robotStates[i] == RobotState.OnPark)
            {
                fullParks++;
            }
        }

        for (int i = 0; i < robotList.Count; i++)
        {
            text += "Robot " + (i + 1) + ": " + robotStates[i].ToString() + "\r\n";
        }

        infoText.text = text;

        // Update the UI text to show relevant information
        infoText.text += $"\r\nFull parks: {fullParks}/{Park.parkTransform.childCount}\r\n\nRemaining objects: {remainingObjects}\r\nDelivered objects: {deliveredObjects}";
    }


    private void InitializeRobots()
    {
        // Initialize robot list and states based on the children of this transform
        foreach (Transform child in transform)
        {
            robotList.Add(child);
            robotStates.Add(RobotState.OnPark);
            robotTarget.Add(null);
        }
    }

    private void UpdateParkStates()
    {
        // Release robots from parked state
        for (int i = 0; i < robotStates.Count; i++)
        {
            if (robotStates[i] != RobotState.OnPark)
            {
                Park.MakeAvailable(i);
            }
        }

        // Find the next available robot to start
        for (int i = 0; i < robotStates.Count; i++)
        {
            if (robotStates[i] == RobotState.OnPark)
            {
                Park.MakeUnavailable(i);
                currentRobotIndex = i;
                break;
            }
        }
    }

    private void MoveRobots()
    {
        // Move robots based on their states
        for (int i = 0; i < robotStates.Count; i++)
        {
            Transform robot = robotList[i];
            NavMeshAgent agent = robot.GetComponent<NavMeshAgent>();
            RobotState currentState = robotStates[i];

            switch (currentState)
            {
                // Move the robot to the resource location
                case RobotState.OnResource:
                    if (!agent.pathPending && agent.remainingDistance < 0.1f)
                    {
                        // Attach the resource to the robot and move to the delivery location
                        robotTarget[i].SetParent(robot.transform);
                        robot.transform.GetChild(1).localPosition = new Vector3(0, 0.2f, 0);
                        string objectTag = robot.transform.GetChild(1).gameObject.tag;

                        // Move the robot to the delivery location based on the object it is carrying
                        if (objectTag == "Silicon")
                        {
                            MoveRobotToSiliconDelivery(agent);
                            robotStates[i] = RobotState.OnDelivery;
                        }
                        else if (objectTag == "Iron Mine" || objectTag == "Cooper Mine")
                        {
                            MoveRobotToMineDelivery(agent);
                            robotStates[i] = RobotState.OnDelivery;
                        }
                        else if (objectTag == "Phone")
                        {
                            MoveRobotToPhoneDelivery(agent);
                            robotStates[i] = RobotState.OnDelivery;
                        }
                    }
                    break;

                // Move the robot to the park
                case RobotState.OnDelivery:
                    if (!agent.pathPending && agent.remainingDistance < 0.1f)
                    {
                        // Attach the destination and move back to the park
                        robot.GetChild(1).parent = null;
                        MoveRobotToPark(i, agent);
                        robotStates[i] = RobotState.OnPark;
                    }
                    break;
            }
        }
    }

    private void StartNextRobot()
    {
        // Start the next robot if conditions are met
        if (robotStates[currentRobotIndex] == RobotState.OnPark)
        {
            Transform robot = robotList[currentRobotIndex];
            NavMeshAgent agent = robot.GetComponent<NavMeshAgent>();
            MoveRobotToProductLocation(agent);
            robotStates[currentRobotIndex] = RobotState.OnResource;
        }
    }

    private void MoveRobotToProductLocation(NavMeshAgent agent)
    {
        // Move the robot to the product location
        robotTarget[currentRobotIndex] = ResourceManager.GetAvailableResource();
        agent.SetDestination(robotTarget[currentRobotIndex].position);
    }

    private void MoveRobotToMineDelivery(NavMeshAgent agent)
    {
        // Move the robot to the destination
        agent.SetDestination(mineDelivery.position);
    }

    private void MoveRobotToSiliconDelivery(NavMeshAgent agent)
    {
        // Move the robot to the destination
        agent.SetDestination(siliconDelivery.position);
    }

    private void MoveRobotToPhoneDelivery(NavMeshAgent agent)
    {
        // Move the robot to the destination
        agent.SetDestination(phoneDelivery.position);
    }

    private void MoveRobotToPark(int index, NavMeshAgent agent)
    {
        // Update remaining and delivered objects, move robot to the park
        remainingObjects--;
        deliveredObjects++;

        agent.SetDestination(Park.GetIndex(index).position);
    }
}
