using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace testCode.Controllers
{
    [Route("[controller]")]
    public class UserDataController: Controller{

        private readonly ILogger<UserDataController> _logger;

        public UserDataController(ILogger<UserDataController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("Add")]
        public JsonResult AddUser(User[] usersList)
        {
            List<User> SavedUsers = new List<User>();

            try{
                //Using file instead of Database for simplicity
                //read file into a string and deserialize JSON to List<User>
                System.IO.StreamReader file = new System.IO.StreamReader(System.AppDomain.CurrentDomain.BaseDirectory + @"\Users.JSON");
                string json = file.ReadToEnd();
                file.Close();
                SavedUsers = JsonConvert.DeserializeObject<List<User>>(json);
                usersList.ToList().ForEach(user =>
                {
                    //validate user
                    if (user.Id > 0 && !string.IsNullOrEmpty(user.FirstName) && !string.IsNullOrEmpty(user.LastName) && user.Age > 18)
                    {
                        //check duplicate Id
                        if (!SavedUsers.Any(savedUser => savedUser.Id == user.Id))
                        {
                            SavedUsers.Add(user);
                        }
                    }
                });

                //sort by LastName then FirstNames
                SavedUsers = SavedUsers.OrderBy(user => user.LastName).ThenBy(user => user.FirstName).ToList();

                //write string to file
                System.IO.File.WriteAllText(System.AppDomain.CurrentDomain.BaseDirectory + @"\Users.JSON", JsonConvert.SerializeObject(SavedUsers));
                
                return Json(new { message = "Users added successfully" });
            }
            catch(Exception ex)
            {
                return Json(new { message = "Error: " + ex.Message });
            }
        }
        

    }        

    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age{ get; set; }
        public int Id { get; set; }
    }

}