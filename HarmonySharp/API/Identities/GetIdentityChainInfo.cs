using System;
using Harmony;
using Newtonsoft.Json;
using RestSharp;

namespace Harmony.API
{
    public class GetIdentityChainInfo
    {

        public HarmonyClient    Harmony     { get; private set; }
        
        public IRestResponse    RestResponse    { get; private set; }
        public Response         DataResponse    { get; private set; }


        public GetIdentityChainInfo(HarmonyClient harmony)
        {
            Harmony = harmony;
        }


        public bool Get(string IdentityChainID)
        {

            var request = new RestRequest($"v1/identities/{IdentityChainID}", Method.GET);


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
            [JsonProperty("version")]
            public string Offset { get; set; }

            [JsonProperty("created_height")]
            public long Limit { get; set; }

            [JsonProperty("chain_id")]
            public string chain_id { get; set; }
            
            [JsonProperty("names")]
            public string names { get; set; }

            [JsonProperty("active_keys")]
            public ActiveKeys[] active_keys { get; set; }

            [JsonProperty("stage")]
            public Stages Stage { get; set; }


            public partial class ActiveKeys
            {

                [JsonProperty("key")]
                public string Key { get; set; }

                [JsonProperty("activated_height")]
                public long Activated_height { get; set; }

                [JsonProperty("retired_height")]
                public long Retired_height { get; set; }

                [JsonProperty("all_keys_href")]
                public string All_keys_href { get; set; }
            }
        }
        
    }
}
