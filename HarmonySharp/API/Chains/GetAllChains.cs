﻿    
using System;
using Newtonsoft.Json;
using RestSharp;

namespace Harmony.API
{
    public class GetAllChains
    {

        public HarmonyClient    Harmony     { get; private set; }
        
        public IRestResponse    RestResponse    { get; private set; }
        public Response         DataResponse    { get; private set; }


        public GetAllChains(HarmonyClient harmony)
        {
            Harmony = harmony;
        }


        public bool Get(int Limit=15, int Offset=0, params Stages[] CallbackStages)
        {

            var request = new RestRequest("v1/chains", Method.GET);

            request.AddJsonBody(
            new
            {
                limit = Limit,
                offset = Offset,
                callback_stages = CallbackStages
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
                [JsonProperty("stage")]
                public Stages Stage { get; set; }

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
