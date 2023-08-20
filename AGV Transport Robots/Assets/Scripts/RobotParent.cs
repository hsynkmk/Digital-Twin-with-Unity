using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class RobotManager : MonoBehaviour
{
    enum RobotState
    {
        OnPark,
        OnProduct,
        OnDestination,
    }

    [SerializeField] private Button startButton;
    private List<Transform> robotList = new List<Transform>();
    private List<RobotState> robotStates = new List<RobotState>();
    private int currentRobotIndex = 0;

    private void Start()
    {
        Button btn = startButton.GetComponent<Button>();
        btn.onClick.AddListener(StartNextRobot);

        foreach (Transform child in transform)
        {
            robotList.Add(child);
            robotStates.Add(RobotState.OnPark); // Initialize each robot's state
        }
    }

    private void Update()
    {
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
                        MoveRobotToDestination(robot, agent);
                        robotStates[i] = RobotState.OnDestination; // Update state
                    }
                    break;
                case RobotState.OnDestination:
                    if (!agent.pathPending && agent.remainingDistance < 0.1f)
                    {
                        MoveRobotToPark(robot, agent);
                        robotStates[i] = RobotState.OnPark; // Reset state for the next cycle
                    }
                    break;
            }
        }
    }

    private void StartNextRobot()
    {
        if (currentRobotIndex < robotList.Count)
        {
            Transform robot = robotList[currentRobotIndex];
            NavMeshAgent agent = robot.GetComponent<NavMeshAgent>();
            MoveRobotToProductLocation(robot, agent);
            robotStates[currentRobotIndex] = RobotState.OnProduct; // Update state
            currentRobotIndex++;
        }
    }

    // Implement the methods to move the robot to the product and destination locations
    private void MoveRobotToProductLocation(Transform robot, NavMeshAgent agent)
    {
        agent.SetDestination(Product.GetLocation().position);
    }

    private void MoveRobotToDestination(Transform robot, NavMeshAgent agent)
    {
        agent.SetDestination(Destination.GetLocation().position);
    }

    private void MoveRobotToPark(Transform robot, NavMeshAgent agent)
    {
        agent.SetDestination(Park.GetLocation().position);
    }
}
