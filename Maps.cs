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
    #region classes
    public class MapSearchEventArgs
    {
        internal Exception _error = null;
        internal MapSearchResults _result = null;

        public Exception Error
        {
            get
            {
                return _error;
            }
        }

        public MapSearchResults Result
        {
            get
            {
                return _result;
            }
        }
    }

    public class MapsApi
    {
        #region Properties and Events

        public string EndpointURI { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        #endregion

        #region Constructors

        public MapsApi(string endpointUri)
        {

            EndpointURI = prepareEndpointURI(endpointUri);

        }

        public MapsApi(string endpointUri, string username, string password)
        {
            EndpointURI = prepareEndpointURI(endpointUri);
            UserName = username;
            Password = password;
        }

        #endregion

        #region Synchronous Methods

        public MapSearchEventArgs ExecuteSearch(int limit)
        {
            MapSearchEventArgs args = new MapSearchEventArgs();
            try
            {
                WebClient request = new WebClient();
                string url = String.Format("{0}/search.json?model=Map&limit={1}", EndpointURI, limit);
                setCredentials(request);

                Stream stream = request.OpenRead(url);
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(MapSearchResults));

                MapSearchResults maps = (MapSearchResults)serializer.ReadObject(stream);
                args._result = maps;
            }
            catch (Exception ex)
            {
                args._error = ex;
            }
            return args;
        }

        public MapDetails GetMapDetails(int id)
        {
            //MapSearchEventArgs args = new MapSearchEventArgs();
            MapDetails maps = null;
            try
            {
                WebClient request = new WebClient();
                string url = String.Format("{0}/maps/{1}.json", EndpointURI, id);
                setCredentials(request);

                Stream stream = request.OpenRead(url);
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(MapDetails));

                maps = (MapDetails)serializer.ReadObject(stream);
                //args._result = maps;
            }
            catch (Exception ex)
            {
                string s = ex.ToString();
                //args._error = ex;
            }
            return maps;
        }
        #endregion

        #region privates

        private string prepareEndpointURI(string url)
        {
            string result = url;
            int notFound = -1;

            int httpExists = result.IndexOf("http://");

            if (httpExists == notFound)
                result = String.Format("http://{0}", url);

            //int finderExists = result.IndexOf("finder");

            //if (finderExists == notFound)
            //{
            //    System.Uri domain = new System.Uri(result);
            //    result = String.Format("http://finder.{0}", domain.Authority);
            //}
            return result;
        }

        private void setCredentials(WebClient request)
        {
            if (!String.IsNullOrEmpty(UserName) && !String.IsNullOrEmpty(Password))
            {
                string authInfo = UserName + ":" + Password;
                authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(UserName + ":" + Password));
                request.Headers["Authorization"] = "Basic " + authInfo;
            }
        }

        public string createMap(string options)
        {
            try
            {
                GeoComWebClient request = new GeoComWebClient();
                string url = String.Format("{0}/maps.json", EndpointURI);
                var result = request.Post(url, UserName, Password, "application/x-www-form-urlencoded", options);
                return result;
            }
            catch
            {
                return "";
            }
        }

        public ResponseStatusEventArgs deleteMap(int mapid)
        {
            ResponseStatusEventArgs args = new ResponseStatusEventArgs();
            try
            {
                GeoComWebClient request = new GeoComWebClient();
                string url = String.Format("{0}/maps/{1}.json", EndpointURI, mapid);
                var result = request.Delete(url, UserName, Password, "application/json");
                args._result = result.Status;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return args;
        }

        public string getSession()
        {
            try
            {
                GeoComWebClient request = new GeoComWebClient();
                string url = String.Format("{0}/sessions.json", EndpointURI);
                var result = request.GetCookie(url, this.UserName, this.Password);
                return result;
            }
            catch
            {
                return "";
            }
        }

        #endregion



    }
    #endregion

}
