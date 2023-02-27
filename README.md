# Bil496-project

Server için  unity hub üzerinden open file seçeneği ile kodun çekildiği klasördeki VRealServer klasörü seçilerek çalıştırılmalıdır.

Client için  unity hub üzerinden open file seçeneği ile kodun çekildiği klasördeki VRealClient klasörü seçilerek çalıştırılmalıdır.

Server tarafında:
* Main sahnesi Unity'deki sahne alanına sürüklenmelidir ve default oluşan Untitled sahnesi silinmelidir.

Client tarafında:
* Main, RoomDrawing ve VReal sahneleri Unity'deki sahne alanına sürüklenmelidir ve default oluşan Untitled sahnesi silinmelidir.
* RoomDrawing ve VReal sahneleri "Unload" konumuna getirilmelidir.
* Edit->Preferences-> VIU kısmında Initiliaze on Startup kutucuğu işaratlenmemelidir.
* Edit->Preferences-> VIU kısmında Simulator kısmı işaretlenmelidir.
* Warningler headset takılmadığı ve EventSystem Warningleridir. Ignore edilebilir.
* VReal sahnesine geçişte kullanılan sample sahne sebebiyle bir exception gelmektedir. Ignore edilebilir.

Not: VReal sahnesinde çalışacaklar aşağıdaki linkteki videoyu izlemesi tavsiye edilir.
	https://www.youtube.com/watch?v=P4UxJJg6VgY&t=402s

Database örnek metod
 NpgsqlConnection conn = ConnectionManager.getConnection();
        conn.Open();
        User user = new User(1, "asd@asd");
        UsersController uc = new UsersController();
        uc.insertUser(conn, user);

plugin dosyası npgsql için asset altına eklenmelidir.
