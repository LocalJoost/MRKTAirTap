
using RealityCollective.ServiceFramework.Definitions;
using RealityCollective.ServiceFramework.Interfaces;
using UnityEngine;

namespace MRTKExtensions.Services
{
    [CreateAssetMenu(menuName = "MRTK3ConfigurationFindingServiceProfile", fileName = "MRTK3ConfigurationFindingServiceProfile", order = (int)CreateProfileMenuItemIndices.ServiceConfig)]
    public class MRTK3ConfigurationFindingServiceProfile : BaseServiceProfile<IServiceModule>
    { }
}
