﻿namespace CommerceApi.Models.UserModels
{
    public class RegisterModel
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int PhoneNumber { get; set; }
    }
}