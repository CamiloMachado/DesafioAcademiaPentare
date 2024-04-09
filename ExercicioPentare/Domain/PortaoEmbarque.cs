using ExercicioPentare.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExercicioPentare.Domain
{
    internal class PortaoEmbarque
    {
        public static int Portao()
        {
            var values = Enum.GetValues(typeof(Enumerador.Embarque.Portao));

            var random = new Random();
            var randomPortao = values.GetValue(random.Next(values.Length)).ToString();

            int portao = 0;

            foreach (var value in values)
            {
                if (randomPortao == value.ToString())
                {
                    portao = (int)value;
                }
            }

            return portao;
        }
    }
}