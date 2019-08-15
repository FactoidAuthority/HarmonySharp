using System;
using System.Collections.Generic;
using System.Net;
using RestSharp;

namespace Harmony
{

    public enum Stages
    {
        replicated,
        factom,
        anchored
    }

    public class HarmonyClient
    {
    
            
        public String AppID { get; private set; }
        public String AppKey { get; private set; }
        
        
        public HarmonyClient(string app_id, string app_key)
        {
            AppID = app_id;
            AppKey = app_key;
        }

               
        
        public IRestResponse MakeRequest(RestRequest request)
        {
            var client = new RestClient("https://ephemeral.api.factom.com");           
            request.AddHeader("accept", "application/json");
            request.AddHeader("content-type", "application/json");
            request.AddHeader("app_id", AppID);
            request.AddHeader("app_key", AppKey);
            return client.Execute(request);
        }
    }
}
