public class Cart{
    public int id;
    public int user_id;
    public int shoppingList_id;
    public Cart(int id, int user_id, int shoppingList_id){
        this.id = id;
        this.user_id = user_id;
        this.shoppingList_id = shoppingList_id;
    }
    public Cart(){
        id = -1;
        user_id = -1;
        shoppingList_id = -1;
    }
    public string toString(){
        return "Cart ID: "+id+"\nUser ID: "+user_id+"\nShopping list id"+shoppingList_id;
    }
}