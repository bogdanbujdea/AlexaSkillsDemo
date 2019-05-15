using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AlexaDemoSkill
{
    public static class HelloFunction
    {
        [FunctionName("Ruby")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string json = await req.ReadAsStringAsync();
            var skillRequest = JsonConvert.DeserializeObject<SkillRequest>(json);

            var requestType = skillRequest.GetRequestType();

            SkillResponse response = null;

            if (requestType == typeof(LaunchRequest))
            {
                response = ResponseBuilder.Tell("Hello .NET Iasi");
            }
            else if (requestType == typeof(IntentRequest))
            {
                var intentRequest = skillRequest.Request as IntentRequest;

                if (intentRequest.Intent.Name == "BestProgrammingLanguage")
                {
                    response = ResponseBuilder.Tell("The best programming language is C#");
                }
                else if (intentRequest.Intent.Name == "LightSwitch")
                {
                    var state = intentRequest.Intent.Slots["state"].Value;
                    if (state == "off")
                    {
                        var progressiveResponse = new ProgressiveResponse(skillRequest);
                        await progressiveResponse.SendSpeech("The lights will be turned off");
                        await LightSwitcher.TurnOff();
                        response = ResponseBuilder.Tell("The lights are now off");
                    }
                }
            }

            return new OkObjectResult(response);
        }
    }
}
