using System;
using Amazon.CloudWatchLogs;
using Microsoft.Extensions.DependencyInjection;
using PodioCore;
using Saasafras;
using Saasafras.AWS.Lambda;
using Saasafras.Event.Interfaces;
using Saasafras.Interfaces;
using Saasafras.Lambda.Interfaces;
using Saasafras.Model;

namespace Saasafras.Event.Container
{
    public class StartUp : IConfigureServices
    {
        public IServiceCollection Configure(IServiceCollection services)
        {
            services.AddScoped<IAmazonCloudWatchLogs, AmazonCloudWatchLogsClient>();
            services.AddScoped<IEventHandler<SaasafrasSolutionCommand<SaasafrasPodioEvent>>, adamHelloWorldFlowContainer.MyHandler>();
            services.AddScoped<ISolutionLoggerFactory, LambdaSolutionLoggerFactory>();
            services.AddScoped<ICommandLambdaMapper, DefaultCommandLambdaMapper>();
            services.AddScoped<IPreprocess<SaasafrasSolutionCommand<SaasafrasPodioEvent>>, PodioContainerPreprocessor>();
            services.AddScoped<ICommandClient, LambdaCommandClient>();
            services.AddScoped<IFunctionLocker, FunctionLocker>();
            return services;
        }
    }
}
