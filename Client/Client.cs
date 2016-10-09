using System;
using UnityEngine;

namespace LunaClient
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    public class Client : MonoBehaviour
    {
        public void Awake()
        {
            DontDestroyOnLoad(this);
            try
            {
                MainSystem.Singleton.Reset();
            }
            catch (Exception e)
            {
                MainSystem.Singleton.HandleException(e, "MainClient-" + nameof(Awake));
            }
        }

        public void OnApplicationQuit()
        {
            MainSystem.Singleton.OnExit();
        }

        public void OnDestroy()
        {
            MainSystem.Singleton.OnExit();
        }

        public void Update()
        {
            try
            {
                MainSystem.Singleton.Update();
            }
            catch (Exception e)
            {
                MainSystem.Singleton.HandleException(e, "MainClient-" + nameof(Update));
            }
        }

        public void OnGUI()
        {
            try
            {
                MainSystem.Singleton.OnGui();
            }
            catch (Exception e)
            {
                MainSystem.Singleton.HandleException(e, "MainClient-" + nameof(OnGUI));
            }
        }

        public void FixedUpdate()
        {
            try
            {
                MainSystem.Singleton.FixedUpdate();
            }
            catch (Exception e)
            {
                MainSystem.Singleton.HandleException(e, "MainClient-" + nameof(FixedUpdate));
            }
        }
    }
}