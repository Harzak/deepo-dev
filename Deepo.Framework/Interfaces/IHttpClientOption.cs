using System;

namespace Deepo.Framework.Interfaces;

public interface IHttpClientOption
{
    Uri BaseAddress { get; set; }
    string Name { get; set; }
    string UserAgent { get; set; }
    string TaskID { get; set; }
}
