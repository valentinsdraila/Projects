using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace UserService.Model;
//[HasSelfValidation]
public class User
{
    /// <summary>Initializes a new instance of the <see cref="User" /> class.</summary>
    public User()
    {
        this.Username=string.Empty;
        this.Password = string.Empty;
        this.FirstName=string.Empty;
        this.Name=string.Empty;
        this.Email = string.Empty;
        this.PhoneNumber = string.Empty;
        this.CreatedAt = DateTime.Now;
        this.Role = "User";
    }

    /// <summary>Initializes a new instance of the <see cref="User" /> class.</summary>
    /// <param name="username">The username.</param>
    /// <param name="password">The hashed password.</param>
    /// <param name="name">The name.</param>
    /// <param name="firstName">The first name.</param>
    /// <param name="email">The email.</param>
    /// <param name="phoneNumber">The phone number.</param>
    public User(string username, string password, string name, string firstName, string email, string phoneNumber)
    {
        this.Username = username;
        this.Password = password;
        this.Name = name;
        this.FirstName = firstName;
        this.Email = email;
        this.PhoneNumber = phoneNumber;
        this.CreatedAt = DateTime.Now;
        this.Role = "User";
    }

    /// <summary>Gets or sets the identifier.</summary>
    /// <value>The identifier.</value>
    [Key]
    public Guid Id { get; set; }
    [Required]
    public string Password { get; set; }
    /// <summary>Gets or sets the username.</summary>
    /// <value>The username.</value>
    //[NotNullValidator(MessageTemplate = "The username cannot be null!")]
    //[StringLengthValidator(3, RangeBoundaryType.Inclusive, 24, RangeBoundaryType.Inclusive, MessageTemplate ="The username length should be between 3 and 16 characters!")]
    [MinLength(3)]
    [MaxLength(24)]
    [RegularExpression(@"^[^\s]+$", ErrorMessage = "The username must not contain spaces.")]
    public string Username { get; set; }

    /// <summary>Gets or sets the name.</summary>
    /// <value>The name.</value>
    //[NotNullValidator(MessageTemplate = "The user's last name cannot be null")]
    [MinLength(2)]
    [MaxLength(24)]
    [RegularExpression(@"^[A-Z][^\s]*$", ErrorMessage = "The last name must begin with an uppercase character and contain no spaces.")]
    public string Name { get; set; }

    /// <summary>Gets or sets the first name.</summary>
    /// <value>The first name.</value>
    [MinLength(2)]
    [MaxLength(24)]
    [RegularExpression("^[A-Z].*", ErrorMessage = "The string must begin with an uppercase character.")]
    public string FirstName { get; set; }

    /// <summary>Gets or sets the date of account creation.</summary>
    /// <value>The date of account creation.</value>
    public DateTime CreatedAt { get; set; }

    /// <summary>Gets or sets the email.</summary>
    /// <value>The email.</value>
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Please enter a valid email address.")]
    public string Email { get; set; }

    /// <summary>Gets or sets the phone number.</summary>
    /// <value>The phone number.</value>
    [RegularExpression("^[0-9]*$", ErrorMessage = "The phone number must only contain digits.")]
    public string PhoneNumber { get; set; }

    /// <summary>Gets or sets a value indicating whether this instance is an admin.</summary>
    /// <value>
    ///   <c>true</c> if this instance is an admin; otherwise, <c>false</c>.</value>
    public string Role { get; set; }

    public List<DeliveryAddress> DeliveryAddresses { get; set; }
}