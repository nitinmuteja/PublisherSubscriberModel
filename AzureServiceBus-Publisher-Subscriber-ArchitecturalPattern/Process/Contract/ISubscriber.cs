using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Process.Contract
{
  public  interface ISubscriber
    {
        void OnMessage<T>(Action<T> anonymousMethod);

    }
}
