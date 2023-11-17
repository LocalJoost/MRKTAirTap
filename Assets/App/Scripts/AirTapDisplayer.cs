using System;
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
    private IHandsAggregatorSubsystem handsAggregatorSubsystem;

    private void Start()
    {
        handsAggregatorSubsystem = XRSubsystemHelpers.GetFirstRunningSubsystem<IHandsAggregatorSubsystem>();
        leftHand.SetActive(false);
        rightHand.SetActive(false);
        findingService = ServiceManager.Instance.GetService<IMRTK3ConfigurationFindingService>();
        
        var rightLineRenderer = findingService.RightHand.gameObject.GetComponentInChildren<LineRenderer>(true);
        var leftLineRenderer = findingService.LeftHand.gameObject.GetComponentInChildren<LineRenderer>(true);
        findingService.LeftHandStatusTriggered.AddListener(t=>
        {
            leftHand.SetActive(t);
            if (t)
            {
                var rayPos = leftLineRenderer.GetPosition(leftLineRenderer.positionCount -1);
                textMesh.text = $"Left hand position: {GetPinchPosition(findingService.LeftHand)}";
                textMesh.text +=
                    $"{Environment.NewLine} Left hand ray position: {rayPos}";
                leftHand.transform.position = rayPos;
            }
        });
        
        findingService.RightHandStatusTriggered.AddListener(t=>
        {
            rightHand.SetActive(t);
            if (t)
            {
                var rayPos = rightLineRenderer.GetPosition(rightLineRenderer.positionCount -1);
                textMesh.text = $"Right hand position: {GetPinchPosition(findingService.RightHand)}";
                textMesh.text +=
                    $"{Environment.NewLine} Right hand ray position: {rayPos}";
                rightHand.transform.position = rayPos;
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