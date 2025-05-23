﻿namespace CleanArchitectureSystem.Application.Models.Identity
{
    public class User
    {
        public string Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
    }
}
