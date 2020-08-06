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
			// ================================= // 
			// PASTE IN VARIABLE VALUES HERE     // 
			// ================================= //
			

			const string FUNCTIONNAME = "adamHelloWorldFlow";
			const string SOLUTIONID = "playgroun_f9839";
			const string CLIENTID = "joeflow_b0b6941"; //Get from aws or Alex
			//Get from aws or Alex
			const string ENVIRONMENTID = "d1"; 
			// workspace and app where the trigger will fire.
			const string EVENTSOURCE = "joeFlow - !BBC - Playground Test Space|Field Types";
			// the item id you are using for the trigger inside Podio. This can be found inside the item in Podio under Actions>Developer. Ronald DEmo App
			const string TRIGGER_ITEM = "1467386578";

			const string JWT = "eyJhbGciOiJIUzI1NiIsInR5cCI6ICJKV1QifQ.eyJPQXV0aENsaWVudElkIjoic2Fhc2FmcmFzLWRldiIsIlByaXZpbGVnZXMiOlt7InNvbHV0aW9uSWQiOm51bGwsInZlcnNpb24iOm51bGwsImNsaWVudEZpbHRlciI6bnVsbCwiY29tbWFuZCI6bnVsbCwidXJsIjpudWxsLCJmaWx0ZXIiOm51bGx9XSwiaXNzIjoiYXBpLnNhYXNzYWZyYXMuY29tIiwic3ViIjoiYWRhbUBicmlja2JyaWRnZWNvbnN1bHRpbmcuY29tIiwiYXVkIjoiaHR0cHM6Ly93d3cuc2Fhc3NhZnJhcy5jb20vYXV0aCIsImlhdCI6MTU5NDY2MzYwMiwibmJmIjoxNTk0NjYzNjAyLCJleHAiOjE1OTQ3NTAwNjJ9.M2vvv73vv73vv73vv73vv73vv70Q77-9KA_vv718cwTvv73vv71s77-9Xg3vv71iZO-_ve-_vQLvv71R77-977-9";









			var custom = new Dictionary<string, string> { { "x-saasafras-jwt", JWT } };
			var clientContext = new LocalLambdaClientContext(custom);

			// replace with your function name
			ILambdaContext context = new LocalLambdaContext($"{FUNCTIONNAME}.aws", "$LATEST", clientContext);

			// replace with your function name
			var container = PodioContainerFactory.GetPodioContainer(FUNCTIONNAME);

			var command = new SaasafrasSolutionCommand<SaasafrasPodioEvent>
			{
				solutionId = SOLUTIONID, //Get from aws or Alex
				version = "0.0",
				clientFilter = new SaasafrasClientFilter
				{
					clientIds = new[] { CLIENTID }, 
					environmentIds = new[] { ENVIRONMENTID } 
				},
				command = "send-podio-event-to-function",
				resource = new SaasafrasPodioEvent
				{
					target = $"{FUNCTIONNAME}:DEV",
					hash = "Ul4rc9sqSAeQ6MQb_QLb71BXXsQVDijkNXUQEjoWUA8",
					source = EVENTSOURCE, 
					key = null,
					hook_id = "16094574",//this doesn't matter for local testing.
					type = "item.create", //this doesn't matter for local testing.
					item_id = TRIGGER_ITEM, 
					item_revision_id = "0" //this doesn't matter for local testing.
				}
			};
			var response = container.EntryPoint(command, context).Result;
			Console.WriteLine($"{response}");
		}
	}

}




