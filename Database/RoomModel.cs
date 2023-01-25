public class RoomModel{
    int id;
    int scene_id;
    int model_id;
    RoomModel(int id, int scene_id, int model_id){
        this.id = id;
        this.scene_id = scene_id;
        this.model_id = model_id;
    }
    RoomModel(){
        id = -1;
        scene_id = -1;
        model_id = -1;
    }
    string toString(){
        return "Room Model ID: "+id+"\nRoom ID: "+scene_id+"\nModel ID: "+model_id;
    }
}