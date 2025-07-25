using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitacionesColoreadas
{
    internal class Celda
    {
        public int X { get; set; }
        public int Y { get; set; }
     
        public Tipo Tipo { get; set; }

        public ConsoleColor Color { get; set; } = ConsoleColor.Red;
    }
}
