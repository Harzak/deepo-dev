
namespace Deepo.Framework.Interfaces;
public interface IPlanning
{
    DateTime? DateNextStart { get; }

    bool ShouldStart();
}
