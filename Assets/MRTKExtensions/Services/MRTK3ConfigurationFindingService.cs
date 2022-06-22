using MRTKExtensions.Services.Interfaces;
using RealityCollective.ServiceFramework.Services;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using RealityCollective.ServiceFramework.Definitions;
using UnityEngine;
using UnityEngine.Events;

namespace MRTKExtensions.Services
{
    [System.Runtime.InteropServices.Guid("dd1c8edc-335b-4510-872a-ced01fca424a")]
    public class MRTK3ConfigurationFindingService : BaseServiceWithConstructor, IMRTK3ConfigurationFindingService
    {
        public MRTK3ConfigurationFindingService(string name, uint priority, BaseProfile profile)
            : base(name, priority)
        {
        }

        public override void Start()
        {
            GetHandControllerLookup();
        }

        #region Nicked from Solver

        private ControllerLookup controllerLookup;

        public ControllerLookup ControllerLookup => controllerLookup;

        private void GetHandControllerLookup()
        {
            if (controllerLookup == null)
            {
                ControllerLookup[] lookups =
                    GameObject.FindObjectsOfType(typeof(ControllerLookup)) as ControllerLookup[];
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

        #endregion



        public ArticulatedHandController LeftHand => 
            (ArticulatedHandController)controllerLookup.LeftHandController;
        public ArticulatedHandController RightHand => 
            (ArticulatedHandController)controllerLookup.RightHandController;

        
        private bool leftHandTriggerStatus;
        private bool rightHandTriggerStatus;
        
        public UnityEvent<bool> LeftHandStatusTriggered { get; } = new UnityEvent<bool>();
        public UnityEvent<bool> RightHandStatusTriggered { get; } = new UnityEvent<bool>();

        public override void Update()
        {
            var newStatus = GetIsTriggered(LeftHand);
            if (newStatus != leftHandTriggerStatus)
            {
                leftHandTriggerStatus = newStatus;
                LeftHandStatusTriggered.Invoke(leftHandTriggerStatus);
            }

            newStatus = GetIsTriggered(RightHand);
            if (newStatus != rightHandTriggerStatus)
            {
                rightHandTriggerStatus = newStatus;
                RightHandStatusTriggered.Invoke(rightHandTriggerStatus);
            }
        }

        private bool GetIsTriggered(ArticulatedHandController hand)
        {
            return hand.currentControllerState.selectInteractionState.value > 0.95f;
        }
    }
}