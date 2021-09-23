using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class SetCameraRotation : MonoBehaviour
{
    public GameObject cinemachineCamera;

    public void SetLevel (float sliderValue)
    {
        cinemachineCamera.GetComponent<CinemachineFreeLook>().m_YAxis.m_AccelTime = sliderValue;
    }
}
