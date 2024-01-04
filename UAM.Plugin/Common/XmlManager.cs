using Csf.Imets.ToolCore.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using UAM.Model.Models;
using UAM.Plugin.Common;
using static UAM.Model.Models.VDNModel;

namespace UAM.Plugin
{
    public class XmlManager
    {
        public static XmlDocument xmlDocument = new XmlDocument();

        public static CommunicationModel createRunExec(string execXml)
        {
            CommunicationModel xml = new CommunicationModel();
            string HostOrder = PubVar.g_CurrentUser.UserName.Trim() + ".xml";
            if (File.Exists(execXml))
            {
                xmlDocument.Load(execXml);
                XmlNodeList hostNode = xmlDocument.DocumentElement.GetElementsByTagName("Host");
                xml.Host_IP = hostNode[0].Attributes["IP"].Value;
                xml.Host_User = hostNode[0].Attributes["User"].Value;
                xml.Host_Password = hostNode[0].Attributes["Password"].Value;
                xml.Host_Exec_Cmd = hostNode[0].Attributes["Host_Exec_Cmd"].Value + HostOrder;
                xml.Host_Stop_Cmd = hostNode[0].Attributes["Host_Stop_Cmd"].Value;
                xml.Vdn_Port = hostNode[0].Attributes["VdnPort"].Value;

                XmlNodeList visualNode = xmlDocument.DocumentElement.GetElementsByTagName("Visual");
                xml.Visual_Type = visualNode[0].Attributes["VisualType"].Value;
            }
            else
            {
                throw new Exception(execXml + " is not exist");
            }
            return xml;
        }

        public static List<AircraftModel> GetAircraftList(string execXml)
        {
            var aircraftList = new List<AircraftModel>();
            //string aircraftFilePath = PubVar.GetFilepathFromKey("AircraftFilepath");
            SmartCollection<AircraftModel> aircraftXmlList = SmartCollection<AircraftModel>.Deserialize(execXml, "Aircrafts");
            foreach (AircraftModel aircraft in aircraftXmlList)
            {
                aircraftList.Add(aircraft);
            }
            return aircraftList;
        }

        public static List<UrlModel> GetMapList(string execXml)
        {
            var mapList = new List<UrlModel>();
            SmartCollection<UrlModel> mapXmlList = SmartCollection<UrlModel>.Deserialize(execXml, "URLs");
            foreach (UrlModel map in mapXmlList)
            {
                mapList.Add(map);
            }
            return mapList;
        }

        public static List<ButtonModel> GetButtonList(string execXml)
        {
            var buttonList = new List<ButtonModel>();
            SmartCollection<ButtonModel> buttonXmlList = SmartCollection<ButtonModel>.Deserialize(execXml, "Buttons");
            foreach (ButtonModel btn in buttonXmlList)
            {
                buttonList.Add(btn);
            }
            return buttonList;
        }

        public static List<RouteModel> GetRouteList(string execXml)
        {
            var RouteList = new List<RouteModel>();
            //string RouteFilePath = PubVar.GetFilepathFromKey("RouteFilepath");
            SmartCollection<RouteModel> RouteXmlList = SmartCollection<RouteModel>.Deserialize(execXml, "Routes");
            foreach (RouteModel Route in RouteXmlList)
            {
                RouteList.Add(Route);
            }
            return RouteList;
        }

        public static List<StickModel> GetStickList(string execXml)
        {
            var stickList = new List<StickModel>();
            //string stickFilePath = PubVar.GetFilepathFromKey("StickFilepath");
            SmartCollection<StickModel> stickXmlList = SmartCollection<StickModel>.Deserialize(execXml, "Joysticks");
            foreach (StickModel stick in stickXmlList)
            {
                stickList.Add(stick);
            }
            return stickList;
        }

        public static List<PageModel> GetPageList(string execXml)
        {
            var pageList = new List<PageModel>();
            //string stickFilePath = PubVar.GetFilepathFromKey("PageFilepath");
            SmartCollection<PageModel> pageXmlList = SmartCollection<PageModel>.Deserialize(execXml, "Pages");
            foreach (PageModel page in pageXmlList)
            {
                pageList.Add(page);
            }
            return pageList;
        }

        public static List<MalfunctionModel> GetMalfunctionList(string execXml)
        {
            var malfunctionList = new List<MalfunctionModel>();

            //string malfunctionFilePath = PubVar.GetFilepathFromKey("MalfunctionFilepath");
            SmartCollection<MalfunctionModel> malfunctionXmlList = SmartCollection<MalfunctionModel>.Deserialize(execXml, "Malfunctions");

            foreach (MalfunctionModel malfunction in malfunctionXmlList)
            {
                malfunctionList.Add(malfunction);
            }
            return malfunctionList;
        }
        public static List<SendRequest> GetRequestList(string execXml)
        {
            var requestList = new List<SendRequest>();

            SmartCollection<SendRequest> requestXmlList = SmartCollection<SendRequest>.Deserialize(execXml, "SendRequests");
            foreach (SendRequest req in requestXmlList)
            {
                requestList.Add(req);
            }
            return requestList;
        }

    }
}
