using ExercicioPentare.Helpers;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace ExercicioPentare.Domain
{
    internal class Embarque
    {
        private readonly IOrganizationService _service;

        public Embarque(IOrganizationService service)
        {
            _service = service;
        }

        public void ValidarCamposPreenchidos(Entity target, Entity preImage = null)
        {
            string[] atributosObrigatorios = new string[]
            {
                "academia_portao",
                "academia_assento",
                "academia_aviaoid"
            };

            List<string> camposObrigatoriosNaoPreenchidos = new List<string>();

            foreach (var atributo in atributosObrigatorios)
            {
                var campoPreenchido = target.Contains(atributo) ? target[atributo] != null :
                    preImage != null && preImage.Contains(atributo) && preImage[atributo] != null;

                if (!campoPreenchido)
                {
                    camposObrigatoriosNaoPreenchidos.Add(atributo);
                }

                if (camposObrigatoriosNaoPreenchidos.Count > 0)
                {
                    throw new InvalidPluginExecutionException($"{string.Join(",", camposObrigatoriosNaoPreenchidos)} são obrigatórios!");
                }
            }
        }

        public void VerificarAssentoEsixtentePorAviao(Entity target, Entity preImage = null)
        {
            var assento = target.Contains("academia_assento") ?
                target.GetAttributeValue<OptionSetValue>("academia_assento") : preImage.GetAttributeValue<OptionSetValue>("academia_assento");

            var aviao = target.Contains("academia_aviaoid") ?
                target.GetAttributeValue<OptionSetValue>("academia_aviaoid") : preImage.GetAttributeValue<OptionSetValue>("academia_aviaoid");

            if (aviao != null && assento != null)
            {
                QueryExpression queryExpression = new QueryExpression("academia_embarque");
                queryExpression.Criteria.AddCondition("academia_assento", ConditionOperator.Equal, assento.Value);
                queryExpression.Criteria.AddCondition("academia_aviaoid", ConditionOperator.Equal, aviao.Value);
                var embarques = _service.RetrieveMultiple(queryExpression);

                if (embarques.Entities.Count > 0)
                {
                    throw new InvalidPluginExecutionException("Não deve existir um resgistro com assento e avião existentes!");
                }
            }
        }
    }
}