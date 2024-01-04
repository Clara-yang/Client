using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;
using UAM.Model;

namespace UAM.Plugin.Common
{
    public class Command : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            DoExecute?.Invoke(parameter); // 执行command时触发
        }
        public Action<object> DoExecute { get; set; }
    }

    public class SmartCollection<T> : ObservableCollection<T>
    {
        private bool m_SuppressNotification;
        public override event NotifyCollectionChangedEventHandler CollectionChanged;

        public static SmartCollection<T> Deserialize(string path, string root)
        {
            try
            {
                SmartCollection<T> newCollection = new SmartCollection<T>();
                XmlSerializer serializer = new XmlSerializer(typeof(List<T>), new XmlRootAttribute(root));
                string xmlString = File.ReadAllText(path);
                using (StringReader reader = new StringReader(xmlString))
                {
                    List<T> newList = (List<T>)serializer.Deserialize(reader);
                    newCollection.AddRange(newList);
                }
                return newCollection;
            }
            catch (Exception ex)
            {
                string message = ex.ToString();
                throw new Exception("Failed to Deserialize File '" + path + "' : \n" + message, ex);
            }
        }

        public void AddRange(IEnumerable<T> list)
        {
            if (list == null)
                throw new ArgumentNullException("list");

            m_SuppressNotification = true;

            foreach (T item in list)
            {
                Add(item);
            }
            m_SuppressNotification = false;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!m_SuppressNotification)
            {
                NotifyCollectionChangedEventHandler handler = CollectionChanged;
                if (handler != null)
                    handler(this, e);
            }
        }
    }

}
