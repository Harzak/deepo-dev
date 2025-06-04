using System;
using System.Collections.Generic;

namespace Deepo.DAL.EF.Models;

public partial class HttpRequest
{
    public int HttpRequest_ID { get; set; }

    public string? ApplicationName { get; set; }

    public string? JobID { get; set; }

    public string? LevelLog { get; set; }

    public DateTime? DateLogCreation { get; set; }

    public string? RequestUri { get; set; }

    public string? HttpMethod { get; set; }

    public string? HttpResponse { get; set; }

    public string? Date { get; set; }

    public string? UserAgent { get; set; }

    public string? Token { get; set; }

    public string? ResponseMessage { get; set; }
}
