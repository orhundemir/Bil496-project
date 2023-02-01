public class User{
    int id;
    string e_mail;
    string name; //Users name surname not in database but may be used for display will set from login
    long sessionTime; //time variable set on starting  a session and may be used to clculate session duration for current session
    User(int id, string e_mail){
        this.id = id;
        this.e_mail = e_mail;
    }
    User(){
        id = -1;
        e_mail = null;
    }
    string toString(){
        return "User ID: "+id+"\nE-mail: "+e_mail;
    }
}
