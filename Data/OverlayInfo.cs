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
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;

namespace GeoIQ.Net.Data
{
    [Serializable]
    [DataContract(Name = "overlay", Namespace="")]
    public class OverlaySublayerInfoContainer
    {
        [DataMember(Name = "id")]
        public int ID { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "data_attributes")]
        public SublayerInfo[] SubLayers { get; set; }
        //[DataMember(Name = "overlay")]
        //public OverlaySublayerInfoContainer Info { get; set; }
    }

    [Serializable]
    [DataContract()]
    public class OverlaySublayerInfo
    {
        private List<string> _tags = null;

        [DataMember(Name = "id")]
        public int ID { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "data_type")]
        public string DataType { get; set; }
        [DataMember(Name = "tags")]
        public string Tags { get; set; }
        [DataMember(Name = "data_attributes")]
        public List<SublayerInfo> SubLayers { get; set; }

        public List<string> TagList
        {
            get
            {
                if (_tags == null)
                {
                    if (!String.IsNullOrEmpty(this.Tags))
                    {
                        string tags = this.Tags.ToLower();
                        string[] t = tags.Split(',');
                        _tags = t.ToList();
                    }
                }
                return _tags;
            }
        }
    }

    [Serializable]
    [DataContract()]
    public class SublayerInfo
    {
        [DataMember(Name = "data_type")]
        public string DataType { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
        [DataMember(Name = "id")]
        public int ID { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "original_name")]
        public string OriginalName { get; set; }
        [DataMember(Name = "overlay_id")]
        public int OverlayID { get; set; }
    }

    [Serializable]
    [DataContract()]
    public class OverlayInfo
    {
        [DataMember(Name = "description")]
        public string Description { get; set; }
        [DataMember(Name = "overlay_id")]
        public int OverlayID { get; set; }
        [DataMember(Name = "tags")]
        public string Tags { get; set; }
        [DataMember(Name = "detail_link")]
        public string DetailLink { get; set; }
        [DataMember(Name = "link")]
        public string Link { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "short_classification")]
        public string ShortClassification { get; set; }
        [DataMember(Name = "can_view")]
        public bool CanView { get; set; }
        [DataMember(Name = "published")]
        public string DatePublished { get; set; }
        [DataMember(Name = "bbox")]
        public string BoundingBox { get; set; }
        [DataMember(Name = "layer_size")]
        public int LayerSize { get; set; }
        [DataMember(Name = "can_edit")]
        public bool CanEdit { get; set; }
        [DataMember(Name = "created")]
        public string DateCreated { get; set; }
        [DataMember(Name = "icon_path")]
        public string IconPath { get; set; }
        [DataMember(Name = "source")]
        public string Source { get; set; }
        [DataMember(Name = "id")]
        public string ID { get; set; }
        [DataMember(Name = "can_download")]
        public bool CanDownload { get; set; }
        [DataMember(Name = "author")]
        public User Author { get; set; }
        [DataMember(Name = "contributor")]
        public User Contributor { get; set; }

        #region "Non-Serialized Members"
        public string title { get; set; }
        public string shapelink
        {
            get
            {
                return this.Link.ToLower().Replace(".json", ".zip");
            }
        }
        public string kmllink
        {
            get
            {
                return this.Link.ToLower().Replace(".json", ".kml");
            }
        }
        public string infolink
        {
            get
            {
                return this.Link.ToLower().Replace(".json", ".html");
            }
        }
        public string atomlink
        {
            get
            {
                return this.Link.ToLower().Replace(".json", ".atom");
            }
        }
        public string csvlink
        {
            get
            {
                return this.Link.ToLower().Replace(".json", ".csv");
            }
        }
        public string spatialitelink
        {
            get
            {
                return this.Link.ToLower().Replace(".json", ".sqlite");
            }
        }
        #endregion
    }

    [Serializable]
    [DataContract()]
    public class User
    {
        [DataMember(Name = "uri")]
        public string URI { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }

    [Serializable]
    [DataContract()]
    public class Overlays
    {
        [DataMember(Name = "entries")]
        public List<OverlayInfo> entries { get; set; }
    }
}
