using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Gères le changement de caméra
public class CameraManager : MonoBehaviour
{
    public List<Camera> cameras = new List<Camera>();
    private int currentCameraIndex = 0;

    [Header("Input Actions")]
    public InputActionReference cameraAction;
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

    // caméra suivante (touche C appuyé)
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

    // changement d'état : activé ou désactivé
    private void UpdateCameras()
    {
        for (int i = 0; i < cameras.Count; i++)
        {
            cameras[i].enabled = (i == currentCameraIndex);
        }
    }

    // Donne la caméra actuel au joueur (les déplacement vont étre ajusté en fonction de la caméra)
    public Camera GetCurrentCamera()
    {
        return cameras[currentCameraIndex];
    }
}
