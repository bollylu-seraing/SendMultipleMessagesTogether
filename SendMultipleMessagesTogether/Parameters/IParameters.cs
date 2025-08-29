using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendMultipleMessagesTogether {
  public interface IParameters {
    bool Read();
    bool Save();

    ILogger Logger { get; }

    ELogType LogType { get; }
    // In case it's a file type logger, here is the filename and path
    string LogFilename { get; }

    string Recipient { get; }
    string Prefix { get; }
  }
}
