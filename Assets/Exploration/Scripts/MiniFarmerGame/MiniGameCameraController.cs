using UnityEngine;
using System.Collections;

public class MiniGameCameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform miniGameCameraPoint;

    [Header("Optional")]
    [SerializeField] private MonoBehaviour followCameraScript;

    [Header("Settings")]
    [SerializeField] private float transitionSpeed = 4f;

    private bool inMiniGameView = false;
    private Vector3 targetPosition;
    private Quaternion targetRotation;

    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    private void LateUpdate()
    {
        if (!inMiniGameView || mainCamera == null || miniGameCameraPoint == null) return;

        mainCamera.transform.position = Vector3.Lerp(
            mainCamera.transform.position,
            targetPosition,
            Time.deltaTime * transitionSpeed
        );

        mainCamera.transform.rotation = Quaternion.Slerp(
            mainCamera.transform.rotation,
            targetRotation,
            Time.deltaTime * transitionSpeed
        );
    }

    public void SwitchToMiniGameView()
    {
        if (mainCamera == null || miniGameCameraPoint == null) return;

        if (followCameraScript != null)
        {
            followCameraScript.enabled = false;
        }

        inMiniGameView = true;
        targetPosition = miniGameCameraPoint.position;
        targetRotation = miniGameCameraPoint.rotation;
    }

    public void SwitchToNormalView()
    {
        inMiniGameView = false;

        if (followCameraScript != null)
        {
            followCameraScript.enabled = true;
        }
    }
}