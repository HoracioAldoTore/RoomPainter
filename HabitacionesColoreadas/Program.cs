using System.Linq;
using System.Numerics;

namespace HabitacionesColoreadas
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //*** Dificultad : 1 ***
            //_Plano = new string[]
            //{
            //    "##### #######",
            //    "#           #",
            //    "#           #",
            //    "#           #",
            //    "#           #",
            //    "#           #",
            //    "#############"
            //};

            //***Dificultad : 2 * **
            //_Plano = new string[]
            //{
            //    "##### #######",
            //    "#           #",
            //    "#           #",
            //    "### #########",
            //    "#           #",
            //    "#           #",
            //    "#############"
            //};

            //*** Dificultad : 3 ***
            //_Plano = new string[]
            //{
            //    "#############",
            //    "#   #    #  #",
            //    "#   #    #  #",
            //    "## #### ### #",
            //    "#   #       #",
            //    "#           #",
            //    "#############"
            //};

            //*** Dificultad : 4 ***
            _Plano = new string[]
            {
                "###################",
                "#   #    #  #  #  #",
                "#   #    #  #  #  #",
                "#  ### ###  #     #",
                "#                 #",
                "#           #     #",
                "###################"
            };

            //*** Dificultad : 5 ***
            //_Plano = new string[]
            //{
            //    "## ############",
            //    "#   #         #",
            //    "#   #         #",
            //    "#   #  #####  #",
            //    "#   #  #   #  #",
            //    "#   #  #   #  #",
            //    "#   #  ## ##  #",
            //    "#   #      #  #",
            //    "#   #      #  #",
            //    "#   ########  #",
            //    "#             #",
            //    "#             #",
            //    "###############",
            //};

            //*** Dificultad : 6 ***
            //_Plano = new string[]
            //{
            //    "##########",
            //    "#   ###  #",
            //    "#    ##  #",
            //    "#     #  #",
            //    "###      #",
            //    "#        #",
            //    "#     #  #",
            //    "## #### ##",
            //    "#        #",
            //    "#        #",
            //    "#  #######",
            //    "#  #  #  #",
            //    "#  #  #  #",
            //    "#        #",
            //    "#        #",
            //    "##########"
            //};

            ValidarPlanoRectangular();
            ValidarPlanoCaracteresPermitidos();

            _CeldasProcesadas = new List<Celda>();

            int color = 1; //Color inicial es: DarkBlue
            Celda celdaCero = GetCeldaCero();
            while (celdaCero != null)
            {               
                celdaCero.Color = (ConsoleColor)color++;
                Contagiar(celdaCero);
                celdaCero = GetCeldaCero();
            };

            AdicionarParedesYPuertas();

            Imprimir();

            Console.ReadKey();
        }

        private static List<Celda> _CeldasProcesadas;

        private static string[] _Plano;

        /// <summary>
        /// Infecta las celdas vecinas.
        /// </summary>
        /// <param name="celdaCero">
        /// Es la primer celda infectada por un virus o color.
        /// </param>
        private static void Contagiar(Celda celdaCero)
        {
            List<Celda> vecinas = new List<Celda>();
            for(int deltaX = -1; deltaX < 2; deltaX++)
            {
                for (int deltaY = -1; deltaY < 2; deltaY++)
                {          
                    int tempX = celdaCero.X + deltaX;
                    int tempY = celdaCero.Y + deltaY;
                    if (!EsPuerta(tempX, tempY))
                    {
                        char caracter = _Plano[tempY][tempX];
                        bool esPiso = caracter == ' ';
                        if (esPiso)
                        {
                            bool existe = _CeldasProcesadas.Any(c => c.X == tempX && c.Y == tempY);
                            if (!existe)
                            {
                                Celda celda = new Celda();
                                celda.X = tempX;
                                celda.Y = tempY;
                                celda.Tipo = Tipo.Piso;
                                celda.Color = celdaCero.Color;
                                vecinas.Add(celda);
                                _CeldasProcesadas.Add(celda);
                            }                            
                        }
                    }                                    
                }               
            }
            foreach (Celda celda in vecinas)
            {
                Contagiar(celda);
            }
        }

        /// <summary>
        /// Evalúa una precondición, que el plano tenga una forma rectangular o cuadrada.
        /// </summary>
        /// <exception cref="Exception"></exception>
        private static void ValidarPlanoRectangular()
        {
            int ancho = _Plano[0].Length;
            int largo = _Plano.Length;
            for (int y = 0; y < largo; y++)
            {               
                bool esIgual = (ancho == _Plano[y].Length);
                if (!esIgual)
                {
                    string msgError = $"EL plano debe ser rectangular.Error en su fila: {y} = '{_Plano[y]}'";
                    throw new Exception(msgError);
                }               
            }
        }

        /// <summary>
        /// Valida que el plano contenga solo espacios y numerales.
        /// </summary>
        /// <exception cref="Exception"></exception>
        private static void ValidarPlanoCaracteresPermitidos()
        {
            for (int y = 0; y < _Plano.Length; y++)
                for (int x = 0; x < _Plano[0].Length; x++)
                {
                    char caracter = _Plano[y][x];
                    if(caracter != ' ' && caracter != '#') 
                    {
                        string msgError = $"EL plano solo puede contener espacios ' ' y numerales '#'. Se ha encontrado un carácter '{caracter}' no valido.";
                        throw new Exception(msgError);
                    }
                }
        }

        /// <summary>
        /// Adiciona al modelo las celdas que conforman las paredes y las puertas.
        /// </summary>
        private static void AdicionarParedesYPuertas()
        {
            for (int y = 0; y < _Plano.Length; y++)
                for (int x = 0; x < _Plano[0].Length; x++)
                {
                    char caracter = _Plano[y][x];
                    Celda celda = new Celda()
                    {
                        X = x,
                        Y = y                        
                    };
                    if (EsPuerta(x, y))
                    {
                        celda.Tipo = Tipo.Puerta;
                        _CeldasProcesadas.Add(celda);
                    }
                    bool esPared = (caracter == '#');
                    if (esPared) 
                    {
                        celda.Tipo = Tipo.Pared;
                        _CeldasProcesadas.Add(celda);
                    }
                }
        }

        /// <summary>
        /// Retorna la CeldaCero que es la primera celda infectada por un virus (color) y a partir de la cual se contagian las restantes.
        /// </summary>
        /// <remarks>
        /// El paciente cero, en epidemiología, es el primer humano infectado por un virus o enfermedad infecciosa
        /// </remarks>
        /// <returns></returns>
        private static Celda GetCeldaCero()
        {
            Celda celdaCero = null; //Es la primer celda infectada por un virus o color.
            
            for (int y = 0; y < _Plano.Length; y++)
                for (int x = 0; x < _Plano[0].Length; x++)
                {                    
                    char caracter = _Plano[y][x];
                    if(caracter == ' ' && !EsPuerta(x,y))
                    {
                        Celda celda = new Celda();
                        celda.X = x;
                        celda.Y = y;
                        celda.Tipo = Tipo.Piso;
            
                        bool exists = _CeldasProcesadas.Any(c => c.X == x && c.Y == y);
                        if(!exists)
                        {
                            celdaCero = celda; //Se encontro la celdaCero
                            break;
                        }                            
                    }
                    if (celdaCero != null) break;
                }

            return celdaCero;
        }

        /// <summary>
        /// Imprime el plano con un color por cada ambiente.
        /// </summary>
        private static void Imprimir()
        {
            foreach(Celda celda in _CeldasProcesadas)
            {
                Console.SetCursorPosition(celda.X, celda.Y);
                if (celda.Tipo == Tipo.Piso)
                {                    
                    Console.BackgroundColor = celda.Color;
                    Console.Write(' ');
                    Console.ResetColor();
                }
                else if(celda.Tipo == Tipo.Puerta)
                {
                    Console.Write(' ');
                }
                else if (celda.Tipo == Tipo.Pared)
                {
                    Console.Write('#');
                }
            }
        }
       
        /// <summary>
        /// Si la celda es una puerta horizontal o vertical retorna true.
        /// </summary>
        /// <param name="planoEnLineas"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private static bool EsPuerta(int x,int y)
        {
            bool esPuerta = false;
            try
            {
                //Puerta horizontal
                if (_Plano[y][x] == ' '
                &&
                _Plano[y][x - 1] == '#'
                &&
                _Plano[y][x + 1] == '#')
                {
                    esPuerta = true;
                }

                //Puerta vertical
                if (_Plano[y][x] == ' '
                &&
                _Plano[y-1][x] == '#'
                &&
                _Plano[y+1][x] == '#')
                {
                    esPuerta = true;
                }
            }
            catch { }
            
            return esPuerta;
        }       
    }
}