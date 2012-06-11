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
using System.Collections.Specialized;
using System.Linq;
using System.Text;

using System.Net;
using System.IO;


namespace GeoIQ.Net
{

    public class ResponseEventArgs : EventArgs
    {
        internal string _location = null;
        internal string _status = null;
        internal Exception _error = null;

        public string Location
        {
            get { return _location; }
        }

        public string Status
        {
            get { return _status; }
        }

        public Exception Error
        {
            get { return _error; }
        }
    }

    public class GeoComWebClient
    {
        public event GeoComAsyncReturn GeoComResponseReceived;

        public delegate void GeoComAsyncReturn(object webclient, ResponseEventArgs response);

        public HttpWebRequest WebReq;
        public Dictionary<string, string> Headers;
        private string NL = "\r\n";

        public GeoComWebClient()
        {
            Headers = new Dictionary<string, string>();
        }

        #region Public Methods

#if !Silverlight
        public ResponseEventArgs Delete(string url, string userName, string password, string content)
        {
            ResponseEventArgs result = new ResponseEventArgs();
            HttpWebResponse webRes = null;
            //Stream requestStream = null;
            WebReq = (HttpWebRequest)WebRequest.Create(url);
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
                //request.Credentials = new NetworkCredential(userName, password);
                string authInfo = userName + ":" + password;
                authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(userName + ":" + password));
                WebReq.Headers["Authorization"] = "Basic " + authInfo;
            }
            WebReq.Method = "DELETE";
            WebReq.KeepAlive = true;
            WebReq.ContentType = content;

            //hasAuthenticationHeader();
            //addHeaders();
            try
            {
                webRes = (HttpWebResponse)WebReq.GetResponse();
                result._status = ((int)webRes.StatusCode).ToString();
                webRes.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (WebReq != null)
                {
                    WebReq = null;
                }

                if (webRes != null)
                {
                    webRes = null;
                }
            }
            return result;
        }

        public ResponseEventArgs Put(string url, string data)
        {
            ResponseEventArgs result = new ResponseEventArgs();
            HttpWebResponse webRes = null;
            Stream requestStream = null;

            byte[] content = System.Text.Encoding.ASCII.GetBytes(data);

            WebReq = (HttpWebRequest)WebRequest.Create(url);
            WebReq.Method = "PUT";
            WebReq.KeepAlive = true;
            WebReq.ContentType = "text/plain";

            hasAuthenticationHeader();
            addHeaders();
            try
            {
                requestStream = WebReq.GetRequestStream();
                requestStream.Write(content, 0, content.Length);
                requestStream.Close();

                webRes = (HttpWebResponse)WebReq.GetResponse();

                var response = webRes.GetResponseStream();

                result._status = webRes.StatusCode.ToString();

                response.Close();
                webRes.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                if (requestStream != null)
                {
                    requestStream = null;
                }

                if (WebReq != null)
                {
                    WebReq = null;
                }

                if (webRes != null)
                {
                    webRes = null;
                }
            }

            return result;
        }

        /// <summary>
        /// Executes an HTTP POST command and retrives the information.		
        /// This function will automatically include a "source" parameter if the "Source" property is set.
        /// </summary>
        /// <param name="url">The URL to perform the POST operation</param>
        /// <param name="userName">The username to use with the request</param>
        /// <param name="password">The password to use with the request</param>
        /// <param name="data">The data to post</param> 
        /// <returns>The response of the request, or null if we got 404 or nothing.</returns>
        public string Post(string url, string userName, string password, string contentType, string data)
        {
            WebRequest request = WebRequest.Create(url);
            try
            {
                //if (this.IgnoreCertificateErrors)
                //    ServicePointManager.CertificatePolicy = new AcceptAllCertificatePolicy();
                if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
                {
                    //request.Credentials = new NetworkCredential(userName, password);
                    string authInfo = userName + ":" + password;
                    authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(userName + ":" + password));
                    request.Headers["Authorization"] = "Basic " + authInfo;
                }
                request.ContentType = contentType;
                request.Method = "POST";

                byte[] bytes = Encoding.UTF8.GetBytes(data);

                request.ContentLength = bytes.Length;

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(bytes, 0, bytes.Length);

                    using (WebResponse response = request.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
                //}

                //return null;
            }
            catch
            {
                return null;
            }
        }

        public string GetCookie(string url, string userName, string password)
        {
            WebRequest request = WebRequest.Create(url);
            try
            {
                //if (this.IgnoreCertificateErrors)
                //    ServicePointManager.CertificatePolicy = new AcceptAllCertificatePolicy();
                if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
                {
                    //request.Credentials = new NetworkCredential(userName, password);
                    string authInfo = userName + ":" + password;
                    authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(userName + ":" + password));
                    request.Headers["Authorization"] = "Basic " + authInfo;
                }
                request.ContentType = "";
                request.Method = "POST";

                byte[] bytes = Encoding.UTF8.GetBytes("");

                request.ContentLength = bytes.Length;

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(bytes, 0, bytes.Length);

                    using (WebResponse response = request.GetResponse())
                    {
                        return response.Headers["Set-Cookie"];
                        //using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        //{
                        //    return reader.ReadToEnd();
                        //}
                    }
                }
                //}

                //return null;
            }
            catch
            {
                return "";
            }
        }

        public ResponseEventArgs UploadFiles(string url, string[] files)
        {
            ResponseEventArgs result = new ResponseEventArgs();
            HttpWebResponse webRes = null;
            Stream requestStream = null;

            string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");

            WebReq = (HttpWebRequest)WebRequest.Create(url);

            WebReq.Method = "POST";
            WebReq.Timeout = System.Threading.Timeout.Infinite;
            WebReq.Accept = "*/*";
            WebReq.PreAuthenticate = true;
            WebReq.KeepAlive = true;
            WebReq.ContentType = "multipart/form-data; boundary=" + boundary;

            hasAuthenticationHeader();
            addHeaders();

            try
            {
                Stream memStream = getFilesMemoryStream(files, boundary);
                byte[] tempBuffer = getByteArray(memStream);
                memStream.Close();

                WebReq.ContentLength = tempBuffer.Length;

                requestStream = WebReq.GetRequestStream();
                requestStream.Write(tempBuffer, 0, tempBuffer.Length);
                requestStream.Close();

                webRes = (HttpWebResponse)WebReq.GetResponse();

                Stream response = webRes.GetResponseStream();
                try
                {
                    result._status = webRes.StatusCode.ToString();
                }
                catch { }
                try
                {
                    result._location = webRes.Headers["Location"].ToString();
                }
                catch { }

                response.Close();
                webRes.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (WebReq != null)
                {
                    WebReq = null;
                }

                if (webRes != null)
                {
                    webRes.Close();
                    WebReq = null;
                }
            }
            return result;
        }
#endif
        public void PutAsync(string url, string data)
        {
            byte[] content = System.Text.Encoding.ASCII.GetBytes(data);

            WebReq = (HttpWebRequest)WebRequest.Create(url);
            WebReq.Method = "PUT";
            WebReq.KeepAlive = true;
            WebReq.ContentType = "text/plain";

            hasAuthenticationHeader();
            addHeaders();

            try
            {
                Stream requestStream = WebReq.GetRequestStream();
                requestStream.Write(content, 0, content.Length);
                requestStream.Close();

                WebReq.BeginGetResponse(new AsyncCallback(GetResponseBack), WebReq);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UploadFilesAsync(string url, string[] files)
        {
            Stream requestStream = null;

            string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");

            WebReq = (HttpWebRequest)WebRequest.Create(url);

            WebReq.Method = "POST";
            //WebReq.AllowWriteStreamBuffering = false;
            WebReq.SendChunked = true;
            WebReq.Accept = "*/*";
            WebReq.PreAuthenticate = true;
            WebReq.KeepAlive = true;
            WebReq.ContentType = "multipart/form-data; boundary=" + boundary;

            ServicePointManager.DefaultConnectionLimit = 100;
            ServicePointManager.MaxServicePointIdleTime = 240000;

            hasAuthenticationHeader();
            addHeaders();

            try
            {
                // Refactor here perhaps?
                //Stream memStream = getFilesMemoryStream(files, boundary);
                //byte[] tempBuffer = getByteArray(memStream);
                //memStream.Close();

                //WebReq.ContentLength = tempBuffer.Length;
                //WebReq.Timeout = tempBuffer.Length;

                requestStream = WebReq.GetRequestStream();
                requestStream.ReadTimeout = 240000;
                requestStream.WriteTimeout = 240000;
                getFilesMemoryStream(requestStream, files, boundary); // WebReq.GetRequestStream();

                //requestStream.Write(tempBuffer, 0, tempBuffer.Length);
                requestStream.Close();
                //WebReq.Timeout = (int)requestStream.Length;
                //WebReq.ReadWriteTimeout = (int)requestStream.Length;
                WebReq.BeginGetResponse(new AsyncCallback(GetResponseBack), WebReq);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GetResponseBack(IAsyncResult asynchronousResult)
        {
            ResponseEventArgs args = new ResponseEventArgs();

            try
            {
                HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);

                args._status = response.StatusCode.ToString();

                if (response.Headers["Location"] != null)
                {
                    args._location = response.Headers["Location"].ToString();
                }

                response.Close();

                OnGeoComResponseReceived(this, args);
            }
            catch (Exception ex)
            {
                args._error = ex;
                OnGeoComResponseReceived(this, args);
            }
        }

        public void OnGeoComResponseReceived(object geoComWebClient, ResponseEventArgs response)
        {
            if (GeoComResponseReceived != null)
            {
                GeoComResponseReceived(geoComWebClient, response);
            }
        }

        #endregion

        #region Private Methods

        private void addHeaders()
        {
            foreach (string key in Headers.Keys)
            {
                WebReq.Headers.Add(String.Format("{0}: {1}", key, Headers[key]));
            }
        }

        private void copyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[8 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
        }


        private byte[] getByteArray(Stream s)
        {
            byte[] result = new byte[s.Length];
            s.Position = 0;
            s.Read(result, 0, result.Length);

            return result;
        }

        private Stream getFilesMemoryStream(string[] files, string boundary)
        {
            Stream result = new MemoryStream();
            FileStream fileStream = null;

            try
            {
                byte[] newLine = System.Text.Encoding.ASCII.GetBytes(NL);

                byte[] boundaryBytes = System.Text.Encoding.ASCII.GetBytes(NL + "--" + boundary + NL);

                string fileHeaderTemplate = "--" + boundary + NL +
                       "Content-Disposition: form-data; name=\"overlay[{0}]\" ; filename=\"{1}\"; " + NL +
                       "Content-Type: application/octet-stream" + NL + NL;

                for (int i = 0; i < files.Length; i++)
                {
                    string fileName = Path.GetFileName(files[i]);
                    string type = getOverlayType(files[i]);
                    string header = "";
                    if (type.ToLower() == "wild")
                    {
                        header += getWildHeaders(fileName, boundary);
                    }
                    header += string.Format(fileHeaderTemplate, getOverlayType(files[i]), fileName);
                    byte[] headerBytes = System.Text.Encoding.UTF8.GetBytes(header);
                    result.Write(headerBytes, 0, headerBytes.Length);

                    fileStream = new FileStream(files[i], FileMode.Open, FileAccess.Read);
                    byte[] buffer = new byte[1024];

                    int bytesRead = 0;

                    while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        result.Write(buffer, 0, bytesRead);
                    }
                    fileStream.Close();
                    result.Write(newLine, 0, newLine.Length);
                }

                result.Write(boundaryBytes, 0, boundaryBytes.Length);

                fileStream = null;
            }
            catch (Exception ex)
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                    fileStream = null;
                }
                throw ex;
            }

            return result;
        }

        private void getFilesMemoryStream(Stream result, string[] files, string boundary)
        {
            //Stream result = new MemoryStream();
            FileStream fileStream = null;

            try
            {
                byte[] newLine = System.Text.Encoding.ASCII.GetBytes(NL);

                byte[] boundaryBytes = System.Text.Encoding.ASCII.GetBytes(NL + "--" + boundary + NL);

                string fileHeaderTemplate = "--" + boundary + NL +
                       "Content-Disposition: form-data; name=\"overlay[{0}]\" ; filename=\"{1}\"; " + NL +
                       "Content-Type: application/octet-stream" + NL + NL;

                for (int i = 0; i < files.Length; i++)
                {
                    string fileName = Path.GetFileName(files[i]);
                    string type = getOverlayType(files[i]);
                    string header = "";
                    if (type.ToLower() == "wild")
                    {
                        header += getWildHeaders(fileName, boundary);
                    }
                    header += string.Format(fileHeaderTemplate, getOverlayType(files[i]), fileName);
                    byte[] headerBytes = System.Text.Encoding.UTF8.GetBytes(header);
                    result.Write(headerBytes, 0, headerBytes.Length);

                    fileStream = new FileStream(files[i], FileMode.Open, FileAccess.Read);
                    byte[] buffer = new byte[1024];

                    int bytesRead = 0;

                    while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        result.Write(buffer, 0, bytesRead);
                    }
                    fileStream.Close();
                    result.Write(newLine, 0, newLine.Length);
                }

                result.Write(boundaryBytes, 0, boundaryBytes.Length);

                fileStream = null;
            }
            catch (Exception ex)
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                    fileStream = null;
                }
                throw ex;
            }

            //return result;
        }

        private string getWildHeaders(string filename, string boundary)
        {
            string retval = "--" + boundary + NL +
                       "Content-Disposition: form-data; name=\"Filename\" " + NL + NL +
                       filename + NL;

            retval += "--" + boundary + NL +
                       "Content-Disposition: form-data; name=\"overlay[file_key_seed]\" " + NL + NL +
                       "1315531831" + NL;

            retval += "--" + boundary + NL +
                       "Content-Disposition: form-data; name=\"fileext\" " + NL + NL +
                       "*.dbf;*.shx;*.prj;*.shp;*.csv;*.kml;*.rss;*.kmz;*.climgen" + NL;

            retval += "--" + boundary + NL +
                       "Content-Disposition: form-data; name=\"uploadify\" " + NL + NL +
                       "true" + NL;

            retval += "--" + boundary + NL +
                       "Content-Disposition: form-data; name=\"z\" " + NL + NL +
                       "z" + NL;

            retval += "--" + boundary + NL +
                       "Content-Disposition: form-data; name=\"folder\" " + NL + NL +
                       "/" + NL;

            retval += "--" + boundary + NL +
                       "Content-Disposition: form-data; name=\"overlay[target_state]\" " + NL + NL +
                       "complete" + NL;

            return retval;
        }

        private Dictionary<string, string> getOverlayDictionary()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            result.Add(".kml", "kml");
            result.Add(".csv", "csv");
            result.Add(".shp", "shp");
            result.Add(".shx", "shx");
            result.Add(".dbf", "dbf");
            result.Add(".xml", "rss");
            result.Add(".atom", "rss");
            result.Add(".rss", "rss");
            result.Add(".prj", "prj");
            result.Add(".climgen", "wild");

            return result;
        }

        private string getOverlayType(string file)
        {
            string result = "";

            Dictionary<string, string> overlayTypes = getOverlayDictionary();

            string ext = Path.GetExtension(file).ToLower();

            foreach (string key in overlayTypes.Keys)
            {
                if (key == ext)
                {
                    result = overlayTypes[key];
                }
            }

            return result;
        }

        private void hasAuthenticationHeader()
        {
            List<string> headerNames = Headers.Keys.ToList<string>();

            if (!headerNames.Contains("Authorization"))
            {
                throw new Exception("No Authorization header. Please include one to use this method.");
            }
        }


    }
        #endregion

}
