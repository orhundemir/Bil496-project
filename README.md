# Bil496-project

Server için sln uzantılı dosya çalıştırılarak vs code altında geliştirme ortamı açılmaktadır.

Client için  unity hub üzerinden open file seçeneği ile kodun çekildiği klasördeki VRealClient klasörü seçilerek çalıştırılmalıdır.

Database örnek metod
 NpgsqlConnection conn = ConnectionManager.getConnection();
        conn.Open();
        User user = new User(1, "asd@asd");
        UsersController uc = new UsersController();
        uc.insertUser(conn, user);

plugin dosyası npgsql için asset altına eklenmelidir.