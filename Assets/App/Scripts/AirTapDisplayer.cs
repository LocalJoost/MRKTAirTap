using MixedReality.Toolkit;
using MixedReality.Toolkit.Input;
using MixedReality.Toolkit.Subsystems;
using MRTKExtensions.Services.Interfaces;
using RealityCollective.ServiceFramework.Services;
using TMPro;
using UnityEngine;

public class AirTapDisplayer : MonoBehaviour
{
    [SerializeField]
    private GameObject leftHand;
    
    [SerializeField]
    private GameObject rightHand;
    
    [SerializeField]
    TextMeshPro textMesh;
    
    private IMRTK3ConfigurationFindingService findingService;
    private HandsAggregatorSubsystem handsAggregatorSubsystem;

    private void Start()
    {
        handsAggregatorSubsystem = XRSubsystemHelpers.GetFirstRunningSubsystem<HandsAggregatorSubsystem>();
        leftHand.SetActive(false);
        rightHand.SetActive(false);
        
        findingService = ServiceManager.Instance.GetService<IMRTK3ConfigurationFindingService>();
        
        findingService.LeftHandStatusTriggered.AddListener(t=>
        {
            leftHand.SetActive(t);
            if (t)
            {
                textMesh.text = $"Left hand position: {GetPinchPosition(findingService.LeftHand)}";
            }
        });
        
        findingService.RightHandStatusTriggered.AddListener(t=>
        {
            rightHand.SetActive(t);
            if (t)
            {
                textMesh.text = $"Right hand position: {GetPinchPosition(findingService.RightHand)}";
            }
        });
    }
    
    private Vector3 GetPinchPosition(ArticulatedHandController handController)
    {
        return handsAggregatorSubsystem.TryGetPinchingPoint(handController.HandNode, out var jointPose)
            ? jointPose.Position
            : Vector3.zero;
    }
}