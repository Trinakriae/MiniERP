namespace MiniERP.RestApi.Models.Users;

public record UpdateUserRequest(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber);
