namespace Deepo.DAL.Service.Interfaces;
public interface IAuthor
{
    string? Name { get; set; }
    string Provider_Identifier { get; }
    string Provider_Code { get; }
}
