using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class RobotManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private Transform mineDelivery;
    [SerializeField] private Transform siliconDelivery;
    [SerializeField] private Transform productDelivery;
    [SerializeField] private Transform phoneDelivery;
    [SerializeField] private Transform materialTransform;
    [SerializeField] private Transform parkTransform;
    [SerializeField] private int minBattery = 60;


    private List<Robot> robotList = new List<Robot>();
    private int currentRobotIndex;
    private int deliveredObjects;
    private int remainingObjects;

    private void Awake()
    {   // Initialize the resource manager and park
        ResourceManager.resourceTransform = materialTransform;
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
        UpdateBatteryText();

        // Update robot states and move them
        UpdateParkStates();
        MoveRobots();

    }

    private void InitializeRobots()
    {
        // Initialize robot list and states based on the children of this transform
        for (int i = 0; i < transform.childCount; i++)
        {
            robotList.Add(new Robot(i, transform.GetChild(i), null, Park.GetIndex(i), RobotState.OnPark, false, 0f, 100));
        }
    }

    private void UpdateBatteryText()
    {
        for (int i = 0; i < robotList.Count; i++)
        {
            //robotList[i].BatteryManager();
            TextMeshPro robotText = robotList[i].transformRobot.GetChild(0).GetChild(2).GetComponent<TextMeshPro>();
            robotText.transform.LookAt(2 * robotText.transform.position - Camera.main.transform.position);
            robotText.text = robotList[i].BatteryManager();
        }
    }

    private void UpdateInfoText()
    {
        // Update the information text
        remainingObjects = ResourceManager.resourceTransform.childCount;
        int fullParks = 0;

        // Count the number of robots in different states
        for (int i = 0; i < robotList.Count; i++)
        {
            if (robotList[i].robotState == RobotState.OnPark)
            {
                fullParks++;
            }
        }

        // Update the UI text to show relevant information
        infoText.text = $"\r\nFull parks: {fullParks}/{Park.parkTransform.childCount}\r\n\nRemaining objects: {remainingObjects}\r\nDelivered objects: {deliveredObjects}";
    }

    private void UpdateParkStates()
    {
        // Release robots from parked state
        for (int i = 0; i < robotList.Count; i++)
        {
            if (robotList[i].robotState != RobotState.OnPark)
            {
                Park.MakeAvailable(i);
            }

            if (robotList[i].robotState == RobotState.OnPark && robotList[i].robotBattery > minBattery + 15)
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
        for (int i = 0; i < robotList.Count; i++)
        {
            Transform robot = robotList[i].transformRobot;
            NavMeshAgent agent = robot.GetComponent<NavMeshAgent>();
            RobotState currentState = robotList[i].robotState;

            switch (currentState)
            {
                // Move the robot to the resource location
                case RobotState.OnResource:
                    if (!agent.pathPending && agent.remainingDistance < 0.1f)
                    {
                        // Attach the resource to the robot and move to the delivery location
                        robotList[i].transformTarget.SetParent(robot.transform);
                        robot.transform.GetChild(1).localPosition = new Vector3(0, 0.2f, 0);
                        string objectTag = robot.transform.GetChild(1).gameObject.tag;

                        // Move the robot to the delivery location based on the object it is carrying
                        if (objectTag == "Silicon")
                        {
                            MoveRobotToSiliconDelivery(agent);
                            robotList[i].robotState = RobotState.OnDelivery;
                        }
                        else if (objectTag == "Iron Mine" || objectTag == "Cooper Mine")
                        {
                            MoveRobotToMineDelivery(agent);
                            robotList[i].robotState = RobotState.OnDelivery;
                        }
                        else if (objectTag == "Phone")
                        {
                            MoveRobotToProductDelivery(agent);
                            robotList[i].robotState = RobotState.OnDelivery;
                        }
                    }
                    break;

                // Move the robot to the park
                case RobotState.OnDelivery:
                    if (!agent.pathPending && agent.remainingDistance < 0.1f)
                    {
                        Rigidbody rb = robot.GetChild(1).GetComponent<Rigidbody>();
                        robot.GetChild(1).parent = null;
                        deliveredObjects++;
                        // Calculate the force direction (for example, forward)
                        Vector3 forceDirection = -transform.forward;

                        // Apply force to the object
                        rb.AddForce(forceDirection * 6, ForceMode.Impulse);

                        MoveRobotToPark(i, agent);
                        robotList[i].robotState = RobotState.OnPark;
                    }
                    break;
            }
        }
    }


    private void StartNextRobot()
    {
        // Start the next robot if conditions are met
        if (robotList[currentRobotIndex].robotState == RobotState.OnPark && robotList[currentRobotIndex].robotBattery > minBattery + 15)
        {
            robotList[currentRobotIndex].transformTarget = ResourceManager.GetAvailableResource();
            Transform robot = robotList[currentRobotIndex].transformRobot;
            NavMeshAgent agent = robot.GetComponent<NavMeshAgent>();
            MoveRobotToProductLocation(agent);
            robotList[currentRobotIndex].robotState = RobotState.OnResource;
        }
    }

    private void MoveRobotToProductLocation(NavMeshAgent agent)
    {
        agent.SetDestination(robotList[currentRobotIndex].transformTarget.position);
    }

    private void MoveRobotToMineDelivery(NavMeshAgent agent)
    {
        agent.SetDestination(mineDelivery.position);
    }

    private void MoveRobotToSiliconDelivery(NavMeshAgent agent)
    {
        agent.SetDestination(siliconDelivery.position);
    }

    private void MoveRobotToProductDelivery(NavMeshAgent agent)
    {
        agent.SetDestination(productDelivery.position);
    }

    private void MoveRobotToPark(int index, NavMeshAgent agent)
    {
        agent.SetDestination(Park.GetIndex(index).position);
    }
}
