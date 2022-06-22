using MRTKExtensions.Services.Interfaces;
using RealityCollective.ServiceFramework.Services;
using UnityEngine;

public class AirTapDisplayer : MonoBehaviour
{
    [SerializeField]
    private GameObject leftHand;
    
    [SerializeField]
    private GameObject rightHand;
    
    
    private IMRTK3ConfigurationFindingService findingService;
    private void Start()
    {
        leftHand.SetActive(false);
        rightHand.SetActive(false);
        
        findingService = ServiceManager.Instance.GetService<IMRTK3ConfigurationFindingService>();
        
        findingService.LeftHandStatusTriggered.AddListener(t=> leftHand.SetActive(t));
        findingService.RightHandStatusTriggered.AddListener(t=> rightHand.SetActive(t));
    }
}