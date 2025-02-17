/* Datos del equipo...
    Ejercicio 3 Juego de Reversi.
    Grupo: 4C.
    Fecha: 11 de febrero de 2025.
    Integrantes:
        - Toledo Herrera, Neyzer Joel.
        - Martin Rojas, Carlos Ariel.
        - Monarrez Barron, Polo Alejandro.
*/

namespace reversi;
class Program {
    static void Main(String[] args) 
    {
        Console.Title = "Reversi";
        do
        {
            ConsoleColor[] colorJugadores = new ConsoleColor[2];
            string[] nombres = new string[2];
            string[,] tablero = new string[8, 8];
            
            for (int i = 0; i < 2; i++) {
                Console.Clear();

                Console.Write($"Ingresa el nombre del jugador {i + 1}: ");
                nombres[i] = Console.ReadLine().ToUpper();
                Console.Clear();
                
                colorJugadores[i] = PedirColor($"{nombres[i]}, selecciona un color: ", colorJugadores[0]);
                Console.Write("> ");        
            }

            InicializarTablero(tablero);

            Jugar(tablero, nombres, colorJugadores);
    
            JuegoTerminado(tablero, nombres, colorJugadores);

        } while (ValidarContinuacion());

        Console.WriteLine("Adios :)");

    }

    // ----TABLERO
    static void InicializarTablero(string[,] tablero) 
    {
        //Centro
        tablero[3,3] = "X";
        tablero[4,4] = "X";
        tablero[3,4] = "O";
        tablero[4,3] = "O";

        //Esquinas
        tablero[0,0] = "X";
        tablero[7,7] = "X";
        tablero[0,7] = "O";
        tablero[7,0] = "O";

        //Test
        // for (int i = 0; i < 8; i++)
        // {
        //     for (int j = 0; j < 8; j++)
        //     {
        //         if(tablero[i,j] == null && i != 3)
        //             tablero[i,j] = Jugador(i);
        //     }
            
        // }

    }
    static void ImprimirTablero(string[,] tablero, ConsoleColor[] colorJugadores) 
    {
        
        Console.WriteLine("\n  A B C D E F G H");

        for (int i = 0; i < 8; i++) {
            Console.Write(i+1);

            for (int j = 0; j < 8; j++) {
                Console.Write(" ");


                if (tablero[i, j] == "X") 
                    ImprimirColor("0", colorJugadores[0]);
                else if (tablero[i, j] == "O")
                    ImprimirColor("0", colorJugadores[1]);
                else
                    Console.Write("-");
                
            }

            Console.WriteLine();
        }
    }

    // ----JUEGO
    static void Jugar(string[,] tablero, string[] nombres, ConsoleColor[] colorJugadores)
    {
        bool juegoTerminado;
        
        int jugadorActual = 0;
        Console.Clear();

        Console.SetCursorPosition(25,2);
        Console.Write("Ingresa cordenadas en formato Letra/Numero\n");

        Console.SetCursorPosition(25,3);
        Console.Write("Ejemplo: A4, f6, b1\n");

        Console.SetCursorPosition(0,12);
        do {
            
            Console.SetCursorPosition(0,0);
            Console.Write(Spaces(Console.WindowWidth));
            Console.SetCursorPosition(0,0);
            // [{Jugador(jugadorActual)}]
            ImprimirColor($"\tTurno de {nombres[jugadorActual]}\n", colorJugadores[jugadorActual]);
            ImprimirTablero(tablero, colorJugadores);

            PedirCoordenadas(tablero, jugadorActual, colorJugadores);
            jugadorActual = (jugadorActual+1) % 2;                                         
    
            juegoTerminado = !HayMovimientosDisponibles(tablero);

        } while (!juegoTerminado);
    }

    static void JuegoTerminado(string[,] tablero, string[] nombres, ConsoleColor[] colorJugadores)
    {
        int fichasJugador1 = ContarFichas(tablero, 0);
        int fichasJugador2 = ContarFichas(tablero, 1);

        Console.SetCursorPosition(0,0);
        Console.Write(Spaces(Console.WindowWidth));
        if (fichasJugador1 > fichasJugador2)
        {
            ImprimirColor($"{nombres[0]} gana!\n", colorJugadores[0]);
        }
        else if (fichasJugador2 > fichasJugador1)
        {
            ImprimirColor($"{nombres[1]} gana!\n", colorJugadores[1]);
        }
        else
        {
            Console.WriteLine("Empate!");
        }
        ImprimirTablero(tablero, colorJugadores);

        Console.WriteLine("\nFin del juego!");
        ImprimirColor($"{nombres[0]}: {fichasJugador1} fichas.", colorJugadores[0]); 
        ImprimirColor($"\n{nombres[1]}: {fichasJugador2} fichas.", colorJugadores[1]);
    }
    static bool EsMovimientoValido(string[,] tablero, int fila, int columna) {
        if(!(fila >= 0 && fila <=7) || !(columna >= 0 && columna <=7))
            return false;

        return tablero[fila,columna] == null;
    }

    static void Reversi(string[,] tablero, int ultimaFila, int ultimaColumna)
    {
        string[,] tab = tablero;
        int fil = ultimaFila;
        int col = ultimaColumna;

        string jugador = tab[fil,col];

        //up, down, left right
        string[] direcciones = ["u", "d", "l", "r"];
        
        foreach (string dir in direcciones)
            EncontrarPareja(tab,jugador,fil, col, dir);
        

    }

    static Boolean EncontrarPareja(String[,] tab, String jug, int fila, int col, String dir)
    {
        int x = 0, y = 0;

        switch (dir.ToLower())
        {
            case "u": y=-1; break;
            case "d": y=1; break;
            case "l": x=-1; break;
            case "r": x=1; break;
            default: 
                Console.WriteLine("Error direccion:"+dir); 
                break;
        }

        String sigCasilla;
        String jugReversa = JugadorReversa(jug);
        try
        {
            sigCasilla = tab[fila+y,col+x];
            
        }
        catch (System.Exception)
        {
            return false;
        }

        // o jugador, o nulo
        if(sigCasilla == null)
            return false;

        if(sigCasilla == jugReversa)
            if(EncontrarPareja(tab, jug, fila+y, col+x, dir)){
                tab[fila+y,col+x] = jug;
                Reversi(tab, fila+y, col+x);
                return true;
            }
        // 
        return jug == sigCasilla;

    }

    static void ColocarFicha(string[,] tablero, int fila, int columna, int jugador) {
        String[] simbolos = ["X","O"];
        tablero[fila,columna] = simbolos[jugador];
    }
    
    //---DATOS

    static void ImprimirColor(String msg, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.Write(msg);
        Console.ResetColor();
    }

    static ConsoleColor PedirColor(String msg, ConsoleColor? noUsar)
    {
        ConsoleColor[] colors = [
            ConsoleColor.Red,
            ConsoleColor.Blue,
            ConsoleColor.Magenta,
            ConsoleColor.Green,
            ConsoleColor.Cyan,
            ConsoleColor.Yellow
        ];

        ConsoleColor color;


        Console.WriteLine(msg);
        ImprimirColores(colors, noUsar);
        Console.Write("> ");

        do {
            String error = "Ingresa una opcion valido";
            try
            {
                int index = Int32.Parse(Console.ReadLine());

                if(index < 1 || index > colors.Length)
                    throw new Exception();

                color = colors[index-1];

                if(color == noUsar) 
                {
                    error = "Este color ya ha sido seleccionado";
                    throw new Exception();
                }
                break;
            }
            catch (System.Exception)
            {
                Console.SetCursorPosition(2, 3);
                Console.WriteLine(Spaces(10));
                Console.WriteLine(error);
                Console.SetCursorPosition(2, 3);
            }

        } while(true);

        return color;
        
        
    }
    static void PedirCoordenadas(string[,] tablero, int jugador, ConsoleColor[] colorJugadores) 
    {
        int columna, fila;
        String pos = "";
        String msg = "";

        Console.SetCursorPosition(0,12);
        Console.Write("> ");


        do
        {

            Console.SetCursorPosition(2,12);
            Console.Write(Spaces(pos.Length));
            Console.SetCursorPosition(2,12);
            Console.ForegroundColor = colorJugadores[jugador];
            pos = Console.ReadLine().ToUpper();
            
            Console.ResetColor();
            try
            {            
                columna = pos[0]-65;
                fila = Int32.Parse(pos[1].ToString())-1;

                if (!EsMovimientoValido(tablero, fila, columna)) {
                    throw new Exception();
                }
            }
            catch (System.Exception)
            {
                Console.SetCursorPosition(25,6);
                msg = "Movimiento inválido... Intente nuevamente.";
                Console.Write(msg);
                Console.SetCursorPosition(0,12);
                continue;
            }
            
            ColocarFicha(tablero, fila, columna, jugador);
            Reversi(tablero, fila, columna);    
            
            
            Console.SetCursorPosition(25,6);
            Console.Write(Spaces(msg.Length));
            Console.SetCursorPosition(0,12);
            Console.Write(Spaces(10));
            Console.SetCursorPosition(0,12);
            break;
        } while (true);
    }

    static void ImprimirColores(ConsoleColor[] colors, ConsoleColor? noUsar) 
    {
        int i = 1;
        foreach (ConsoleColor color in colors) 
        {
            if(noUsar == color) 
            {
                i++;
                continue;
            }

            Console.Write($"{i}.");
            ImprimirColor(color.ToString()+"\t", color);

            if(i==3) Console.WriteLine();

            i++;
        }

        Console.WriteLine();
    }
    
    //---TERMINAR JUEGO
    static bool HayMovimientosDisponibles(string[,] tablero) {
        for (int i = 0; i < 8; i++) {
            for (int j = 0; j < 8; j++) {
                if (EsMovimientoValido(tablero, i, j)) {
                    return true;
                }
            }
        }
        return false;
    }
    static int ContarFichas(string[,] tablero, int jugador) {
        int contador = 0;
        string fichaJugador = Jugador(jugador);
        for (int i = 0; i < 8; i++) {
            for (int j = 0; j < 8; j++) {
                if (tablero[i, j] == fichaJugador) {
                    contador++;
                }
            }
        }
        return contador;
    }
  
    //---Jugador        
    static String JugadorReversa(String simbolo)
    {
        return Jugador(NumJugador(simbolo)+1);
    }

    static String Jugador(int i)
    {
        switch (i%2)
        {
            case 0: return "X";
            case 1: return "O";
            default: return "-";
        }
    }
    
    static int NumJugador(String simbolo)
    {
        switch (simbolo[0])
        {
            case 'X': return 0;
            case 'O': return 1;
            default: return -1;
        }
    }

    static String Spaces(int cant)
    {
        String space = "";
        for (int i = 0; i < cant; i++)
        {
            space += " ";
        }
        return space;
    }

    static bool ValidarContinuacion()
        {
            String respuesta;
            bool validacion;

            Console.Write("\n¿Desea realizar otro juego? (S/n): ");

            // guardar posicion del cursor antes de leer el numero
            int posx = Console.CursorLeft;
            int posy = Console.CursorTop;

            do{
                // borrar lo que se haya escrito por teclado
                PrintXY(posx, posy, Spaces(Console.WindowWidth));
                PrintXY(posx, posy, "");
                respuesta = Console.ReadLine().ToLower();

                //validar que escribio S o N
                validacion = respuesta.Equals("s") || respuesta.Equals("n");
            } while (!validacion);

            return respuesta.Equals("s");
        }

    static void PrintXY(int x, int y, string text)
{
    Console.SetCursorPosition(x,y);
    Console.Write(text);
}  

}