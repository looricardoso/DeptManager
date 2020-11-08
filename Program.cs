using System;
using DeptManager.db;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DeptManager
{
    class Program
    {
        static void Main(string[] args)
        {
            string filtro;
            Console.WriteLine(" --- Localizar departamento --- ");

            using (var db = new employeesContext())
            {
                var todosDeptos = db.Departments;
                foreach (var departamentos in todosDeptos)
                {
                    string listaNomeDepto = departamentos.DeptName;
                    string listaCodDepto = departamentos.DeptNo;
                    Console.WriteLine($"Para {listaNomeDepto}, digite {listaCodDepto}");
                }
            }

            Console.WriteLine("Digite o código do departamento desejado:");
            filtro = Console.ReadLine();
            using (var db = new employeesContext())
            {
                var emp = db.Employees;
                var depto = db.Departments.Include(deptoManager => deptoManager.DeptManager).SingleOrDefault(d => d.DeptNo == filtro);
                if (depto != null) {
                    Console.WriteLine($"O departamento informado foi: {depto.DeptName}");
                    foreach (var m in depto.DeptManager.Where(filter => filter.ToDate.Year > 2019))
                    {
                        var idEmpregado = m.EmpNo;
                        foreach (var empreg in db.Employees.Where(loc => loc.EmpNo == idEmpregado))
                        {
                            if (empreg.Gender == "M") {
                                Console.WriteLine($"{depto.DeptName} Managed by Mr. {empreg.FirstName}.");
                            } else if (empreg.Gender == "F") {
                                Console.WriteLine($"{depto.DeptName} Managed by Mrs. {empreg.FirstName}.");
                            };
                            
                        }
                    }
                } else {
                    Console.WriteLine("Departamento não localizado");
                }
            }
        }
    }
}
