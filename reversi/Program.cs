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
    static void Main(String[] args) {
        Console.Clear();

        /*
            Si les esto ney, falta agregar una que otra validdacion
        */

        ConsoleColor[] colors = [
            ConsoleColor.Red,
            ConsoleColor.Blue,
            ConsoleColor.Magenta,
            ConsoleColor.Yellow,
            ConsoleColor.Green,
            ConsoleColor.Cyan
        ];
        
        ConsoleColor[] colorJugadores = new ConsoleColor[2];
        string[] nombres = new string[2];
        string[,] tablero = new string[8, 8];
        
        // for (int i = 0; i < 2; i++) {
        //     Console.Write($"Ingresa el nombre del jugador {i + 1}: ");
        //     nombres[i] = Console.ReadLine();
        //     Console.WriteLine($"{nombres[i]}, selecciona un color: ");
        //     PrintColors(colors);
        //     Console.Write("> ");
        //     colorJugadores[i] = colors[Int32.Parse(Console.ReadLine()) -1];
        // }

        nombres = ["pepe", "tilin"];
        colorJugadores = [colors[0], colors[1]];

        InicializarTablero(tablero);

        bool juegoTerminado;
        
        int jugadorActual = 0;

        do {
            char symbol = jugadorActual == 0 ? 'X' : 'O';

            ImprimirColor($"Turno de {nombres[jugadorActual]} ({symbol})\n", colorJugadores[jugadorActual]);
            PrintTable(tablero, colorJugadores);

            PedirCoordenadas(tablero, jugadorActual, colorJugadores);
            jugadorActual = (jugadorActual+1) % 2;                                         
    
            juegoTerminado = !HayMovimientosDisponibles(tablero);

        } while (!juegoTerminado);

        PrintTable(tablero, colorJugadores);
        int fichasJugador1 = ContarFichas(tablero, 1);
        int fichasJugador2 = ContarFichas(tablero, 2);

        Console.WriteLine($"Fin del juego! {nombres[0]}: {fichasJugador1} fichas, {nombres[1]}: {fichasJugador2} fichas.");
        if (fichasJugador1 > fichasJugador2)
        {
            Console.WriteLine($"{nombres[0]} gana!");
        }
        else if (fichasJugador2 > fichasJugador1)
        {
            Console.WriteLine($"{nombres[1]} gana!");
        }
        else
        {
            Console.WriteLine("Empate!");
        }
    }

    static void InicializarTablero(string[,] tablero) {
        tablero[3, 1] = "X";
        tablero[3, 2] = "O";
        tablero[3, 3] = "O";
        tablero[3, 4] = "O";
        tablero[3, 5] = "O";
        tablero[3, 7] = "X";

        tablero[0,3] = "X";
        tablero[1,3] = "O";
        tablero[2,3] = "O";


    }
    static void PrintTable(string[,] tablero, ConsoleColor[] colorJugadores) {
        
        Console.WriteLine("\n  A B C D E F G H");

        for (int i = 0; i < 8; i++) {
            Console.Write(i+1);

            for (int j = 0; j < 8; j++) {
                Console.Write(" ");


                if (tablero[i, j] == "X") 
                    ImprimirColor("X", colorJugadores[0]);
                else if (tablero[i, j] == "O")
                    ImprimirColor("O", colorJugadores[1]);
                else
                    Console.Write("-");
                
            }

            Console.WriteLine();
        }
    }

    static void ImprimirColor(String msg, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.Write(msg);
        Console.ResetColor();
    }

    static void PrintColors(ConsoleColor[] colors) {
        int i = 1;
        foreach (ConsoleColor color in colors) {
            Console.Write($"{i}.");
            ImprimirColor(color.ToString()+" ", color);
            i++;
        }
        Console.WriteLine();
    }
    static void PedirCoordenadas(string[,] tablero, int jugador, ConsoleColor[] colorJugadores) {
        Console.Write("Ingrese la fila-columna (a1): ");
        String pos = Console.ReadLine().ToUpper();

        int columna = pos[0]-65;
        int fila = Int32.Parse(pos[1].ToString())-1;

        if (EsMovimientoValido(tablero, fila, columna)) {
            ColocarFicha(tablero, fila, columna, jugador, colorJugadores);
            Reversi(tablero, fila, columna);
        } else {
            Console.WriteLine("Movimiento inválido... Intente nuevamente.");
            PedirCoordenadas(tablero, jugador, colorJugadores);
        }
    }
    static bool EsMovimientoValido(string[,] tablero, int fila, int columna) {
        if(!(fila >= 0 && fila <=7) || !(columna >= 0 && columna <=7))
            return false;

    return tablero[fila,columna] == null;
    }

    static void ColocarFicha(string[,] tablero, int fila, int columna, int jugador, ConsoleColor[] colorJugadores) {
        String[] simbolos = ["X","O"];
        tablero[fila,columna] = simbolos[jugador];
    }
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

    static void Reversi(string[,] tablero, int ultimaFila, int ultimaColumna)
    {
        string[,] t = tablero;
        int f = ultimaFila;
        int c = ultimaColumna;

        string jugador = t[f,c];

        //up, down, left right
        string[] direcciones = ["u", "d", "l", "r"];
        
        foreach (string dir in direcciones)
        {
            EncontrarPareja(t,jugador,f, c, dir);

        }

    }

    static Boolean EncontrarPareja(String[,] tablero, String jugador, int f, int c, String direccion)
    {
        int x = 0, y = 0;

        switch (direccion.ToLower())
        {
            case "u": y=-1; break;
            case "d": y=1; break;
            case "l": x=-1; break;
            case "r": x=1; break;
            default: 
                Console.WriteLine("Error direccion:"+direccion); 
                break;
        }

        String siguienteCasilla;
        String jugadorReversa = JugadorReversa(jugador);
        try
        {
            siguienteCasilla = tablero[f+y,c+x];
            
        }
        catch (System.Exception)
        {
            return false;
        }

        // o jugador, o nulo
        if(siguienteCasilla == null)
            return false;

        if(siguienteCasilla == jugadorReversa)
            if(EncontrarPareja(tablero, jugador, f+y, c+x, direccion)){
                tablero[f+y,c+x] = jugador;
                Reversi(tablero, f+y, c+x);
                return true;
            }
        // 
        return jugador == siguienteCasilla;

    }

        
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


}
