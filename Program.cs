using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Instituto 
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hola que tal mundo");

            Console.WriteLine("preparado");
        }

        
        static void SeedDatabase()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Database.Migrate();

                if (context.Colegioes.Any())
                {
                   
                    return;
                }

                var colegio1 = new Colegio();
                colegio1.Nombre = "Colegio 1";

                var estudiante1 = new Estudiante();
                estudiante1.Nombre = "Paul";
                estudiante1.Edad = 999;
                estudiante1.Detalles = new EstudianteDetalle() { Becado = true, CategoriaDePago = 1 };

                var estudiante2 = new Estudiante();
                estudiante2.Nombre = "valeria";
                estudiante2.Edad = 15;
                estudiante2.Detalles = new EstudianteDetalle() { Becado = false, Carrera = "Ingeniería de Software", CategoriaDePago = 1 };


                var estudiante3 = new Estudiante();
                estudiante3.Nombre = "Samanta";
                estudiante3.Edad = 25;
                estudiante3.Detalles = new EstudianteDetalle() { Becado = true, Carrera = "Licenciatura en Derecho", CategoriaDePago = 2 };


                var direccion1 = new Direccion();
                direccion1.Calle = "Calle 20";
                estudiante1.Direccion = direccion1;

                var grado1 = new Grado();
                grado1.Nombre = "Base de datos";

                var grado2 = new Grado();
                grado2.Nombre = "Administracion de empresas";

                var colegio2 = new Colegio();
                colegio2.Nombre = "Colegio 2";

                colegio1.Alumnos.Add(estudiante1);
                colegio1.Alumnos.Add(estudiante2);

                colegio2.Alumnos.Add(estudiante3);

                context.Add(colegio1);
                context.Add(colegio2);
                context.Add(grado1);
                context.Add(grado2);

                context.SaveChanges();

                var EstudianteGrado = new EstudianteGrado();
                EstudianteGrado.Activo = true;
                EstudianteGrado.GradoId = grado1.Id;
                EstudianteGrado.EstudianteId = estudiante1.Id;

                var EstudianteGrado2 = new EstudianteGrado();
                EstudianteGrado2.Activo = false;
                EstudianteGrado2.GradoId = grado1.Id;
                EstudianteGrado2.EstudianteId = estudiante2.Id;

                context.Add(EstudianteGrado);
                context.Add(EstudianteGrado2);
                context.SaveChanges();
            }
        }

        static void EjemploInsertarEstudiante()
        {
            using (var context = new ApplicationDbContext())
            {
                var estudiante = new Estudiante();
                estudiante.Nombre = "Mateo";
                context.Add(estudiante);
                context.SaveChanges();
            }
        }

        static void EjemploActualizarEstudianteModeloConectado()
        {
            using (var context = new ApplicationDbContext())
            {
                var alumnos = context.Alumnos.Where(x => x.Nombre == "Anita").ToList();

                alumnos[0].Nombre += " Apellido";

                context.SaveChanges();

            }
        }

        static void EjemploActualizarEstudianteModeloDesconectado(Estudiante estudiante)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Entry(estudiante).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
            }
        }

        static void EjemploRemoverEstudianteModeloConectado()
        {
            using (var context = new ApplicationDbContext())
            {
                var estudiante = context.Alumnos.FirstOrDefault();
                context.Remove(context);
                context.SaveChanges();
            }
        }

        static void EjemploRemoverEstudianteModeloDesonectado(Estudiante estudiante)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Entry(estudiante).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                context.SaveChanges();
            }
        }

        static void AgregarModeloUnoAUnoConectado()
        {
            using (var context = new ApplicationDbContext())
            {
             
                var estudiante = new Estudiante();
                estudiante.Nombre = "Maria";
                estudiante.Edad = 99;

                var direccion = new Direccion();
                direccion.Calle = "Calle 4";
                estudiante.Direccion = direccion;

                context.Add(estudiante);
                context.SaveChanges();
            }
        }

        static void AgregarModeloUnoAUnoModeloDesconectado(Direccion direccion)
        {
  
            using (var context = new ApplicationDbContext())
            {
                context.Entry(direccion).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
            }

        }

        static void TraerDataRelacionada()
        {
        
            using (var context = new ApplicationDbContext())
            {
                var alumnos = context.Alumnos.Include(x => x.Direccion).ToList();
            }
        }

        static void AgregarModeloMuchosAMuchosModeloDesconectado(Estudiante estudiante)
        {
           
            using (var context = new ApplicationDbContext())
            {
                context.Add(estudiante);
                context.SaveChanges();
            }
        }

        static void TraerDataRelacionadaUnoAMuchos()
        {
            using (var context = new ApplicationDbContext())
            {

                var colegioesAlumnos1 = context.Colegioes.Where(x => x.Id == 1).Include(x => x.Alumnos).ToList();
                var colegioesAlumnos = context.Colegioes.Where(x => x.Id == 1)
                    .Select(x => new { Colegio = x, Alumnos = x.Alumnos.Where(e => e.Edad > 18).ToList() }).ToList();

            }
        }

        static void InsertarDataRelacionadaMuchosAMuchos()
        {
            using (var context = new ApplicationDbContext())
            {
                var estudiante = context.Alumnos.FirstOrDefault();
                var grado = context.Grados.FirstOrDefault();

                var EstudianteGrado = new EstudianteGrado();

                EstudianteGrado.GradoId = grado.Id;
                EstudianteGrado.EstudianteId = estudiante.Id;
                EstudianteGrado.Activo = true;

                context.Add(EstudianteGrado);
                context.SaveChanges();
            }
        }

        static void TraerDataRelacionadaMuchosAMuchos()
        {
            using (var context = new ApplicationDbContext())
            {
                var grado = context.Grados.Where(x => x.Id == 1).Include(x => x.AlumnosGrados)
                    .ThenInclude(y => y.Estudiante).FirstOrDefault();
            }
        }

        static void StringInterpolationEnEF2()
        {
            using (var context = new ApplicationDbContext())
            {
                var nombre = "'German' or 1=1";
              
                var estudiante = context.Alumnos.FromSql($"select * from Alumnos where Nombre = {nombre}").ToList();
            }
        }

        static void FiltroPorTipo()
        {
            using (var context = new ApplicationDbContext())
            {
               
                var alumnosGrados = context.AlumnosGrados.ToList();
            }
        }

        static void EliminadoSuave()
        {
            
            using (var context = new ApplicationDbContext())
            {
                var estudiante = context.Alumnos.FirstOrDefault();
                context.Remove(estudiante);
                context.SaveChanges();
            }
        }

        static void EjemploConcurrencyCheck()
        {
            using (var context = new ApplicationDbContext())
            {
                var est = context.Alumnos.FirstOrDefault();
                est.Nombre += " 2";
                est.Edad += 1;
                context.SaveChanges();
            }
        }

        static void FuncionEscalarEnEF()
        {
            using (var context = new ApplicationDbContext())
            {
                var alumnos = context.Alumnos
                    .Where(x => ApplicationDbContext.Cantidad_De_Grados_Activos(x.Id) > 0).ToList();
            }
        }

        static void FuncionalidadTableSplitting()
        {
            using (var context = new ApplicationDbContext())
            {
                var alumnos = context.Alumnos.Include(x => x.Detalles).ToList();
            }
        }
    }

    class Colegio
    {
        public Colegio()
        {
            Alumnos = new List<Estudiante>();
        }
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<Estudiante> Alumnos { get; set; }
    }

    class Estudiante
    {
        public int Id { get; set; }
        [ConcurrencyCheck]
        public string Nombre { get; set; }
        public int Edad { get; set; }
        public int ColegioId { get; set; }
        public bool EstaEliminado { get; set; }

        private string _Apellido;

        public string Apellido
        {
            get { return _Apellido; }
            set
            {
                _Apellido = value.ToUpper();
            }
        }
        public Direccion Direccion { get; set; }
        public List<EstudianteGrado> AlumnosGrados { get; set; }
        public EstudianteDetalle Detalles { get; set; }
    }

    class EstudianteDetalle
    {
        public int Id { get; set; }
        public bool Becado { get; set; }
        public string Carrera { get; set; }
        public int CategoriaDePago { get; set; }
        public Estudiante Estudiante { get; set; }
    }

    class Direccion
    {
        public int Id { get; set; }
        public string Calle { get; set; }
        public int EstudianteId { get; set; }
    }

    class Grado
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string Nombre { get; set; }
        public List<EstudianteGrado> AlumnosGrados { get; set; }
    }

    class EstudianteGrado
    {
        public int EstudianteId { get; set; }
        public int GradoId { get; set; }
        public bool Activo { get; set; }
        public Estudiante Estudiante { get; set; }
        public Grado Grado{ get; set; }
    }
}
