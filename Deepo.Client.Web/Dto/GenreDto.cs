namespace Deepo.Client.Web.Dto;

[Serializable]

public class GenreDto :  IEquatable<GenreDto>
{
    public string Name { get; set; } = string.Empty;
    public Guid Identifier { get; set; } = Guid.Empty;

    public bool Equals(GenreDto? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return this.Identifier == other.Identifier;
    }

    public override bool Equals(object? obj) => obj is GenreDto state && Equals(state);

    public override int GetHashCode() => Identifier.GetHashCode();

    public override string ToString() => Name;
}

