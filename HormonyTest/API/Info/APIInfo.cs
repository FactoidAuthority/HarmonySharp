using System;
using HarmonySharp;
using Newtonsoft.Json;
using RestSharp;

namespace HarmonyTest.API
{
    public class APIInfo
    {


        public HarmonyClient    Harmony     { get; private set; }

        
        public IRestResponse    RestResponse    { get; private set; }
        public Response         DataResponse    { get; private set; }


        public APIInfo(HarmonyClient harmony)
        {
            Harmony = harmony;
        }


        public bool Get()
        {

            var request = new RestRequest("v1", Method.GET);

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
            public string version { get; set; }

            [JsonProperty("links")]
            public Links links { get; set; }

            public class Links
            {
                [JsonProperty("chains")]
                public string chains { get; set; }
            }

        }
        
        
        
    }
}
