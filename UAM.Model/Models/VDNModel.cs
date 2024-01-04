using Csf.Imets.ToolCore.Vdn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UAM.Model.Models
{
    public class VDNModel
    {
        public class Subscribe
        {
            public string TopicName { get; set; }
            public string VariableName { get; set; }
            public string Units { get; set; }
            public string Scope { get; set; }
            public string VdnType { get; set; }
            public string VariableValue { get; set; }
            public BaseDataType VariableData { get; set; }
        }

        public class SendRequest : NotifyObject
        {
            public SendRequest() { }
            public SendRequest(string topic, string queue, string request)
            {
                this.SendTopic = topic;
                this.SendQueue = queue;
                this.SendRequestName = request;
                this.SendParameters = new List<object> { };
            }
            public string SendTopic { get; set; }
            public string SendQueue { get; set; }
            public string SendRequestName { get; set; }
            public string SendType { get; set; }
            public string ControlName { get; set; }
            public int ControlId { get; set; }
            public bool IsInput { get; set; }
            public string InputHeader { get; set; }
            public string InPutMin { get; set; }
            public string InPutMax { get; set; }
            public int ParameterIndex { get; set; }

            public List<object> _SendParameters;
            public List<object> SendParameters
            {
                get { return _SendParameters; }
                set { _SendParameters = value; RaisePropertyChanged("SendParameters"); }
            }
            //public object _VdnValue;
            public string VdnField;
            //{
            //    get { return _VdnValue; }
            //    set { _VdnValue = value; RaisePropertyChanged("VdnValue"); }
            //}

        }

    }
}
