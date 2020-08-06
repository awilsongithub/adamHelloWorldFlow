using System;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Microsoft.Extensions.Configuration;
using PodioCore;
using Saasafras.Event.Helpers;
using Saasafras.Lambda;
using PodioCore.Items;
using Saasafras.Lambda.Interfaces;
using Saasafras.Interfaces;
using Saasafras.Model;
using System.Linq;
using PodioCore.Utils.ItemFields;
using System.Collections.Generic;
using PodioCore.Models;
using PodioCore.Models.Request;
using PodioCore.Services;
using Saasafras.Event;
using PodioCore.Comments;

//Don't forget to configure the aws-lambda-tools-*.json
//Use dotnet lambda deploy-function -cfg aws-lambda-tools-{NameOfThisFunction}.json 
//change namespace to {functionName}Container, for clarity

namespace adamHelloWorldFlowContainer
{
	public class MyHandler : IEventHandler<SaasafrasSolutionCommand<SaasafrasPodioEvent>>
	{
		private readonly ISolutionLoggerFactory solutionLoggerFactory;
		private readonly ISaasafrasDictionary saasafrasDictionary;
		private readonly IConfiguration configuration;
		private readonly IAccessTokenProvider accessTokenProvider;
		private readonly IFunctionLocker functionLocker;


		public MyHandler(IFunctionLocker functionLocker, ISolutionLoggerFactory solutionLoggerFactory, ISaasafrasDictionary saasafrasDictionary, IConfiguration configuration, IAccessTokenProvider accessTokenProvider)
		{
			this.functionLocker = functionLocker;
			this.solutionLoggerFactory = solutionLoggerFactory;
			this.saasafrasDictionary = saasafrasDictionary;
			this.configuration = configuration;
			this.accessTokenProvider = accessTokenProvider;
		}

		public async Task<FunctionContainerResponse> Handle(SaasafrasSolutionCommand<SaasafrasPodioEvent> command)
		{
			client.log("This will go the client log file.");
			solution.log("This will go to the solution log file.");

			//check for valid itemId
			if (!int.TryParse(command.resource.item_id, out int itemId))
				throw new ArgumentException("we were expecting an integer");

			//get Podio client 
			var podio = new Podio(accessTokenProvider);
			

			var userService = new PodioCore.Services.UserService(podio);
			solution.log("calling podio");
			var user = await userService.GetUser();
			solution.log($"podio api user_id : {user.UserId}");
			solution.log($"podio api email : {user.Mail}");

			//get item that fired the event
			var currentItem = await podio.GetFullItem(itemId) ?? throw new Exception($"failed to get item {itemId}");
			client.log($"Item was created in {currentItem.App.Config.Name}.");

			//update the function name
			var functionName = "adamHelloWorldFlow";
			var uniqueId = currentItem.ItemId.ToString();
			var lockId = await functionLocker.LockFunction(functionName, uniqueId);

			if (string.IsNullOrEmpty(lockId))
			{
				throw new Exception($"Failed to acquire lock for {functionName} and id {uniqueId}");
			}

			try
			{





				/**=======================
				* WRITE YOU LOGIC HERE !
				=========================*/


				// easily reference Podio spaces, apps and fields
				//var mySolution = await saasafrasDictionary.GetDictionary();
				//int MyAppMyFieldId = int.Parse(dictionary["My Space| My App | My Field"])




				// access spaces, apps or fields from the solution dictionary
				//var dictionary = await saasafrasDictionary.GetDictionary();

				//var demoApp = dictionary["!BBC - Playground Test Space| Demo App"];
				//Console.WriteLine(demoApp);
				


				

				//int FieldsAppTitleFieldId = int.Parse(dictionary["!BBC - Playground Test Space|Field Types|Title"]);

				//System.Console.WriteLine(FieldsAppTitleFieldId); // id of the field

				//var FieldsAppTitleField = currentItem.Field<TextItemField>(FieldsAppTitleFieldId);

				//System.Console.WriteLine(FieldsAppTitleField.Label); // id of the field

				await podio.CommentOnItem("adams first comment", currentItem.ItemId, false);
				



			}

			catch (Exception ex)
			{
				Console.WriteLine($"Function {functionName}");
				Console.WriteLine($"Exception: {ex}");
			}

			finally
			{
				await functionLocker.UnlockFunction(functionName, uniqueId, lockId);
			}

			return new FunctionContainerResponse
			{
				message = "success"
			};
		}
		public async System.Threading.Tasks.Task Init(IServiceProvider services, IConfiguration configuration)
		{
			await System.Threading.Tasks.Task.CompletedTask;
		}
	}
}
