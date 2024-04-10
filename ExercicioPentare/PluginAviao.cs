using Microsoft.Xrm.Sdk;
using System;
using ExercicioPentare.Helpers;
using ExercicioPentare.Domain;

namespace ExercicioPentare
{
    public class PluginAviao : IPlugin
    {
        private ITracingService _tracingService;

        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService adminService = serviceFactory.CreateOrganizationService(null);
            _tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            if (CreatePostOperation(context, adminService) == false) { return; }
        }

        private bool CreatePostOperation(IPluginExecutionContext context, IOrganizationService adminService)
        {
            if (PluginBase.Validate(context, PluginBase.MessageName.Create, PluginBase.Stage.PostOperation, PluginBase.Mode.Synchronous))
            {
                if (!context.InputParameters.Contains("Target") || !(context.InputParameters["Target"] is Entity)) { return false; }

                Entity target = context.InputParameters["Target"] as Entity;

                Aviao aviao = new Aviao(adminService);
                aviao.CriarEmbarquesAoGerarRegistroAviao(target);
            }
            return true;
        }
    }
}