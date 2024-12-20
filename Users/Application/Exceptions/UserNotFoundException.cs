﻿namespace Users.Application.Exceptions
{
    public class UserNotFoundException : UserBaseException
    {
        public UserNotFoundException(string instance)
        {
            Type = "user-not-found-exception";
            Title = "User not found exception.";
            Detail = "Required user cannot be found.";
            Instance = instance;
        }
    }
}
