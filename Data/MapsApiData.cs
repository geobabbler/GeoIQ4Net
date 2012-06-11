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
    [DataContract()]
    public class MapDetails
    {
        [DataMember(Name = "updated_at")]
        public String LastUpdated { get; set; }
        [DataMember(Name = "title")]
        public string Title { get; set; }
        [DataMember(Name = "id")]
        public int ID { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
        [DataMember(Name = "classification")]
        public string Classification { get; set; }
        [DataMember(Name = "basemap")]
        public string Basemap { get; set; }
        [DataMember(Name = "created_at")]
        public String CreateDate { get; set; }
        [DataMember(Name = "layers")]
        public List<MapLayerInfo> Layers { get; set; }
    }

    [Serializable]
    [DataContract()]
    public class MapLayerInfo
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }
        [DataMember(Name = "title")]
        public string Title { get; set; }
        [DataMember(Name = "source")]
        public string Source { get; set; }
    }

    [Serializable]
    [DataContract()]
    public class MapInfo
    {
        [DataMember(Name = "sortable_name")]
        public string SortableName { get; set; }
        [DataMember(Name = "score")]
        public double Score { get; set; }
        [DataMember(Name = "updated_at")]
        public string LastUpdated { get; set; }
        [DataMember(Name = "download_group_ids")]
        public List<int> DownloadGroups { get; set; }
        [DataMember(Name = "min_longitude")]
        public double MinLongitude { get; set; }
        [DataMember(Name = "title")]
        public string Title { get; set; }
        [DataMember(Name = "max_longitude")]
        public double MaxLongtiude { get; set; }
        [DataMember(Name = "text")]
        public string Text { get; set; }
        [DataMember(Name = "is_featured")]
        public bool IsFeatured { get; set; }
        [DataMember(Name = "user_login")]
        public string UserLogin { get; set; }
        [DataMember(Name = "is_private")]
        public bool isPrivate { get; set; }
        [DataMember(Name = "min_latitude")]
        public double MinLatitude { get; set; }
        [DataMember(Name = "edit_group_ids")]
        public List<int> EditGroups { get; set; }
        [DataMember(Name = "type")]
        public string Type { get; set; }
        [DataMember(Name = "pk")]
        public int Key { get; set; }
        [DataMember(Name = "shared")]
        public bool Shared { get; set; }
        [DataMember(Name = "id")]
        public string ID { get; set; }
        [DataMember(Name = "updated_by")]
        public string UpdatedBy { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
        [DataMember(Name = "max_latitude")]
        public double MaxLatitude { get; set; }
        [DataMember(Name = "num_charts")]
        public int NumCharts { get; set; }
        [DataMember(Name = "view_group_ids")]
        public List<int> ViewGroups { get; set; }
        [DataMember(Name = "is_public")]
        public bool IsPublic { get; set; }
        [DataMember(Name = "num_layers")]
        public int LayerCount { get; set; }
        [DataMember(Name = "created_at")]
        public string CreateDate { get; set; }
        [DataMember(Name = "is_copy")]
        public bool IsCopy { get; set; }
        [DataMember(Name = "state")]
        public string State { get; set; }
    }


    [Serializable]
    [DataContract()]
    public class MapSearchResult
    {
        [DataMember(Name = "title")]
        public string Title { get; set; }
        [DataMember(Name = "pk")]
        public int Key { get; set; }
        [DataMember(Name = "type")]
        public string Type { get; set; }
        [DataMember(Name = "short_classification")]
        public string ShortClassification { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
        [DataMember(Name = "bbox")]
        public string BBox { get; set; }
        [DataMember(Name = "author")]
        public UserInfo Author { get; set; }
        [DataMember(Name = "created")]
        public string CreateDate { get; set; }
        [DataMember(Name = "permissions")]
        public Permissions Permissions { get; set; }
        [DataMember(Name = "tags")]
        public string Tags { get; set; }
        [DataMember(Name = "id")]
        public string ID { get; set; }
        [DataMember(Name = "link")]
        public string Link { get; set; }
    }

    [Serializable]
    [DataContract()]
    public class UserInfo
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "uri")]
        public string URI { get; set; }
    }

    [Serializable]
    [DataContract()]
    public class Permissions
    {
        [DataMember(Name = "view")]
        public bool View { get; set; }
        [DataMember(Name = "edit")]
        public bool Edit { get; set; }
        [DataMember(Name = "download")]
        public bool Download { get; set; }
    }

    [Serializable]
    [DataContract()]
    public class MapSearchResults
    {
        [DataMember(Name = "entries")]
        public List<MapSearchResult> Entries { get; set; }
    }
}
