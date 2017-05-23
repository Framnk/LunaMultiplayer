﻿using LunaClient.Base;
using LunaClient.Systems.CraftLibrary;
using LunaClient.Systems.Flag;
using LunaClient.Systems.KerbalSys;
using LunaClient.Systems.Lock;
using LunaClient.Systems.ModApi;
using LunaClient.Systems.Scenario;
using LunaClient.Systems.TimeSyncer;
using LunaClient.Systems.VesselChangeSys;
using LunaClient.Systems.VesselDockSys;
using LunaClient.Systems.VesselFlightStateSys;
using LunaClient.Systems.VesselImmortalSys;
using LunaClient.Systems.VesselLockSys;
using LunaClient.Systems.VesselPositionSys;
using LunaClient.Systems.VesselProtoSys;
using LunaClient.Systems.VesselRangeSys;
using LunaClient.Systems.VesselRemoveSys;
using LunaClient.Systems.VesselUpdateSys;
using LunaClient.Systems.Warp;
using LunaClient.Utilities;
using UnityEngine;

namespace LunaClient.Windows.Systems
{
    public partial class SystemsWindow : Window<SystemsWindow>
    {
        #region Fields & properties

        private float WindowHeight { get; } = 400;
        private float WindowWidth { get; } = 400;

        private bool VesselSystems { get; set; }
        private bool DisplayFast { get; set; }
        private float LastUpdateTime { get; set; }
        private float DisplayUpdateSInterval { get; } = 1f;

        #region Vessel systems

        private bool VesselChange { get; set; }
        private string VesselChangeProfilerText { get; set; }

        private bool VesselDock { get; set; }
        private string VesselDockProfilerText { get; set; }

        private bool VesselFlightState { get; set; }
        private string VesselFlightStateProfilerText { get; set; }

        private bool VesselImmortal { get; set; }
        private string VesselImmortalProfilerText { get; set; }

        private bool VesselLock { get; set; }
        private string VesselLockProfilerText { get; set; }

        private bool VesselPosition { get; set; }
        private string VesselPositionProfilerText { get; set; }

        private bool VesselProto { get; set; }
        private string VesselProtoProfilerText { get; set; }

        private bool VesselRange { get; set; }
        private string VesselRangeProfilerText { get; set; }

        private bool VesselRemove { get; set; }
        private string VesselRemoveProfilerText { get; set; }

        private bool VesselUpdate { get; set; }
        private string VesselUpdateProfilerText { get; set; }

        #endregion

        #region Other systems

        private bool CraftLibrary { get; set; }
        private string CraftLibraryProfilerText { get; set; }

        private bool Flag { get; set; }
        private string FlagProfilerText { get; set; }

        private bool Scenario { get; set; }
        private string ScenarioProfilerText { get; set; }

        private bool TimeSyncer { get; set; }
        private string TimeSyncerProfilerText { get; set; }

        private bool ModApi { get; set; }
        private string ModApiProfilerText { get; set; }

        private bool Lock { get; set; }
        private string LockProfilerText { get; set; }

        private bool Kerbal { get; set; }
        private string KerbalProfilerText { get; set; }

        private bool Warp { get; set; }
        private string WarpProfilerText { get; set; }

        #endregion

        #endregion

        public override void Update()
        {
            SafeDisplay = Display;
            if (Display && (DisplayFast || Time.realtimeSinceStartup - LastUpdateTime > DisplayUpdateSInterval))
            {
                LastUpdateTime = Time.realtimeSinceStartup;

                VesselChangeProfilerText = VesselChangeSystem.Singleton.GetProfilersData();
                VesselDockProfilerText = VesselDockSystem.Singleton.GetProfilersData();
                VesselFlightStateProfilerText = VesselFlightStateSystem.Singleton.GetProfilersData();
                VesselImmortalProfilerText = VesselImmortalSystem.Singleton.GetProfilersData();
                VesselLockProfilerText = VesselLockSystem.Singleton.GetProfilersData();
                VesselPositionProfilerText = VesselPositionSystem.Singleton.GetProfilersData();
                VesselProtoProfilerText = VesselProtoSystem.Singleton.GetProfilersData();
                VesselRangeProfilerText = VesselRangeSystem.Singleton.GetProfilersData();
                VesselRemoveProfilerText = VesselRemoveSystem.Singleton.GetProfilersData();
                VesselUpdateProfilerText = VesselUpdateSystem.Singleton.GetProfilersData();
                CraftLibraryProfilerText = CraftLibrarySystem.Singleton.GetProfilersData();
                FlagProfilerText = FlagSystem.Singleton.GetProfilersData();
                ScenarioProfilerText = ScenarioSystem.Singleton.GetProfilersData();
                TimeSyncerProfilerText = TimeSyncerSystem.Singleton.GetProfilersData();
                ModApiProfilerText = ModApiSystem.Singleton.GetProfilersData();
                LockProfilerText = LockSystem.Singleton.GetProfilersData();
                KerbalProfilerText = KerbalSystem.Singleton.GetProfilersData();
                WarpProfilerText = WarpSystem.Singleton.GetProfilersData();
            }
        }

        public override void OnGui()
        {
            base.OnGui();
            if (SafeDisplay)
                WindowRect =
                    LmpGuiUtil.PreventOffscreenWindow(GUILayout.Window(6715 + MainSystem.WindowOffset, WindowRect,
                        DrawContent, "LunaMultiPlayer - Systems", WindowStyle, LayoutOptions));
            CheckWindowLock();
        }

        public override void SetStyles()
        {
            WindowRect = new Rect(Screen.width - (WindowWidth + 50), Screen.height / 2f - WindowHeight / 2f, WindowWidth,
                WindowHeight);
            MoveRect = new Rect(0, 0, 10000, 20);

            LayoutOptions = new GUILayoutOption[4];
            LayoutOptions[0] = GUILayout.MinWidth(WindowWidth);
            LayoutOptions[1] = GUILayout.MaxWidth(WindowWidth);
            LayoutOptions[2] = GUILayout.MinHeight(WindowHeight);
            LayoutOptions[3] = GUILayout.MaxHeight(WindowHeight);

            WindowStyle = new GUIStyle(GUI.skin.window);
            ButtonStyle = new GUIStyle(GUI.skin.button);

            TextAreaOptions = new GUILayoutOption[1];
            TextAreaOptions[0] = GUILayout.ExpandWidth(true);

            LabelStyle = new GUIStyle(GUI.skin.label);
        }

        public override void RemoveWindowLock()
        {
            if (IsWindowLocked)
            {
                IsWindowLocked = false;
                InputLockManager.RemoveControlLock("LMP_SystemsWindowsLock");
            }
        }

        private void CheckWindowLock()
        {
            if (!MainSystem.Singleton.GameRunning)
            {
                RemoveWindowLock();
                return;
            }

            if (HighLogic.LoadedSceneIsFlight)
            {
                RemoveWindowLock();
                return;
            }

            if (SafeDisplay)
            {
                Vector2 mousePos = Input.mousePosition;
                mousePos.y = Screen.height - mousePos.y;

                var shouldLock = WindowRect.Contains(mousePos);

                if (shouldLock && !IsWindowLocked)
                {
                    InputLockManager.SetControlLock(ControlTypes.ALLBUTCAMERAS, "LMP_SystemsWindowsLock");
                    IsWindowLocked = true;
                }
                if (!shouldLock && IsWindowLocked)
                    RemoveWindowLock();
            }

            if (!SafeDisplay && IsWindowLocked)
                RemoveWindowLock();
        }
    }
}
