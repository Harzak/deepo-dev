﻿namespace Deepo.Dto;

[Serializable]
public class Author
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;
}

