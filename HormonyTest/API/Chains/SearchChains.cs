using System;
using HarmonySharp;
using Newtonsoft.Json;
using RestSharp;

namespace HarmonyTest.API
{
    public class SearchChains
    {


        public HarmonyClient    Harmony     { get; private set; }

        
        public IRestResponse    RestResponse    { get; private set; }
        public Response         DataResponse    { get; private set; }


        public SearchChains(HarmonyClient harmony)
        {
            Harmony = harmony;
        }


        public bool Get(byte[][] External_ids, int Limit=15, int Offset=0)
        {
            var idsHex = new string[External_ids.Length];
            for (var f=0;f < External_ids.Length; f++)
            {
                idsHex[f] = External_ids[f].ToHexString();
            }        

            var request = new RestRequest("v1/chains/search", Method.POST);

            request.AddQueryParameter("limit", $"{Limit}");
            request.AddQueryParameter("offset", $"{Offset}");

            request.AddParameter("application/json", JsonConvert.SerializeObject(
                new
                {
                    external_ids = idsHex
                }), ParameterType.RequestBody);


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
                [JsonProperty("stage")]
                public string Stage { get; set; }
        
                [JsonProperty("href")]
                public string Href { get; set; }
        
                [JsonProperty("external_ids")]
                public string[] ExternalIds { get; set; }
        
                [JsonProperty("created_at")]
                public DateTimeOffset CreatedAt { get; set; }
        
                [JsonProperty("chain_id")]
                public string ChainId { get; set; }
            }
        }
    }
}
