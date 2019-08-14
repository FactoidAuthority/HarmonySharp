using System;
using HarmonySharp;
using Newtonsoft.Json;
using RestSharp;

namespace HarmonyTest.API
{
    public class GetIdentityChainsKeys
    {

        public HarmonyClient    Harmony     { get; private set; }
        
        public IRestResponse    RestResponse    { get; private set; }
        public Response         DataResponse    { get; private set; }


        public GetIdentityChainsKeys(HarmonyClient harmony)
        {
            Harmony = harmony;
        }


        public bool Get(string IdentityChainID, int Limit=15, int Offset=0)
        {

            var request = new RestRequest($"v1/identities/{IdentityChainID}/keys", Method.GET);

            request.AddJsonBody(
            new
            {
                limit = Limit,
                offset = Offset,
            });

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
            [JsonProperty("offset")]
            public long Offset { get; set; }

            [JsonProperty("limit")]
            public long Limit { get; set; }

            [JsonProperty("data")]
            public Datum[] Data { get; set; }

            [JsonProperty("count")]
            public long Count { get; set; }


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
