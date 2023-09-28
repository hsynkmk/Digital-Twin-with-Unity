using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class RobotSpeed : MonoBehaviour
{ 
    [SerializeField] Slider speedSlider;
    [SerializeField] TextMeshProUGUI speedValueText;

    public void HandleSpeed()
    {
        if (speedSlider.value == 0)
            transform.parent.parent.parent.GetComponentInParent<NavMeshAgent>().speed = 0;

        else
            transform.parent.parent.parent.GetComponentInParent<NavMeshAgent>().speed = speedSlider.value;

        speedValueText.text = speedSlider.value.ToString("0.00");
    }
}
