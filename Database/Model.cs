public class Model{
    int id;
    object model; //TO DO Find correct object/type
    string name;
    double price;
    int stock;
    Model(int id, object model, string name, double price, int stock){
        this.id = id;
        this.model = model;
        this.name = name;
        this.price = price;
        this.stock = stock;
    }
    Model(){
        id = -1;
        model = null;
        name = null;
        price = -1;
        stock = -1;
    }
    string toString(){
        return "Model ID: "+id+"\nName: "+name+"\nPrice: "+price+"\nStock: "+stock;
    }
}