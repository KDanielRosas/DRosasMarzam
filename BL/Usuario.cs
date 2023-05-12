using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BL
{
    public class Usuario
    {
        public static int Login(string userName, string password)
        {
            using (DL.DrosasMarzamContext context = new())
            {
                //var query = context.Database.ExecuteSqlRaw($"UsuarioLogin '{userName}','{password}'");

                var parameter = new SqlParameter
                {
                    ParameterName = "Code",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Output,
                };

                context.Database.ExecuteSqlRaw($"EXEC @code = UsuarioLogin '{userName}','{password}'", parameter);

                return (int)parameter.Value;
                
            }
        }//Login
    }
}