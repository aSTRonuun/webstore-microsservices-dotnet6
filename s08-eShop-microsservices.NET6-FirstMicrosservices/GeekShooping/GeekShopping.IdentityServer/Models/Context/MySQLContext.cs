using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.IdentityServer.Models.Context;

public class MySQLContext : IdentityDbContext<AplicationUser>
{
    public MySQLContext() { }

    public MySQLContext(DbContextOptions<MySQLContext> options) : base(options) { }
}


