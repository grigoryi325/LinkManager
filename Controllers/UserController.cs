using SaveLink.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SaveLink.Controllers
{
    public class UserController : Controller
    {
        DataContext db = new DataContext();

        // GET: User
        public ActionResult Index(string error_change_account_data = "")
        {
            Session["error_sare"] = 0;


            Session["category_id"] = 0;
           
            var user_id = Convert.ToInt32(Session["Id"]);
            var email = Session["Email"].ToString();
            var role = Session["Role"].ToString();

            ViewBag.ErrorChangeAccountData = error_change_account_data;
            error_change_account_data = "";
            if (role == "user")
            {

                //categories for menu
                var categories = db.Categories.Where(x => x.UserId == user_id && x.DeleteCategory != "Yes").OrderByDescending(x => x.AddCatOrLinkTime).ToList();

                if (categories.Count() == 0) {

                    Category category = new Category { Name = "Вкладка 1", UserId = user_id , AddCatOrLinkTime = DateTime.Now};
                    db.Categories.Add(category);
                    db.SaveChanges();

                    var id_new_category = db.Categories.Where(x => x.Name == "Вкладка 1" && x.UserId == user_id && x.DeleteCategory != "Yes").First();

                    Link link = new Link { Url = "http://localhost:28233/", Description = "Это автоматически созданая первая ссылка, для примера, добавляйте новые ссылки, размещайте их вовкладках, редактируйте вкладки, ссылки, или удаляйте.", CategoryId = id_new_category.Id };
                    db.Links.Add(link);
                    db.SaveChanges();

                    var categoriess = db.Categories.Where(x => x.UserId == user_id && x.DeleteCategory != "Yes").OrderByDescending(x => x.AddCatOrLinkTime).ToList();

                    var first_categoryy = db.Categories.Where(x => x.UserId == user_id && x.DeleteCategory != "Yes").OrderByDescending(x => x.AddCatOrLinkTime).First(); ;
                    ViewBag.links = db.Links.Where(x => x.CategoryId == first_categoryy.Id && x.Category.UserId == user_id).OrderByDescending(x => x.Id).ToList();

                    return View(categoriess);
                }
                else
                {
                    //after logging to user account, downloaded links  last added category
                    var first_category = db.Categories.Where(x => x.UserId == user_id && x.DeleteCategory != "Yes").OrderByDescending(x => x.AddCatOrLinkTime).First(); ;
                    ViewBag.links = db.Links.Where(x => x.CategoryId == first_category.Id && x.Category.UserId == user_id).OrderByDescending(x => x.Id).ToList();

                    return View(categories);
                }
            }
            else
            {
                return RedirectToAction("Index", "Error");
            }
        }

        //partial ajax view for add links to workzone
        public ActionResult Links(int id = 0, string search = "") {
            Session["category_id"] = 0;
            var user_id = Convert.ToInt32(Session["Id"]);
            var email = Session["Email"].ToString();
            var role = Session["Role"].ToString();

            if (search != "")
            {
                var allinks = db.Links.Where(a => a.Description.Contains(search) && a.Category.UserId == user_id).OrderByDescending(x => x.Id).ToList();
                return PartialView(allinks);
            }
            else
            {         
                var links = db.Links.Where(x => x.CategoryId == id && x.Category.UserId == user_id).OrderByDescending(x => x.Id).ToList();
                return PartialView(links);
            }
        }

        [HttpPost]
        public ActionResult AddLink(Link link, Category category)
        {
            var user_id = Convert.ToInt32(Session["Id"]);
            var email = Session["Email"].ToString();
            var role = Session["Role"].ToString();
            //var user_id = Convert.ToInt32(Session["Id"]);
            if (category.Name != null) {
                category.UserId = user_id;
                category.AddCatOrLinkTime = DateTime.Now;

                db.Categories.Add(category);
                db.SaveChanges();

                var id_new_category = db.Categories.Where(x => x.Name == category.Name && x.UserId == user_id).First();

                link.CategoryId = id_new_category.Id;
                db.Links.Add(link);
                db.SaveChanges();

                ViewBag.IdNewAddCat = category.Id;
                ViewBag.NameNewAddCat = category.Name;
            }
            else
            {
                Category cat_add_link = db.Categories.Where(x => x.Id == link.CategoryId).First();
                cat_add_link.AddCatOrLinkTime = DateTime.Now;
                db.Links.Add(link);
                db.SaveChanges();              
            }

            ViewBag.links = db.Links.Where(x => x.CategoryId == link.CategoryId && x.Category.UserId == user_id).OrderByDescending(x => x.Id).ToList();
            //categories for menu
            //var cat = db.Categories.Where(x => x.UserId == user_id && x.DeleteCategory != "Yes").OrderByDescending(x => x.Id);
            var cat = db.Categories.Where(x => x.UserId == user_id && x.DeleteCategory != "Yes").OrderByDescending(x => x.AddCatOrLinkTime);


            //if (category.Name != null)
            //{
            //    ViewBag.IdNewAddCat = category.Id;
            //    ViewBag.NameNewAddCat = category.Name;
            //}

            return PartialView(cat);
        }

        public ActionResult EditLink(int Id) {
            ViewBag.Link = db.Links.Where(x => x.Id == Id).First(); 
            return PartialView();
        }

        public ActionResult DelLink(int Id) {
            Link link = db.Links.Where(x => x.Id == Id).First();
            db.Links.Remove(link);
            db.SaveChanges();
            return View();
        }

        [HttpPost]
        public ActionResult SaveEditLink(int Id, string Url, string Description) {
            Link link = db.Links.Where(x => x.Id == Id).First();
            link.Url = Url;
            link.Description = Description;
            db.Entry(link).State = EntityState.Modified;
            db.SaveChanges();

            ViewBag.Link = db.Links.Where(x => x.Id == link.Id).First();
            return PartialView();
        }


        /*---------------------------------------Edit Category, and Links-------------------------------*/

        [HttpPost]
        public ActionResult EditCategory(int Id, string Name) {

            var user_id = Convert.ToInt32(Session["Id"]);

            var last_name = db.Categories.Where(x => x.Id == Id).First();

            var check = db.Categories.Where(x => x.Name == Name && x.UserId == user_id).Count();


            if (check == 1)
            {
                var check_cat = db.Categories.Where(x => x.Name == Name && x.UserId == user_id).First();

                if (check_cat.Id != Id)
                {
                    //ошибка, что категория с таким именем существует
                    ViewBag.Id = Id;
                    ViewBag.Name = Name;
                    ViewBag.Error = "Error";

                    ViewBag.LastName = last_name.Name;
                    return PartialView();
                }
                else
                {
                    Category category = db.Categories.Where(x => x.Id == Id).First();
                    category.Name = Name;

                    db.Entry(category).State = EntityState.Modified;
                    db.SaveChanges();

                    ViewBag.Id = Id;
                    ViewBag.Name = Name;

                    Session["category_id"] = Id;

                    return PartialView();
                }
            }
            else
            {
                if (check != 0)
                {
                    //ошибка, что категория с таким именем существует
                    ViewBag.Id = Id;
                    ViewBag.Name = Name;
                    ViewBag.Error = "Error";

                    ViewBag.LastName = last_name.Name;
                    return PartialView();
                }
                else
                {
                    Category category = db.Categories.Where(x => x.Id == Id).First();
                    category.Name = Name;

                    db.Entry(category).State = EntityState.Modified;
                    db.SaveChanges();

                    ViewBag.Id = Id;
                    ViewBag.Name = Name;

                    Session["category_id"] = Id;

                    return PartialView();
                }
            }         
        }

        public ActionResult DelCategory(int Id) {
                Category category = db.Categories.Where(x => x.Id == Id).First();
                category.DeleteCategory = "Yes";
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
        }

        /*-------------------------------------------Acccount-------------------------------------------*/

        [HttpPost]
        public ActionResult Registration(Link link, Category category) {
            Session["error_share"] = 0;
            Session["ErrorLogin"] = "";

            var login = "";
            var password = "";

            var check = 0;

            do
            {
                login = GenerateLogin();
                password = GeneratePass();

                check = db.Users.Where(x => x.Email == login).Count();
            } while (check != 0);

            User user = new User { Email = login, Password = password, FirstLogining="Yes" };
            db.Users.Add(user);
            db.SaveChanges();

            var new_user_id = db.Users.Where(x => x.Email == login).First();

            Session["Id"] = new_user_id.Id;
            Session["Email"] = login;
            Session["Role"] = "user";
            Session["Password"] = password;
            Session["FirstLogining"] = user.FirstLogining.ToString();

            var user_id = Convert.ToInt32(Session["Id"]);
            var email = Session["Email"].ToString();
            var role = Session["Role"].ToString();

            category.UserId = user_id;
            category.AddCatOrLinkTime = DateTime.Now;
            db.Categories.Add(category);
            db.SaveChanges();
            var id_new_category = db.Categories.Where(x => x.Name == category.Name && x.UserId == user_id).First();

            link.CategoryId = id_new_category.Id;
            db.Links.Add(link);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        /*Generate Login and Password*/
        public static string GenerateLogin()
        {
            int[] arr = new int[6];
            int[] symbols = { 65, 66, 67, 68, 69, 70, 71, 72, 97, 98, 99, 100, 101, 102, 103, 104, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122 };
            Random rnd = new Random();
            string Password = "";

            for (int i = 0; i < arr.Length; i++)
            {
                var rnd_mass_i = rnd.Next(0, (symbols.Length - 1));
                arr[i] = symbols[rnd_mass_i];
                Password += (char)arr[i];
            }
            return Password;
        }

        public static string GeneratePass()
        {
            int[] arr = new int[6];
            int[] symbols = { 33, 65, 66, 67, 68, 69, 70, 71, 72, 35, 97, 98, 99, 100, 101, 102, 103, 104, 36, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 63, 83, 84, 85, 86, 87, 88, 89, 90, 64, 105, 106, 107, 108, 109, 37, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122 };
            Random rnd = new Random();
            string Password = "";

            for (int i = 0; i < arr.Length; i++)
            {
                var rnd_mass_i = rnd.Next(0, (symbols.Length - 1));
                arr[i] = symbols[rnd_mass_i];
                Password += (char)arr[i];
            }
            return Password;
        }

        public ActionResult SignIn() {
            return View();
        }

        [HttpPost]
        public ActionResult SignIn(string Email, string Password)
        {
            try
            {
                var logining_user = db.Users.Where(x => x.Email == Email && x.Password == Password).First();

                Session["Id"] = logining_user.Id;
                Session["Email"] = logining_user.Email;
                Session["Role"] = logining_user.Role;
                Session["Password"] = logining_user.Password;
                Session["FirstLogining"] = logining_user.FirstLogining;
                Session["CreatingAccountForLink"] = logining_user.CreatingAccountForLink;
                Session["ErrorLogin"] = "";
                //Session["error_sare"] = 0;

                return RedirectToAction("Index", "User");
            }
            catch (Exception) {
                ViewBag.ErrorLogining = "Не верный Логин или Пароль";
                return View();
            }
        }

        //вход по ссылке 
        public ActionResult SignInForLink(string code)
        {
            var login = "";

            try
            {
                login = Session["Email"].ToString();
            }
            catch (NullReferenceException)
            {
                login = "";
            }
            //доступ по ссылке имеют все у кого она есть, до смены получателем учетных данных
            var logining_user = db.Users.Where(x => (x.CreatingAccountForLink == "http://localhost:28233/User/SignInForLink/?code=" + code) && (x.UserGivesLink != login) && x.FirstLogining == "Yes").First();

            Session["Id"] = logining_user.Id;
            Session["Email"] = logining_user.Email;
            Session["Role"] = logining_user.Role;
            Session["Password"] = logining_user.Password;
            Session["FirstLogining"] = logining_user.FirstLogining;
            Session["CreatingAccountForLink"] = logining_user.CreatingAccountForLink;
            Session["ErrorLogin"] = "";
            //Session["error_sare"] = 0;


            var email = Session["Email"].ToString();

            //ПРИ РАЗШАРИВАНИИ СЫЛОК ПО СЫЛКЕ создаем вкладку и сылку по умолчанию"
            var check_cat = db.Categories.Where(x => x.UserId == logining_user.Id).Count();

            if (check_cat == 0)
            {
                Category category = new Category { Name = "Вкладка 1", UserId = logining_user.Id, AddCatOrLinkTime = DateTime.Now };
                db.Categories.Add(category);
                db.SaveChanges();

                var id_new_category = db.Categories.Where(x => x.Name == "Вкладка 1" && x.UserId == logining_user.Id).First();

                Link link = new Link { Url = "http://localhost:28233/", Description = "Это автоматически созданая ссылка, для примера. Добавляйте новые ссылки, размещайте их вовкладках, редактируйте вкладки, ссылки, делитесь ссылками с друзями...", CategoryId = id_new_category.Id };
                db.Links.Add(link);
                db.SaveChanges();
            }
            /////////////////////////////////////////////////////

            //формируем список пользователей которые нам или мы им отправили сылки              
            var shared = db.Shareds.Where(x => (x.Recipient == email) || (x.Sender == email)).OrderByDescending(x => x.Id).ToList();

            //список отправителей
            List<CategorySender> categorySenders = new List<CategorySender>();

            //предыдущий отправитель
            var previus_sender = "";

            //формируем список отправителей
            foreach (var s in shared)
            {
                if (s.Sender != email)
                {
                    categorySenders.Add(new CategorySender(s.Sender));
                    previus_sender = s.Sender;
                }
                else
                {
                    categorySenders.Add(new CategorySender(s.Recipient));
                    previus_sender = s.Recipient;
                }
            }

            //групируем отправителей по логинах что бы не было повторений и отправляем результат в представление
            ViewBag.Sender = categorySenders.GroupBy(x => x.Sender).Select(y => new SenderViewModel { Sender = y.Key });

            //список пользователей в порядке кто последний отправлял сылки
            var last_shared = db.Shareds.Where(x => (x.Recipient == email) || (x.Sender == email)).OrderByDescending(x => x.Id).First(); ;

            //выбираем логин отправителя исключая собственный
            string login_sender = last_shared.Sender;

            if (last_shared.Sender == email)
            {
                login_sender = last_shared.Recipient;
            }

            //выбираем всю последнюю переписку между получателем и отправителем
            ViewBag.Shared = db.Shareds.Where(x => (((x.Recipient == email) && (x.Sender == login_sender)) || ((x.Recipient == login_sender) && (x.Sender == email)))).OrderByDescending(x => x.Id).ToList();

            /////////////////////////////////////////////////
            var categories = db.Categories.Where(x => x.UserId == logining_user.Id && x.DeleteCategory != "Yes").OrderByDescending(x => x.AddCatOrLinkTime).ToList();
            var first_category = db.Categories.Where(x => x.UserId == logining_user.Id && x.DeleteCategory != "Yes").OrderByDescending(x => x.AddCatOrLinkTime).First(); ;
            ViewBag.links = db.Links.Where(x => x.CategoryId == first_category.Id && x.Category.UserId == logining_user.Id).OrderByDescending(x => x.Id).ToList();


            return View(categories);
        }

        [HttpPost]
        public ActionResult ChangeAccountData(int Id, string Email, string Password, string LastEmail, string LastPass) {

            var check = db.Users.Where(x => x.Email == Email).Count();

            if (check == 0)
            {
                var pass = Session["Password"].ToString();

                User user = db.Users.Where(x => x.Id == Id && x.Password == LastPass).First();
                var recipient = db.Shareds.Where(x => x.Recipient == LastEmail).ToList();

                user.Email = Email;
                user.Password = Password;
                user.FirstLogining = "No";

                foreach (var r in recipient) {
                    r.Recipient = Email;
                }

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();

                Session["Id"] = Id;
                Session["Email"] = Email;
                Session["Role"] = "user";
                Session["Password"] = Password;
                Session["FirstLogining"] = user.FirstLogining;
                //Session["ErrorLogin"] = "";


                return RedirectToAction("Index");
            }
            else if ((check == 1)&&(Email == LastEmail))
            {             
                return RedirectToAction("Index");               
            }
            else
            {             
                return RedirectToAction("Index", new { error_change_account_data = "этот логин занят"});
            }
        }

        ///////////////////Обычная рабочая зона/////////////////////////
        public ActionResult Workzone()
        {

            Session["error_sare"] = 0;

            Session["category_id"] = 0;

            var user_id = Convert.ToInt32(Session["Id"]);
            var email = Session["Email"].ToString();
            var role = Session["Role"].ToString();

            if (role == "user")
            {
                //categories for menu
                var category = db.Categories.Where(x => x.UserId == user_id && x.DeleteCategory != "Yes").OrderByDescending(x => x.AddCatOrLinkTime);
                //after logging to user account, downloaded links  last added category
                var first_category = db.Categories.Where(x => x.UserId == user_id && x.DeleteCategory != "Yes").OrderByDescending(x => x.AddCatOrLinkTime).First();
                ViewBag.links = db.Links.Where(x => x.CategoryId == first_category.Id && x.Category.UserId == user_id).OrderByDescending(x => x.Id).ToList();

                return PartialView(category);
            }
            else
            {
                return RedirectToAction("Index", "Error");
            }
        }

        /////////////////////////////////Отправка ссылок, просмотр всех сылок////////////////////////

        public ActionResult SharedLinks(string Recipient, string Description, string SharedLinks, int check = 0) {

            Session["category_id"] = 0;

            var email = Session["Email"].ToString();

            List<string> id = new List<string>();

            if((SharedLinks != null)&&(SharedLinks != ""))
            {
                //с переданой строки формируем масив id 
                string[] mass_id = SharedLinks.Split(new char[] { ',' });

                for (int i = 0; i < mass_id.Length; i++)
                {
                    if (mass_id[i] != "")
                    {
                        id.Add(mass_id[i]);
                    }

                }

            }

            //РАЗШАРИВАНИЕ СЫЛОК 
            if ((SharedLinks != null) && ((Recipient != null) && (Recipient != "")))
            {
                var check_recipient = db.Users.Where(x => x.Email == Recipient).Count();
                //ошибки еще надо пофиксить
                if (check_recipient == 0)
                {
                    ViewBag.LoginError = "Error";
                }
                else if (Recipient == email)
                {
                    ViewBag.LoginError = "Error";
                }
                else
                {
                    //то инициализируем транзакцию передачи сылок и сохраняем ее в базу
                    Shared shared = new Shared { Sender = email, Recipient = Recipient, DateTime = DateTime.Now, Description = Description };
                    db.Shareds.Add(shared);
                    db.SaveChanges();

                    //привязываем переданые сылке к транзакции
                    foreach (var i in id)
                    {

                        var id_link = int.Parse(i);
                        Link link = db.Links.Find(id_link);
                        shared.Links.Add(link);
                    }
                    //сохраняем изменения в бд
                    db.Entry(shared).State = EntityState.Modified;
                    db.SaveChanges();


                    //формируем список пользователей которые нам или мы им отправили сылки              
                    var shareds = db.Shareds.Where(x => (x.Recipient == email) || (x.Sender == email)).OrderByDescending(x => x.Id).ToList();

                    List<CategorySender> categorySenders = new List<CategorySender>();

                    var previus_sender = "";

                    foreach (var s in shareds)
                    {
                        if (s.Sender != email)
                        {
                            categorySenders.Add(new CategorySender(s.Sender));
                            previus_sender = s.Sender;
                        }
                        else
                        {
                            categorySenders.Add(new CategorySender(s.Recipient));
                            previus_sender = s.Recipient;
                        }
                    }

                    ViewBag.Sender = categorySenders.GroupBy(x => x.Sender).Select(y => new SenderViewModel { Sender = y.Key });

                    //список пользователей в порядке кто последний отправляли сылки
                    var last_shared = db.Shareds.Where(x => (x.Recipient == email) || (x.Sender == email)).OrderByDescending(x => x.Id).First(); ;
                    //выбираем логин отправителя исключая собственный

                    string login_sender = last_shared.Sender;

                    if (last_shared.Sender == email)
                    {
                        login_sender = last_shared.Recipient;
                    }

                    //выбираем всю переписку между получателем и отправителем
                    ViewBag.Shared = db.Shareds.Where(x => (((x.Recipient == email) && (x.Sender == login_sender)) || ((x.Recipient == login_sender) && (x.Sender == email)))).OrderByDescending(x => x.Id).ToList();
                }
            }
            //ПЕРЕХОД ВО ВКЛАДКУ ОТПРАВЛЕННЫЕ
            else if (((SharedLinks == "") || (SharedLinks == null)) && (check != 0))
            {
                Session["category_id"] = 0;          

                var check_shared_users = db.Shareds.Where(x => (x.Recipient == email || x.Sender == email) && (x.DelChatForOne != email && x.DelChatForTwo != email)).Count();

                if (check_shared_users != 0)
                {
                    //формируем список пользователей которые нам или мы им отправили сылки              
                    var shared = db.Shareds.Where(x => (((x.Recipient == email) || (x.Sender == email)) && ((x.DelChatForOne != email) && (x.DelChatForTwo != email)))).OrderByDescending(x => x.Id).ToList();

                    //список отправителей
                    List<CategorySender> categorySenders = new List<CategorySender>();

                    //предыдущий отправитель
                    var previus_sender = "";

                    //формируем список отправителей
                    foreach (var s in shared)
                    {
                        if (s.Sender != email)
                        {
                            categorySenders.Add(new CategorySender(s.Sender));
                            previus_sender = s.Sender;
                        }
                        else
                        {
                            categorySenders.Add(new CategorySender(s.Recipient));
                            previus_sender = s.Recipient;
                        }
                    }

                    //групируем отправителей по логинах что бы не было повторений и отправляем результат в представление
                    ViewBag.Sender = categorySenders.GroupBy(x => x.Sender).Select(y => new SenderViewModel { Sender = y.Key });

                    //список пользователей в порядке кто последний отправлял сылки
                    var last_shared = db.Shareds.Where(x => (((x.Recipient == email) || (x.Sender == email)) && ((x.DelChatForOne != email) && (x.DelChatForTwo != email)))).OrderByDescending(x => x.Id).First();



                    //отмечаем новый не просмотренные ссылки
                    var shareds = db.Shareds.Where(x => x.Recipient == email).ToList();

                    foreach (var s in shareds)
                    {
                        if ((s.ReadStatus == null) && (s.Sender == last_shared.Sender))
                        {
                            s.ReadStatus = "read";
                            s.NewShared = "new";
                            db.SaveChanges();
                        }
                        else if ((s.ReadStatus == "read") && (s.Sender == last_shared.Sender))
                        {
                            s.NewShared = null;
                            db.SaveChanges();
                        }
                    }
                    ////////////////////////////////////


                    //выбираем логин отправителя исключая собственный
                    string login_sender = last_shared.Sender;

                    if (last_shared.Sender == email)
                    {
                        login_sender = last_shared.Recipient;
                    }

                    //выбираем всю последнюю переписку между получателем и отправителем
                    ViewBag.Shared = db.Shareds.Where(x => ((((x.Recipient == email) && (x.Sender == login_sender)) && ((x.DelChatForOne != email) && (x.DelChatForTwo != email))) || (((x.Recipient == login_sender) && (x.Sender == email)) && ((x.DelChatForOne != email) && (x.DelChatForTwo != email))))).OrderByDescending(x => x.Id).ToList();
                }
                else
                {
                    ViewBag.NoShareds = "null";
                }
            }
            //РОЗШАРИВАНИЕ ПО СЫЛКЕ
            else if ((SharedLinks != null) && (Recipient == ""))
            {
                //автоматическое создание пользователя при розшаривании сылок когда получатель не зарегистрирован
                Session["error_sare"] = 0;
                Session["ErrorLogin"] = "";

                var login = "";
                var password = "";
                var code_link = "";
                var creating_account_for_link = "";

                var check_login = 0;
                var check_creating_account_for_link = 0;

                do
                {
                    login = GenerateLogin();
                    password = GeneratePass();

                    code_link = GenerateLogin();

                    creating_account_for_link = "http://localhost:28233/User/SignInForLink/?code=" + code_link;

                    check_login = db.Users.Where(x => x.Email == login).Count();
                    check_creating_account_for_link = db.Users.Where(x => x.CreatingAccountForLink == creating_account_for_link).Count();

                } while ((check_login != 0) && (check_creating_account_for_link != 0));


                User user = new User { Email = login, Password = password, FirstLogining = "Yes", CreatingAccountForLink = creating_account_for_link, UserGivesLink = email };
                db.Users.Add(user);
                db.SaveChanges();

                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////




                //то инициализируем транзакцию передачи сылок и сохраняем ее в базу
                Shared shared = new Shared { Sender = email, Recipient = login, DateTime = DateTime.Now, Description = "Доступ по ссылке: " + creating_account_for_link + " Описание: " + Description };
                db.Shareds.Add(shared);
                db.SaveChanges();

                //привязываем переданые сылке к транзакции
                foreach (var i in id)
                {
                    var id_link = int.Parse(i);
                    Link link = db.Links.Find(id_link);
                    shared.Links.Add(link);
                }
                //сохраняем изменения в бд
                db.Entry(shared).State = EntityState.Modified;
                db.SaveChanges();


                //формируем список пользователей которые нам или мы им отправили сылки              
                var shareds = db.Shareds.Where(x => (x.Recipient == email) || (x.Sender == email)).OrderByDescending(x => x.Id).ToList();

                List<CategorySender> categorySenders = new List<CategorySender>();

                var previus_sender = "";

                foreach (var s in shareds)
                {
                    if (s.Sender != email)
                    {
                        categorySenders.Add(new CategorySender(s.Sender));
                        previus_sender = s.Sender;
                    }
                    else
                    {
                        categorySenders.Add(new CategorySender(s.Recipient));
                        previus_sender = s.Recipient;
                    }
                }

                ViewBag.Sender = categorySenders.GroupBy(x => x.Sender).Select(y => new SenderViewModel { Sender = y.Key });

                //список пользователей в порядке кто последний отправляли сылки
                var last_shared = db.Shareds.Where(x => (x.Recipient == email) || (x.Sender == email)).OrderByDescending(x => x.Id).First(); ;
                //выбираем логин отправителя исключая собственный

                string login_sender = last_shared.Sender;

                if (last_shared.Sender == email)
                {
                    login_sender = last_shared.Recipient;
                }

                //выбираем всю переписку между получателем и отправителем
                ViewBag.Shared = db.Shareds.Where(x => (((x.Recipient == email) && (x.Sender == login_sender)) || ((x.Recipient == login_sender) && (x.Sender == email)))).OrderByDescending(x => x.Id).ToList();

            }

            return PartialView();

        }

        ///////////////////////////отмена розшаривания своего блока сылок в чате///////////////////////////////////
        public ActionResult CancelSharedLinks(int Id) {
            Shared shared = db.Shareds.Where(x => x.Id == Id).First();
            db.Shareds.Remove(shared);
            db.SaveChanges();
            return View();
        }

        ///////////////////////////сохранение блока ссылок с чата, где id - это id транзакции в таблице Shared////////////////////////////////////////
        [HttpPost]
        public ActionResult SaveSharedLinks(string ShareID, string Category_Id, string NameCategory) {

            var user_id = Convert.ToInt32(Session["Id"]);
            var share_id = int.Parse(ShareID);
            var SendLinks = db.Shareds.Where(x => x.Id == share_id).First();

            if (NameCategory != null)
            {
                var check_cat = db.Categories.Where(x => x.Name == NameCategory).Count();

                if (check_cat == 0)
                {
                    Category category = new Category { Name = NameCategory, UserId = user_id, AddCatOrLinkTime = DateTime.Now };
                    db.Categories.Add(category);
                    db.SaveChanges();

                    var id_new_category = db.Categories.Where(x => x.Name == category.Name && x.UserId == user_id).First();

                    foreach (var l in SendLinks.Links)
                    {
                        Link link = new Link { Url = l.Url, Description = l.Description, CategoryId = id_new_category.Id };

                        db.Links.Add(link);
                    }
                    db.SaveChanges();
                }
            }
            else
            {
                var cat_id = int.Parse(Category_Id);
                foreach (var l in SendLinks.Links)
                {
                    Link link = new Link { Url = l.Url, Description = l.Description, CategoryId = cat_id };

                    db.Links.Add(link);

                }
                db.SaveChanges();
            }

            return View();
        }

        public ActionResult DelChat(string Sender)
        {
            var email = Session["Email"].ToString();

            var shared = db.Shareds.Where(x => ((x.Sender == Sender && x.Recipient == email)||(x.Sender == email && x.Recipient == Sender))).ToList();

            foreach (var s in shared)
            {
                if (s.DelChatForOne == null)
                {
                    
                    s.DelChatForOne = email; 
                }
                else if (s.DelChatForTwo == null)
                {
                    s.DelChatForTwo = email;             
                }
            }

            db.SaveChanges();

            return PartialView();
        }

        ///////////////////просмотр ссылок по отправителям/////////////////////////
        public ActionResult ViewSharedLinks(string login_sender)
        {
            var email = Session["Email"].ToString();

            ViewBag.Sender = db.Shareds.Where(x => (x.Recipient == email) || (x.Sender == email)).OrderByDescending(x => x.Id).GroupBy(x => x.Sender).Select(y => new SenderViewModel { Sender = y.Key }).ToList();

            ViewBag.Shared = db.Shareds.Where(x => ((x.Recipient == email) && (x.Sender == login_sender)) || ((x.Recipient == login_sender) && (x.Sender == email))).OrderByDescending(x => x.Id).ToList();

            //если ты нажал на отправителя значит ты просмотрел все отправленные ссылки
          
            var shareds = db.Shareds.Where(x => x.Recipient == email).ToList();

            foreach (var s in shareds)
            {
                if((s.ReadStatus == null) && (s.Sender == login_sender))
                {
                    s.ReadStatus = "read";
                    s.NewShared = "new";
                    db.SaveChanges();
                }
                else if((s.ReadStatus == "read") && (s.Sender == login_sender)){
                    s.NewShared = null;
                    db.SaveChanges();
                }
            }

            return PartialView();
        }
    }
}