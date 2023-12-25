namespace IdentityProvider.Models
{
    public class Controller
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public List<Action> Actions { get; set; }
    }
}
