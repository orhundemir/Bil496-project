public class ShoppingList{
    int id;
    int user_id;
    int model_id;
    ShoppingList(int id, int user_id, int model_id){
        this.id = id;
        this.user_id = user_id;
        this.model_id = model_id;
    }
    ShoppingList(){
        id = -1;
        user_id = -1;
        model_id = -1;
    }
    string toString(){
        return "Hopping List ID: "+id+"\nUser ID: "+user_id+"\nModel ID: "+model_id;
    }
}