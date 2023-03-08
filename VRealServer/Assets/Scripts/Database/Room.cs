public class Room{
    public int id;
    public string name;
    public Room(int id, string name){
        this.id = id;
        this.name = name;
    }
    public Room(){
        id = -1;
        name = null;
    }
    public string toString(){
        return "Room ID: "+id+"\nName: "+name;
    }
}