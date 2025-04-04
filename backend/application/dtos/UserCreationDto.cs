﻿using application.transformers;

namespace application.dtos;

public class UserCreationDto
{
    private string _email;

    public string Email
    {
        get => _email;
        set => _email = InputSanitizer.Sanitize(value);
    }

    public string HashedPassword { get; set; }
    public string Salt { get; set; }
}