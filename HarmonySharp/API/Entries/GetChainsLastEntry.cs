using System;
using Newtonsoft.Json;
using RestSharp;

namespace Harmony.API
{
    public class GetChainsLastEntry
    {

        public HarmonyClient    Harmony     { get; private set; }
        
        public IRestResponse    RestResponse    { get; private set; }
        public Response         DataResponse    { get; private set; }


        public GetChainsLastEntry(HarmonyClient harmony)
        {
            Harmony = harmony;
        }


        public bool Get(string ChainID)
        {
            var request = new RestRequest($"v1/chains/{ChainID}/entries/last", Method.GET);

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
            public _Data Data { get; set; }

            public partial class _Data
            {
                [JsonProperty("stage")]
                public string Stage { get; set; }

                [JsonProperty("external_ids")]
                public string[] ExternalIds { get; set; }

                [JsonProperty("entry_hash")]
                public string EntryHash { get; set; }

                [JsonProperty("eblock")]
                public _Eblock Eblock { get; set; }

                [JsonProperty("dblock")]
                public _Dblock Dblock { get; set; }

                [JsonProperty("created_at")]
                public DateTimeOffset CreatedAt { get; set; }

                [JsonProperty("content")]
                public string Content { get; set; }

                [JsonProperty("chain")]
                public _Chain Chain { get; set; }


                public partial class _Chain
                {
                    [JsonProperty("href")]
                    public string Href { get; set; }

                    [JsonProperty("chain_id")]
                    public string ChainId { get; set; }
                }

                public partial class _Dblock
                {
                    [JsonProperty("keymr")]
                    public string Keymr { get; set; }

                    [JsonProperty("href")]
                    public string Href { get; set; }

                    [JsonProperty("height")]
                    public long Height { get; set; }
                }

                public partial class _Eblock
                {
                    [JsonProperty("keymr")]
                    public string Keymr { get; set; }

                    [JsonProperty("href")]
                    public string Href { get; set; }
                }
            }
        }
    }
}
