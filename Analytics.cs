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
using System.Net;
using System.Net.Security;
using System.Web;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

using GeoIQ.Net.Data;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Xml;

namespace GeoIQ.Net
{
    public enum MergeOptions
    {
        combine,
        prefer_1,
        prefer_2
    }

    public class Analytics
    {
        #region URL Templates

        private string _getStateTemplate = "{0}/overlays/get_state/{1}";
        private string _bufferTemplate = "{0}/analysis.json?calculation=buffer&ds1={1}&distance={2}&unit={3}";
        private string _intersectTemplate = "{0}/analysis.json?calculation=intersect&ds1={1}&ds2={2}&merge={3}";
        private string _clipTemplate = "{0}/analysis.json?calculation=clip&ds1={1}&ds2={2}";
        private string _dissolveTemplate = "{0}/analysis.json?calculation=dissolve&ds1={1}&atr_1={2}";

        #endregion

        #region Properties and Delegates

        public string EndpointURI { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Exception LastError { get; set; }

        #endregion


        #region Constructors

        public Analytics(string endpointUri)
        {

            EndpointURI = endpointUri;

        }

        public Analytics(string endpointUri, string username, string password)
        {
            EndpointURI = endpointUri;
            UserName = username;
            Password = password;
        }

        #endregion

        #region Synchronous Methods

        public string GetState(int overlayid)
        {
            string retval = "";
            try
            {
                WebClient request = new WebClient();
                string url = String.Format(_getStateTemplate, EndpointURI, overlayid);
                setCredentials(request);
                retval = request.DownloadString(url);
            }
            catch (Exception ex)
            {
                this.LastError = ex;
                retval = "error";
            }
            return retval;
        }

        public AnalyticsResponse Buffer(int overlayid, int distance, string units)
        {
            AnalyticsResponse retval = null;
            try
            {
                GeoComWebClient request = new GeoComWebClient();
                string url = String.Format(_bufferTemplate, EndpointURI, overlayid, distance, units);
                setCredentials(request);

                //validateFileType(files);
                string response = request.Post(url, UserName, Password, "application/json", "");
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(GeoIQ.Net.Data.AnalyticsResponse));
                byte[] bytes = Encoding.ASCII.GetBytes(response);
                System.IO.MemoryStream stream = new System.IO.MemoryStream(bytes);
                stream.Position = 0;
                GeoIQ.Net.Data.AnalyticsResponse result = (GeoIQ.Net.Data.AnalyticsResponse)serializer.ReadObject(stream);

                retval = result;
            }
            catch (Exception ex)
            {
                this.LastError = ex;
                retval = null;
            }
            return retval;
        }

        public AnalyticsResponse Clip(int targetoverlayid, int clippingoverlayid)
        {
            AnalyticsResponse retval = null;
            try
            {
                GeoComWebClient request = new GeoComWebClient();
                string url = String.Format(_clipTemplate, EndpointURI, targetoverlayid, clippingoverlayid);
                setCredentials(request);

                //validateFileType(files);
                string response = request.Post(url, UserName, Password, "application/json", "");
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(GeoIQ.Net.Data.AnalyticsResponse));
                byte[] bytes = Encoding.ASCII.GetBytes(response);
                System.IO.MemoryStream stream = new System.IO.MemoryStream(bytes);
                stream.Position = 0;
                GeoIQ.Net.Data.AnalyticsResponse result = (GeoIQ.Net.Data.AnalyticsResponse)serializer.ReadObject(stream);

                retval = result;
            }
            catch (Exception ex)
            {
                this.LastError = ex;
                retval = null;
            }
            return retval;
        }

        public AnalyticsResponse Dissolve(int targetoverlayid, string columnName)
        {
            AnalyticsResponse retval = null;
            try
            {
                GeoComWebClient request = new GeoComWebClient();
                string url = String.Format(_dissolveTemplate, EndpointURI, targetoverlayid, columnName);
                setCredentials(request);

                //validateFileType(files);
                string response = request.Post(url, UserName, Password, "application/json", "");
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(GeoIQ.Net.Data.AnalyticsResponse));
                byte[] bytes = Encoding.ASCII.GetBytes(response);
                System.IO.MemoryStream stream = new System.IO.MemoryStream(bytes);
                stream.Position = 0;
                GeoIQ.Net.Data.AnalyticsResponse result = (GeoIQ.Net.Data.AnalyticsResponse)serializer.ReadObject(stream);

                retval = result;
            }
            catch (Exception ex)
            {
                this.LastError = ex;
                retval = null;
            }
            return retval;
        }


        public AnalyticsResponse Intersect(int overlayid1, int overlayid2, MergeOptions merge)
        {
            AnalyticsResponse retval = null;
            try
            {
                GeoComWebClient request = new GeoComWebClient();
                string mergestring = Enum.GetName(typeof(MergeOptions), merge);
                string url = String.Format(_intersectTemplate, EndpointURI, overlayid1, overlayid2, mergestring);
                setCredentials(request);

                //validateFileType(files);
                string response = request.Post(url, UserName, Password, "application/json", "");
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(GeoIQ.Net.Data.AnalyticsResponse));
                byte[] bytes = Encoding.ASCII.GetBytes(response);
                System.IO.MemoryStream stream = new System.IO.MemoryStream(bytes);
                stream.Position = 0;
                GeoIQ.Net.Data.AnalyticsResponse result = (GeoIQ.Net.Data.AnalyticsResponse)serializer.ReadObject(stream);

                retval = result;
            }
            catch (Exception ex)
            {
                this.LastError = ex;
                retval = null;
            }
            return retval;
        }

        #endregion


        #region Private Methods

        private void setCredentials(WebClient request)
        {
            if (!String.IsNullOrEmpty(UserName) && !String.IsNullOrEmpty(Password))
            {
                string authInfo = UserName + ":" + Password;
                authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(UserName + ":" + Password));
                request.Headers["Authorization"] = "Basic " + authInfo;
            }
        }

        private void setCredentials(GeoComWebClient request)
        {
            if (!String.IsNullOrEmpty(UserName) && !String.IsNullOrEmpty(Password))
            {
                string authInfo = UserName + ":" + Password;
                authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(UserName + ":" + Password));
                request.Headers["Authorization"] = "Basic " + authInfo;
            }
        }

        #endregion
    }
}
