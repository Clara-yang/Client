using Csf.Imets.ToolCore.Dal.A664.Plugins;
using Csf.Imets.ToolCore.Vdn;
using Renci.SshNet;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using UAM.Plugin.Common;
using static UAM.Model.Models.VDNModel;

namespace UAM.Plugin
{
    public class VDNData
    {
        private static VDNConnectHelper connectHelper = new VDNConnectHelper();
        private static VDNConnectHelper helper = VDNConnectHelper.CreateInstance(); 
        private static NetVdnClient m_NetVdnClient = helper.GetClient();
        private Thread connectThread;
        private Timer thread_Time;

        #region
        public static int MotionSwitch = 2;
        public static bool MotionFreeze = false;
        public static string KernelModeString = "";
        public static bool BJKernelModeString = false;
        public static bool FlightFreeze = false;
        public static bool CrashOverride = false;
        public static bool BJCrashOverride = false;
        public static bool BJClearCrash = false;
        public static bool PowerOn = false;
        public static bool EngineOn = false;
        public static bool PowerFreeze = false;
        public static bool LocationFreeze = false;
        public static bool HighlyFreeze = false;
        public static bool FlightPlanGenerated = false;
        public static bool APState = false;
        public static bool BJAPState = false;
        public static string Current_View = "Cockpit_View";
        public static double DestinationLat = 0.00;
        public static double DestinationLon = 0.00;
        public static double DestinationAlt = 0.00;
        public static double DestinationHeading = 0.00;

        public static bool CIGISwitch = false;
        public static int CIGICloudyType = 0;
        public static int CIGILayerID = 0;
        public static double CIGICoverage = 0.00;
        public static string CIGISeason = "";
        public static string CIGITimeOfDay = "";
        public static string CIGIWeather = "";
        public static string GroundWindDirection = "";
        public static string GroundWindVelocity = "";
        public static string HollowDirection = "";
        public static string HollowVelocity = "";
        public static string GustSpeed = "";
        public static bool CIGIRandomLightningEnable = false;
        public static int CIGISeverity = 0;
        public static double CIGIThickness = 0.00;
        public static double CIGITransitionBand = 0.00;
        public static double CIGIVisibility = 0.00;
         
        public static bool VisualConnect = false;

        public static double Latitude = 0.00;
        public static double Longitude = 0.00;
        public static double Altitude = 0.00;
        public static double Roll = 0.00;
        public static double Pitch = 0.00;
        public static double Heading = 0.00;

        public static bool CtrSurLock0IsActive = false;
        public static bool CtrSurLock1IsActive = false;
        public static bool CtrSurLock2IsActive = false;
        public static bool CtrSurLock3IsActive = false;
        public static bool CtrSurLock4IsActive = false;
        public static bool CtrSurLock5IsActive = false;
        public static bool CtrSurLock6IsActive = false;
        public static bool CtrSurLock7IsActive = false;
        public static bool CtrSurLock8IsActive = false;
        public static bool CtrSurLock9IsActive = false;
        public static bool CtrSurLock0IsEnable = true;
        public static bool CtrSurLock1IsEnable = true;
        public static bool CtrSurLock2IsEnable = true;
        public static bool CtrSurLock3IsEnable = true;
        public static bool CtrSurLock4IsEnable = true;
        public static bool CtrSurLock5IsEnable = true;
        public static bool CtrSurLock6IsEnable = true;
        public static bool CtrSurLock7IsEnable = true;
        public static bool CtrSurLock8IsEnable = true;
        public static bool CtrSurLock9IsEnable = true;
        public static bool CtrSurLoss0IsActive = false;
        public static bool CtrSurLoss1IsActive = false;
        public static bool CtrSurLoss2IsActive = false;
        public static bool CtrSurLoss3IsActive = false;
        public static bool CtrSurLoss4IsActive = false;
        public static bool CtrSurLoss5IsActive = false;
        public static bool CtrSurLoss6IsActive = false;
        public static bool CtrSurLoss7IsActive = false;
        public static bool CtrSurLoss8IsActive = false;
        public static bool CtrSurLoss9IsActive = false;
        public static bool CtrSurLoss0IsEnable = true;
        public static bool CtrSurLoss1IsEnable = true;
        public static bool CtrSurLoss2IsEnable = true;
        public static bool CtrSurLoss3IsEnable = true;
        public static bool CtrSurLoss4IsEnable = true;
        public static bool CtrSurLoss5IsEnable = true;
        public static bool CtrSurLoss6IsEnable = true;
        public static bool CtrSurLoss7IsEnable = true;
        public static bool CtrSurLoss8IsEnable = true;
        public static bool CtrSurLoss9IsEnable = true;

        public static bool MotorLoss0IsActive = false;
        public static bool MotorLoss1IsActive = false;
        public static bool MotorLoss2IsActive = false;
        public static bool MotorLoss3IsActive = false;
        public static bool MotorLoss4IsActive = false;
        public static bool MotorLoss5IsActive = false;
        public static bool MotorLoss6IsActive = false;
        public static bool MotorLoss7IsActive = false;
        public static bool MotorLoss8IsActive = false;
        public static bool MotorLoss9IsActive = false;
        public static bool MotorLoss9IsEnable = false;
        public static bool MotorLoss8IsEnable = false;
        public static bool MotorLoss7IsEnable = false;
        public static bool MotorLoss6IsEnable = false;
        public static bool MotorLoss5IsEnable = false;
        public static bool MotorLoss4IsEnable = false;
        public static bool MotorLoss3IsEnable = false;
        public static bool MotorLoss2IsEnable = false;
        public static bool MotorLoss1IsEnable = false;
        public static bool MotorLoss0IsEnable = false;
        #endregion 

        public void Init()
        {
            connectHelper.Init();
            Start();
        }
        public void Start()
        {
            connectThread = new Thread(DoSubscribe);
            connectThread.Start();
            thread_Time = new Timer(UpdateVariable, null, 10000, 10000);
        }
        public void Stop()
        {
            thread_Time.Dispose();
            if (m_NetVdnClient != null)
            {
                m_NetVdnClient.Disconnect();
            }
        }
        private void UpdateVariable(object state)
        {
            if (m_NetVdnClient != null)
            {
                m_NetVdnClient.State.ToString();
            }
        }
        public string GetConnectState()
        {
            if (PubVar.g_NetVdnClient != null)
            {
                return PubVar.g_NetVdnClient.State.ToString();
            }
            else
            {
                return "Unconnect";
            }
        }
        public bool SendRequset(SendRequest request)
        {
            try
            {
                Request myRequest = new Request(request.SendRequestName);
                myRequest.Topic = request.SendTopic;
                myRequest.Queue = request.SendQueue;
                foreach (object parameter in request.SendParameters)
                {
                    myRequest.Add(parameter);
                }
                if (PubVar.g_NetVdnClient != null)
                {
                    if (PubVar.g_NetVdnClient.State == HostConnectionState.Connected)
                    {
                        PubVar.g_NetVdnClient.SendRequest(myRequest);
                        return true;
                    }
                    else { return false; }
                }
                else { return false; }
            }
            catch { return false; }
        }

        public void DoSubscribe()
        {
            FullVdnName DestinationLat_name = new FullVdnName("ReLocate", "Destination_Reposition_Lat", "deg", VdnScope.Outputs, VdnType.Float64);
            VdnVariable DestinationLat_var = PubVar.g_NetVdnClient.Subscribe(DestinationLat_name);
            DestinationLat_var.ValueChanged += DestinationLat_var_ValueChanged;

            FullVdnName DestinationLon_name = new FullVdnName("ReLocate", "Destination_Reposition_Lon", "deg", VdnScope.Outputs, VdnType.Float64);
            VdnVariable DestinationLon_var = PubVar.g_NetVdnClient.Subscribe(DestinationLon_name);
            DestinationLon_var.ValueChanged += DestinationLon_var_ValueChanged;

            FullVdnName DestinationAlt_name = new FullVdnName("ReLocate", "Destination_Reposition_Alt", "ft", VdnScope.Meta, VdnType.Float64);
            VdnVariable DestinationAlt_var = PubVar.g_NetVdnClient.Subscribe(DestinationAlt_name);
            DestinationAlt_var.ValueChanged += DestinationAlt_var_ValueChanged;

            FullVdnName DestinationHeading_name = new FullVdnName("ReLocate", "Destination_Reposition_Heading", "deg", VdnScope.Outputs, VdnType.Float64);
            VdnVariable DestinationHeading_var = PubVar.g_NetVdnClient.Subscribe(DestinationHeading_name);
            DestinationHeading_var.ValueChanged += DestinationHeading_var_ValueChanged;

            #region Malfunction  
            FullVdnName CtrSurLock0IsActive_name = new FullVdnName("ControlSurface", "Malfunction.Lock_Malfunction_Active[0]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable CtrSurLock0IsActive_var = PubVar.g_NetVdnClient.Subscribe(CtrSurLock0IsActive_name);
            CtrSurLock0IsActive_var.ValueChanged += CtrSurLock0IsActive_var_ValueChanged;
            FullVdnName CtrSurLock0IsEnable_name = new FullVdnName("ControlSurface", "Malfunction.Lock_Malfunction_Enabled[0]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable CtrSurLock0IsEnable_var = PubVar.g_NetVdnClient.Subscribe(CtrSurLock0IsEnable_name);
            CtrSurLock0IsEnable_var.ValueChanged += CtrSurLock0IsEnable_var_ValueChanged;

            FullVdnName CtrSurLock1IsActive_name = new FullVdnName("ControlSurface", "Malfunction.Lock_Malfunction_Active[1]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable CtrSurLock1IsActive_var = PubVar.g_NetVdnClient.Subscribe(CtrSurLock1IsActive_name);
            CtrSurLock1IsActive_var.ValueChanged += CtrSurLock1IsActive_var_ValueChanged;
            FullVdnName CtrSurLock1IsEnable_name = new FullVdnName("ControlSurface", "Malfunction.Lock_Malfunction_Enabled[1]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable CtrSurLock1IsEnable_var = PubVar.g_NetVdnClient.Subscribe(CtrSurLock1IsEnable_name);
            CtrSurLock1IsEnable_var.ValueChanged += CtrSurLock1IsEnable_var_ValueChanged;

            FullVdnName CtrSurLock2IsActive_name = new FullVdnName("ControlSurface", "Malfunction.Lock_Malfunction_Active[2]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable CtrSurLock2IsActive_var = PubVar.g_NetVdnClient.Subscribe(CtrSurLock2IsActive_name);
            CtrSurLock2IsActive_var.ValueChanged += CtrSurLock2IsActive_var_ValueChanged;
            FullVdnName CtrSurLock2IsEnable_name = new FullVdnName("ControlSurface", "Malfunction.Lock_Malfunction_Enabled[2]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable CtrSurLock2IsEnable_var = PubVar.g_NetVdnClient.Subscribe(CtrSurLock2IsEnable_name);
            CtrSurLock2IsEnable_var.ValueChanged += CtrSurLock2IsEnable_var_ValueChanged;

            FullVdnName CtrSurLock3IsActive_name = new FullVdnName("ControlSurface", "Malfunction.Lock_Malfunction_Active[3]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable CtrSurLock3IsActive_var = PubVar.g_NetVdnClient.Subscribe(CtrSurLock3IsActive_name);
            CtrSurLock3IsActive_var.ValueChanged += CtrSurLock3IsActive_var_ValueChanged;
            FullVdnName CtrSurLock3IsEnable_name = new FullVdnName("ControlSurface", "Malfunction.Lock_Malfunction_Enabled[3]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable CtrSurLock3IsEnable_var = PubVar.g_NetVdnClient.Subscribe(CtrSurLock3IsEnable_name);
            CtrSurLock3IsEnable_var.ValueChanged += CtrSurLock3IsEnable_var_ValueChanged;

            FullVdnName CtrSurLock4IsActive_name = new FullVdnName("ControlSurface", "Malfunction.Lock_Malfunction_Active[4]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable CtrSurLock4IsActive_var = PubVar.g_NetVdnClient.Subscribe(CtrSurLock4IsActive_name);
            CtrSurLock4IsActive_var.ValueChanged += CtrSurLock4IsActive_var_ValueChanged;
            FullVdnName CtrSurLock4IsEnable_name = new FullVdnName("ControlSurface", "Malfunction.Lock_Malfunction_Enabled[4]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable CtrSurLock4IsEnable_var = PubVar.g_NetVdnClient.Subscribe(CtrSurLock4IsEnable_name);
            CtrSurLock4IsEnable_var.ValueChanged += CtrSurLock4IsEnable_var_ValueChanged;

            FullVdnName CtrSurLock5IsActive_name = new FullVdnName("ControlSurface", "Malfunction.Lock_Malfunction_Active[5]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable CtrSurLock5IsActive_var = PubVar.g_NetVdnClient.Subscribe(CtrSurLock5IsActive_name);
            CtrSurLock5IsActive_var.ValueChanged += CtrSurLock5IsActive_var_ValueChanged;
            FullVdnName CtrSurLock5IsEnable_name = new FullVdnName("ControlSurface", "Malfunction.Lock_Malfunction_Enabled[5]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable CtrSurLock5IsEnable_var = PubVar.g_NetVdnClient.Subscribe(CtrSurLock5IsEnable_name);
            CtrSurLock5IsEnable_var.ValueChanged += CtrSurLock5IsEnable_var_ValueChanged;

            FullVdnName CtrSurLock6IsActive_name = new FullVdnName("ControlSurface", "Malfunction.Lock_Malfunction_Active[6]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable CtrSurLock6IsActive_var = PubVar.g_NetVdnClient.Subscribe(CtrSurLock6IsActive_name);
            CtrSurLock6IsActive_var.ValueChanged += CtrSurLock6IsActive_var_ValueChanged;
            FullVdnName CtrSurLock6IsEnable_name = new FullVdnName("ControlSurface", "Malfunction.Lock_Malfunction_Enabled[6]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable CtrSurLock6IsEnable_var = PubVar.g_NetVdnClient.Subscribe(CtrSurLock6IsEnable_name);
            CtrSurLock6IsEnable_var.ValueChanged += CtrSurLock6IsEnable_var_ValueChanged;

            FullVdnName CtrSurLock7IsActive_name = new FullVdnName("ControlSurface", "Malfunction.Lock_Malfunction_Active[7]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable CtrSurLock7IsActive_var = PubVar.g_NetVdnClient.Subscribe(CtrSurLock7IsActive_name);
            CtrSurLock7IsActive_var.ValueChanged += CtrSurLock7IsActive_var_ValueChanged;
            FullVdnName CtrSurLock7IsEnable_name = new FullVdnName("ControlSurface", "Malfunction.Lock_Malfunction_Enabled[7]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable CtrSurLock7IsEnable_var = PubVar.g_NetVdnClient.Subscribe(CtrSurLock7IsEnable_name);
            CtrSurLock7IsEnable_var.ValueChanged += CtrSurLock7IsEnable_var_ValueChanged;

            FullVdnName CtrSurLock8IsActive_name = new FullVdnName("ControlSurface", "Malfunction.Lock_Malfunction_Active[8]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable CtrSurLock8IsActive_var = PubVar.g_NetVdnClient.Subscribe(CtrSurLock8IsActive_name);
            CtrSurLock8IsActive_var.ValueChanged += CtrSurLock8IsActive_var_ValueChanged;
            FullVdnName CtrSurLock8IsEnable_name = new FullVdnName("ControlSurface", "Malfunction.Lock_Malfunction_Enabled[8]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable CtrSurLock8IsEnable_var = PubVar.g_NetVdnClient.Subscribe(CtrSurLock8IsEnable_name);
            CtrSurLock8IsEnable_var.ValueChanged += CtrSurLock8IsEnable_var_ValueChanged;

            FullVdnName CtrSurLock9IsActive_name = new FullVdnName("ControlSurface", "Malfunction.Lock_Malfunction_Active[9]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable CtrSurLock9IsActive_var = PubVar.g_NetVdnClient.Subscribe(CtrSurLock9IsActive_name);
            CtrSurLock9IsActive_var.ValueChanged += CtrSurLock9IsActive_var_ValueChanged;
            FullVdnName CtrSurLock9IsEnable_name = new FullVdnName("ControlSurface", "Malfunction.Lock_Malfunction_Enabled[9]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable CtrSurLock9IsEnable_var = PubVar.g_NetVdnClient.Subscribe(CtrSurLock9IsEnable_name);
            CtrSurLock9IsEnable_var.ValueChanged += CtrSurLock9IsEnable_var_ValueChanged;

            FullVdnName CtrSurLoss0IsActive_name = new FullVdnName("ControlSurface", "Malfunction.LossEffectiveness_Malfunction_Active[0]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable CtrSurLoss0IsActive_var = PubVar.g_NetVdnClient.Subscribe(CtrSurLoss0IsActive_name);
            CtrSurLoss0IsActive_var.ValueChanged += CtrSurLoss0IsActive_var_ValueChanged;
            FullVdnName CtrSurLoss0IsEnable_name = new FullVdnName("ControlSurface", "Malfunction.LossEffectiveness_Malfunction_Enabled[0]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable CtrSurLoss0IsEnable_var = PubVar.g_NetVdnClient.Subscribe(CtrSurLoss0IsEnable_name);
            CtrSurLoss0IsEnable_var.ValueChanged += CtrSurLoss0IsEnable_var_ValueChanged;

            FullVdnName CtrSurLoss1IsActive_name = new FullVdnName("ControlSurface", "Malfunction.LossEffectiveness_Malfunction_Active[1]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable CtrSurLoss1IsActive_var = PubVar.g_NetVdnClient.Subscribe(CtrSurLoss1IsActive_name);
            CtrSurLoss1IsActive_var.ValueChanged += CtrSurLoss1IsActive_var_ValueChanged;
            FullVdnName CtrSurLoss1IsEnable_name = new FullVdnName("ControlSurface", "Malfunction.LossEffectiveness_Malfunction_Enabled[1]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable CtrSurLoss1IsEnable_var = PubVar.g_NetVdnClient.Subscribe(CtrSurLoss1IsEnable_name);
            CtrSurLoss1IsEnable_var.ValueChanged += CtrSurLoss1IsEnable_var_ValueChanged;

            FullVdnName CtrSurLoss2IsActive_name = new FullVdnName("ControlSurface", "Malfunction.LossEffectiveness_Malfunction_Active[2]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable CtrSurLoss2IsActive_var = PubVar.g_NetVdnClient.Subscribe(CtrSurLoss2IsActive_name);
            CtrSurLoss2IsActive_var.ValueChanged += CtrSurLoss2IsActive_var_ValueChanged;
            FullVdnName CtrSurLoss2IsEnable_name = new FullVdnName("ControlSurface", "Malfunction.LossEffectiveness_Malfunction_Enabled[2]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable CtrSurLoss2IsEnable_var = PubVar.g_NetVdnClient.Subscribe(CtrSurLoss2IsEnable_name);
            CtrSurLoss2IsEnable_var.ValueChanged += CtrSurLoss2IsEnable_var_ValueChanged;

            FullVdnName CtrSurLoss3IsActive_name = new FullVdnName("ControlSurface", "Malfunction.LossEffectiveness_Malfunction_Active[3]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable CtrSurLoss3IsActive_var = PubVar.g_NetVdnClient.Subscribe(CtrSurLoss3IsActive_name);
            CtrSurLoss3IsActive_var.ValueChanged += CtrSurLoss3IsActive_var_ValueChanged;
            FullVdnName CtrSurLoss3IsEnable_name = new FullVdnName("ControlSurface", "Malfunction.LossEffectiveness_Malfunction_Enabled[3]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable CtrSurLoss3IsEnable_var = PubVar.g_NetVdnClient.Subscribe(CtrSurLoss3IsEnable_name);
            CtrSurLoss3IsEnable_var.ValueChanged += CtrSurLoss3IsEnable_var_ValueChanged;

            FullVdnName CtrSurLoss4IsActive_name = new FullVdnName("ControlSurface", "Malfunction.LossEffectiveness_Malfunction_Active[4]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable CtrSurLoss4IsActive_var = PubVar.g_NetVdnClient.Subscribe(CtrSurLoss4IsActive_name);
            CtrSurLoss4IsActive_var.ValueChanged += CtrSurLoss4IsActive_var_ValueChanged;
            FullVdnName CtrSurLoss4IsEnable_name = new FullVdnName("ControlSurface", "Malfunction.LossEffectiveness_Malfunction_Enabled[4]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable CtrSurLoss4IsEnable_var = PubVar.g_NetVdnClient.Subscribe(CtrSurLoss4IsEnable_name);
            CtrSurLoss4IsEnable_var.ValueChanged += CtrSurLoss4IsEnable_var_ValueChanged;

            FullVdnName CtrSurLoss5IsActive_name = new FullVdnName("ControlSurface", "Malfunction.LossEffectiveness_Malfunction_Active[5]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable CtrSurLoss5IsActive_var = PubVar.g_NetVdnClient.Subscribe(CtrSurLoss5IsActive_name);
            CtrSurLoss5IsActive_var.ValueChanged += CtrSurLoss5IsActive_var_ValueChanged;
            FullVdnName CtrSurLoss5IsEnable_name = new FullVdnName("ControlSurface", "Malfunction.LossEffectiveness_Malfunction_Enabled[5]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable CtrSurLoss5IsEnable_var = PubVar.g_NetVdnClient.Subscribe(CtrSurLoss5IsEnable_name);
            CtrSurLoss5IsEnable_var.ValueChanged += CtrSurLoss5IsEnable_var_ValueChanged;

            FullVdnName CtrSurLoss6IsActive_name = new FullVdnName("ControlSurface", "Malfunction.LossEffectiveness_Malfunction_Active[6]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable CtrSurLoss6IsActive_var = PubVar.g_NetVdnClient.Subscribe(CtrSurLoss6IsActive_name);
            CtrSurLoss6IsActive_var.ValueChanged += CtrSurLoss6IsActive_var_ValueChanged;
            FullVdnName CtrSurLoss6IsEnable_name = new FullVdnName("ControlSurface", "Malfunction.LossEffectiveness_Malfunction_Enabled[6]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable CtrSurLoss6IsEnable_var = PubVar.g_NetVdnClient.Subscribe(CtrSurLoss6IsEnable_name);
            CtrSurLoss6IsEnable_var.ValueChanged += CtrSurLoss6IsEnable_var_ValueChanged;

            FullVdnName CtrSurLoss7IsActive_name = new FullVdnName("ControlSurface", "Malfunction.LossEffectiveness_Malfunction_Active[7]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable CtrSurLoss7IsActive_var = PubVar.g_NetVdnClient.Subscribe(CtrSurLoss7IsActive_name);
            CtrSurLoss7IsActive_var.ValueChanged += CtrSurLoss7IsActive_var_ValueChanged;
            FullVdnName CtrSurLoss7IsEnable_name = new FullVdnName("ControlSurface", "Malfunction.LossEffectiveness_Malfunction_Enabled[7]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable CtrSurLoss7IsEnable_var = PubVar.g_NetVdnClient.Subscribe(CtrSurLoss7IsEnable_name);
            CtrSurLoss7IsEnable_var.ValueChanged += CtrSurLoss7IsEnable_var_ValueChanged;

            FullVdnName CtrSurLoss8IsActive_name = new FullVdnName("ControlSurface", "Malfunction.LossEffectiveness_Malfunction_Active[8]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable CtrSurLoss8IsActive_var = PubVar.g_NetVdnClient.Subscribe(CtrSurLoss8IsActive_name);
            CtrSurLoss8IsActive_var.ValueChanged += CtrSurLoss8IsActive_var_ValueChanged;
            FullVdnName CtrSurLoss8IsEnable_name = new FullVdnName("ControlSurface", "Malfunction.LossEffectiveness_Malfunction_Enabled[8]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable CtrSurLoss8IsEnable_var = PubVar.g_NetVdnClient.Subscribe(CtrSurLoss8IsEnable_name);
            CtrSurLoss8IsEnable_var.ValueChanged += CtrSurLoss8IsEnable_var_ValueChanged;

            FullVdnName CtrSurLoss9IsActive_name = new FullVdnName("ControlSurface", "Malfunction.LossEffectiveness_Malfunction_Active[9]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable CtrSurLoss9IsActive_var = PubVar.g_NetVdnClient.Subscribe(CtrSurLoss9IsActive_name);
            CtrSurLoss9IsActive_var.ValueChanged += CtrSurLoss9IsActive_var_ValueChanged;
            FullVdnName CtrSurLoss9IsEnable_name = new FullVdnName("ControlSurface", "Malfunction.LossEffectiveness_Malfunction_Enabled[9]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable CtrSurLoss9IsEnable_var = PubVar.g_NetVdnClient.Subscribe(CtrSurLoss9IsEnable_name);
            CtrSurLoss9IsEnable_var.ValueChanged += CtrSurLoss9IsEnable_var_ValueChanged;

            FullVdnName MotorLoss9IsActive_name = new FullVdnName("Engines", "Motor.Malfunction.Motor_LossEffectiveness_Malfunction_Active[9]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable MotorLoss9IsActive_var = PubVar.g_NetVdnClient.Subscribe(MotorLoss9IsActive_name);
            MotorLoss9IsActive_var.ValueChanged += MotorLoss9IsActive_var_ValueChanged;
            FullVdnName MotorLoss9IsEnable_name = new FullVdnName("Engines", "Motor.Malfunction.Motor_LossEffectiveness_Malfunction_Enabled[9]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable MotorLoss9IsEnable_var = PubVar.g_NetVdnClient.Subscribe(MotorLoss9IsEnable_name);
            MotorLoss9IsEnable_var.ValueChanged += MotorLoss9IsEnable_var_ValueChanged;

            FullVdnName MotorLoss8IsActive_name = new FullVdnName("Engines", "Motor.Malfunction.Motor_LossEffectiveness_Malfunction_Active[8]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable MotorLoss8IsActive_var = PubVar.g_NetVdnClient.Subscribe(MotorLoss8IsActive_name);
            MotorLoss8IsActive_var.ValueChanged += MotorLoss8IsActive_var_ValueChanged;
            FullVdnName MotorLoss8IsEnable_name = new FullVdnName("Engines", "Motor.Malfunction.Motor_LossEffectiveness_Malfunction_Enabled[8]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable MotorLoss8IsEnable_var = PubVar.g_NetVdnClient.Subscribe(MotorLoss8IsEnable_name);
            MotorLoss8IsEnable_var.ValueChanged += MotorLoss8IsEnable_var_ValueChanged;

            FullVdnName MotorLoss7IsActive_name = new FullVdnName("Engines", "Motor.Malfunction.Motor_LossEffectiveness_Malfunction_Active[7]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable MotorLoss7IsActive_var = PubVar.g_NetVdnClient.Subscribe(MotorLoss7IsActive_name);
            MotorLoss7IsActive_var.ValueChanged += MotorLoss7IsActive_var_ValueChanged;
            FullVdnName MotorLoss7IsEnable_name = new FullVdnName("Engines", "Motor.Malfunction.Motor_LossEffectiveness_Malfunction_Enabled[7]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable MotorLoss7IsEnable_var = PubVar.g_NetVdnClient.Subscribe(MotorLoss7IsEnable_name);
            MotorLoss7IsEnable_var.ValueChanged += MotorLoss7IsEnable_var_ValueChanged;

            FullVdnName MotorLoss6IsActive_name = new FullVdnName("Engines", "Motor.Malfunction.Motor_LossEffectiveness_Malfunction_Active[6]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable MotorLoss6IsActive_var = PubVar.g_NetVdnClient.Subscribe(MotorLoss6IsActive_name);
            MotorLoss6IsActive_var.ValueChanged += MotorLoss6IsActive_var_ValueChanged;
            FullVdnName MotorLoss6IsEnable_name = new FullVdnName("Engines", "Motor.Malfunction.Motor_LossEffectiveness_Malfunction_Enabled[6]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable MotorLoss6IsEnable_var = PubVar.g_NetVdnClient.Subscribe(MotorLoss6IsEnable_name);
            MotorLoss6IsEnable_var.ValueChanged += MotorLoss6IsEnable_var_ValueChanged;

            FullVdnName MotorLoss5IsActive_name = new FullVdnName("Engines", "Motor.Malfunction.Motor_LossEffectiveness_Malfunction_Active[5]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable MotorLoss5IsActive_var = PubVar.g_NetVdnClient.Subscribe(MotorLoss5IsActive_name);
            MotorLoss5IsActive_var.ValueChanged += MotorLoss5IsActive_var_ValueChanged;
            FullVdnName MotorLoss5IsEnable_name = new FullVdnName("Engines", "Motor.Malfunction.Motor_LossEffectiveness_Malfunction_Enabled[5]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable MotorLoss5IsEnable_var = PubVar.g_NetVdnClient.Subscribe(MotorLoss5IsEnable_name);
            MotorLoss5IsEnable_var.ValueChanged += MotorLoss5IsEnable_var_ValueChanged;

            FullVdnName MotorLoss4IsActive_name = new FullVdnName("Engines", "Motor.Malfunction.Motor_LossEffectiveness_Malfunction_Active[4]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable MotorLoss4IsActive_var = PubVar.g_NetVdnClient.Subscribe(MotorLoss4IsActive_name);
            MotorLoss4IsActive_var.ValueChanged += MotorLoss4IsActive_var_ValueChanged;
            FullVdnName MotorLoss4IsEnable_name = new FullVdnName("Engines", "Motor.Malfunction.Motor_LossEffectiveness_Malfunction_Enabled[4]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable MotorLoss4IsEnable_var = PubVar.g_NetVdnClient.Subscribe(MotorLoss4IsEnable_name);
            MotorLoss4IsEnable_var.ValueChanged += MotorLoss4IsEnable_var_ValueChanged;

            FullVdnName MotorLoss3IsActive_name = new FullVdnName("Engines", "Motor.Malfunction.Motor_LossEffectiveness_Malfunction_Active[3]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable MotorLoss3IsActive_var = PubVar.g_NetVdnClient.Subscribe(MotorLoss3IsActive_name);
            MotorLoss3IsActive_var.ValueChanged += MotorLoss3IsActive_var_ValueChanged;
            FullVdnName MotorLoss3IsEnable_name = new FullVdnName("Engines", "Motor.Malfunction.Motor_LossEffectiveness_Malfunction_Enabled[3]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable MotorLoss3IsEnable_var = PubVar.g_NetVdnClient.Subscribe(MotorLoss3IsEnable_name);
            MotorLoss3IsEnable_var.ValueChanged += MotorLoss3IsEnable_var_ValueChanged;

            FullVdnName MotorLoss2IsActive_name = new FullVdnName("Engines", "Motor.Malfunction.Motor_LossEffectiveness_Malfunction_Active[2]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable MotorLoss2IsActive_var = PubVar.g_NetVdnClient.Subscribe(MotorLoss2IsActive_name);
            MotorLoss2IsActive_var.ValueChanged += MotorLoss2IsActive_var_ValueChanged;
            FullVdnName MotorLoss2IsEnable_name = new FullVdnName("Engines", "Motor.Malfunction.Motor_LossEffectiveness_Malfunction_Enabled[2]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable MotorLoss2IsEnable_var = PubVar.g_NetVdnClient.Subscribe(MotorLoss2IsEnable_name);
            MotorLoss2IsEnable_var.ValueChanged += MotorLoss2IsEnable_var_ValueChanged;

            FullVdnName MotorLoss1IsActive_name = new FullVdnName("Engines", "Motor.Malfunction.Motor_LossEffectiveness_Malfunction_Active[1]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable MotorLoss1IsActive_var = PubVar.g_NetVdnClient.Subscribe(MotorLoss1IsActive_name);
            MotorLoss1IsActive_var.ValueChanged += MotorLoss1IsActive_var_ValueChanged;
            FullVdnName MotorLoss1IsEnable_name = new FullVdnName("Engines", "Motor.Malfunction.Motor_LossEffectiveness_Malfunction_Enabled[1]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable MotorLoss1IsEnable_var = PubVar.g_NetVdnClient.Subscribe(MotorLoss1IsEnable_name);
            MotorLoss1IsEnable_var.ValueChanged += MotorLoss1IsEnable_var_ValueChanged;

            FullVdnName MotorLoss0IsActive_name = new FullVdnName("Engines", "Motor.Malfunction.Motor_LossEffectiveness_Malfunction_Active[0]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable MotorLoss0IsActive_var = PubVar.g_NetVdnClient.Subscribe(MotorLoss0IsActive_name);
            MotorLoss0IsActive_var.ValueChanged += MotorLoss0IsActive_var_ValueChanged;
            FullVdnName MotorLoss0IsEnable_name = new FullVdnName("Engines", "Motor.Malfunction.Motor_LossEffectiveness_Malfunction_Enabled[0]", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable MotorLoss0IsEnable_var = PubVar.g_NetVdnClient.Subscribe(MotorLoss0IsEnable_name);
            MotorLoss0IsEnable_var.ValueChanged += MotorLoss0IsEnable_var_ValueChanged;
            #endregion

            #region 主页面控制按钮状态
            FullVdnName Motion = new FullVdnName("Motion", "requestTest", "none", VdnScope.Outputs, VdnType.Int64);
            VdnVariable Motion_var = PubVar.g_NetVdnClient.Subscribe(Motion);
            Motion_var.ValueChanged += MotionSwitch_var_ValueChanged;

            FullVdnName CurrentView = new FullVdnName("Visual", "Current_View", "N/A", VdnScope.Outputs, VdnType.Text);
            VdnVariable CurrentView_var = PubVar.g_NetVdnClient.Subscribe(CurrentView);
            CurrentView_var.ValueChanged += CurrentView_var_ValueChanged;

            FullVdnName APState_name = new FullVdnName("FlightControlWrapper", "Current_AP_State", "", VdnScope.Outputs, VdnType.Bool);
            VdnVariable APState_var = PubVar.g_NetVdnClient.Subscribe(APState_name);
            APState_var.ValueChanged += APState_var_ValueChanged;

            FullVdnName FlightPlanGenerated_name = new FullVdnName("Fms", "FlightPlan_Generated", "n/a", VdnScope.Outputs, VdnType.Bool);
            VdnVariable FlightPlanGenerated_var = PubVar.g_NetVdnClient.Subscribe(FlightPlanGenerated_name);
            FlightPlanGenerated_var.ValueChanged += FlightPlanGenerated_var_ValueChanged;

            FullVdnName MotionSwitch = new FullVdnName("Motion", "requestTest", "none", VdnScope.Outputs, VdnType.Int64);
            VdnVariable MotionSwitch_var = PubVar.g_NetVdnClient.Subscribe(MotionSwitch);
            MotionSwitch_var.ValueChanged += MotionSwitch_var_ValueChanged;

            FullVdnName BJKernelModeString_name = new FullVdnName("BJInterface", "FreezeStatus_TotalFreeze", "", VdnScope.Outputs, VdnType.Bool);
            VdnVariable BJKernelModeString_var = PubVar.g_NetVdnClient.Subscribe(BJKernelModeString_name);
            BJKernelModeString_var.ValueChanged += BJKernelModeString_var_ValueChanged;

            FullVdnName KernelModeString_name = new FullVdnName("Kernel", "Mode_String", "String", VdnScope.Meta, VdnType.Text);
            VdnVariable KernelModeString_var = PubVar.g_NetVdnClient.Subscribe(KernelModeString_name);
            KernelModeString_var.ValueChanged += KernelModeString_var_ValueChanged;

            FullVdnName MotionFreeze = new FullVdnName("Motion", "Motion_Freeze", "Freeze", VdnScope.Outputs, VdnType.Bool);
            VdnVariable MotionFreeze_var = PubVar.g_NetVdnClient.Subscribe(MotionFreeze);
            MotionFreeze_var.ValueChanged += MotionFreeze_var_ValueChanged;

            FullVdnName FlightFreeze_name = new FullVdnName("EquationsOfMotion", "Flight_Freeze", "", VdnScope.Outputs, VdnType.Bool);
            VdnVariable FlightFreeze_var = PubVar.g_NetVdnClient.Subscribe(FlightFreeze_name);
            FlightFreeze_var.ValueChanged += FlightFreeze_var_ValueChanged;

            FullVdnName CrashDetection_name = new FullVdnName("Crash", "Crash_Override", "", VdnScope.Outputs, VdnType.Bool);
            VdnVariable CrashDetection_var = PubVar.g_NetVdnClient.Subscribe(CrashDetection_name);
            CrashDetection_var.ValueChanged += CrashDetection_var_ValueChanged;

            FullVdnName BJCrashDetection_name = new FullVdnName("BJInterface", "CrashOverrideResponse_CrashOverrideCommandAccept", "N/A", VdnScope.Outputs, VdnType.Bool);
            VdnVariable BJCrashDetection_var = PubVar.g_NetVdnClient.Subscribe(BJCrashDetection_name);
            BJCrashDetection_var.ValueChanged += BJCrashDetection_var_ValueChanged;

            FullVdnName BJClearCrash_name = new FullVdnName("BJInterface", "ClearCrashResponse_ClearCrashCommandAccept", "N/A", VdnScope.Outputs, VdnType.Bool);
            VdnVariable BJClearCrash_var = PubVar.g_NetVdnClient.Subscribe(BJClearCrash_name);
            BJClearCrash_var.ValueChanged += BJClearCrash_var_ValueChanged;

            FullVdnName PowerOn_name = new FullVdnName("ElectricPower", "Battery_Bus", "", VdnScope.Outputs, VdnType.Bool);
            VdnVariable PowerOn_var = PubVar.g_NetVdnClient.Subscribe(PowerOn_name);
            PowerOn_var.ValueChanged += PowerOn_var_ValueChanged;

            FullVdnName PowerFreeze_name = new FullVdnName("ElectricPower", "Battery_Freeze", "", VdnScope.Outputs, VdnType.Bool);
            VdnVariable PowerFreeze_var = PubVar.g_NetVdnClient.Subscribe(PowerFreeze_name);
            PowerFreeze_var.ValueChanged += PowerFreeze_var_ValueChanged;

            FullVdnName EngineOn_name = new FullVdnName("ElectricPower", "Propulsion_On_PushBotton", "", VdnScope.Outputs, VdnType.Bool);
            VdnVariable EngineOn_var = PubVar.g_NetVdnClient.Subscribe(EngineOn_name);
            EngineOn_var.ValueChanged += EngineOn_var_ValueChanged;

            FullVdnName VisualConnect_name = new FullVdnName("Visual", "Visual_On", "", VdnScope.Outputs, VdnType.Bool);
            VdnVariable VisualConnect_var = PubVar.g_NetVdnClient.Subscribe(VisualConnect_name);
            VisualConnect_var.ValueChanged += VisualConnect_var_ValueChanged;

            FullVdnName LocationFreeze_name = new FullVdnName("AircraftLocation", "Position_Freeze", "", VdnScope.Outputs, VdnType.Bool);
            VdnVariable LocationFreeze_var = PubVar.g_NetVdnClient.Subscribe(LocationFreeze_name);
            LocationFreeze_var.ValueChanged += LocationFreeze_var_ValueChanged;

            FullVdnName HighlyFreeze_name = new FullVdnName("AircraftLocation", "Altitude_Freeze", "", VdnScope.Outputs, VdnType.Bool);
            VdnVariable HighlyFreeze_var = PubVar.g_NetVdnClient.Subscribe(HighlyFreeze_name);
            HighlyFreeze_var.ValueChanged += HighlyFreeze_var_ValueChanged;

            #endregion

            #region Environment 
            // Visual_Weather天气具体情况
            FullVdnName Weather_name = new FullVdnName("Visual", "Weather.Current_Weather", "N/A", VdnScope.Outputs, VdnType.Text);
            VdnVariable Weather_var = PubVar.g_NetVdnClient.Subscribe(Weather_name);
            Weather_var.ValueChanged += Weather_var_ValueChanged;
            // 风
            FullVdnName GroundWindDirection_name = new FullVdnName("Wind", "Surface_Winds_Direction", "deg", VdnScope.Outputs, VdnType.Float64);
            VdnVariable GroundWindDirection_var = PubVar.g_NetVdnClient.Subscribe(GroundWindDirection_name);
            GroundWindDirection_var.ValueChanged += GroundWindDirection_var_ValueChanged;
            FullVdnName GroundWindVelocity_name = new FullVdnName("Wind", "Surface_Winds_Speed", "kt", VdnScope.Outputs, VdnType.Float64);
            VdnVariable GroundWindVelocity_var = PubVar.g_NetVdnClient.Subscribe(GroundWindVelocity_name);
            GroundWindVelocity_var.ValueChanged += GroundWindVelocity_var_ValueChanged;
            FullVdnName HollowDirection_name = new FullVdnName("Wind", "Intermediate_Winds_Direction", "deg", VdnScope.Outputs, VdnType.Float64);
            VdnVariable HollowDirection_var = PubVar.g_NetVdnClient.Subscribe(HollowDirection_name);
            HollowDirection_var.ValueChanged += HollowDirection_var_ValueChanged;
            FullVdnName HollowVelocity_name = new FullVdnName("Wind", "Intermediate_Winds_Speed", "kt", VdnScope.Outputs, VdnType.Float64);
            VdnVariable HollowVelocity_var = PubVar.g_NetVdnClient.Subscribe(HollowVelocity_name);
            HollowVelocity_var.ValueChanged += HollowVelocity_var_ValueChanged;
            FullVdnName GustSpeed_name = new FullVdnName("Wind", "Change_Wind_Gusts", "kt", VdnScope.Outputs, VdnType.Float64);
            VdnVariable GustSpeed_var = PubVar.g_NetVdnClient.Subscribe(GustSpeed_name);
            GustSpeed_var.ValueChanged += GustSpeed_var_ValueChanged;
            // Visual_CloudType 云类型
            FullVdnName Cloud_Type = new FullVdnName("Visual", "Weather.CloudType", "N/A", VdnScope.Outputs, VdnType.Int64);
            VdnVariable Cloud_var = PubVar.g_NetVdnClient.Subscribe(Cloud_Type);
            Cloud_var.ValueChanged += CloudyType_var_ValueChanged;
            // Visual_Coverage 云的覆盖率
            FullVdnName Coverage = new FullVdnName("Visual", "Weather.Coverage", "%", VdnScope.Outputs, VdnType.Float64);
            VdnVariable Coverage_var = PubVar.g_NetVdnClient.Subscribe(Coverage);
            Coverage_var.ValueChanged += Coverage_var_ValueChanged;
            // Visual_Season当前季节
            FullVdnName Season = new FullVdnName("Visual", "Weather.Current_Season", "N/A", VdnScope.Outputs, VdnType.Text);
            VdnVariable Season_var = PubVar.g_NetVdnClient.Subscribe(Season);
            Season_var.ValueChanged += Season_var_ValueChanged;
            // Visual_TimeOfDay时间
            FullVdnName TimeOfDay = new FullVdnName("Visual", "Weather.Current_TimeOfDay", "N/A", VdnScope.Outputs, VdnType.Text);
            VdnVariable TimeOfDay_var = PubVar.g_NetVdnClient.Subscribe(TimeOfDay);
            TimeOfDay_var.ValueChanged += TimeOfDay_var_ValueChanged;
            // Visual_LayerID 降水等级
            FullVdnName LayerID = new FullVdnName("Visual", "Weather.LayerID", "N/A", VdnScope.Outputs, VdnType.Int64);
            VdnVariable LayerID_var = PubVar.g_NetVdnClient.Subscribe(LayerID);
            LayerID_var.ValueChanged += LayerID_var_ValueChanged;
            // Visual_Severity 降水严重程度
            FullVdnName Severity = new FullVdnName("Visual", "Weather.Severity", "N/A", VdnScope.Outputs, VdnType.Int64);
            VdnVariable Severity_var = PubVar.g_NetVdnClient.Subscribe(Severity);
            Severity_var.ValueChanged += Severity_var_ValueChanged;
            // Visual_RandomLightningEnable 闪电
            FullVdnName RandomLightningEnable = new FullVdnName("Visual", "Weather.RandomLightningEnable", "N/A", VdnScope.Outputs, VdnType.Bool);
            VdnVariable RandomLightningEnable_var = PubVar.g_NetVdnClient.Subscribe(RandomLightningEnable);
            RandomLightningEnable_var.ValueChanged += RandomLightningEnable_var_ValueChanged;
            // Visual_Thickness 厚度
            FullVdnName Thickness = new FullVdnName("Visual", "Weather.Thickness", "ft", VdnScope.Outputs, VdnType.Float64);
            VdnVariable Thickness_var = PubVar.g_NetVdnClient.Subscribe(Thickness);
            Thickness_var.ValueChanged += Thickness_var_ValueChanged;
            // Visual_TransitionBand过渡带
            FullVdnName TransitionBand = new FullVdnName("Visual", "Weather.TransitionBand", "ft", VdnScope.Outputs, VdnType.Float64);
            VdnVariable TransitionBand_var = PubVar.g_NetVdnClient.Subscribe(TransitionBand);
            TransitionBand_var.ValueChanged += TransitionBand_var_ValueChanged;
            // Visual_Visibility能见度
            FullVdnName Visibility = new FullVdnName("Visual", "Weather.Visibility", "m", VdnScope.Outputs, VdnType.Float64);
            VdnVariable Visibility_var = PubVar.g_NetVdnClient.Subscribe(Visibility);
            Visibility_var.ValueChanged += Visibility_var_ValueChanged;
            #endregion

            #region 经纬高 roll pitch heading
            FullVdnName Latitude_name = new FullVdnName("AircraftLocation", "Latitude", "deg", VdnScope.Outputs, VdnType.Float64);
            VdnVariable Latitude_var = PubVar.g_NetVdnClient.Subscribe(Latitude_name);
            Latitude_var.ValueChanged += Latitude_var_ValueChanged;

            FullVdnName Longitude_name = new FullVdnName("AircraftLocation", "Longitude", "deg", VdnScope.Outputs, VdnType.Float64);
            VdnVariable Longitude_var = PubVar.g_NetVdnClient.Subscribe(Longitude_name);
            Longitude_var.ValueChanged += Longitude_var_ValueChanged;

            FullVdnName AltitudeMsl_name = new FullVdnName("AircraftLocation", "Altitude_Msl", "ft", VdnScope.Outputs, VdnType.Float64);
            VdnVariable AltitudeMsl_var = PubVar.g_NetVdnClient.Subscribe(AltitudeMsl_name);
            AltitudeMsl_var.ValueChanged += AltitudeMsl_var_ValueChanged;

            FullVdnName Roll_name = new FullVdnName("EquationsOfMotion", "Roll_Attitude", "deg", VdnScope.Outputs, VdnType.Float64);
            VdnVariable Roll_var = PubVar.g_NetVdnClient.Subscribe(Roll_name);
            Roll_var.ValueChanged += Roll_var_ValueChanged;

            FullVdnName Pitch_name = new FullVdnName("EquationsOfMotion", "Pitch_Attitude", "deg", VdnScope.Outputs, VdnType.Float64);
            VdnVariable Pitch_var = PubVar.g_NetVdnClient.Subscribe(Pitch_name);
            Pitch_var.ValueChanged += Pitch_var_ValueChanged;

            FullVdnName Heading_name = new FullVdnName("EquationsOfMotion", "True_Heading", "deg", VdnScope.Outputs, VdnType.Float64);
            VdnVariable Heading_var = PubVar.g_NetVdnClient.Subscribe(Heading_name);
            Heading_var.ValueChanged += Heading_var_ValueChanged;
            #endregion
        }

        #region 值改变事件
        private void MotorLoss0IsActive_var_ValueChanged(object sender, object lastValue)
        {
            MotorLoss0IsActive = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void MotorLoss1IsActive_var_ValueChanged(object sender, object lastValue)
        {
            MotorLoss1IsActive = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void MotorLoss2IsActive_var_ValueChanged(object sender, object lastValue)
        {
            MotorLoss2IsActive = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void MotorLoss3IsActive_var_ValueChanged(object sender, object lastValue)
        {
            MotorLoss3IsActive = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void MotorLoss4IsActive_var_ValueChanged(object sender, object lastValue)
        {
            MotorLoss4IsActive = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void MotorLoss5IsActive_var_ValueChanged(object sender, object lastValue)
        {
            MotorLoss5IsActive = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void MotorLoss6IsActive_var_ValueChanged(object sender, object lastValue)
        {
            MotorLoss6IsActive = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void MotorLoss7IsActive_var_ValueChanged(object sender, object lastValue)
        {
            MotorLoss7IsActive = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void MotorLoss8IsActive_var_ValueChanged(object sender, object lastValue)
        {
            MotorLoss8IsActive = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void MotorLoss9IsActive_var_ValueChanged(object sender, object lastValue)
        {
            MotorLoss9IsActive = bool.Parse((sender as VdnVariable).Value.ToString());
        }

        private void MotorLoss0IsEnable_var_ValueChanged(object sender, object lastValue)
        {
            MotorLoss0IsEnable = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void MotorLoss1IsEnable_var_ValueChanged(object sender, object lastValue)
        {
            MotorLoss1IsEnable = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void MotorLoss2IsEnable_var_ValueChanged(object sender, object lastValue)
        {
            MotorLoss2IsEnable = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void MotorLoss3IsEnable_var_ValueChanged(object sender, object lastValue)
        {
            MotorLoss3IsEnable = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void MotorLoss4IsEnable_var_ValueChanged(object sender, object lastValue)
        {
            MotorLoss4IsEnable = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void MotorLoss5IsEnable_var_ValueChanged(object sender, object lastValue)
        {
            MotorLoss5IsEnable = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void MotorLoss6IsEnable_var_ValueChanged(object sender, object lastValue)
        {
            MotorLoss6IsEnable = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void MotorLoss7IsEnable_var_ValueChanged(object sender, object lastValue)
        {
            MotorLoss7IsEnable = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void MotorLoss8IsEnable_var_ValueChanged(object sender, object lastValue)
        {
            MotorLoss8IsEnable = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void MotorLoss9IsEnable_var_ValueChanged(object sender, object lastValue)
        {
            MotorLoss9IsEnable = bool.Parse((sender as VdnVariable).Value.ToString());
        }


        private void CtrSurLoss9IsEnable_var_ValueChanged(object sender, object lastValue)
        {
            CtrSurLoss9IsEnable = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void CtrSurLoss8IsEnable_var_ValueChanged(object sender, object lastValue)
        {
            CtrSurLoss8IsEnable = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void CtrSurLoss7IsEnable_var_ValueChanged(object sender, object lastValue)
        {
            CtrSurLoss7IsEnable = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void CtrSurLoss6IsEnable_var_ValueChanged(object sender, object lastValue)
        {
            CtrSurLoss6IsEnable = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void CtrSurLoss5IsEnable_var_ValueChanged(object sender, object lastValue)
        {
            CtrSurLoss5IsEnable = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void CtrSurLoss4IsEnable_var_ValueChanged(object sender, object lastValue)
        {
            CtrSurLoss4IsEnable = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void CtrSurLoss3IsEnable_var_ValueChanged(object sender, object lastValue)
        {
            CtrSurLoss3IsEnable = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void CtrSurLoss2IsEnable_var_ValueChanged(object sender, object lastValue)
        {
            CtrSurLoss2IsEnable = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void CtrSurLoss1IsEnable_var_ValueChanged(object sender, object lastValue)
        {
            CtrSurLoss1IsEnable = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void CtrSurLoss0IsEnable_var_ValueChanged(object sender, object lastValue)
        {
            CtrSurLoss0IsEnable = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void CtrSurLoss9IsActive_var_ValueChanged(object sender, object lastValue)
        {
            CtrSurLoss9IsActive = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void CtrSurLoss8IsActive_var_ValueChanged(object sender, object lastValue)
        {
            CtrSurLoss8IsActive = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void CtrSurLoss7IsActive_var_ValueChanged(object sender, object lastValue)
        {
            CtrSurLoss7IsActive = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void CtrSurLoss6IsActive_var_ValueChanged(object sender, object lastValue)
        {
            CtrSurLoss6IsActive = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void CtrSurLoss5IsActive_var_ValueChanged(object sender, object lastValue)
        {
            CtrSurLoss5IsActive = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void CtrSurLoss4IsActive_var_ValueChanged(object sender, object lastValue)
        {
            CtrSurLoss4IsActive = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void CtrSurLoss3IsActive_var_ValueChanged(object sender, object lastValue)
        {
            CtrSurLoss3IsActive = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void CtrSurLoss2IsActive_var_ValueChanged(object sender, object lastValue)
        {
            CtrSurLoss2IsActive = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void CtrSurLoss1IsActive_var_ValueChanged(object sender, object lastValue)
        {
            CtrSurLoss1IsActive = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void CtrSurLoss0IsActive_var_ValueChanged(object sender, object lastValue)
        {
            CtrSurLoss0IsActive = bool.Parse((sender as VdnVariable).Value.ToString());
        }

        private void CtrSurLock9IsEnable_var_ValueChanged(object sender, object lastValue)
        {
            CtrSurLock9IsEnable = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void CtrSurLock1IsEnable_var_ValueChanged(object sender, object lastValue)
        {
            CtrSurLock1IsEnable = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void CtrSurLock2IsEnable_var_ValueChanged(object sender, object lastValue)
        {
            CtrSurLock2IsEnable = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void CtrSurLock3IsEnable_var_ValueChanged(object sender, object lastValue)
        {
            CtrSurLock3IsEnable = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void CtrSurLock4IsEnable_var_ValueChanged(object sender, object lastValue)
        {
            CtrSurLock4IsEnable = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void CtrSurLock5IsEnable_var_ValueChanged(object sender, object lastValue)
        {
            CtrSurLock5IsEnable = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void CtrSurLock6IsEnable_var_ValueChanged(object sender, object lastValue)
        {
            CtrSurLock6IsEnable = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void CtrSurLock7IsEnable_var_ValueChanged(object sender, object lastValue)
        {
            CtrSurLock7IsEnable = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void CtrSurLock8IsEnable_var_ValueChanged(object sender, object lastValue)
        {
            CtrSurLock8IsEnable = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void CtrSurLock0IsEnable_var_ValueChanged(object sender, object lastValue)
        {
            CtrSurLock0IsEnable = bool.Parse((sender as VdnVariable).Value.ToString());
        }

        private void CtrSurLock9IsActive_var_ValueChanged(object sender, object lastValue)
        {
            CtrSurLock9IsActive = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void CtrSurLock8IsActive_var_ValueChanged(object sender, object lastValue)
        {
            CtrSurLock8IsActive = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void CtrSurLock7IsActive_var_ValueChanged(object sender, object lastValue)
        {
            CtrSurLock7IsActive = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void CtrSurLock6IsActive_var_ValueChanged(object sender, object lastValue)
        {
            CtrSurLock6IsActive = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void CtrSurLock5IsActive_var_ValueChanged(object sender, object lastValue)
        {
            CtrSurLock5IsActive = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void CtrSurLock4IsActive_var_ValueChanged(object sender, object lastValue)
        {
            CtrSurLock4IsActive = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void CtrSurLock3IsActive_var_ValueChanged(object sender, object lastValue)
        {
            CtrSurLock3IsActive = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void CtrSurLock2IsActive_var_ValueChanged(object sender, object lastValue)
        {
            CtrSurLock2IsActive = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void CtrSurLock1IsActive_var_ValueChanged(object sender, object lastValue)
        {
            CtrSurLock1IsActive = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void CtrSurLock0IsActive_var_ValueChanged(object sender, object lastValue)
        {
            CtrSurLock0IsActive = bool.Parse((sender as VdnVariable).Value.ToString());
        }

        private void Roll_var_ValueChanged(object sender, object lastValue)
        {
            Roll = double.Parse((sender as VdnVariable).Value.ToString());
        }
        private void Heading_var_ValueChanged(object sender, object lastValue)
        {
            Heading = double.Parse((sender as VdnVariable).Value.ToString());
        }
        private void Pitch_var_ValueChanged(object sender, object lastValue)
        {
            Pitch = double.Parse((sender as VdnVariable).Value.ToString());
        }
        private void AltitudeMsl_var_ValueChanged(object sender, object lastValue)
        {
            Altitude = double.Parse((sender as VdnVariable).Value.ToString());
        }
        private void Longitude_var_ValueChanged(object sender, object lastValue)
        {
            Longitude = double.Parse((sender as VdnVariable).Value.ToString());
        }
        private void Latitude_var_ValueChanged(object sender, object lastValue)
        {
            Latitude = double.Parse((sender as VdnVariable).Value.ToString());
        }
        private void Visibility_var_ValueChanged(object sender, object lastValue)
        {
            CIGIVisibility = double.Parse((sender as VdnVariable).Value.ToString());
        }
        private void TransitionBand_var_ValueChanged(object sender, object lastValue)
        {
            CIGITransitionBand = double.Parse((sender as VdnVariable).Value.ToString());
        }
        private void Thickness_var_ValueChanged(object sender, object lastValue)
        {
            CIGIThickness = double.Parse((sender as VdnVariable).Value.ToString());
        }
        private void RandomLightningEnable_var_ValueChanged(object sender, object lastValue)
        {
            CIGIRandomLightningEnable = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void Severity_var_ValueChanged(object sender, object lastValue)
        {
            CIGISeverity = int.Parse((sender as VdnVariable).Value.ToString());
        }
        private void LayerID_var_ValueChanged(object sender, object lastValue)
        {
            CIGILayerID = int.Parse((sender as VdnVariable).Value.ToString());
        }
        private void TimeOfDay_var_ValueChanged(object sender, object lastValue)
        {
            CIGITimeOfDay = (sender as VdnVariable).Value.ToString();
        }
        private void Season_var_ValueChanged(object sender, object lastValue)
        {
            CIGISeason = (sender as VdnVariable).Value.ToString();
        }
        private void Coverage_var_ValueChanged(object sender, object lastValue)
        {
            CIGICoverage = double.Parse((sender as VdnVariable).Value.ToString());
        }
        private void CloudyType_var_ValueChanged(object sender, object lastValue)
        {
            CIGICloudyType = int.Parse((sender as VdnVariable).Value.ToString());
        }
        private void Weather_var_ValueChanged(object sender, object lastValue)
        {
            CIGIWeather = (sender as VdnVariable).Value.ToString();
        }
        private void GroundWindDirection_var_ValueChanged(object sender, object lastValue)
        {
            GroundWindDirection = (sender as VdnVariable).Value.ToString();
        }
        private void GroundWindVelocity_var_ValueChanged(object sender, object lastValue)
        {
            GroundWindVelocity = (sender as VdnVariable).Value.ToString();
        }
        private void HollowDirection_var_ValueChanged(object sender, object lastValue)
        {
            HollowDirection = (sender as VdnVariable).Value.ToString();
        }
        private void HollowVelocity_var_ValueChanged(object sender, object lastValue)
        {
            HollowVelocity = (sender as VdnVariable).Value.ToString();
        }
        private void GustSpeed_var_ValueChanged(object sender, object lastValue)
        {
            GustSpeed = (sender as VdnVariable).Value.ToString();
        }
        private void DestinationHeading_var_ValueChanged(object sender, object lastValue)
        {
            DestinationHeading = double.Parse((sender as VdnVariable).Value.ToString());
        }
        private void DestinationAlt_var_ValueChanged(object sender, object lastValue)
        {
            DestinationAlt = double.Parse((sender as VdnVariable).Value.ToString());
        }
        private void DestinationLon_var_ValueChanged(object sender, object lastValue)
        {
            DestinationLon = double.Parse((sender as VdnVariable).Value.ToString());
        }
        private void DestinationLat_var_ValueChanged(object sender, object lastValue)
        {
            DestinationLat = double.Parse((sender as VdnVariable).Value.ToString());
        }
        private void CurrentView_var_ValueChanged(object sender, object lastValue)
        {
            Current_View = (sender as VdnVariable).Value.ToString();
        }
        private void APState_var_ValueChanged(object sender, object lastValue)
        {
            APState = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void FlightPlanGenerated_var_ValueChanged(object sender, object lastValue)
        {
            FlightPlanGenerated = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void PowerOn_var_ValueChanged(object sender, object lastValue)
        {
            PowerOn = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void PowerFreeze_var_ValueChanged(object sender, object lastValue)
        {
            PowerFreeze = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void FlightFreeze_var_ValueChanged(object sender, object lastValue)
        {
            FlightFreeze = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void EngineOn_var_ValueChanged(object sender, object lastValue)
        {
            EngineOn = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void CrashDetection_var_ValueChanged(object sender, object lastValue)
        {
            CrashOverride = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void BJCrashDetection_var_ValueChanged(object sender, object lastValue)
        {
            BJCrashOverride = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void BJClearCrash_var_ValueChanged(object sender, object lastValue)
        {
            BJClearCrash = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void MotionFreeze_var_ValueChanged(object sender, object lastValue)
        {
            MotionFreeze = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void MotionSwitch_var_ValueChanged(object sender, object lastValue)
        {
            MotionSwitch = int.Parse((sender as VdnVariable).Value.ToString());
        }
        private void VisualConnect_var_ValueChanged(object sender, object lastValue)
        {
            VisualConnect = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void LocationFreeze_var_ValueChanged(object sender, object lastValue)
        {
            LocationFreeze = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void HighlyFreeze_var_ValueChanged(object sender, object lastValue)
        {
            HighlyFreeze = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        private void KernelModeString_var_ValueChanged(object sender, object lastValue)
        {
            KernelModeString = (sender as VdnVariable).Value.ToString();
        }
        private void BJKernelModeString_var_ValueChanged(object sender, object lastValue)
        {
            BJKernelModeString = bool.Parse((sender as VdnVariable).Value.ToString());
        }
        #endregion
    }
}
