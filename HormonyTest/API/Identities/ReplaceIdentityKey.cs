using System;
using HarmonySharp;
using Newtonsoft.Json;
using RestSharp;

namespace HarmonyTest.API
{
    public class ReplaceIdentityKey
    {

        public HarmonyClient    Harmony     { get; private set; }
        
        public IRestResponse    RestResponse    { get; private set; }
        public Response         DataResponse    { get; private set; }


        public ReplaceIdentityKey(HarmonyClient harmony)
        {
            Harmony = harmony;
        }


        public bool Get(string IdentityChainID, string OldKey, string NewKey, string SignerKey, string Signature, string CallbackURL = "")
        {

            var request = new RestRequest($"v1/identities/{IdentityChainID}/keys", Method.POST);

            request.AddJsonBody(
            new
            {
                old_key = OldKey,
                new_key = NewKey,
                signer_key = SignerKey,
                signature = Signature,
                callback_url = CallbackURL
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
            [JsonProperty("entry_hash")]
            public string Entry_hash { get; set; }

            [JsonProperty("stage")]
            public Stages Stage { get; set; }

        }
    }
}
