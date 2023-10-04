using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using System;

public class RobotManager : MonoBehaviour
{
    [SerializeField] private Transform mineDelivery;
    [SerializeField] private Transform siliconDelivery;
    [SerializeField] private Transform productDelivery;
    [SerializeField] private Transform productLocation;
    [SerializeField] private Transform parkTransform;
    [SerializeField] private Transform spawnTransform;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private int minBattery = 60;
    [SerializeField] private TextMeshProUGUI logText;

    public List<Robot> robotList = new List<Robot>();
    private int currentRobotIndex;
    private int deliveredObjects;
    private int remainingObjects;

    private void Awake()
    {
        Park.parkTransform = parkTransform;
        Park.Initialize();
    }

    private void Start()
    {
        InitializeRobots();
    }

    private void Update()
    {
        if (ResourceManager.HasAvailableResources())
            StartNextRobot();

        UpdateInfoText();
        UpdateBatteryText();

        UpdateParkStates();
        MoveRobots();

    }

    private void InitializeRobots()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            robotList.Add(new Robot(i, transform.GetChild(i), null, Park.GetIndex(i), RobotState.OnPark, false, 0f, 100));
        }
    }

    private void UpdateBatteryText()
    {
        for (int i = 0; i < robotList.Count; i++)
        {

            //robotText.text = robotList[i].BatteryManager();
            //robotText.transform.LookAt(2 * robotText.transform.position - Camera.main.transform.position);
            robotList[i].transformRobot.GetChild(0).GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();
            robotList[i].transformRobot.GetChild(0).GetChild(2).GetComponent<TextMeshPro>().text = (i + 1).ToString();

            TextMeshProUGUI batteryText = robotList[i].transformRobot.GetChild(0).GetChild(3).GetChild(2).GetComponent<TextMeshProUGUI>();
            batteryText.text = robotList[i].BatteryManager();
        }
    }

    private void UpdateInfoText()
    {
        int fullParks = 0;

        for (int i = 0; i < robotList.Count; i++)
        {
            Slider batterySlider = robotList[i].transformRobot.GetChild(0).GetChild(3).GetChild(0).GetComponent<Slider>();
            TextMeshProUGUI batteryValue = robotList[i].transformRobot.GetChild(0).GetChild(3).GetChild(2).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI positionText = robotList[i].transformRobot.GetChild(0).GetChild(3).GetChild(4).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI targetText = robotList[i].transformRobot.GetChild(0).GetChild(3).GetChild(5).GetComponent<TextMeshProUGUI>();

            if (robotList[i].robotState == RobotState.OnPark)
            {
                fullParks++;
            }

            batterySlider.value = robotList[i].robotBattery;
            batteryValue.text = batterySlider.value.ToString("00.00");
            positionText.text = $"Position: {robotList[i].transformRobot.position.x.ToString("0.0")} {robotList[i].transformRobot.position.y.ToString("0.0")} {robotList[i].transformRobot.position.z.ToString("0.0")}";

            if (robotList[i].transformTarget != null)
            {
                targetText.text = "Target: " + robotList[i].transformTarget.tag;
            }
        }

        //infoText.text = $"\r\nFull parks: {fullParks}/{Park.parkTransform.childCount}\r\n\nRemaining objects: {remainingObjects}\r\nDelivered objects: {deliveredObjects}";
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
                // Move the robot to the spawn location
                case RobotState.OnSpawn:
                    if (!agent.pathPending && agent.remainingDistance < 0.1f)
                    {
                        GameObject newMine = Instantiate(robotList[i].transformTarget.transform.gameObject, spawnPosition.position + new Vector3(0, -1, 0), Quaternion.identity);
                        newMine.transform.SetParent(robotList[i].transformRobot);
                        robotList[i].robotState = RobotState.OnResource;
                        UpdateLogText();
                    }
                    break;

                case RobotState.OnResource:
                    if (!agent.pathPending && agent.remainingDistance < 0.1f)
                    {
                        if (robotList[i].transformTarget.CompareTag("Phone"))
                        {
                            robotList[i].transformTarget.SetParent(robot.transform);
                            robot.transform.GetChild(1).localPosition = new Vector3(0, 0.2f, 0);
                        }

                        string objectTag = robot.transform.GetChild(1).gameObject.tag;

                        if (objectTag == "Silicon")
                        {
                            MoveRobotToSiliconDelivery(agent);
                            robotList[i].transformTarget = siliconDelivery;
                            robotList[i].robotState = RobotState.OnDelivery;
                        }
                        else if (objectTag == "Iron Mine" || objectTag == "Cooper Mine")
                        {
                            MoveRobotToMineDelivery(agent);
                            robotList[i].transformTarget = mineDelivery;
                            robotList[i].robotState = RobotState.OnDelivery;
                        }
                        else if (objectTag == "Phone")
                        {
                            MoveRobotToProductDelivery(agent);
                            robotList[i].transformTarget = productDelivery;
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
                        Vector3 forceDirection = transform.forward;
                        rb.AddForce(forceDirection * 7, ForceMode.Impulse);

                        MoveRobotToPark(i, agent);
                        robotList[i].transformTarget = parkTransform;
                        robotList[i].robotState = RobotState.OnPark;
                    }
                    break;
            }
        }
    }

    private void UpdateLogText()
    {
        string currentText = logText.text;

        // Split the text by newline characters to create an array of lines.
        string[] lines = currentText.Split('\n');

        // Check if there is at least one line of text.
        if (lines.Length > 1)
        {
            // Remove the first line.
            string newText = string.Join("\n", lines, 1, lines.Length - 1);

            // Update the TextMeshPro component with the modified text.
            logText.text = newText;
        }
        else
        {
            // If there is only one line or no text, you can clear the TextMeshPro component.
            logText.text = "";
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

            if (robotList[currentRobotIndex].transformTarget.CompareTag("Phone"))
            {
                MoveRobotToProductLocation(agent);
                robotList[currentRobotIndex].robotState = RobotState.OnResource;
            }
            else if (robotList[currentRobotIndex].transformTarget != null)
            {
                MoveRobotToSpawnLocation(agent);
                robotList[currentRobotIndex].robotState = RobotState.OnSpawn;
            }
        }
    }

    private void MoveRobotToSpawnLocation(NavMeshAgent agent)
    {
        agent.SetDestination(spawnTransform.position);
    }
    private void MoveRobotToProductLocation(NavMeshAgent agent)
    {
        agent.SetDestination(productLocation.position);
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