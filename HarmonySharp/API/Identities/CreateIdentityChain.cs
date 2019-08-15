using System;
using Harmony;
using Newtonsoft.Json;
using RestSharp;
using System.Linq;

namespace Harmony.API
{
    public class CreateIdentityChain
    {

        public HarmonyClient    Harmony     { get; private set; }
        
        public IRestResponse    RestResponse    { get; private set; }
        public Response         DataResponse    { get; private set; }


        public CreateIdentityChain(HarmonyClient harmony)
        {
            Harmony = harmony;
        }


        public bool Post(string[] Names, string[] Keys, string CallbackURL = "", params Stages[] Callback_Stages)
        {

            var request = new RestRequest($"v1/identities", Method.POST);

            request.AddJsonBody(
            new
            {
                names = Names.ToBase64StringArray(),
                keys = Keys.ToBase64StringArray(),
                callback_url = CallbackURL,
                callback_stages = Callback_Stages
            });

            RestResponse = Harmony.MakeRequest(request);

            if (RestResponse.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                DataResponse = JsonConvert.DeserializeObject<Response>(RestResponse.Content);
                return true;
            }

            return false;
        }



        public partial class Response
        {
        
            [JsonProperty("chain_id")]
            public string ChainID { get; set; }
        
            [JsonProperty("entry_hash")]
            public string Entry_hash { get; set; }

            [JsonProperty("stage")]
            public Stages Stage { get; set; }

        }
    }
}
