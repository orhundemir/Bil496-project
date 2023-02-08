using Npgsql;
using Newtonsoft.Json;
using System.Collections.Generic;


public class CartController{
    public bool insertCart(NpgsqlConnection conn, Cart cart){//Inserting given cart to database id will be determined by last cart's id+1 in database
        NpgsqlCommand command = conn.CreateCommand();
        string query = "INSERT INTO CART (user_id, shoppingList_id) VALUES ("+cart.user_id+","+cart.shoppingList_id+")";
        command.CommandText = query;
        command.ExecuteNonQuery();
        ConnectionManager.closeConnection(conn);
        return true;
    }
    public Cart selectCart(NpgsqlConnection conn, int id){//Returning cart with given id
        Cart cart = new Cart();
        NpgsqlCommand command = conn.CreateCommand();
        string query = "SELECT * FROM CART WHERE id = "+id;
        command.CommandText = query;
        NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read()){
            cart.id  = int.Parse(JsonConvert.SerializeObject(reader.GetValue(0)));
            cart.user_id = int.Parse(JsonConvert.SerializeObject(reader.GetValue(1)));
            cart.shoppingList_id = int.Parse((JsonConvert.SerializeObject(reader.GetValue(2))));
        }
        ConnectionManager.closeConnection(conn);
        return cart;
    }
    public List<Cart> selectUSerCarts(NpgsqlConnection conn, User user){//Selecting a users constructed cart in database
        List<Cart> carts = new List<Cart>();
        NpgsqlCommand command = conn.CreateCommand();
        string query = "SELECT * FROM CART WHERE user_id = "+user.id;
        command.CommandText = query;
        NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read()){
            Cart cart = new Cart();
            cart.id  = int.Parse(JsonConvert.SerializeObject(reader.GetValue(0)));
            cart.user_id = int.Parse(JsonConvert.SerializeObject(reader.GetValue(1)));
            cart.shoppingList_id = int.Parse((JsonConvert.SerializeObject(reader.GetValue(2))));
            carts.Add(cart);
        }
        return carts;
    }
}