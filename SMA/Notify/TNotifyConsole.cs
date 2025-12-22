using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace SMA {

  public class TNotifyConsole : INotify, IDisposable {

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool AllocConsole();

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool FreeConsole();

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GetStdHandle(int nStdHandle);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool SetStdHandle(int nStdHandle, IntPtr hHandle);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr CreateFileA(
      string lpFileName,
      uint dwDesiredAccess,
      uint dwShareMode,
      IntPtr lpSecurityAttributes,
      uint dwCreationDisposition,
      uint dwFlagsAndAttributes,
      IntPtr hTemplateFile);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool CloseHandle(IntPtr hObject);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool WriteFile(
      IntPtr hFile,
      byte[] lpBuffer,
      uint nNumberOfBytesToWrite,
      out uint lpNumberOfBytesWritten,
      IntPtr lpOverlapped);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool ReadConsoleInput(
      IntPtr hConsoleInput,
      [Out] INPUT_RECORD[] lpBuffer,
      uint nLength,
      out uint lpNumberOfEventsRead);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool PeekConsoleInput(
      IntPtr hConsoleInput,
      [Out] INPUT_RECORD[] lpBuffer,
      uint nLength,
      out uint lpNumberOfEventsRead);

    [StructLayout(LayoutKind.Sequential)]
    private struct INPUT_RECORD {
      public ushort EventType;
      public KEY_EVENT_RECORD KeyEvent;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct KEY_EVENT_RECORD {
      public bool bKeyDown;
      public ushort wRepeatCount;
      public ushort wVirtualKeyCode;
      public ushort wVirtualScanCode;
      public char UnicodeChar;
      public uint dwControlKeyState;
    }

    private const int STD_OUTPUT_HANDLE = -11;
    private const int STD_INPUT_HANDLE = -10;
    private const uint GENERIC_WRITE = 0x40000000;
    private const uint GENERIC_READ = 0x80000000;
    private const uint FILE_SHARE_READ = 0x00000001;
    private const uint FILE_SHARE_WRITE = 0x00000002;
    private const uint OPEN_EXISTING = 3;
    private const ushort KEY_EVENT = 1;

    // ANSI color codes
    private const string ANSI_RED = "\x1b[31m";
    private const string ANSI_GREEN = "\x1b[32m";
    private const string ANSI_YELLOW = "\x1b[33m";
    private const string ANSI_BLUE = "\x1b[34m";
    private const string ANSI_MAGENTA = "\x1b[35m";
    private const string ANSI_CYAN = "\x1b[36m";
    private const string ANSI_BOLD = "\x1b[1m";
    private const string ANSI_RESET = "\x1b[0m";

    private bool consoleAllocated = false;
    private bool disposed = false;
    private IntPtr consoleOutputHandle = IntPtr.Zero;
    private IntPtr consoleInputHandle = IntPtr.Zero;

    public TNotifyConsole() {
      // Allocate new console
      consoleAllocated = AllocConsole();

      if (consoleAllocated) {
        // Get handles to the new console
        consoleOutputHandle = GetStdHandle(STD_OUTPUT_HANDLE);
        consoleInputHandle = GetStdHandle(STD_INPUT_HANDLE);
      }
    }

    private void WriteToConsole(string message) {
      if (!consoleAllocated || consoleOutputHandle == IntPtr.Zero) {
        return;
      }

      try {
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(message);
        WriteFile(consoleOutputHandle, buffer, (uint)buffer.Length, out uint written, IntPtr.Zero);
      } catch {
        // swallow exceptions
      }
    }

    private ConsoleKeyInfo ReadFromConsole() {
      if (!consoleAllocated || consoleInputHandle == IntPtr.Zero) {
        return default(ConsoleKeyInfo);
      }

      INPUT_RECORD[] buffer = new INPUT_RECORD[1];

      while (true) {
        // Peek to see if there are any events without consuming them
        if (PeekConsoleInput(consoleInputHandle, buffer, 1, out uint eventsRead) && eventsRead > 0) {
          // Read the actual event
          if (ReadConsoleInput(consoleInputHandle, buffer, 1, out eventsRead) && eventsRead > 0) {
            if (buffer[0].EventType == KEY_EVENT && buffer[0].KeyEvent.bKeyDown) {
              return new ConsoleKeyInfo(
                buffer[0].KeyEvent.UnicodeChar,
                (ConsoleKey)buffer[0].KeyEvent.wVirtualKeyCode,
                (buffer[0].KeyEvent.dwControlKeyState & 0x00000001) != 0,
                (buffer[0].KeyEvent.dwControlKeyState & 0x00000002) != 0,
                (buffer[0].KeyEvent.dwControlKeyState & 0x00000004) != 0);
            }
          }
        } else {
          // No event available, yield to other threads and let Windows process messages
          Thread.Sleep(50);
          Application.DoEvents();
        }
      }
    }

    public void MessageInfo(string message) {
      if (!consoleAllocated) {
        return;
      }

      WriteToConsole(message + "\nPress any key to continue ...\n");
      ReadFromConsole();
    }

    public void MessageError(string message) {
      if (!consoleAllocated) {
        return;
      }

      WriteToConsole($"{ANSI_RED}{message}{ANSI_RESET}\nPress any key to continue ...\n");
      ReadFromConsole();
    }

    public DialogResult MessageYesNo(string message) {
      if (!consoleAllocated) {
        return DialogResult.No;
      }

      WriteToConsole(message + "\nPress 'Y' for Yes or 'N' for No ...\n");

      ConsoleKeyInfo keyInfo;
      do {
        keyInfo = ReadFromConsole();
      } while (keyInfo.KeyChar != 'Y' && keyInfo.KeyChar != 'y' && keyInfo.KeyChar != 'N' && keyInfo.KeyChar != 'n');

      return (keyInfo.KeyChar == 'Y' || keyInfo.KeyChar == 'y') ? DialogResult.Yes : DialogResult.No;
    }

    public DialogResult MessageOkCancel(string message) {
      if (!consoleAllocated) {
        return DialogResult.Cancel;
      }

      WriteToConsole(message + "\nPress Enter for OK or Esc for Cancel ...\n");

      ConsoleKeyInfo keyInfo;
      do {
        keyInfo = ReadFromConsole();
      } while (keyInfo.Key != ConsoleKey.Enter && keyInfo.Key != ConsoleKey.Escape);

      return keyInfo.Key == ConsoleKey.Enter ? DialogResult.OK : DialogResult.Cancel;
    }

    /// <summary>
    /// Display a success message in green
    /// </summary>
    public void MessageSuccess(string message) {
      if (!consoleAllocated) {
        return;
      }

      WriteToConsole($"{ANSI_GREEN}{message}{ANSI_RESET}\nPress any key to continue ...\n");
      ReadFromConsole();
    }

    /// <summary>
    /// Display a warning message in yellow
    /// </summary>
    public void MessageWarning(string message) {
      if (!consoleAllocated) {
        return;
      }

      WriteToConsole($"{ANSI_YELLOW}{message}{ANSI_RESET}\nPress any key to continue ...\n");
      ReadFromConsole();
    }

    /// <summary>
    /// Display a colored message without waiting for input
    /// </summary>
    public void MessageLog(string message, ConsoleMessageType messageType = ConsoleMessageType.Info) {
      if (!consoleAllocated) {
        return;
      }

      string colorCode = GetColorCode(messageType);
      WriteToConsole($"{colorCode}{message}{ANSI_RESET}\n");
    }

    /// <summary>
    /// Display multiple choice selection (A, B, C, etc.)
    /// </summary>
    public char MessageMultipleChoice(string message, params string[] options) {
      if (!consoleAllocated || options.Length == 0) {
        return '\0';
      }

      WriteToConsole(message + "\n");
      for (int i = 0; i < options.Length; i++) {
        char option = (char)('A' + i);
        WriteToConsole($"  {option}) {options[i]}\n");
      }
      WriteToConsole($"\nSelect an option (A-{(char)('A' + options.Length - 1)}) ...\n");

      ConsoleKeyInfo keyInfo;
      char maxOption = (char)('A' + options.Length - 1);
      do {
        keyInfo = ReadFromConsole();
      } while ((keyInfo.KeyChar < 'A' || keyInfo.KeyChar > maxOption) && 
               (keyInfo.KeyChar < 'a' || keyInfo.KeyChar > (char)('a' + options.Length - 1)));

      return char.ToUpper(keyInfo.KeyChar);
    }

    /// <summary>
    /// Display a numbered menu with validation
    /// </summary>
    public int MessageNumberedMenu(string message, params string[] options) {
      if (!consoleAllocated || options.Length == 0) {
        return -1;
      }

      WriteToConsole(message + "\n");
      for (int i = 0; i < options.Length; i++) {
        WriteToConsole($"  {i + 1}) {options[i]}\n");
      }
      WriteToConsole($"\nSelect an option (1-{options.Length}) ...\n");

      string input = "";
      ConsoleKeyInfo keyInfo;
      while (true) {
        keyInfo = ReadFromConsole();
        if (keyInfo.Key == ConsoleKey.Enter && input.Length > 0) {
          int choice;
          if (int.TryParse(input, out choice) && choice > 0 && choice <= options.Length) {
            return choice - 1;
          }
          input = "";
          WriteToConsole("Invalid choice. Try again.\n");
        } else if (keyInfo.Key == ConsoleKey.Backspace && input.Length > 0) {
          input = input.Substring(0, input.Length - 1);
        } else if (char.IsDigit(keyInfo.KeyChar)) {
          input += keyInfo.KeyChar;
        }
      }
    }

    private string GetColorCode(ConsoleMessageType messageType) {
      switch (messageType) {
        case ConsoleMessageType.Info:
          return ANSI_BLUE;
        case ConsoleMessageType.Success:
          return ANSI_GREEN;
        case ConsoleMessageType.Warning:
          return ANSI_YELLOW;
        case ConsoleMessageType.Error:
          return ANSI_RED;
        default:
          return ANSI_RESET;
      }
    }

    public void Dispose() {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing) {
      if (!disposed) {
        if (consoleAllocated) {
          if (consoleOutputHandle != IntPtr.Zero) {
            CloseHandle(consoleOutputHandle);
          }
          if (consoleInputHandle != IntPtr.Zero) {
            CloseHandle(consoleInputHandle);
          }

          FreeConsole();
          consoleAllocated = false;
          consoleOutputHandle = IntPtr.Zero;
          consoleInputHandle = IntPtr.Zero;
        }
        disposed = true;
      }
    }

    ~TNotifyConsole() {
      Dispose(false);
    }

  }

  /// <summary>
  /// Enumeration for console message types with associated colors
  /// </summary>
  public enum ConsoleMessageType {
    Info = 0,
    Success = 1,
    Warning = 2,
    Error = 3
  }

}