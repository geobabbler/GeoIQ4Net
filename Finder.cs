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

//using GeoCommonsWebClient;

namespace GeoIQ.Net
{
    #region Delegates

    public delegate void AsynchSearchCompleteHandler(SearchEventArgs args);
    public delegate void AsynchUploadFileCompleteHandler(ResponseStatusEventArgs args);
    public delegate void AsynchValidateUserCompleteHandler(ResponseStatusEventArgs args);

    #endregion

    #region EventArgs classes

    public class SearchEventArgs
    {
        internal Exception _error = null;
        internal Overlays _result = null;

        public Exception Error
        {
            get
            {
                return _error;
            }
        }

        public Overlays Result
        {
            get
            {
                return _result;
            }
        }
    }

    public class ResponseStatusEventArgs
    {
        internal Exception _error = null;
        internal string _location = null;
        internal string _result = null;

        public Exception Error
        {
            get
            {
                return _error;
            }

        }

        public string Location
        {
            get
            {
                return _location;
            }
        }

        public string Result
        {
            get
            {
                return _result;
            }
        }

    }



    #endregion

    #region Finder

    public class Finder
    {
        #region  Properties and Delegates

        public string EndpointURI { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Exception LastError { get; set; }


        public event AsynchSearchCompleteHandler AsynchSearchComplete;
        public event AsynchUploadFileCompleteHandler AsyncUploadFileComplete;
        public event AsynchValidateUserCompleteHandler AsyncValidateCredentialsComplete;

        private static List<string> allowedTypes = new List<string>
        {
            ".kml",
            ".csv",
            ".shp",
            ".shx",
            ".dbf",
            ".xml",
            ".atom",
            ".rss",
            ".prj",
            ".climgen"
        };

        #endregion

        #region Constructors

        public Finder(string endpointUri)
        {

            EndpointURI = prepareEndpointURI(endpointUri);

        }

        public Finder(string endpointUri, string username, string password)
        {
            EndpointURI = prepareEndpointURI(endpointUri);
            UserName = username;
            Password = password;
        }

        #endregion

        #region Synchronous Methods
        //compiler directive to prevent synchronous method
        //from being made available if library is compiled for Silverlight
#if !SILVERLIGHT

        public ResponseStatusEventArgs Delete(int overlayID)
        {
            ResponseStatusEventArgs args = new ResponseStatusEventArgs();
            try
            {
                GeoComWebClient request = new GeoComWebClient();
                string url = String.Format("{0}/datasets/{1}.json", EndpointURI, overlayID);
                setCredentials(request);

                //validateFileType(files);
                ResponseEventArgs response = request.Delete(url, UserName, Password, "application/json");

                args._result = response.Status;
                //args._location = response.Location;
            }
            catch
            {
            }
            return args;
        }

        public SearchEventArgs ExecuteSearch(int limit, string terms, double maxx, double maxy, double minx, double miny)
        {
            SearchEventArgs args = new SearchEventArgs();
            try
            {
                WebClient request = new WebClient();
                string url = String.Format("{0}/search.json?query={1}&limit={2}&bbox={3},{4},{5},{6}", EndpointURI, terms, limit, minx, miny, maxx, maxy);
                setCredentials(request);

                Stream stream = request.OpenRead(url);
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(GeoIQ.Net.Data.Overlays));

                GeoIQ.Net.Data.Overlays overlays = (GeoIQ.Net.Data.Overlays)serializer.ReadObject(stream);
                args._result = overlays;
            }
            catch (Exception ex)
            {
                args._error = ex;
            }
            return args;
        }

        public SearchEventArgs ExecuteSearch(int limit, string terms, string model)
        {
            SearchEventArgs args = new SearchEventArgs();
            try
            {
                WebClient request = new WebClient();
                string url = String.Format("{0}/search.json?query={1}&limit={2}&model={3}", EndpointURI, terms, limit, model);
                setCredentials(request);

                Stream stream = request.OpenRead(url);
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(GeoIQ.Net.Data.Overlays));

                GeoIQ.Net.Data.Overlays overlays = (GeoIQ.Net.Data.Overlays)serializer.ReadObject(stream);
                args._result = overlays;
            }
            catch (Exception ex)
            {
                args._error = ex;
            }
            return args;
        }

        public List<string> GetTagList()
        {
            List<string> retval = new List<string>();
            try
            {
                WebClient request = new WebClient();
                string url = String.Format("{0}/tags.json", EndpointURI);
                setCredentials(request);

                Stream stream = request.OpenRead(url);
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<string>));
                retval = (List<string>)serializer.ReadObject(stream);
            }
            catch (Exception ex)
            {
            }
            return retval;
       }

        public OverlaySublayerInfo GetLayerDetails(int overlayid)
        {
            OverlaySublayerInfo retval = null;
            try
            {
                WebClient request = new WebClient();
                string url = String.Format("{0}/overlays/{1}.json?include_attributes=1", EndpointURI, overlayid);
                setCredentials(request);

                Stream stream = request.OpenRead(url);
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(GeoIQ.Net.Data.OverlaySublayerInfo));
                retval = (GeoIQ.Net.Data.OverlaySublayerInfo)serializer.ReadObject(stream);
            }
            catch(Exception ex)
            {
                this.LastError = ex;
            }

            return retval;
        }

        public ResponseStatusEventArgs UploadFile(string[] files)
        {
            ResponseStatusEventArgs args = UploadFile(files, null);
            //try
            //{
            //    GeoComWebClient request = new GeoComWebClient();
            //    string url = String.Format("{0}/overlays.json?tags=test%20tags", EndpointURI);
            //    setCredentials(request);

            //    validateFileType(files);
            //    ResponseEventArgs response = request.UploadFiles(url, files);

            //    args._result = response.Status;
            //    try
            //    {
            //        args._location = response.Location;
            //    }
            //    catch { }

            //}
            //catch (Exception ex)
            //{
            //    args._error = ex;
            //}
            return args;
        }

        public ResponseStatusEventArgs UploadFile(string[] files, string[] tags)
        {
            ResponseStatusEventArgs args = new ResponseStatusEventArgs();
            try
            {
                string parms = "";
                if (tags != null)
                {
                    parms = "?tags=" + String.Join("%20", tags);
                }
                GeoComWebClient request = new GeoComWebClient();
                string url = String.Format("{0}/datasets.json" + parms, EndpointURI);
                setCredentials(request);

                validateFileType(files);
                ResponseEventArgs response = request.UploadFiles(url, files);

                args._result = response.Status;
                try
                {
                    args._location = response.Location;
                }
                catch { }

            }
            catch (Exception ex)
            {
                args._error = ex;
            }
            return args;
        }

        public ResponseStatusEventArgs UploadFile(string[] files, string[] tags, string title)
        {
            ResponseStatusEventArgs args = new ResponseStatusEventArgs();
            try
            {
                string parms = "";
                string tags2 = "";
                string title2 = "";
                List<string> parmlist = new List<string>();

                if (tags != null)
                {
                    tags2 = "tags=" + String.Join("%20", tags);
                    parmlist.Add(tags2);
                }
                if (title != null)
                {
                    title2 = "name=" + title;
                    parmlist.Add(title2);
                }
                if (parmlist.Count > 0)
                {
                    parms = "?";
                    var tmp = String.Join("&", parmlist.ToArray());
                    parms += tmp;
                }
                GeoComWebClient request = new GeoComWebClient();
                string url = String.Format("{0}/datasets.json" + parms, EndpointURI);
                setCredentials(request);

                validateFileType(files);
                ResponseEventArgs response = request.UploadFiles(url, files);

                args._result = response.Status;
                try
                {
                    args._location = response.Location;
                }
                catch { }

            }
            catch (Exception ex)
            {
                args._error = ex;
            }
            return args;
        }

        public ResponseStatusEventArgs UpdateTags(int overlayid, List<string> taglist)
        {
            ResponseStatusEventArgs args = new ResponseStatusEventArgs();
            try
            {
                GeoComWebClient request = new GeoComWebClient();
                string url = String.Format("{0}/datasets/{1}.json?tags=", EndpointURI, overlayid);
                string tags = String.Join("%20", taglist.ToArray());
                url += tags;
                setCredentials(request);

                //validateFileType(files);
                ResponseEventArgs response = request.Put(url, "");

                args._result = response.Status;
                args._location = response.Location;

            }
            catch (Exception ex)
            {
                args._error = ex;
            }
            return args;
        }

        public ResponseStatusEventArgs UpdateTitle(int overlayid, string title)
        {
            ResponseStatusEventArgs args = new ResponseStatusEventArgs();
            try
            {
                GeoComWebClient request = new GeoComWebClient();
                string url = String.Format("{0}/datasets/{1}.json?title={2}", EndpointURI, overlayid, title);
                setCredentials(request);

                //validateFileType(files);
                ResponseEventArgs response = request.Put(url, "");

                args._result = response.Status;
                args._location = response.Location;

            }
            catch (Exception ex)
            {
                args._error = ex;
            }
            return args;
        }

        public ResponseStatusEventArgs UpdateTitle(string overlayUrl, string title)
        {
            ResponseStatusEventArgs args = new ResponseStatusEventArgs();
            try
            {
                GeoComWebClient request = new GeoComWebClient();
                string url = overlayUrl + "?title=" + title;
                setCredentials(request);

                //validateFileType(files);
                ResponseEventArgs response = request.Put(url, "");

                args._result = response.Status;
                args._location = response.Location;

            }
            catch (Exception ex)
            {
                args._error = ex;
            }
            return args;
        }
        


        public ResponseStatusEventArgs ValidateUser()
        {
            ResponseStatusEventArgs args = new ResponseStatusEventArgs();
            try
            {
                WebClient request = new WebClient();

                string url = String.Format("{0}/#", EndpointURI);
                setCredentials(request);
                string result = request.DownloadString(url);

                args._result = "valid";
            }
            catch (Exception ex)
            {
                args._error = ex;
            }
            return args;
        }


#endif
        #endregion

        #region Asynchronous Methods
        #region ExecuteSearchAsync
        public void ExecuteSearchAsynch(int limit, string terms, double maxx, double maxy, double minx, double miny)
        {
            WebClient request = new WebClient();
            string url = String.Format("{0}/search.json?query={1}&limit={2}&bbox={3},{4},{5},{6}", EndpointURI, terms, limit, minx, miny, maxx, maxy);
            setCredentials(request);

            request.DownloadStringCompleted += new DownloadStringCompletedEventHandler(request_DownloadStringCompleted);
            request.DownloadStringAsync(new Uri(url));
        }

        void request_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            SearchEventArgs args = new SearchEventArgs();

            if (e.Error == null)
            {
                string s = e.Result;
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(GeoIQ.Net.Data.Overlays));
                byte[] bytes = Encoding.ASCII.GetBytes(s);
                System.IO.MemoryStream stream = new System.IO.MemoryStream(bytes);
                GeoIQ.Net.Data.Overlays overlays = (GeoIQ.Net.Data.Overlays)serializer.ReadObject(stream);
                args._result = overlays;
            }
            else
            {
                args._error = e.Error;
            }
            if (this.AsynchSearchComplete != null)
                AsynchSearchComplete(args);
        }
        #endregion


        #region UploadFilesAsync
        public void UploadFileAsync(string[] files)
        {
            GeoComWebClient request = new GeoComWebClient();
            string url = String.Format("{0}/datasets.xml", EndpointURI);
            setCredentials(request);

            validateFileType(files);

            request.GeoComResponseReceived += new GeoComWebClient.GeoComAsyncReturn(UploadFilesDataResponse);
            request.UploadFilesAsync(url, files);
        }


        private void UploadFilesDataResponse(object webClient, ResponseEventArgs e)
        {
            ResponseStatusEventArgs args = new ResponseStatusEventArgs();
            if (e.Error == null)
            {
                args._result = e.Status;
                args._location = e.Location;
            }
            else
            {
                args._error = e.Error;
            }
            if (this.AsyncUploadFileComplete != null)
                AsyncUploadFileComplete(args);
        }


        #endregion

        #region ValidateUserAsync
        public void ValidateUserAsync()
        {
            WebClient request = new WebClient();

            string url = String.Format("{0}/#", EndpointURI);
            setCredentials(request);

            request.DownloadStringCompleted += new DownloadStringCompletedEventHandler(validateCredentials_DownloadStringCompleted);
            request.DownloadStringAsync(new Uri(url));
        }

        void validateCredentials_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            ResponseStatusEventArgs args = new ResponseStatusEventArgs();
            if (e.Error == null)
            {
                args._result = "Credentials validate.";
            }
            else
            {
                args._error = e.Error;
            }
            if (this.AsyncValidateCredentialsComplete != null)
            {
                AsyncValidateCredentialsComplete(args);
            }
        }

        #endregion
        #endregion

        #region Static Methods

        public static bool isValidFileType(string file)
        {

            string ext = Path.GetExtension(file.ToLower());

            return allowedTypes.Contains(ext);
        }


        #endregion

        #region Private Methods


        private string getStatus(byte[] response)
        {
            string result = "";
            string raw = System.Text.Encoding.ASCII.GetString(response);
            string[] words = raw.Split(' ');
            if (words != null)
            {
                result = words[1];
            }
            return result;
        }

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

        private void setCredentials(GeoComWebClient request)
        {
            if (!String.IsNullOrEmpty(UserName) && !String.IsNullOrEmpty(Password))
            {
                string authInfo = UserName + ":" + Password;
                authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(UserName + ":" + Password));
                request.Headers["Authorization"] = "Basic " + authInfo;
            }
        }

        private void validateFileType(string[] files)
        {
            for (int i = 0; i < files.Length; i++)
            {
                if (!isValidFileType(files[i]))
                {
                    string ext = Path.GetExtension(files[i].ToLower());
                    throw new Exception("GeoCommons API doesn't support file extension type: " + ext);
                }
            }

        }

        #endregion
    }

    #endregion

}
