using Microsoft.Xrm.Sdk;
using System;
using ExercicioPentare.Helpers;
using System.Collections.Generic;
using System.Linq;
using ExercicioPentare.Domain;
using System.Runtime.Remoting.Services;

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

            try
            {
                if (CreatePostOperation(context, adminService) == false) { return; }
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException($"Ocorreu um erro ao executar {this.GetType().FullName}: \n{ex.Message}\n\n  - {ex.StackTrace}");
            }
        }

        private bool CreatePostOperation(IPluginExecutionContext context, IOrganizationService adminService)
        {
            if (PluginBase.Validate(context, PluginBase.MessageName.Create, PluginBase.Stage.PostOperation, PluginBase.Mode.Synchronous))
            {
                if (!context.InputParameters.Contains("Target") || !(context.InputParameters["Target"] is Entity target)) { return false; }

                List<Enumerador.Embarque.Assento> assentos = Enum.GetValues(typeof(Enumerador.Embarque.Assento)).OfType<Enumerador.Embarque.Assento>().ToList();

                var portao = PortaoEmbarque.Portao();

                foreach (var assento in assentos)
                {
                    try
                    {
                        Entity embarque = new Entity("academia_embarque");
                        embarque["academia_portao"] = new OptionSetValue(portao);
                        embarque["academia_assento"] = new OptionSetValue(assento.GetHashCode());
                        embarque["academia_aviaoid"] = new EntityReference(target.LogicalName, target.Id);
                        adminService.Create(embarque);
                    }
                    catch (InvalidPluginExecutionException er)
                    {
                        throw new InvalidPluginExecutionException(er.Message);
                    }
                }
            }
            return true;
        }
    }
}