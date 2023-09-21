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
    [SerializeField] private Transform productDelivery;
    [SerializeField] private Transform phoneDelivery;
    [SerializeField] private Transform productTransform;
    [SerializeField] private Transform parkTransform;
    [SerializeField] private int minBattery = 60;

    private List<Transform> robotList = new List<Transform>();
    private List<Transform> robotTarget = new List<Transform>();
    private float[] timers; // Array to store timers for each robot
    private int[] robotBatteries; // Array to store battery levels for each robot
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

        int numRobots = robotList.Count;
        timers = new float[numRobots];
        robotBatteries = new int[numRobots];

        // Initialize timers and battery levels for each robot
        for (int i = 0; i < numRobots; i++)
        {
            timers[i] = 0f;
            robotBatteries[i] = 100; // Initial battery level
        }
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
        BatteryManager();
    }

    private void BatteryManager()
    {
        float decreaseInterval = 1f; // Decrease the int every 1 second

        for (int i = 0; i < robotStates.Count; i++)
        {
            // Update the timer with the time passed since the last frame for each robot
            timers[i] += Time.deltaTime;
            NavMeshAgent agent = robotList[i].GetComponent<NavMeshAgent>();

            if (robotStates[i] == RobotState.OnPark && !agent.pathPending && agent.remainingDistance < 0.1f)
            {
                Light parkLight = Park.parkTransform.GetChild(i).GetChild(2).GetComponent<Light>();
                parkLight.enabled = true;
                if (timers[i] >= decreaseInterval && robotBatteries[i] < 100)
                {
                    // Decrease the int variable by 1
                    robotBatteries[i]++;
                    parkLight.enabled = false;
                    // Reset the timer for this robot
                    timers[i] = 0f;
                }
            }
            else
            {
                // Check if the timer has reached the desired interval (1 second) for each robot
                if (timers[i] >= decreaseInterval)
                {
                    // Decrease the int variable by 1 for this robot
                    robotBatteries[i]--;

                    // Reset the timer for this robot
                    timers[i] = 0f;

                    // Check if the int variable has reached a certain value and handle it
                    if ((robotBatteries[i] <= minBattery) && (!agent.pathPending && agent.remainingDistance < 0.1f))
                    {
                        agent.SetDestination(Park.GetIndex(i).position);
                    }
                }
            }

            // Update the battery text for each robot
            TextMeshPro robotText = robotList[i].GetChild(0).GetChild(2).GetComponent<TextMeshPro>();
            robotText.transform.LookAt(2 * robotText.transform.position - Camera.main.transform.position);
            robotText.text = "Battery: " + robotBatteries[i].ToString() + "\nState: " + robotStates[i].ToString();
        }
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

            if (robotStates[i] == RobotState.OnPark && robotBatteries[i] > minBattery + 15)
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
                            MoveRobotToProductDelivery(agent);
                            robotStates[i] = RobotState.OnDelivery;
                        }
                    }
                    break;

                // Move the robot to the park
                case RobotState.OnDelivery:
                    if (!agent.pathPending && agent.remainingDistance < 0.1f)
                    {
                        Rigidbody rb = robot.GetChild(1).GetComponent<Rigidbody>();
                        robot.GetChild(1).parent = null;
                        // Calculate the force direction (for example, forward)
                        Vector3 forceDirection = -transform.forward;

                        // Apply force to the object
                        rb.AddForce(forceDirection * 6, ForceMode.Impulse);

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
        if (robotStates[currentRobotIndex] == RobotState.OnPark && robotBatteries[currentRobotIndex] > minBattery + 15)
        {
            robotTarget[currentRobotIndex] = ResourceManager.GetAvailableResource();
            Transform robot = robotList[currentRobotIndex];
            NavMeshAgent agent = robot.GetComponent<NavMeshAgent>();
            MoveRobotToProductLocation(agent);
            robotStates[currentRobotIndex] = RobotState.OnResource;
        }
    }

    private void MoveRobotToProductLocation(NavMeshAgent agent)
    {
        // Move the robot to the product location

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

    private void MoveRobotToProductDelivery(NavMeshAgent agent)
    {
        // Move the robot to the destination
        agent.SetDestination(productDelivery.position);
    }

    private void MoveRobotToPark(int index, NavMeshAgent agent)
    {
        // Update remaining and delivered objects, move robot to the park
        remainingObjects--;
        deliveredObjects++;
        agent.SetDestination(Park.GetIndex(index).position);
    }
}
