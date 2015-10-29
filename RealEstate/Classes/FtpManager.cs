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
        public float progress { get; private set; }

        public delegate void UpdateProgress(double progress);

        public event UpdateProgress OnProgressChange;

        public void UploadFile(string source, string dest)
        {
            ManualResetEvent waitObject;

            Uri target = new Uri(dest);
            FtpState state = new FtpState();
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(target);
            request.Method = WebRequestMethods.Ftp.UploadFile;

            request.Credentials = new NetworkCredential("ingenkia", "cT8CSz9u");

            state.Request = request;
            state.FileName = source;

            waitObject = state.OperationComplete;

            request.BeginGetRequestStream(new AsyncCallback(EndGetStreamCallback), state);

            waitObject.WaitOne();

            if (state.OperationException != null)
            {
                throw state.OperationException;
            }
            else
            {
                Console.WriteLine("Done");
            }
        }

        private void EndGetStreamCallback(IAsyncResult ar)
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

                    if (OnProgressChange != null)
                        OnProgressChange(count / (double)stream.Length);
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

        private void EndGetResponseCallback(IAsyncResult ar)
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
