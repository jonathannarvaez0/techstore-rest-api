namespace Application.Models
{
    public class UserModel
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string? email { get; set; }
        public string? contact { get; set; }
        public string? firstname { get; set; }
        public string? lastname { get; set; }
    }
}
