using DowntimeTracker.Api.Models;

namespace DowntimeTracker.Api.Data;

public static class DbSeeder
{
    public static void Seed(AppDbContext context)
    {
        if (context.Modules.Any()) return; // already seeded, skip

        var module = new Module { Name = "LB11", Silo = "Silo A" };
        context.Modules.Add(module);
        context.SaveChanges();

        var heads = new List<Head>
        {
            new Head { Code = "H01", ModuleId = module.Id },
            new Head { Code = "H02", ModuleId = module.Id },
            new Head { Code = "H07", ModuleId = module.Id },
            new Head { Code = "H10", ModuleId = module.Id },
        };
        context.Heads.AddRange(heads);

        var lamination = new Department { Name = "Lamination" };
        var cutting = new Department { Name = "Cutting" };
        context.Departments.AddRange(lamination, cutting);
        context.SaveChanges();

        context.Reasons.AddRange(
            new Reason { DepartmentId = lamination.Id, ReasonText = "INPUT DELAY - LA" },
            new Reason { DepartmentId = lamination.Id, ReasonText = "MACHINE BREAKDOWN - LA" },
            new Reason { DepartmentId = cutting.Id, ReasonText = "INPUT DELAY - CT" }
        );

        // Test user — password is "Test@123", hashed below
        context.Users.Add(new User
        {
            Username = "testuser",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Test@123"),
            Role = "Operator"
        });

        context.SaveChanges();
    }
}