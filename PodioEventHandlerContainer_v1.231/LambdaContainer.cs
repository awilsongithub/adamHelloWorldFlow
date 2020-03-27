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

namespace PodioEventHandlerContainer_v1._22
{
    public class LambdaContainer : SaasafrasFunctionContainer<SaasafrasSolutionCommand<SaasafrasPodioEvent>>
    {
        static LambdaContainer container;
        private static IConfigurationBuilder configBuilder;
        static LambdaContainer()
        {
            configBuilder = new ConfigurationBuilder();
            container = new LambdaContainer(configBuilder);
            container.Start();
        }
        public LambdaContainer()
        { }
        public LambdaContainer(IConfigurationBuilder configurationBuilder) : base(configurationBuilder)
        { }
        public async Task<FunctionContainerResponse> Handler(SaasafrasSolutionCommand<SaasafrasPodioEvent> command, ILambdaContext lambdaContext)
        {
            //add the solution config
            configBuilder.AddSaasafrasConfig(command.solutionId, command.version);
            //unpack the client & env
            return await container.EntryPoint(command, lambdaContext);
        }

        static void Main(string[] args)
        {
            //create a local context for testing
            var custom = new Dictionary<string, string> { { "x-saasafras-jwt", "this is a real jwt" } };
            var clientContext = new LocalLambdaClientContext(custom);
            ILambdaContext context = new LocalLambdaContext("WindsorChaseQualifiedLeadsDataFunction1", "$LATEST", clientContext);

            var command = new SaasafrasSolutionCommand<SaasafrasPodioEvent>
            {
                solutionId = "windsorchase",
                version = "0.0",
                clientFilter = new SaasafrasClientFilter
                {
                    clientIds = new[] { "windsorchase" },
                    environmentIds = new[] { "wc1" }
                },
                command = "send-podio-event-to-function",
                resource = new SaasafrasPodioEvent
                {
                    target = "WindsorChaseQualifiedLeadsDataFunction1:DEV",
                    hash = "Ul4rc9sqSAeQ6MQb_QLb71BXXsQVDijkNXUQEjoWUA8",
                    source = "2.)  Customer Profiles|Qualified Leads Data",
                    key = null,
                    hook_id = "16094574",
                    type = "item.create",
                    item_id = "1222010808",
                    item_revision_id = "0"
                }
            };
            var response = container.EntryPoint(command, context).Result;
            Console.WriteLine($"{response}");

            //TODO call function locker to unlock this function

        }
    }
}
