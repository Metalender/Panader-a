using System.ComponentModel;
using static System.Console;

namespace consola
{
    public class View
    {
        public View()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
        }
        const string CANCELINPUT = "fin";
        public List<T> EnumToList<T>() => new List<T>(Enum.GetValues(typeof(T)).Cast<T>());
        public void LimpiarPantalla() => Clear();
        public void MostrarYReturn(Object obj, ConsoleColor color = ConsoleColor.White)
        {
            ForegroundColor = color;
            Write(obj.ToString() + " ");
            ForegroundColor = ConsoleColor.White;
            ReadLine();
        }
        public void Mostrar(Object obj, ConsoleColor color = ConsoleColor.White)
        {
            ForegroundColor = color;
            WriteLine(obj.ToString());
            ForegroundColor = ConsoleColor.White;
        }
        public void MostrarListaEnumerada<T>(string titulo, List<T> datos)
        {
            Mostrar(titulo, ConsoleColor.Yellow);
            WriteLine();
            for (int i = 0; i < datos.Count; i++)
            {
                WriteLine($"  {i + 1,3:###}.- {datos[i].ToString()}");
            }
            WriteLine();
        }

        public T TryObtenerDatoDeTipo<T>(string prompt, string @default = "")
        {
            var msg = prompt.Trim() + ": ";
            if (@default != "") msg += "\b\b (" + @default + "): ";

            while (true)
            {
                Write(msg);
                var input = ReadLine();
                if (input.ToLower().Trim() == CANCELINPUT) throw new Exception("Entrada cancelada por el usuario");
                if (input == "") input = @default;
                try
                {
                    var valor = TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(input);
                    return (T)valor;
                }
                catch (Exception)
                {
                    if (input != "")
                        Mostrar($"Error: '{input}' no reconocido como entrada permitida", ConsoleColor.DarkRed);
                }
            }
        }
        public int TryObtenerValorEnRangoInt(int min, int max, string prompt)
        {
            int input = int.MaxValue;
            while (input < min || input > max)
                try
                {
                    input = TryObtenerDatoDeTipo<int>(prompt);
                }
                catch (Exception)
                {
                    throw;
                };
            return input;
        }
        public T TryObtenerElementoDeLista<T>(string titulo, List<T> datos, string prompt)
        {
            MostrarListaEnumerada(titulo, datos);
            try
            {
                var input = TryObtenerValorEnRangoInt(1, datos.Count, prompt);
                return datos[input - 1];
            }
            catch (Exception)
            {
                throw;
            };
        }
        public (int x, int y) TryObtenerTuplaInt(string prompt, (int xMax, int yMax) limites)
        {
            var msg = prompt.Trim() + ": ";
            while (true)
            {
                Write(msg);
                var input = ReadLine();
               
                if (input.ToLower().Trim() == CANCELINPUT) throw new Exception("Entrada cancelada por el usuario");
                try
                {
                    var valores = input.Split(",");
                    if (valores.Length != 2) throw new Exception();
                    var x = Int16.Parse(valores[0]);
                    var y = Int16.Parse(valores[1]);
                    if (x < 0 || x >= limites.xMax) throw new Exception();
                    if (y < 0 || y >= limites.yMax) throw new Exception();
                    return (x: x, y: y);
                }
                catch (Exception)
                {
                    if (input != "")
                        Mostrar($"Error: '{input}' no reconocido como entrada permitida", ConsoleColor.DarkRed);
                }
            }
        }
        public int[] TryObtenerArrayInt(string prompt, int size, char separador = ',')
        {
            var msg = prompt.Trim() + ": ";
            while (true)
            {
                Write(msg);
                var input = ReadLine();
             
                if (input.ToLower().Trim() == CANCELINPUT) throw new Exception("Entrada cancelada por el usuario");
                try
                {
                    var valores = input.Split(separador);
                    if (valores.Length != size) throw new Exception();
                    var ints = new int[size];
                    for (var i = 0; i < valores.Length; i++)
                        ints[i] = Int16.Parse(valores[i]);
                    return ints;
                }
                catch (Exception)
                {
                    if (input != "")
                        Mostrar($"Error: '{input}' no reconocido como entrada permitida", ConsoleColor.DarkRed);
                }
            }
        }
        public DateTime TryObtenerFecha(string prompt)
        {
            var promptF = prompt.Trim() + " (d/m/a4): ";
            while (true)
            {
                var input = TryObtenerArrayInt(promptF, 3, '/');
                try
                {
                    return new DateTime(input[2], input[1], input[0], 0, 0, 0);
                }
                catch (Exception)
                {
                    Mostrar($"Error: '{input[0]}/{input[1]}/{input[2]}' no reconocido como fecha permitida", ConsoleColor.DarkRed);
                }
            }
        }
        public char TryObtenerCaracterDeString(string prompt, string opciones, char predeterminado = 'S')
        {
            var msg = prompt.Trim() + " (" + predeterminado + "): ";
            while (true)
            {
                Write(msg);
                var input = ReadLine();
                
                if (input.ToLower().Trim() == CANCELINPUT) throw new Exception("Entrada cancelada por el usuario");
                if (input == "") input = predeterminado.ToString();
                try
                {
                    if (input.Length != 1) throw new Exception();
                    var c = input.ToUpper()[0];
                    if (!opciones.Contains(c)) throw new Exception();
                    return c;
                }
                catch (Exception)
                {
                    Mostrar($"Error: '{input}' no reconocido como valor permitido en {opciones}", ConsoleColor.DarkRed);
                }
            }
        }

    }
}