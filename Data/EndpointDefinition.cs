//The MIT License

//Copyright (c) 2012 Zekiah Technologies, Inc.

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in
//all copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace GeoIQ.Net.Data
{
    [Serializable]
    [DataContract()]
    public class EndpointDefinition
    {
        [DataMember(Name = "endpoint")]
        public string EndpointURL { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }

        //non-serialized members
        public string UserID { get; set; }
        public string Password { get; set; }
    }

    [Serializable]
    [DataContract()]
    public class EndpointDefinitions
    {

        public EndpointDefinitions()
        {
            this.Items = new ObservableCollection<EndpointDefinition>();
        }

        [DataMember(Name = "items")]
        
        public ObservableCollection<EndpointDefinition> Items { get; set; }
    }

    public class EndpointManager
    {
        private EndpointDefinitions _endpoints = null;
        private string _saveFolder = "";

        public EndpointManager()
        {
            _saveFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\GeoIQ";
        }

        public EndpointDefinitions Endpoints
        {
            get { return _endpoints; }
        }

        public string SaveFolder
        {
            get { return _saveFolder; }
            set
            {
                _saveFolder = value;
                _endpoints = null;
                Load();
            }
        }

        public void Load()
        {
            if (!System.IO.Directory.Exists(_saveFolder))
            {
                System.IO.Directory.CreateDirectory(_saveFolder);
            }
            if (System.IO.File.Exists(_saveFolder + "\\endpoints.json"))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(EndpointDefinitions));
                System.IO.FileStream fs = new System.IO.FileStream(_saveFolder + "\\endpoints.json", System.IO.FileMode.Open);
                //System.IO.TextReader reader = new System.IO.StreamReader(_saveFolder + "\\endpoints.json");
                _endpoints = (EndpointDefinitions)serializer.ReadObject(fs);
                fs.Close();
            }
            else
            {
                _endpoints = new EndpointDefinitions();
                _endpoints.Items.Add(new EndpointDefinition { Name = "GeoCommons", EndpointURL = "http://finder.geocommons.com" });
                Save();
                //System.IO.MemoryStream ms = new System.IO.MemoryStream();
                //serializer.WriteObject(ms, Extension.Endpoints);
                //byte[] data = ms.ToArray();
                //System.IO.FileStream fs = new System.IO.FileStream(_saveFolder + "\\endpoints.json", System.IO.FileMode.Create, System.IO.FileAccess.Write);
                //fs.Write(data, 0, data.Length);
                //fs.Close();
                //ms.Close();
                //serializer.WriteObject(
            }
            _endpoints.Items.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Items_CollectionChanged);
        }

        void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Save();
        }

        public void Save()
        {
            if (_endpoints.Items.Count == 0)
            {
                _endpoints.Items.Add(new EndpointDefinition { Name = "GeoCommons", EndpointURL = "http://finder.geocommons.com" });
            }
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(EndpointDefinitions));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            serializer.WriteObject(ms, _endpoints);
            byte[] data = ms.ToArray();
            System.IO.FileStream fs = new System.IO.FileStream(_saveFolder + "\\endpoints.json", System.IO.FileMode.Create, System.IO.FileAccess.Write);
            fs.Write(data, 0, data.Length);
            fs.Close();
            ms.Close();
        }
    }
}
