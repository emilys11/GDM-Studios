using UnityEngine;

public class GateController : MonoBehaviour
{
    [Header("Gate Parts")]
    [SerializeField] private Transform leftGate;
    [SerializeField] private Transform rightGate;

    [Header("Settings")]
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float openSpeed = 2f;

    private Quaternion leftClosed;
    private Quaternion rightClosed;
    private Quaternion leftOpen;
    private Quaternion rightOpen;

    private bool opening = false;
    private bool closing = false;

    private void Start()
    {
        if (leftGate == null || rightGate == null)
        {
            Debug.LogError("GateController: Left or Right gate is not assigned.");
            return;
        }

        leftClosed = leftGate.localRotation;
        rightClosed = rightGate.localRotation;

        leftOpen = leftClosed * Quaternion.Euler(0f, -openAngle, 0f);
        rightOpen = rightClosed * Quaternion.Euler(0f, openAngle, 0f);
    }

    private void Update()
    {
        if (opening)
        {
            if (leftGate != null)
            {
                leftGate.localRotation = Quaternion.Slerp(
                    leftGate.localRotation,
                    leftOpen,
                    Time.deltaTime * openSpeed
                );
            }

            if (rightGate != null)
            {
                rightGate.localRotation = Quaternion.Slerp(
                    rightGate.localRotation,
                    rightOpen,
                    Time.deltaTime * openSpeed
                );
            }
        }

        if (closing)
        {
            if (leftGate != null)
            {
                leftGate.localRotation = Quaternion.Slerp(
                    leftGate.localRotation,
                    leftClosed,
                    Time.deltaTime * openSpeed
                );
            }

            if (rightGate != null)
            {
                rightGate.localRotation = Quaternion.Slerp(
                    rightGate.localRotation,
                    rightClosed,
                    Time.deltaTime * openSpeed
                );
            }
        }
    }

    public void OpenGate()
    {
        opening = true;
        closing = false;
    }

    public void CloseGate()
    {
        closing = true;
        opening = false;
    }
}