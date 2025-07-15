using System.ComponentModel.DataAnnotations;
namespace Test.Entities
{ 
public class User
{
        public DateTime ModifiedDate;

        public int Id { get; set; }

    [Required(ErrorMessage = "First name is required.")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required.")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string EmailAddress { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Confirm password is required.")]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; }

    [Required(ErrorMessage = "Phone number is required.")]
    public string PhoneNumber { get; set; }

    public string UserType { get; set; }
    public string UserImage { get; set; }

    public bool IsDeleted { get; set; } = false;
    public DateTime? ModificationDate { get; set; }
}


public class MessageRes
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object? Data { get; set; }
    }
    
}
