namespace IdentityProvider.Models
{
    public class Action
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public int ControllerId { get; set; }
        public Controller Controller { get; set; }
    }
}
