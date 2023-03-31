using Shop.WebUI.Models;
using Dapper;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Shop.WebUI.InMemory;

public class Repository {
    IDbConnection connection;

    public Repository() {
        connection = new MySqlConnection("Server=127.0.0.1;Port=3307;database=charityshowcase;user id=ejoh;password=YEVT4w2^N4uv2q48TnA9#k&ep");
    }
    public List<T> Collection<T>(string sql) {
        return connection.Query<T>(sql).ToList();
    }
    public void Insert<T>(string table, T parameters) {
    }
}
