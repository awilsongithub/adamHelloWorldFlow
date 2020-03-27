# template
A template for creating Saasafras functions for Podio.
This is a generic template with needed Nugets and set up for work within Podio. 

Rename TemplateFunction.cs to your function name. 
Start the name of the function with the client name, and then what the function does.
Update var functionName (roughly line 67) inside TemplateFunction.cs

Rename the namespace to your function name, with Container at the end. {functionName*}Container
StartUp.cs namespace needs to be Saasafras.Event.Container

To test locally, make the following updates in LambdaContainer.cs Main() method 
Get the JWT for the environment. //Get JWT from Alex.
ILambdaContext context = new LocalLambdaContext("FunctionName*.aws", "$LATEST", clientContext);
var container = PodioContainerFactory.GetPodioContainer("FunctionName*");
Inside var command
solutionId = "theEnvironmentsSolutionId", //Can find in aws or get from Alex.
clientIds = new[] { "theEnvironmentClientId" }, //same as solutionId. 
target = "FunctionName*:DEV",
source = "WorkspaceName|AppName", //the workspace and app that triggers the function inside Podio.
item_id = "5555555555", //the item id that you are using for testing. This can be found inside the item in Podio under Actions>Developer.

Rename the aws....json file with your function name. Remove "TemplateFunction" from the name and replace it with the name of your function.
Inside aws...json file, update the function-handler to 
"ProjectName*::NamespaceName*.LambdaContainer::Handler"
Inside aws...json file, update the function-name to
"FunctionName*"

**"FunctionName" and all similar examples need to be what you named YOUR function. FuntionName in here is a placeholder.

Write your function code inside the try{} block.
