using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepo.Fetcher.Viewer.Interfaces;

public interface IListener : IDisposable
{
    public delegate void DataChanged();
    event EventHandler OnInsert;
    event EventHandler OnUpdate;
    event EventHandler OnDelete;

    bool StartListener();

    void StopListener();
}