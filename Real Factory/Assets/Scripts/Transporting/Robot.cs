using UnityEngine.AI;
using UnityEngine;

public class Robot:MonoBehaviour
{
    public enum RobotState
    {
        OnPark,
        OnResource,
        OnDelivery,
    }

    public int Index { get; private set; }
    public RobotState State { get; private set; }
    public Transform Transform { get; private set; }

    private Transform mineDelivery;
    private Transform siliconDelivery;
    private Transform phoneDelivery;
    private Transform target;

    public Robot(Transform transform, Transform mineDelivery, Transform siliconDelivery, Transform phoneDelivery)
    {
        Transform = transform;
        this.mineDelivery = mineDelivery;
        this.siliconDelivery = siliconDelivery;
        this.phoneDelivery = phoneDelivery;
        State = RobotState.OnPark;
    }

    public void UpdateRobot()
    {
        NavMeshAgent agent = Transform.GetComponent<NavMeshAgent>();

        switch (State)
        {
            case RobotState.OnResource:
                if (!agent.pathPending && agent.remainingDistance < 0.1f)
                {
                    Transform.GetChild(1).parent = Transform;
                    Transform.GetChild(1).localPosition = new Vector3(0, 0.2f, 0);
                    string objectTag = Transform.GetChild(1).gameObject.tag;

                    if (objectTag == "Silicon")
                    {
                        MoveToDestination(agent, siliconDelivery);
                        State = RobotState.OnDelivery;
                    }
                    else if (objectTag == "Iron Mine" || objectTag == "Cooper Mine")
                    {
                        MoveToDestination(agent, mineDelivery);
                        State = RobotState.OnDelivery;
                    }
                    else if (objectTag == "Phone")
                    {
                        MoveToDestination(agent, phoneDelivery);
                        State = RobotState.OnDelivery;
                    }
                }
                break;

            case RobotState.OnDelivery:
                if (!agent.pathPending && agent.remainingDistance < 0.1f)
                {
                    Transform.GetChild(1).parent = null;
                    MoveToDestination(agent, Park.GetIndex(Index));
                    State = RobotState.OnPark;
                }
                break;
        }
    }

    public void StartRobot()
    {
        NavMeshAgent agent = Transform.GetComponent<NavMeshAgent>();
        target = ResourceManager.GetAvailableResource();
        agent.SetDestination(target.position);
        State = RobotState.OnResource;
    }

    private void MoveToDestination(NavMeshAgent agent, Transform destination)
    {
        agent.SetDestination(destination.position);
    }
}