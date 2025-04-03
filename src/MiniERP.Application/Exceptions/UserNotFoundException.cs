namespace MiniERP.Application.Exceptions
{
    public class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException(int userId)
            : base($"User with ID {userId} was not found.") { }
    }
}