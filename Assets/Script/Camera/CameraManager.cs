using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    public List<Camera> cameras = new List<Camera>();
    private int currentCameraIndex = 0;

    [Header("Input Actions")]
    public InputActionReference cameraAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (cameras.Count > 0)
        {
            UpdateCameras();
        }
    }

    private void OnEnable()
    {
        cameraAction.action.performed += SwitchCamera;
    }

    private void OnDisable()
    {
        cameraAction.action.performed -= SwitchCamera;
    }

    // Update is called once per frame
    void SwitchCamera(InputAction.CallbackContext _ctx)
    {
         NextCamera();

    }

    public void NextCamera()
    {
        if (cameras.Count == 0) return;

        currentCameraIndex = (currentCameraIndex + 1) % cameras.Count;
        UpdateCameras();
    }

    private void UpdateCameras()
    {
        for (int i = 0; i < cameras.Count; i++)
        {
            cameras[i].enabled = (i == currentCameraIndex);
        }
    }

    private void SwitchToCamera(int index)
    {
        if (index >= 0 && index < cameras.Count)
        {
            currentCameraIndex = index;
            UpdateCameras();
        }
    }

    public Camera GetCurrentCamera()
    {
        return cameras[currentCameraIndex];
    }
}
