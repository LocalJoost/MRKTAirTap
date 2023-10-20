using MRTKExtensions.Services.Interfaces;
using RealityCollective.ServiceFramework.Services;
using MixedReality.Toolkit;
using MixedReality.Toolkit.Input;
using MixedReality.Toolkit.Subsystems;
using UnityEngine;
using UnityEngine.Events;

namespace MRTKExtensions.Services
{
    [System.Runtime.InteropServices.Guid("dd1c8edc-335b-4510-872a-ced01fca424a")]
    public class MRTK3ConfigurationFindingService : BaseServiceWithConstructor, IMRTK3ConfigurationFindingService
    {
        public MRTK3ConfigurationFindingService(string name, uint priority, MRTK3ConfigurationFindingServiceProfile profile)
            : base(name, priority)
        {
        }
        
        public ControllerLookup ControllerLookup => controllerLookup;

        private bool leftHandTriggerStatus = false;
        private bool rightHandTriggerStatus = false;

        private IHandsAggregatorSubsystem handsAggregatorSubsystem;
        
        public ArticulatedHandController LeftHand => (ArticulatedHandController)controllerLookup.LeftHandController;
        public ArticulatedHandController RightHand => (ArticulatedHandController)controllerLookup.RightHandController;

        private const float PinchTreshold = 0.95f;
        
        /// <inheritdoc />
        public override void Initialize()
        {
        }

        /// <inheritdoc />
        public override void Start()
        {
            GetHandControllerLookup();
            handsAggregatorSubsystem = XRSubsystemHelpers.GetFirstRunningSubsystem<IHandsAggregatorSubsystem>();
        }

        private ControllerLookup controllerLookup;

        private void GetHandControllerLookup()
        {
            if (controllerLookup == null)
            {
                ControllerLookup[] lookups = GameObject.FindObjectsOfType(typeof(ControllerLookup)) as ControllerLookup[];
                if (lookups.Length == 0)
                {
                    Debug.LogError(
                        "Could not locate an instance of the ControllerLookup class in the hierarchy. It is recommended to add ControllerLookup to your camera rig.");
                }
                else if (lookups.Length > 1)
                {
                    Debug.LogWarning(
                        "Found more than one instance of the ControllerLookup class in the hierarchy. Defaulting to the first instance.");
                    controllerLookup = lookups[0];
                }
                else
                {
                    controllerLookup = lookups[0];
                }
            }
        }

        public UnityEvent<bool> LeftHandStatusTriggered { get; }= new();
        
        public UnityEvent<bool> RightHandStatusTriggered { get; }= new();

        /// <inheritdoc />
        public override void Update()
        {
            if (!TryUpdateByTrigger())
            {
                TryUpdateByPinch();
            }
        }
        
        private bool TryUpdateByTrigger()
        {
            var triggeredByTrigger = false;
            var newStatus = GetIsTriggered(LeftHand);
            if (newStatus != leftHandTriggerStatus && LeftHand)
            {
                leftHandTriggerStatus = newStatus;
                LeftHandStatusTriggered.Invoke(leftHandTriggerStatus);
                triggeredByTrigger = true;
            }
            
            newStatus = GetIsTriggered(RightHand);
            if (newStatus != rightHandTriggerStatus)
            {
                rightHandTriggerStatus = newStatus;
                RightHandStatusTriggered.Invoke(rightHandTriggerStatus);
                triggeredByTrigger = true;
            }

            return triggeredByTrigger;
        }
        
        private bool GetIsTriggered(ArticulatedHandController hand)
        {
            return hand.currentControllerState.selectInteractionState.value > PinchTreshold;
        }

        private void TryUpdateByPinch()
        {
            if (handsAggregatorSubsystem != null)
            {
                var newStatus = TryUpdateByPinch(LeftHand);
                if (newStatus != leftHandTriggerStatus)
                {
                    leftHandTriggerStatus = newStatus;
                    LeftHandStatusTriggered.Invoke(leftHandTriggerStatus);
                }
            
                newStatus = TryUpdateByPinch(RightHand);
                if (newStatus != rightHandTriggerStatus)
                {
                    rightHandTriggerStatus = newStatus;
                    RightHandStatusTriggered.Invoke(rightHandTriggerStatus);    
                }
            }
        }

        private bool TryUpdateByPinch(ArticulatedHandController handController)
        {
            var progressDetectable = 
                handsAggregatorSubsystem.TryGetPinchProgress(handController.HandNode,
                                                             out bool isReadyToPinch, 
                                                             out bool isPinching, 
                                                             out float pinchAmount);
            return progressDetectable && isPinching && pinchAmount > PinchTreshold;
        }
    }
}
