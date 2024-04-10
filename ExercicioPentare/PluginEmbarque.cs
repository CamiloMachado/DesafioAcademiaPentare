using ExercicioPentare.Domain;
using ExercicioPentare.Helpers;
using Microsoft.Xrm.Sdk;
using System;

namespace ExercicioPentare
{
    public class PluginEmbarque : IPlugin
    {
        private ITracingService _tracingService;

        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService adminService = serviceFactory.CreateOrganizationService(null);
            _tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            if (CreatePreValidation(context, adminService) == false) { return; }
            if (UpdatePreValidation(context, adminService) == false) { return; }
        }

        private bool CreatePreValidation(IPluginExecutionContext context, IOrganizationService adminService)
        {
            if (PluginBase.Validate(context, PluginBase.MessageName.Create, PluginBase.Stage.PreValidation, PluginBase.Mode.Synchronous))
            {
                if (!context.InputParameters.Contains("Target") || !(context.InputParameters["Target"] is Entity)) { return false; }

                Entity target = context.InputParameters["Target"] as Entity;

                Embarque embarque = new Embarque(adminService);
                embarque.ValidarCamposPreenchidos(target);
                embarque.VerificarAssentoEsixtentePorAviao(target);
            }
            return true;
        }

        private bool UpdatePreValidation(IPluginExecutionContext context, IOrganizationService adminService)
        {
            if (PluginBase.Validate(context, PluginBase.MessageName.Update, PluginBase.Stage.PreValidation, PluginBase.Mode.Synchronous))
            {
                if (!context.InputParameters.Contains("Target") && !(context.InputParameters["Target"] is Entity)) { return false; }
                if (!context.PreEntityImages.Contains("PreImage") && !(context.PreEntityImages["PreImage"] is Entity)) { return false; }

                Entity target = context.InputParameters["Target"] as Entity;
                Entity preImage = context.PreEntityImages["PreImage"];

                Embarque embarque = new Embarque(adminService);

                embarque.ValidarCamposPreenchidos(target, preImage);
                embarque.VerificarAssentoEsixtentePorAviao(target, preImage);
            }
            return true;
        }
    }
}