namespace Todo.Core.Dtos
{
    public class ApplicationUserDto : CreateNewUserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
    }
}
