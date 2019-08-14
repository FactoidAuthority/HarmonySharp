using System;
using HarmonySharp;
using Newtonsoft.Json;
using RestSharp;

namespace HarmonyTest.API
{
    public class GetKeyInfo
    {

        public HarmonyClient    Harmony     { get; private set; }
        
        public IRestResponse    RestResponse    { get; private set; }
        public Response         DataResponse    { get; private set; }


        public GetKeyInfo(HarmonyClient harmony)
        {
            Harmony = harmony;
        }


        public bool Get(string IdentityChainID, string Key)
        {

            var request = new RestRequest($"v1/identities/{IdentityChainID}/keys/{Key}", Method.GET);


            RestResponse = Harmony.MakeRequest(request);

            if (RestResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                DataResponse = JsonConvert.DeserializeObject<Response>(RestResponse.Content);
                return true;
            }

            return false;
        }



        public partial class Response
        {
            [JsonProperty("data")]
            public Datum[] Data { get; set; }

            public partial class Datum
            {
                [JsonProperty("key")]
                public string Key { get; set; }

                [JsonProperty("activated_height")]
                public long Activated_height { get; set; }

                [JsonProperty("retired_height")]
                public long Retired_height { get; set; }

                [JsonProperty("priority")]
                public long Priority { get; set; }

                [JsonProperty("entry_hash")]
                public string Entry_hash { get; set; }
            }
        }
        
        
        
    }
}
