using Npgsql;
using Newtonsoft.Json;
public class ModelController{
    public bool insertModel(NpgsqlConnection conn, Model model){//Inserting given Model to database id will be determined by last model's id+1 in database
        NpgsqlCommand command = conn.CreateCommand();
        string query = "INSERT INTO MODEL (modelUrl, name, price, stock) VALUES ('"+model.model+"','"+model.name+"',"+model.price+","+model.stock+")";
        command.CommandText = query;
        command.ExecuteNonQuery();
        return true;
    }
    public Model selectModel(NpgsqlConnection conn, int id){//Returning model with given id usefull for getting model from relational tables
        Model model = new Model();
        NpgsqlCommand command = conn.CreateCommand();
        string query = "SELECT * FROM MODEL WHERE id = "+id;
        command.CommandText = query;
        NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read()){
            model.id  = int.Parse(JsonConvert.SerializeObject(reader.GetValue(0)));
            model.model = JsonConvert.SerializeObject(reader.GetValue(1));//Model's URL address
            model.name = JsonConvert.SerializeObject(reader.GetValue(2));
            model.price = float.Parse(JsonConvert.SerializeObject(reader.GetValue(3)));
            model.stock = int.Parse(JsonConvert.SerializeObject(reader.GetValue(4)));
        }
        return model;
    }
}