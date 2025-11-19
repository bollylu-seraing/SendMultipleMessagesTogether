using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendMultipleMessagesTogether.Process {
  public interface IProcess {
    bool SendToIndicator();
    bool CleanupSentItems();
  }
}
