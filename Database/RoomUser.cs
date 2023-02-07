public class RoomUser{
    public int id;
    public int user_id;
    public int scene_id;
    public RoomUser(int id, int user_id, int scene_id){
        this.id = id;
        this.user_id = user_id;
        this.scene_id = scene_id;
    }
    public RoomUser(){
        id = -1;
        user_id = -1;
        scene_id = -1;
    }
    public string toString(){
        return "Room_User ID: "+id+"\nUser ID: "+user_id+"\nRoom ID: "+scene_id;
    }
}