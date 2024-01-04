using Csf.Imets.ToolCore.Vdn;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using UAM.Model.Entity;
using UAM.Model.Models;
using static UAM.Model.Models.VDNModel;

namespace UAM.Plugin.Common
{
    public static class PubVar
    {
        public static EntityUser g_CurrentUser;
        public static DateTime g_CurrentDate = DateTime.Now;

        public static string g_HostIP;
        public static string g_HostUser;
        public static string g_HostPassword;
        public static string g_VdnPort;
        public static string g_VisualType;
        public static string g_XmlPath;

        public static NetVdnClient g_NetVdnClient;
        public static List<AircraftModel> g_AircraftList;
        public static List<RouteModel> g_RouteList;
        public static List<StickModel> g_StickList;
        public static List<PageModel> g_PageList;
        public static StickModel g_CurrentStick;
        public static List<MalfunctionModel> g_MalfunctionList;
        public static List<SendRequest> g_SendRequestList;
        public static List<ButtonModel> g_ButtonList;
        public static List<UrlModel> g_ConfigureUrl;
        public static bool g_AirRePosition = false;

        public static int CurrentRoute;
        public static string CurrentRouteName;
        public static string CurrentAircraft;
        public static string CurrentAircraftName;
        public static string AircraftName;
        public static string DepartureVertiport;
        public static string DeparturePad;
        public static string DestinationVertiport;
        public static string DestinationPad;
        public static string AircraftCode;
        public static string CurrentDay;
        public static string CurrentSeason;

        private static string apppath = AppDomain.CurrentDomain.BaseDirectory;
        private static DirectoryInfo directoryInfo = new DirectoryInfo(apppath);
        public static string g_ProgramPath = directoryInfo.Parent.Parent.Parent.FullName;

        public static string ProgramPath
        {
            get
            {
                return g_ProgramPath;
            }
            internal set
            {
                g_ProgramPath = value;
            }
        }
        public static string GetFilepathFromKey(string resourceName)
        {
            string xamlFilepath = (string)GetResource(resourceName);
            return GetFullFilepath(xamlFilepath);
        }
        public static object GetResource(object key)
        {
            if ((key == null) || (key.ToString() == ""))
            {
                return null; //don't bother nothing to look up
            }
            return TryResource(key);
        }
        public static string GetFullFilepath(string path)
        {
            // Check Absolute Path
            if (File.Exists(path))
            {
                FileInfo info = new FileInfo(path);
                return info.FullName;
            }
            // Check Program Path
            string fullFilepath = ProgramPath + path;
            return fullFilepath;
        }
        public static object TryResource(object key)
        {
            try
            {
                if (Application.Current != null)
                {
                    return Application.Current.TryFindResource(key);
                }
            }
            catch (Exception ex)
            {
                if (Application.Current != null)
                {
                    string resourceKey = key.ToString();
                    Trace.WriteLine("ResourceLocator:TryResource - Unable to find Resource:" + resourceKey + "/nException =" +
                                    ex.ToString());
                }
            }
            return null;
        }

    }
}
