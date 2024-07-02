using Csf.Imets.ToolCore.AutoFidelityTesting;
using Csf.Imets.ToolCore.Vdn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using UAM.Client.Views.CustomGraphics;
using UAM.Model.Enum;
using UAM.Model.Models;
using UAM.Plugin;
using UAM.Plugin.Common;
using UAM.Plugin.NetVDN;
using static UAM.Model.Models.VDNModel;

namespace UAM.Client.Common
{
    public static class CommonMethod
    {
        static VDNData vdnClient = new VDNData();
        static VDNDataUpadate dataUpdate = VDNDataUpadate.Instance;
        public static SendRequest request = new SendRequest();

        public static NumricKeypad keypad;
        public static string KeypadDisplay(SendRequest req)
        {
            keypad = new NumricKeypad();
            keypad.Header = req.InputHeader;
            keypad.Min = req.InPutMin;
            keypad.Max = req.InPutMax;
            keypad.ShowDialog();
            if (keypad.DialogResult == true)
                return keypad.InputText;
            else return "false";
        }

        public static SendRequest SendRequestMethod(string topic, string queue, string req, List<object> para)
        {
            request = new SendRequest(topic, queue, req);
            foreach (var item in para)
            {
                request.SendParameters.Add(item);
            }
            return request;
        }

        /// <summary>
        /// 故障request
        /// </summary>
        /// <param name="requests"></param>
        /// <returns></returns>
        public static List<string> SendRequest_Mal(List<SendRequest> requests)
        {
            string keypadValue = string.Empty;
            object subValue = "";
            List<string> str = new List<string>();
            var sendParams = new List<object>();
            string topicName = string.Empty;
            string queueName = string.Empty;
            string reqName = string.Empty;
            int controlId = 0;

            var reqs = requests.GroupBy(m => new SendRequest
            {
                SendTopic = m.SendTopic,
                SendQueue = m.SendQueue,
                SendRequestName = m.SendRequestName
            });

            foreach (var req in reqs)
            {
                topicName = req.Key.SendTopic;
                queueName = req.Key.SendQueue;
                reqName = req.Key.SendRequestName;
                foreach (var item in req)
                {
                    controlId = item.ControlId;
                    if (item.VdnField != "")
                    {
                        if (item.VdnField != null)
                            subValue = GetPropertiesValue(dataUpdate, item.VdnField);
                        subValue = subValue.ToString() == "" ? false : subValue;
                        sendParams.Add(!bool.Parse(subValue.ToString()));
                    }

                    if (item.IsInput == true && subValue.Equals(false))
                    {
                        sendParams.Add(Convert.ToInt64(controlId));
                        keypadValue = KeypadDisplay(item);
                        if (keypadValue != "false")
                        {
                            sendParams.Add(Convert.ToDouble(keypadValue));
                            str.Add(keypadValue.ToString());
                        }
                    }
                }
                if (subValue.ToString() == true.ToString() && controlId!= 0)
                {
                    sendParams.Add(Convert.ToInt64(controlId));
                    sendParams.Add(Convert.ToDouble(0));
                    break;
                }
            }

            var sendRequest = SendRequestMethod(topicName, queueName, reqName, sendParams);
            vdnClient.SendRequset(sendRequest);

            return str;
        }

        /// <summary>
        /// sensor传感器request
        /// </summary>
        /// <param name="requests"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static void SendRequest_Sensor(List<SendRequest> requests, List<object> param)
        {
            string topicName = string.Empty;
            string queueName = string.Empty;
            string reqName = string.Empty;

            var reqs = requests.GroupBy(m => new SendRequest
            {
                SendTopic = m.SendTopic,
                SendQueue = m.SendQueue,
                SendRequestName = m.SendRequestName
            });

            foreach (var req in reqs)
            {
                topicName = req.Key.SendTopic;
                queueName = req.Key.SendQueue;
                reqName = req.Key.SendRequestName;
            }
            var sendRequest = SendRequestMethod(topicName, queueName, reqName, param);
            vdnClient.SendRequset(sendRequest);
        }

        public static object GetPropertiesValue<TSource>(TSource source, string param)
        {
            var sourceProperties = typeof(TSource).GetProperties();
            object value = "";

            foreach (PropertyInfo p in sourceProperties)
            {
                object pvalue = p.GetValue(source, null);
                string pname = p.Name;
                if (pname == param)
                    value = pvalue;
            }
            return value;
        }

    }
}
