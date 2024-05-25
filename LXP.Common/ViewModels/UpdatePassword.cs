namespace LXP.Common.ViewModels
{

    public class UpdatePassword
    {

        public string? Email { get; set; }

        public string? OldPassword { get; set; }

        public string? NewPassword { get; set; }

    }

    public class ResultUpdatePassword
    {
        public bool success { get; set; }
    }
}
