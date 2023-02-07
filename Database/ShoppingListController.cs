using Npgsql;
using Newtonsoft.Json;
using System.Collections.Generic;

public class ShoppingListController{
    
    bool insertShoppingList(NpgsqlConnection conn, ShoppingList shoppingList){//Inserting given shopping list to database id will be determined by last shopping list's id+1 in database
        NpgsqlCommand command = conn.CreateCommand();
        string query = "INSERT INTO SHOPPING_LIST (user_id, model_id) VALUES ("+shoppingList.user_id+","+shoppingList.model_id+")";
        command.CommandText = query;
        command.ExecuteNonQuery();
        ConnectionManager.closeConnection(conn);
        return true;
    }
    ShoppingList selectShoppingList(NpgsqlConnection conn, int id){//Returning shopping list with given id
        ShoppingList shoppingList = new ShoppingList();
        NpgsqlCommand command = conn.CreateCommand();
        string query = "SELECT * FROM SHOPPING_LIST WHERE id = "+id;
        command.CommandText = query;
        NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read()){
            shoppingList.id  = int.Parse(JsonConvert.SerializeObject(reader.GetValue(0)));
            shoppingList.user_id = int.Parse(JsonConvert.SerializeObject(reader.GetValue(1)));
            shoppingList.model_id = int.Parse((JsonConvert.SerializeObject(reader.GetValue(2))));
        }
        ConnectionManager.closeConnection(conn);
        return shoppingList;
    }
    List<ShoppingList> selectUSerShoppingLists(NpgsqlConnection conn, User user){//Selecting a users shopping lists in database
        List<ShoppingList> shoppingLists = new List<ShoppingList>();
        NpgsqlCommand command = conn.CreateCommand();
        string query = "SELECT * FROM SHOPPING_LIST WHERE user_id = "+user.id;
        command.CommandText = query;
        NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read()){
            ShoppingList shoppingList = new ShoppingList();
            shoppingList.id  = int.Parse(JsonConvert.SerializeObject(reader.GetValue(0)));
            shoppingList.user_id = int.Parse(JsonConvert.SerializeObject(reader.GetValue(1)));
            shoppingList.model_id = int.Parse((JsonConvert.SerializeObject(reader.GetValue(2))));
            shoppingLists.Add(shoppingList);
        }
        return shoppingLists;
    }
    List<ShoppingList> selectUSerShoppingLists(NpgsqlConnection conn, Model model){//Selecting a models shopping lists in database
        List<ShoppingList> shoppingLists = new List<ShoppingList>();
        NpgsqlCommand command = conn.CreateCommand();
        string query = "SELECT * FROM SHOPPING_LIST WHERE model_id = "+model.id;
        command.CommandText = query;
        NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read()){
            ShoppingList shoppingList = new ShoppingList();
            shoppingList.id  = int.Parse(JsonConvert.SerializeObject(reader.GetValue(0)));
            shoppingList.user_id = int.Parse(JsonConvert.SerializeObject(reader.GetValue(1)));
            shoppingList.model_id = int.Parse((JsonConvert.SerializeObject(reader.GetValue(2))));
            shoppingLists.Add(shoppingList);
        }
        return shoppingLists;
    }
}