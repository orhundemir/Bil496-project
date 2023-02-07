public class RoomModel{
    public int id;
    public int scene_id;
    public int model_id;
    public int x;//Model's x,y and z coordinates ill be held in database
    public int y;
    public int z;
    public RoomModel(int id, int scene_id, int model_id, int x, int y, int z){
        this.id = id;
        this.scene_id = scene_id;
        this.model_id = model_id;
        this.x = x;
        this.y = y;
        this.z = z;
    }
    public RoomModel(){
        id = -1;
        scene_id = -1;
        model_id = -1;
        x = -1;
        y = -1;
        z = -1;
    }
    public string toString(){
        return "Room Model ID: "+id+"\nRoom ID: "+scene_id+"\nModel ID: "+model_id;
    }
}