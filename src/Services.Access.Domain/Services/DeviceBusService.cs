using Services.Access.Domain.Interfaces.Services;
using System;
using static Services.Access.Domain.Model.Enums;

namespace Services.Access.Domain.Services
{
    /// <summary>
    /// Service for emulating the bus communication with IoT devices such as Site Gateways and 
    /// Physical lock devices that have been deployed and connected to the internet through a site gateway
    /// 
    /// This is a mock-up implementation of the IDeviceBusInterface, which returns random states in respose
    /// 
    /// A real implementation of this service would connect to actual devices through various communication channels and protocols
    /// Such as TCP/IP or MQTT
    /// </summary>

    public class DeviceBusService : IDeviceBusService
    {
        public LockState GetLockState(Guid id)
        {
            return GetRandomState<LockState>();
        }

        public SiteState GetSiteState(Guid id)
        {
            return SiteState.Online;
        }

        public LockState SetLockState(Guid id, LockState state)
        {
            return state;
        }

        public SiteState SetSiteState(Guid id, SiteState state)
        {
            return state;
        }

        private T GetRandomState<T>()
        {
            Random _R = new Random();
            var v = Enum.GetValues(typeof(T));
            return (T)v.GetValue(_R.Next(v.Length));
        }
    }
}
