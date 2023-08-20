using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class RobotManager : MonoBehaviour
{
    [SerializeField] private Button startButton;
    private List<Transform> robotList = new List<Transform>();
    private int currentRobotIndex = 0;

    private void Start()
    {
        Button btn = startButton.GetComponent<Button>();
        btn.onClick.AddListener(StartNextRobot);

        foreach (Transform child in transform)
        {
            robotList.Add(child);
        }
    }

    private void StartNextRobot()
    {
        if (currentRobotIndex < robotList.Count)
        {
            NavMeshAgent agent = robotList[currentRobotIndex].GetComponent<NavMeshAgent>();
            agent.SetDestination(Product.GetLocation().position);
            currentRobotIndex++;
        }
    }
}
