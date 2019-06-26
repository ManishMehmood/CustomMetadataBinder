using System;
using System.Net;
using System.ServiceModel;
using System.Xml;
using Tridion.ContentManager.CoreService.Client;

namespace SdlWebCustomMetadataBindingLibrary.Factories
{
    class CoreServiceSession:IDisposable
    {
        private string _coreServiceVersion;

        private CoreServiceClient _client;

        //public CoreServiceSession()
        //{
        //    InitializeClient("CoreService", CredentialCache.DefaultNetworkCredentials);
        //}

        public CoreServiceSession(string endPoint, string username, string password)
        {
            InitializeClient(endPoint, username,password);
        }

        private void InitializeClient(string endPoint, string username,string password)
        {
            try
            {
                var credentials = new NetworkCredential(username, password);
                var basicHttpBinding = new BasicHttpBinding
                {
                    MaxReceivedMessageSize = 10485760,
                    ReaderQuotas = new XmlDictionaryReaderQuotas
                    {
                        MaxStringContentLength = 10485760,
                        MaxArrayLength = 10485760
                    },
                    Security = new BasicHttpSecurity
                    {
                        Mode = BasicHttpSecurityMode.TransportCredentialOnly,
                        Transport = new HttpTransportSecurity
                        {
                            ClientCredentialType = HttpClientCredentialType.Windows

                        }
                    }
                };
                var remoteaddress = new EndpointAddress(endPoint);
                // ChannelFactory<ICoreService> factory = new ChannelFactory<ICoreService>(basicHttpBinding, endPoint);
                _client = new CoreServiceClient(basicHttpBinding, remoteaddress);
                _client.ChannelFactory.Credentials.Windows.ClientCredential = credentials;
                
                if (_client != null) _coreServiceVersion = _client.GetApiVersion();
            }

            catch (EndpointNotFoundException ex) { throw ex; }
            catch (Exception ex) { throw ex; }
        }

        public CoreServiceClient CoreServiceClient
        {
            get { return _client; }
        }

        public UserData User
        {
            get { return _client.GetCurrentUser(); }
        }

        public string CoreServiceVersion
        {
            get { return _coreServiceVersion; }
        }

        public void Dispose()
        {
            if (_client.State == CommunicationState.Faulted)
            {
                _client.Abort();
            }
            else
            {
                _client.Close();
            }
        }
    }
}
