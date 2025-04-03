namespace MiniERP.RestApi.Models.Users;

public record CreateUserRequest(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber);
