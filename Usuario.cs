using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace VideoClub
{
    public class Usuario
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["videoClub"].ConnectionString;
        static SqlConnection connection = new SqlConnection(connectionString);

        public string Mail { get; set; }
        public string Contra { get; set; }
        public int Age { get; set; }


        public Usuario()
        {
        }

        public void inicio()
        {
            Console.Write(@"                                                                                        
                               ████████                                      
                              ████████████░░░░░░░░██                                    
                          ████░░░░░░░░██░░░░░░████                                      
                        ██░░░░░░░░░░░░░░░░░░██░░██                                      
                      ██░░░░░░░░░░░░░░░░░░░░░░░░░░██                                    
                    ██░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░██                                  
                    ██░░░░██░░░░░░░░░░░░░░░░░░░░░░░░░░██                                
                    ██░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░██                                
                    ██░░░░░░░░░░░░░░░░░░██░░░░░░░░░░░░░░██                              
                  ██░░░░░░██████████░░░░██████░░░░░░░░░░██                              
                  ██░░░░██          ██░░░░██  ████░░░░░░██                              
                    ████              ██░░██      ██░░░░░░██                            
                                        ████        ██░░░░░░██                          
                                                    ██░░░░░░░░████                      
                                                      ██░░░░░░░░░░██                    
                                                      ██░░████░░░░░░██                  
                                                      ██░░██  ████████                  
                                                      ██░░██                            
                                                        ██                              
                                                                                        
                                                                                                                                                                              
                                                                                        
░░      ░░    ░░  ░░    ░░      ░░      ░░      ░░  ░░  ░░      ░░      ░░      ░░
            ");
            Console.WriteLine();
            Console.WriteLine("🐬 VHS CLUB 🐬");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n¿Qué quieres hacer?");
            Console.ResetColor();
            Console.WriteLine("1. Iniciar sesión");
            Console.WriteLine("2. Crear una cuenta");
            int opcion = Convert.ToInt32(Console.ReadLine());

            Usuario user = new Usuario();

            switch (opcion)
            {
                case 1:
                    user.Login();
                    string mail = user.Mail;
                    int age = user.Age;
                    Peliculas films = new Peliculas(mail, age);
                    films.menuPeliculas();
                    break;

                case 2:
                    user.Register();
                    break;
            }
        }

        public void Login() {
            connection.Open();
            Mail = "";
            Contra = "";
            SqlDataReader lector;
            bool conexion;

            Console.Clear();
            Console.WriteLine("🐬 VHS CLUB 🐬");
            do
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("\nEscribe tu email: ");
                Console.ResetColor();
                Mail = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Escribe tu contraseña: ");
                Console.ResetColor();
                //Contra = Console.ReadLine();
                List<string> lstClave = new List<string>();
                do { lstClave.Add(Convert.ToString(Console.ReadKey(true).Key));
                Console.Write("*"); } while (lstClave[lstClave.Count() - 1] != Convert.ToString(ConsoleKey.Enter));
                Contra = ""; for (int i = 0; i < lstClave.Count() - 1; i++) { Contra += lstClave[i]; }


                SqlCommand command = new SqlCommand("SELECT Mail, Birthdate, Pass, Firstname FROM Users WHERE Mail = @mail AND Pass = @pass", connection);
                command.Parameters.AddWithValue("@mail", Mail);
                command.Parameters.AddWithValue("@pass", Contra);
                lector = command.ExecuteReader();
                if (lector.Read())
                {
                    conexion = true;
                    Console.Clear();
                    Console.WriteLine("🐬 VHS CLUB 🐬");
                    Console.WriteLine($"¡Bienvenido/a {lector["Firstname"]}!");
                    connection.Close();
                    Age = Edad(Mail);
                }
                else
                {
                    conexion = false;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n❌ Los datos son incorrectos, intentalo de nuevo ❌");
                    Console.ResetColor();
                    lector.Close();
                }
            } while (conexion == false);
            connection.Close();
        }

        // DEVUELVE EDAD
        public int Edad(string mail)
        {
            connection.Open();
            SqlCommand command = new SqlCommand($"SELECT Birthdate FROM Users WHERE Mail = '{mail}'", connection);
            SqlDataReader lector = command.ExecuteReader();
            Age = 0;

            while (lector.Read())
            {
                string dateString = $"{lector["Birthdate"]}";
                DateTime fecha = DateTime.Parse(dateString);
                DateTime hoy = DateTime.Now;
                int year = Convert.ToInt32(fecha.Year);
                int year2 = Convert.ToInt32(hoy.Year);
                Age = (year2 - year) - 1;
            }
            connection.Close();
            return Age;
        }

        public void Register() {
            connection.Open();
            Console.Clear();
            Console.WriteLine("🐬 VHS CLUB 🐬");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("\nNombre: ");
            Console.ResetColor();
            string firstname = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Apellido: ");
            Console.ResetColor();
            string surname = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Fecha de nacimiento (año/mes/día):");
            Console.ResetColor();
            DateTime birthdate = Convert.ToDateTime(Console.ReadLine());
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Mail: ");
            Console.ResetColor();
            string email = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Contraseña: ");
            Console.ResetColor();
            //string pass = Console.ReadLine();
            List<string> lstClave = new List<string>();
            do
            {  lstClave.Add(Convert.ToString(Console.ReadKey(true).Key));
                Console.Write("*");
            } while (lstClave[lstClave.Count() - 1] != Convert.ToString(ConsoleKey.Enter));
            string pass = ""; for (int i = 0; i < lstClave.Count() - 1; i++) { pass += lstClave[i]; }

            string query = $"INSERT INTO Users(Firstname, Surname, Birthdate, Mail, Pass) values ('{firstname}', '{surname}', '{birthdate}', '{email}', '{pass}')";
            SqlCommand command = new SqlCommand(query, connection);
            command.ExecuteNonQuery();

            connection.Close();
            Console.Clear();
            Console.WriteLine("🐬 VHS CLUB 🐬");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Usuario registrado!");
            Console.ResetColor();
            Console.WriteLine("Pulsa ENTER para iniciar sesión.");
            if(Console.ReadKey().Key == ConsoleKey.Enter)
            {
                inicio();
            }
        }
    }
}
