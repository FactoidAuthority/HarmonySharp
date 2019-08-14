using System;
using HarmonySharp;
using Newtonsoft.Json;
using RestSharp;

namespace HarmonyTest.API
{
    public class CreateAChain
    {


        public HarmonyClient    Harmony     { get; private set; }

        
        public IRestResponse    RestResponse    { get; private set; }
        public Response         DataResponse    { get; private set; }


        public CreateAChain(HarmonyClient harmony)
        {
            Harmony = harmony;
        }


        public bool Send(EntryData firstEntry, String CallbackURL = null, params Stages[] CallbackStages)
        {

            var request = new RestRequest("v1/chains", Method.POST);


            if (!String.IsNullOrEmpty(CallbackURL))
            {
                request.AddJsonBody(
                new
                {
                    callback_url = CallbackURL,
                    callback_stages = CallbackStages,
                    external_ids = firstEntry.ExtIDsBase64Strings,
                    content = firstEntry.ContentBase64String
                });
            }
            else
            {
                request.AddJsonBody(
                new
                {
                    external_ids = firstEntry.ExtIDsBase64Strings,
                    content = firstEntry.ContentBase64String
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

            [JsonProperty("chain_id")]
            public string ChainID { get; set; }
        }
    }
}
