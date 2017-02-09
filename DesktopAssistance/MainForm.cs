using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DesktopAssistance.Model;

namespace DesktopAssistance
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left, Top, Right, Bottom;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct APPBARDATA
    {
        //  typedef struct _AppBarData
        //          {
        //            DWORD cbSize;
        //            HWND hWnd;
        //            UINT uCallbackMessage;
        //            UINT uEdge;
        //            RECT rc;
        //            LPARAM lParam;
        //          } APPBARDATA, *PAPPBARDATA;
        public static readonly int cbSize = Marshal.SizeOf(typeof(APPBARDATA));
        public IntPtr hWnd;
        public uint uCallbackMessage;
        public uint uEdge;
        public RECT rc;
        public int lParam;
    }

    public enum ABMsg
    {
        ABM_NEW = 0,
        ABM_REMOVE = 1,
        ABM_QUERYPOS = 2,
        ABM_SETPOS = 3,
        ABM_GETSTATE = 4,
        ABM_GETTASKBARPOS = 5,
        ABM_ACTIVATE = 6,
        ABM_GETAUTOHIDEBAR = 7,
        ABM_SETAUTOHIDEBAR = 8,
        ABM_WINDOWPOSCHANGED = 9,
        ABM_SETSTATE = 10
    }

    public enum ABEdge
    {
        ABE_LEFT = 0,
        ABE_TOP = 1,
        ABE_RIGHT = 2,
        ABE_BOTTOM = 3
    }

    public partial class MainForm : Form
    {
        private RunEngine _engine;
        private CommandsManager _commands;
        private SimpleSpeechRecognizer _recognizer;

        /// <summary>
///     Sends an appbar message to the system.
///     <para>
///         Go to https://msdn.microsoft.com/en-us/library/windows/desktop/bb762108%28v=vs.85%29.aspx for more
///         information.
///     </para>
/// </summary>
/// <param name="dwMessage">
///     C++ ( dwMessage [in] Type: DWORD )<br />Appbar message value to send. This parameter can be one of the following
///     values.<br />
///     <list type="table">
///         <listheader>
///             <term>Possible AppBar Values</term>
///             <description>Any possible app bar value used int he function</description>
///         </listheader>
///         <item>
///             <term>ABM_NEW (0x00000000)</term>
///             <description>
///                 Registers a new appbar and specifies the message identifier that the system should use to
///                 send notification messages to the appbar.
///             </description>
///         </item>
///         <item>
///             <term>ABM_REMOVE (0x00000001)</term>
///             <description>Unregisters an appbar, removing the bar from the system's internal list.</description>
///         </item>
///         <item>
///             <term>ABM_QUERYPOS (0x00000002)</term>
///             <description>Requests a size and screen position for an appbar.</description>
///         </item>
///         <item>
///             <term>ABM_SETPOS (0x00000003)</term>
///             <description>Sets the size and screen position of an appbar.</description>
///         </item>
///         <item>
///             <term>ABM_GETSTATE (0x00000004)</term>
///             <description>Retrieves the autohide and always-on-top states of the Windows taskbar.</description>
///         </item>
///         <item>
///             <term>ABM_GETTASKBARPOS (0x00000005)</term>
///             <description>
///                 Retrieves the bounding rectangle of the Windows taskbar. Note that this applies only to the
///                 system taskbar. Other objects, particularly toolbars supplied with third-party software, also can be
///                 present. As a result, some of the screen area not covered by the Windows taskbar might not be visible
///                 to the user. To retrieve the area of the screen not covered by both the taskbar and other app bars—the
///                 working area available to your application—, use the GetMonitorInfo function.
///             </description>
///         </item>
///         <item>
///             <term>ABM_ACTIVATE (0x00000006)</term>
///             <description>
///                 Notifies the system to activate or deactivate an appbar. The lParam member of the APPBARDATA
///                 pointed to by pData is set to TRUE to activate or FALSE to deactivate.
///             </description>
///         </item>
///         <item>
///             <term>ABM_GETAUTOHIDEBAR (0x00000007)</term>
///             <description>Retrieves the handle to the autohide appbar associated with a particular edge of the screen.</description>
///         </item>
///         <item>
///             <term>ABM_SETAUTOHIDEBAR (0x00000008)</term>
///             <description>Registers or unregisters an autohide appbar for an edge of the screen.</description>
///         </item>
///         <item>
///             <term>ABM_WINDOWPOSCHANGED (0x00000009)</term>
///             <description>Notifies the system when an appbar's position has changed.</description>
///         </item>
///         <item>
///             <term>ABM_SETSTATE (0x0000000A)</term>
///             <description>Windows XP and later: Sets the state of the appbar's autohide and always-on-top attributes.</description>
///         </item>
///     </list>
/// </param>
/// <param name="pData">
///     C++ ( pData [in, out] Type: PAPPBARDATA )<br /> A pointer to an APPBARDATA structure. The content
///     of the structure on entry and on exit depends on the value set in the dwMessage parameter. See the individual
///     message pages for specifics.
/// </param>
/// <returns>
///     C++ ( Type: UINT_PTR )<br />This function returns a message-dependent value. For more information, see the
///     Windows SDK documentation for thespecific appbar message sent.
/// </returns>
        [DllImport("shell32.dll")]
        static extern IntPtr SHAppBarMessage(uint dwMessage,  [In] ref APPBARDATA pData);

        const int WM_USER = 0x0400;

        public MainForm()
        {
            InitializeComponent();

            HandleCreated += OnHandleCreated;

            HandleDestroyed += OnHandleDestroyed;

            Load += OnLoad;
        }

        private void OnLoad(object sender, EventArgs eventArgs)
        {
            GoBottomOfScreen();

            LoadCommands();

            textBoxCommands.Focus();
        }

        private void OnHandleCreated(object sender, EventArgs eventArgs)
        {

        }

        private void LoadCommands()
        {
            _commands = new CommandsManager();

            ShowInfo(_commands.Loaded ? $"{_commands.Commands.Contexts.Sum(s => s.Commands.Count)} commands ready in {_commands.Commands.Contexts.Count} context" : "No commands found");

            _engine = new RunEngine(_commands, (sender, s) => ShowInfo(s));

            _recognizer = new SimpleSpeechRecognizer();
            _recognizer.OnSpeech += RecognizerOnOnSpeech;
            var speechCommands = _engine.GetAllCommands();
            speechCommands.Add("go");
            _recognizer.Init(speechCommands.ToArray());
        }

        private void RecognizerOnOnSpeech(string s, float f, string[] a)
        {
            if (f > 0.8)
            {
                ShowInfo(string.Format("'{0}' with chance {1:F2}. Alternatives: ({2})", s, f, string.Join(", ", a)));

                if (f > 0.9)
                {
                    if (s == "go")
                        _engine.RunCommand(textBoxCommands.Text);
                    else textBoxCommands.Text = s.Trim().ToLower();
                }
            }
        }

        private void OnHandleDestroyed(object sender, EventArgs eventArgs)
        {
            APPBARDATA abd = new APPBARDATA();
            abd.hWnd = Handle;
            // Ignored members: uCallbackMessage, uEdge, rc, lParam
            SHAppBarMessage((int)ABMsg.ABM_REMOVE, ref abd);
        }

        private void GoBottomOfScreen()
        {
            var screen = Screen.AllScreens.Length > 1 ? Screen.AllScreens[1] : Screen.PrimaryScreen;

            Left = screen.WorkingArea.Left;
            Width = screen.WorkingArea.Width;
            //SetDesktopLocation(screen.WorkingArea.Bottom - Height, screen.WorkingArea.Left);

            APPBARDATA abd = new APPBARDATA();
            abd.hWnd = Handle;
            abd.uCallbackMessage = WM_USER + 100;
            // Ignored members: uEdge, rc, lParam
            Debug.WriteLine("ABM_NEW: " + SHAppBarMessage((int)ABMsg.ABM_NEW, ref abd).ToInt32());


            //APPBARDATA abd;
            //abd.cbSize = sizeof(abd);
            //abd.hWnd = hwndOfWindowToPosition;
            abd.uEdge = (int)ABEdge.ABE_BOTTOM;
            abd.rc.Top = screen.WorkingArea.Top;
            abd.rc.Left = screen.WorkingArea.Left;
            abd.rc.Right = screen.WorkingArea.Right;
            abd.rc.Bottom = screen.WorkingArea.Bottom;

            //SetRect(&abd.rc, 0, 0, GetSystemMetrics(SM_CXSCREEN),
            //        GetSystemMetrics(SM_CYSCREEN));
            // Ignored members: uCallbackMessage, lParam
            Debug.WriteLine("ABM_QUERYPOS: " + SHAppBarMessage((int)ABMsg.ABM_QUERYPOS, ref abd).ToInt32());


            switch ((ABEdge)abd.uEdge)
            {
                case ABEdge.ABE_LEFT:
                    abd.rc.Right = abd.rc.Left + Width;
                    break;

                case ABEdge.ABE_TOP:
                    abd.rc.Bottom = abd.rc.Top + Height;
                    break;

                case ABEdge.ABE_RIGHT:
                    abd.rc.Left = abd.rc.Right - Width;
                    break;

                case ABEdge.ABE_BOTTOM:
                    abd.rc.Top = abd.rc.Bottom - Height;
                    break;
            }


            // Leave all the members as they were when
            // the ABM_QUERYPOS message was sent
            // Ignored members: uCallbackMessage, lParam
            Debug.WriteLine("ABM_SETPOS: " + SHAppBarMessage((int)ABMsg.ABM_SETPOS, ref abd).ToInt32());


            Top = abd.rc.Top;

            //SetWindowPos(abd.hWnd, NULL, abd.rc.left, abd.rc.top,
            // abd.rc.right - abd.rc.left, abd.rc.bottom - abd.rc.top,
            // SWP_NOZORDER | SWP_NOACTIVATE);


            //APPBARDATA abd;
            //abd.cbSize = sizeof(abd);
            //abd.hWnd = hwndOfWindowToMakeAutohide;
            abd.uEdge = (int)ABEdge.ABE_BOTTOM;
            abd.lParam = 1;      // Make us autohide
                                    // Ignored members: uCallbackMessage, rc
            //SHAppBarMessage((int)ABMsg.ABM_SETAUTOHIDEBAR, ref abd);
        }

        public void ShowInfo(string message)
        {
            textBoxInfo.Text = message;
        }

        private void textBoxCommands_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                InvokeCommand(textBoxCommands.Text.Trim());
            }
        }

        private void InvokeCommand(string command)
        {
            _engine.RunCommand(command.ToLower());
        }
    }
}
