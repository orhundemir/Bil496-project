public class User{
    int id;
    string e_mail;
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