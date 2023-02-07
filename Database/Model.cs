public class Model{
    public int id;
    public string model; 
    public string name;
    public double price;
    public int stock;
    public Model(int id, string model, string name, double price, int stock){
        this.id = id;
        this.model = model;
        this.name = name;
        this.price = price;
        this.stock = stock;
    }
    public Model(){
        id = -1;
        model = null;
        name = null;
        price = -1;
        stock = -1;
    }
    public string toString(){
        return "Model ID: "+id+"\nName: "+name+"\nPrice: "+price+"\nStock: "+stock;
    }
}