using System;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Saasafras.Event.Helpers;
using Saasafras.Event.Lambda;
using Saasafras.Interfaces;
using Saasafras.Lambda;
using Microsoft.Extensions.DependencyInjection;
using Saasafras.AWS.Lambda;
using Saasafras.Model;
using System.Linq;
using System.Collections.Generic;
using Saasafras.Event;
using Microsoft.Extensions.Configuration;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace TemplateFunctionContainer
{
    public class LambdaContainer
    {
        public async Task<FunctionContainerResponse> Handler(SaasafrasSolutionCommand<SaasafrasPodioEvent> command, ILambdaContext lambdaContext)
        {
            var container = PodioContainerFactory.GetPodioContainer("TemplateFunctionContainer");
            lambdaContext.Logger.LogLine("Entered container.");
            var message = await container.EntryPoint(command, lambdaContext);
            lambdaContext.Logger.LogLine(message.ToString());
            return message;
        }

        static void Main(string[] args)
        {
            //create a local context for testing
            const string JWT = ""; //input the solution's actual JWT here.
            var custom = new Dictionary<string, string> { { "x-saasafras-jwt", JWT } };
            var clientContext = new LocalLambdaClientContext(custom);
            ILambdaContext context = new LocalLambdaContext("TemplateFunction.aws", "$LATEST", clientContext);

            var container = PodioContainerFactory.GetPodioContainer("TemplateFuntionName");

            var command = new SaasafrasSolutionCommand<SaasafrasPodioEvent>
            {
                solutionId = "", //Get from aws or Alex
                version = "0.0",
                clientFilter = new SaasafrasClientFilter
                {
                    clientIds = new[] { "" }, //Get from aws or Alex
                    environmentIds = new[] { "" } //Get from aws or Alex
                },
                command = "send-podio-event-to-function",
                resource = new SaasafrasPodioEvent
                {
                    target = "TemplateFunction:DEV",
                    hash = "Ul4rc9sqSAeQ6MQb_QLb71BXXsQVDijkNXUQEjoWUA8",
                    source = "Workspace|App", //Put in the workspace and app where the trigger will happen.
                    key = null,
                    hook_id = "16094574",//this doesn't matter for local testing.
                    type = "item.create", //this doesn't matter for local testing.
                    item_id = "1222010808", // the item id you are using for the trigger inside Podio. This can be found inside the item in Podio under Actions>Developer.
                    item_revision_id = "0" //this doesn't matter for local testing.
                }
            };
            var response = container.EntryPoint(command, context).Result;
            Console.WriteLine($"{response}");
        }
    }
        
}

