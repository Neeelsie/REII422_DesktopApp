using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Net;


namespace RealEstate.Classes
{
    class FtpManager
    {
        ConfigManager configManager = new ConfigManager();

        string webDir = "";
        string username = "";
        string password = "";

        public FtpManager()
        {
            if (configManager.ConfigLoaded)
            {
                webDir = configManager.FtpWebDirectory;
                username = configManager.FtpUser;
                password = configManager.FtpPassword;
            }
        }

        private bool UploadFile(string sourceFilePath, string destFileName)
        {
            ManualResetEvent waitObject;

            Uri target = new Uri(webDir + '/' + destFileName);
            FtpState state = new FtpState();
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(target);
            request.Method = WebRequestMethods.Ftp.UploadFile;

            request.Credentials = new NetworkCredential(username, password);

            state.Request = request;
            state.FileName = sourceFilePath;

            waitObject = state.OperationComplete;

            request.BeginGetRequestStream(new AsyncCallback(EndGetStreamCallback), state);

            waitObject.WaitOne();

            if( state.OperationException != null)
            {
                throw state.OperationException;
                return false;
            }
            else
            {
                Console.WriteLine("Done");
                return true;
            }
        }

        private static void EndGetStreamCallback(IAsyncResult ar)
        {
            FtpState state = (FtpState)ar.AsyncState;

            Stream requestStream = null;

            try
            {
                requestStream = state.Request.EndGetRequestStream(ar);
                const int bufferLength = 2048;
                byte[] buffer = new byte[bufferLength];
                int count = 0;
                int readBytes = 0;

                FileStream stream = File.OpenRead(state.FileName);
                do
                {
                    readBytes = stream.Read(buffer, 0, bufferLength);
                    requestStream.Write(buffer, 0, readBytes);
                    count += readBytes;
                }
                while (readBytes != 0);

                requestStream.Close();

                state.Request.BeginGetResponse(new AsyncCallback(EndGetResponseCallback), state);


            }
            catch (Exception e)
            {
                Console.WriteLine("Could not get the request stream.");
                state.OperationException = e;
                state.OperationComplete.Set();
                return;
            }
        }

        private static void EndGetResponseCallback(IAsyncResult ar)
        {
            FtpState state = (FtpState)ar.AsyncState;
            FtpWebResponse response = null;

            try
            {
                response = (FtpWebResponse)state.Request.EndGetResponse(ar);
                response.Close();
                state.StatusDescription = response.StatusDescription;
                state.OperationComplete.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error getting response.");
                state.OperationException = e;
                state.OperationComplete.Set();
            }
        }
    }
}
