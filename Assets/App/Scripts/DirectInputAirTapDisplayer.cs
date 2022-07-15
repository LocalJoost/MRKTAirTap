using UnityEngine;
using UnityEngine.InputSystem;

public class DirectInputAirTapDisplayer : MonoBehaviour
{
    [SerializeField]
    private GameObject leftHand;
    [SerializeField]
    private GameObject rightHand;

    [SerializeField]
    private InputActionReference leftHandReference;
    [SerializeField]
    private InputActionReference rightHandReference;
    
    private void Start()
    {
        leftHand.SetActive(false);
        rightHand.SetActive(false);
        leftHandReference.action.performed += ProcessLeftHand;
        rightHandReference.action.performed += ProcessRightHand;
    }

    private void ProcessRightHand(InputAction.CallbackContext ctx)
    {
        ProcessHand(ctx, rightHand);
    }

    private void ProcessLeftHand(InputAction.CallbackContext ctx)
    {
        ProcessHand(ctx, leftHand);
    }

    private void ProcessHand(InputAction.CallbackContext ctx, GameObject g)
    {
        g.SetActive(ctx.ReadValue<float>() > 0.95f);
    }

    private void OnDestroy()
    {
        leftHandReference.action.performed -= ProcessLeftHand;
        rightHandReference.action.performed -= ProcessRightHand;
    }
}