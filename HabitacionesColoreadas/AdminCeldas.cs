using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitacionesColoreadas
{
    internal class AdminCeldas
    {
        private List<string> coordenadas = new List<string>();

        public void Add(Celda celda)
        {
            if(!Exists(celda))
                coordenadas.Add($"{celda.X}-{celda.Y}");
        }

        public bool Exists(Celda celda)
        {
            bool existe = coordenadas.Contains($"{celda.X}-{celda.Y}");
            return existe;
        }
    }
}
