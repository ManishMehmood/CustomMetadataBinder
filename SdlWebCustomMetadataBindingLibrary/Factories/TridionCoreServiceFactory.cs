using System;
using System.Configuration;
using System.Net;
using System.ServiceModel;
using System.Xml;
using Tridion.ContentManager.CoreService.Client;

namespace SdlWebCustomMetadataBindingLibrary.Factories
{
    class TridionCoreServiceFactory
    {
        public static CoreServiceClient CreateCoreService(string endpoint,string username,string password)
        {
            return new CoreServiceSession(endpoint,username,password).CoreServiceClient;
        }
    }
}
