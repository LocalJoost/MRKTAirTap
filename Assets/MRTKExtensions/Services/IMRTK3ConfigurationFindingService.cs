using MixedReality.Toolkit;
using MixedReality.Toolkit.Input;
using RealityCollective.ServiceFramework.Interfaces;
using UnityEngine.Events;

namespace MRTKExtensions.Services.Interfaces
{
    public interface IMRTK3ConfigurationFindingService : IService
    {
        ControllerLookup ControllerLookup { get; }
        ArticulatedHandController LeftHand { get; }
        ArticulatedHandController RightHand { get; }

        UnityEvent<bool> LeftHandStatusTriggered { get; }
        UnityEvent<bool> RightHandStatusTriggered { get; }
    }
}