public class Cart{
    int id;
    int user_id;
    Cart(int id, int user_id){
        this.id = id;
        this.user_id = user_id;
    }
    Cart(){
        id = -1;
        user_id = -1;
    }
    string toString(){
        return "Cart ID: "+id+"\nUser ID: "+user_id;
    }
}