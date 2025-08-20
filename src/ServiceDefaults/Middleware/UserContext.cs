namespace ServiceDefaults.Middleware;

public class UserContext
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string[] Roles { get; set; } = [];
    public Guid? ActingOnBehalfOfChildId { get; set; } // Optional for parent-child scenarios
}