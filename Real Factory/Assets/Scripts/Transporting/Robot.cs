using UnityEngine.AI;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

// Enum to represent the different states of the robot
public enum RobotState
{
    OnPark,
    OnResource,
    OnDelivery,
    OnSpawn,
    OnProduct
}

public class Robot
{
    private int robotID;
    public Transform transformRobot { get; set; }
    public Transform transformTarget { get; set; }
    public Transform transformPark { get; set; }

    public RobotState robotState { get; set; }
    public bool hasReachedDest { get; set; }
    public float timer { get; set; }
    public int robotBattery { get; set; }
    public int minBattery { get; set; }

    public Robot(int robotID, Transform transformRobot, Transform transformTarget, Transform transformPark, RobotState robotState, bool hasReachedDest, float timer, int robotBattery)
    {
        this.robotID = robotID;
        this.transformRobot = transformRobot;
        this.transformTarget = transformTarget;
        this.transformPark = transformPark;
        this.robotState = robotState;
        this.hasReachedDest = hasReachedDest;
        this.timer = timer;
        this.robotBattery = robotBattery;
    }

    public string BatteryManager()
    {
        float decreaseInterval = 1f; // Decrease the int every 1 second

        // Update the timer with the time passed since the last frame for each robot
        timer += Time.deltaTime;
        NavMeshAgent agent = transformRobot.GetComponent<NavMeshAgent>();

        if (robotState == RobotState.OnPark && !agent.pathPending && agent.remainingDistance < 0.1f)
        {
            Light parkLight = Park.parkTransform.GetChild(robotID).GetChild(2).GetComponent<Light>();
            parkLight.enabled = true;
            if (timer >= decreaseInterval && robotBattery < 100)
            {
                // Decrease the int variable by 1
                robotBattery++;
                parkLight.enabled = false;
                // Reset the timer for this robot
                timer = 0f;
            }
        }
        else
        {
            // Check if the timer has reached the desired interval (1 second) for each robot
            if (timer >= decreaseInterval)
            {
                // Decrease the int variable by 1 for this robot
                robotBattery--;

                // Reset the timer for this robot
                timer = 0f;

                // Check if the int variable has reached a certain value and handle it
                if ((robotBattery <= minBattery) && (!agent.pathPending && agent.remainingDistance < 0.1f))
                {
                    agent.SetDestination(Park.GetIndex(robotID).position);
                }
            }
        }

        // Update the battery text for each robot

        return "Battery: " + robotBattery.ToString() + "\nState: " + robotState.ToString();
    }
}