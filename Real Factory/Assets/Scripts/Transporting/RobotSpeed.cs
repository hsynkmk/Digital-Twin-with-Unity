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
        transform.parent.parent.parent.GetComponentInParent<NavMeshAgent>().speed = speedSlider.value;
        speedValueText.text = speedSlider.value.ToString("00.00");
    }
}
