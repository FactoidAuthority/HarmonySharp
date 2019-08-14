using System;
using HarmonySharp;
using Newtonsoft.Json;
using RestSharp;
using static HarmonySharp.HarmonyClient;

namespace HarmonyTest.API
{
    public class CreateAnEntry
    {
    
        HarmonyClient Harmony;

        public IRestResponse    RestResponse    { get; private set; }
        public Response         DataResponse    { get; private set; }
            
        public CreateAnEntry(HarmonyClient harmony)
        {
            Harmony = harmony;
        }

        public bool WriteToChain(string ChainId, EntryData entry, String CallbackURL = null, params Stages[] CallbackStages)
        {

            var request = new RestRequest($"v1/chains/{ChainId}/entries",Method.POST);

            if (!String.IsNullOrEmpty(CallbackURL))
            {
                request.AddJsonBody(
                new
                {
                    callback_url = CallbackURL,
                    callback_stages = CallbackStages,
                    external_ids = entry.ExtIDsBase64Strings,
                    content = entry.ContentBase64String
                });
            }
            else
            {
                request.AddJsonBody(
                new
                {
                    external_ids = entry.ExtIDsBase64Strings,
                    content = entry.ContentBase64String
                });
            }
            
            RestResponse = Harmony.MakeRequest(request);

            
            if (RestResponse.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                DataResponse = JsonConvert.DeserializeObject<Response>(RestResponse.Content);
                return true;
            }
            return false;
        }
        
        public class Response
        {
            [JsonProperty("stage")]
            public Stages Stage { get; set; }
    
            [JsonProperty("entry_hash")]
            public string EntryHash { get; set; }
        }
        
    }
}
