﻿namespace CleanArchitectureSystem.Application.Exceptions
{
    public class NotFoundException(string name, object key) : Exception($"{name} ({key}) was not found.")
    {
    }
}
