# template
A template for creating Saasafras functions for Podio.
This is a generic template with needed Nugets and set up for work within Podio. 
Rename the aws....json file with your function name. Remove "TemplateFunction" from the name and replace it with the name of your function.
Rename TemplateFunction.cs to your function name. 
Rename the namespace to your function name, with Container at the end. {functionName}Container
StartUp.cs namespace needs to be Saasafras.Event.Container
