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
    public class Analytics
    {
        #region

        public string _bufferTemplate = "{0}/analysis.json?calculation=buffer&ds1={1}&distance={2}&unit={3}";

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

        public AnalyticsResponse Buffer(int overlayid, int distance, string units)
        {
            ResponseStatusEventArgs args = new ResponseStatusEventArgs();
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

                //args._result = response.Status;
                //args._location = response.Location;
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
