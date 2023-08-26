using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;
using Unity.VisualScripting;

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
    [SerializeField] Transform mineDelivery;
    [SerializeField] Transform siliconDelivery;
    [SerializeField] Transform phoneDelivery;

    private GameObject SiliconPlace;
    private GameObject MineralPlace;
    private GameObject TruckPlace;

    private List<Transform> robotList = new List<Transform>();
    private List<Transform> robotTarget = new List<Transform>();
    private List<RobotState> robotStates = new List<RobotState>();

    private int currentRobotIndex = 0;
    private int deliveredObjects = 0;
    private int remainingObjects;


    private void Start()
    {
        // Subscribe to the button's click event
        SiliconPlace = GameObject.FindGameObjectWithTag("Silicon Place");
        MineralPlace = GameObject.FindGameObjectWithTag("Mineral Place");
        TruckPlace = GameObject.FindGameObjectWithTag("Truck Place");


        // Initialize robots and update remaining objects
        InitializeRobots();
        remainingObjects = Objects.resourceTransform.childCount;
        //UpdateInfoText();

    }

    private void Update()
    {
        if (Objects.IsAvailable())
            StartNextRobot();

        // Update the information text on each frame
        UpdateInfoText();

        // Update robot states and move them
        UpdateParkStates();
        MoveRobots();
    }
    
    private void UpdateInfoText()
    {
        int workingRobots = 0;
        int fullParks = 0;
        string text = "";

        // Count the number of robots in different states
        for (int i = 0; i < robotStates.Count; i++)
        {
            if (robotStates[i] == RobotState.OnPark)
            {
                fullParks++;
            }
            else if (robotStates[i] == RobotState.OnResource || robotStates[i] == RobotState.OnDelivery)
            {
                workingRobots++;
            }
        }

        for(int i = 0; i<robotList.Count; i++)
        {
            text += "Robot " + i + 1 + ": " + robotStates[i].ToString() + "\r\n";
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
                Park.MakeUnAvailable(i);
                currentRobotIndex = i;
                break;
            }
        }
    }

    private void MoveRobots()
    {
        Debug.Log("dis: "+Objects.resourceTransform.childCount);

        // Move robots based on their states
        for (int i = 0; i < robotStates.Count; i++)
        {
            Transform robot = robotList[i];
            NavMeshAgent agent = robot.GetComponent<NavMeshAgent>();
            RobotState currentState = robotStates[i];

            switch (currentState)
            {
                case RobotState.OnResource:
                    if (!agent.pathPending && agent.remainingDistance < 0.1f)
                    {
                        robotTarget[i].SetParent(robot.transform);
                        robot.transform.GetChild(1).localPosition = new Vector3(0, 0.2f, 0);
                        string objectTag = robot.transform.GetChild(1).gameObject.tag;


                        if (objectTag == "Silicon")
                        {
                            MoveRobotToSiliconDelivery(robot, agent);
                            robotStates[i] = RobotState.OnDelivery;
                        }
                        else if(objectTag == "Iron Mine" || objectTag == "Cooper Mine")
                        {
                            MoveRobotToMineDelivery(robot, agent);
                            robotStates[i] = RobotState.OnDelivery;
                        }
                        else if(objectTag == "Phone")
                        {
                            MoveRobotToPhoneDelivery(robot, agent);
                            robotStates[i] = RobotState.OnDelivery;
                        }
                    }
                    break;

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
            MoveRobotToProductLocation(robot, agent);
            robotStates[currentRobotIndex] = RobotState.OnResource;
        }
    }

    private void MoveRobotToProductLocation(Transform robot, NavMeshAgent agent)
    {
        // Move the robot to the product location
        robotTarget[currentRobotIndex] = Objects.GetAvailableProduct();

        for(int i = 0; i < robotTarget.Count; i++)
        {
            Debug.Log(robotTarget[i]);
        }
        
        agent.SetDestination(robotTarget[currentRobotIndex].position);
    }

    private void MoveRobotToMineDelivery(Transform robot, NavMeshAgent agent)
    {
        // Move the robot to the destination
        agent.SetDestination(mineDelivery.position);
    }

    private void MoveRobotToSiliconDelivery(Transform robot, NavMeshAgent agent)
    {
        // Move the robot to the destination
        agent.SetDestination(siliconDelivery.position);
    }

    private void MoveRobotToPhoneDelivery(Transform robot, NavMeshAgent agent)
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
