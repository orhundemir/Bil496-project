public class Room{
    int id;
    string name;
    Room(int id, string name){
        this.id = id;
        this.name = name;
    }
    Room(){
        id = -1;
        name = null;
    }
    string toString(){
        return "Room ID: "+id+"\nName: "+name;
    }
}