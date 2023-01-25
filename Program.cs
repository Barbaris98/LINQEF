using Microsoft.EntityFrameworkCore;


namespace LINQEF
{
    internal class Program
    {
        static void Main(string[] args)
        {

            using (ApplicationContext db = new ApplicationContext())
            {
                // пересоздаем базу данных
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                Company microsoft = new Company { Name = "Microsoft" };
                Company google = new Company { Name = "Google" };
                db.Companies.AddRange(microsoft, google);

                User tom = new User { Name = "Tom", Age = 36, Company = microsoft };
                User bob = new User { Name = "Bob", Age = 39, Company = google };
                User alice = new User { Name = "Alice", Age = 28, Company = microsoft };
                User kate = new User { Name = "Kate", Age = 25, Company = google };

                db.Users.AddRange(tom, bob, alice, kate);
                db.SaveChanges();
            }
            using (ApplicationContext db = new ApplicationContext())
            {
                var users = (from user in db.Users.Include(p => p.Company)
                             where user.CompanyId == 1
                             select user).ToList();

                foreach (var user in users)
                    Console.WriteLine($"{user.Name} ({user.Age}) - {user.Company?.Name}");
            }


            /*
            using (ApplicationContext db = new ApplicationContext())
            {
                var users = db.Users.Include(p => p.Company).Where(p => p.CompanyId == 1);
            }
            */


        }
    }
}