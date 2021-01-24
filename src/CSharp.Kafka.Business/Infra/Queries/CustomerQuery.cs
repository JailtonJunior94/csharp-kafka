namespace CSharp.Kafka.Business.Infra.Queries
{
    public static class CustomerQuery
    {
        public const string Get = @"SELECT
                                      Id,
                                      Name,
                                      Email,
                                      CreatedAt,
                                      UpdatedAt,
                                      Active
                                    FROM
                                      dbo.Customers (NOLOCK)";
        public const string GetById = @"SELECT
                                          Id,
                                          Name,
                                          Email,
                                          CreatedAt,
                                          UpdatedAt,
                                          Active
                                        FROM
                                          dbo.Customers (NOLOCK)
                                        WHERE
                                          Id = @id";
        public const string Add = @"INSERT INTO
                                      dbo.Customers
                                    VALUES
                                      (@name, @email, @createdAt, @updatedAt, @active); SELECT SCOPE_IDENTITY()";
        public const string Update = @"UPDATE
                                          dbo.Customers
                                        SET
                                          name = @name,
                                          email = @email
                                        WHERE
                                          Id = @id";
        public const string Delete = @"DELETE FROM
                                          dbo.Customers
                                        WHERE
                                          Id = @id";
    }
}
