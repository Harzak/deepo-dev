using System;

namespace Deepo.Framework.Web.Model;

/// <summary>
/// Represents an authentication token with value, type, and expiration tracking capabilities.
/// </summary>
public class TokenModel
{
    /// <summary>
    /// Gets or sets the token value used for authentication.
    /// </summary>
    public string Value { get; set; }
    
    /// <summary>
    /// Gets or sets the type or scheme of the authentication token.
    /// </summary>
    public string Type { get; set; }
    
    /// <summary>
    /// Gets a value indicating whether the token has expired.
    /// </summary>
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

