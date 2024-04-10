using Microsoft.Xrm.Sdk;
using System;
using System.Linq;

namespace ExercicioPentare.Domain
{
    internal class Aviao
    {
        private readonly IOrganizationService _service;

        public Aviao(IOrganizationService service)
        {
            _service = service;
        }

        public void CriarEmbarquesAoGerarRegistroAviao(Entity target)
        {
            Random random = new Random();
            var assentos = Enumerable.Range(1, 10);

            foreach (var assento in assentos)
            {
                try
                {
                    Entity embarque = new Entity("academia_embarque");
                    embarque["academia_portao"] = new OptionSetValue(random.Next(1, 4));
                    embarque["academia_assento"] = new OptionSetValue(assento);
                    embarque["academia_aviaoid"] = new EntityReference(target.LogicalName, target.Id);
                    _service.Create(embarque);
                }
                catch (InvalidPluginExecutionException er)
                {
                    throw new InvalidPluginExecutionException(er.Message);
                }
            }
        }
    }
}