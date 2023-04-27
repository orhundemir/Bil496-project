public class Room{
    public int id;
    public string name;
    public string wall;
    public string ceiling;
    public string floor;
    public string furniture;
    public Room(int id, string name, string wall, string ceiling, string floor, string furniture){
        this.id = id;
        this.name = name;
        this.wall = wall;
        this.ceiling = ceiling;
        this.floor = floor;
        this.furniture = furniture;
    }
    public Room(){
        id = -1;
        name = null;
    }
    public string toString(){
        return "Room ID: "+id+"\nName: "+name;
    }
}