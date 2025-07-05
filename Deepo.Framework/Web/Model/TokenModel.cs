using System;

namespace Deepo.Framework.Web.Model;

public class TokenModel
{
    public string Value { get; set; }
    public string Type { get; set; }
    public bool IsExpired { get => DateTime.Now > _expiration; }

    private readonly DateTime _expiration;

    public TokenModel()
    {
        _expiration = DateTime.MinValue;
        Value = string.Empty;
        Type = string.Empty;
    }

    public TokenModel(string value, string type, TimeSpan expiresIn)
    {
        Value = value;
        Type = type;
        _expiration = DateTime.Now.Add(expiresIn);
    }
}

