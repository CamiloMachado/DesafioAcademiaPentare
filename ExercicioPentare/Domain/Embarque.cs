using Microsoft.Xrm.Sdk;
using System.Collections.Generic;

namespace ExercicioPentare.Domain
{
    public class Embarque
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
    }
}