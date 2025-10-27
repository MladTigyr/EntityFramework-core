using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;
using System.Linq;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            SoftUniContext context = new SoftUniContext();

            string result = GetEmployeesFullInformation(context);
            Console.WriteLine(result);
        }

        //problem 3
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .OrderBy(e => e.EmployeeId)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.MiddleName,
                    e.JobTitle,
                    e.Salary
                })
                .ToArray();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        //problem 4
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .Where(e => e.Salary > 50000)
                .Select(e => new
                {
                    e.FirstName,
                    e.Salary
                })
                .OrderBy(e => e.FirstName)
                .ToArray();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} - {e.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        //problem 5
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .Where(e => e.Department.Name == "Research and Development")
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.Department.Name,
                    e.Salary
                })
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName)
                .ToArray();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} from Research and Development - ${e.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        //problem 6
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            Address address = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            context.Addresses.Add(address);

            Employee employee = context.Employees
                   .First(e => e.LastName == "Nakov");

            employee.Address = address;

            context.SaveChanges();

            var employees = context.Employees
                .OrderByDescending(e => e.AddressId)
                .Select(e => new
                {
                    e.Address.AddressText
                })
                .Take(10)
                .ToArray();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.AddressText}");
            }

            return sb.ToString().TrimEnd();
        }

        //problem 7
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .Include(e => e.Manager)
                .Include(e => e.EmployeesProjects)
                    .ThenInclude(ep => ep.Project)
                .Select(e => new
                {
                    EmployeeFirstName = e.FirstName,
                    EmployeeLastName = e.LastName,
                    ManagerFirstName = e.Manager.FirstName,
                    ManagerLastName = e.Manager.LastName,
                    e.EmployeesProjects
                })
                .Take(10)
                .ToArray();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.EmployeeFirstName} {e.EmployeeLastName} - Manager: {e.ManagerFirstName} {e.ManagerLastName}");

                if (e.EmployeesProjects != null)
                {
                    foreach (var ep in e.EmployeesProjects
                                 .Where(ep => ep.Project.StartDate.Year >= 2001 && ep.Project.StartDate.Year <= 2003))
                    {
                        var project = ep.Project;
                        var start = project.StartDate.ToString("M/d/yyyy h:mm:ss tt");
                        var end = project.EndDate.HasValue
                            ? project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt")
                            : "not finished";

                        sb.AppendLine($"--{project.Name} - {start} - {end}");
                    }
                }

            }

            return sb.ToString().TrimEnd();
        }

        //problem 8
        public static string GetAddressesByTown(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var addresses = context.Addresses
                .Include(a => a.Town)
                .Include(a => a.Employees)
                .OrderByDescending(a => a.Employees.Count)
                .ThenBy(a => a.Town.Name)
                .ThenBy(a => a.AddressText)
                .Take(10)
                .ToArray();

            foreach (var a in addresses)
            {
                sb.AppendLine($"{a.AddressText}, {a.Town.Name} - {a.Employees.Count} employees");
            }

            return sb.ToString().TrimEnd();
        }

        //problem 9
        public static string GetEmployee147(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            Employee employee = context.Employees

                .Include(e => e.EmployeesProjects)
                    .ThenInclude(ep => ep.Project)
                .First(e => e.EmployeeId == 147);

            sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
            if (employee.EmployeesProjects != null)
            {
                foreach (var ep in employee.EmployeesProjects
                             .OrderBy(ep => ep.Project.Name))
                {
                    sb.AppendLine($"{ep.Project.Name}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        //problem 10
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var departments = context.Departments
                .Include(d => d.Employees)
                    .ThenInclude(e => e.Manager)
                .Where(d => d.Employees.Count > 5)
                .OrderBy(d => d.Employees.Count)
                .ThenBy(d => d.Name)
                .ToArray();

            foreach (var d in departments)
            {
                sb.AppendLine($"{d.Name} - {d.Manager.FirstName} {d.Manager.LastName}");

                foreach (var e in d.Employees
                                    .OrderBy(e => e.FirstName)
                                    .ThenBy(e => e.LastName))
                {
                    sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        //problem 11
        public static string GetLatestProjects(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var projects = context.Projects
                .OrderByDescending(p => p.StartDate.Year)
                .ThenByDescending(p => p.StartDate.Month)
                .ThenByDescending(p => p.StartDate.Day)
                .Take(10)
                .OrderBy(p => p.Name)
                .ToArray();

            foreach (var p in projects)
            {
                sb.AppendLine($"{p.Name}");
                sb.AppendLine($"{p.Description}");
                sb.AppendLine($"{p.StartDate.ToString("M/d/yyyy h:mm:ss tt")}");
            }

            return sb.ToString().TrimEnd();
        }

        //problem 12
        public static string IncreaseSalaries(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .Include(e => e.Department)
                .Where(e => e.Department.Name == "Engineering" || e.Department.Name == "Tool Design" || e.Department.Name == "Marketing" || e.Department.Name == "Information Services")
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToArray();

            foreach (var e in employees)
            {
                e.Salary += e.Salary * 0.12m;
            }

            context.SaveChanges();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} (${e.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }

        //problem 13
        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .Where(e => e.FirstName.ToLower().StartsWith("sa"))
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    e.Salary
                })
                .ToArray();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }

        //problem 14
        public static string DeleteProjectById(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            Project projectToDelete = context.Projects
                .First(p => p.ProjectId == 2);

            var employees = context.Employees
                .Include(e => e.EmployeesProjects)
                    .ThenInclude(ep => ep.Project)
                .Where(e => e.EmployeesProjects.Any(ep => ep.ProjectId == 2))
                .ToList();

            foreach (var e in employees)
            {
                List<EmployeeProject> projects = e.EmployeesProjects.ToList();

                projects.RemoveAll(ep => ep.ProjectId == 2);

                e.EmployeesProjects = projects;
            }

            context.Projects.Remove(projectToDelete);

            context.SaveChanges();

            var projectsToDisplay = context.Projects
                .Take(10)
                .ToArray();

            foreach (var p in projectsToDisplay)
            {
                sb.AppendLine($"{p.Name}");
            }

            return sb.ToString().TrimEnd();
        }

        //problem 15
        public static string RemoveTown(SoftUniContext context)
        {
            Town townToDelete = context.Towns
                .First(t => t.Name == "Seattle");

            var addresses = context.Addresses
                .Include(a => a.Town)
                .Where(a => a.Town.Name == "Seattle")
                .ToList();

            var employees = context.Employees
                .Include(e => e.Address)
                .Where(e => addresses.Contains(e.Address))
                .ToList();

            int counter = addresses.Count;

            foreach (var e in employees)
            {
                e.Address = null;
            }

            context.Addresses.RemoveRange(addresses);

            context.Remove(townToDelete);

            context.SaveChanges();

            return $"{counter} addresses in Seattle were deleted";
        }
    }
}
