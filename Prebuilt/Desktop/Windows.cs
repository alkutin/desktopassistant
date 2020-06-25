using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Prebuilt.Desktop
{
    enum GetWindowType : uint
    {
        /// <summary>
        /// The retrieved handle identifies the window of the same type that is highest in the Z order.
        /// <para/>
        /// If the specified window is a topmost window, the handle identifies a topmost window.
        /// If the specified window is a top-level window, the handle identifies a top-level window.
        /// If the specified window is a child window, the handle identifies a sibling window.
        /// </summary>
        GW_HWNDFIRST = 0,

        /// <summary>
        /// The retrieved handle identifies the window of the same type that is lowest in the Z order.
        /// <para />
        /// If the specified window is a topmost window, the handle identifies a topmost window.
        /// If the specified window is a top-level window, the handle identifies a top-level window.
        /// If the specified window is a child window, the handle identifies a sibling window.
        /// </summary>
        GW_HWNDLAST = 1,

        /// <summary>
        /// The retrieved handle identifies the window below the specified window in the Z order.
        /// <para />
        /// If the specified window is a topmost window, the handle identifies a topmost window.
        /// If the specified window is a top-level window, the handle identifies a top-level window.
        /// If the specified window is a child window, the handle identifies a sibling window.
        /// </summary>
        GW_HWNDNEXT = 2,

        /// <summary>
        /// The retrieved handle identifies the window above the specified window in the Z order.
        /// <para />
        /// If the specified window is a topmost window, the handle identifies a topmost window.
        /// If the specified window is a top-level window, the handle identifies a top-level window.
        /// If the specified window is a child window, the handle identifies a sibling window.
        /// </summary>
        GW_HWNDPREV = 3,

        /// <summary>
        /// The retrieved handle identifies the specified window's owner window, if any.
        /// </summary>
        GW_OWNER = 4,

        /// <summary>
        /// The retrieved handle identifies the child window at the top of the Z order,
        /// if the specified window is a parent window; otherwise, the retrieved handle is NULL.
        /// The function examines only child windows of the specified window. It does not examine descendant windows.
        /// </summary>
        GW_CHILD = 5,

        /// <summary>
        /// The retrieved handle identifies the enabled popup window owned by the specified window (the
        /// search uses the first such window found using GW_HWNDNEXT); otherwise, if there are no enabled
        /// popup windows, the retrieved handle is that of the specified window.
        /// </summary>
        GW_ENABLEDPOPUP = 6
    }

    public class Windows : PrebuiltBase
    {


        [DllImport("user32.dll")]
        static extern IntPtr GetTopWindow(IntPtr hWnd);

        /// <summary>
        /// Retrieves a handle to a window that has the specified relationship (Z-Order or owner) to the specified window.
        /// </summary>
        /// <remarks>The EnumChildWindows function is more reliable than calling GetWindow in a loop. An application that
        /// calls GetWindow to perform this task risks being caught in an infinite loop or referencing a handle to a window
        /// that has been destroyed.</remarks>
        /// <param name="hWnd">A handle to a window. The window handle retrieved is relative to this window, based on the
        /// value of the uCmd parameter.</param>
        /// <param name="uCmd">The relationship between the specified window and the window whose handle is to be
        /// retrieved.</param>
        /// <returns>
        /// If the function succeeds, the return value is a window handle. If no window exists with the specified relationship
        /// to the specified window, the return value is NULL. To get extended error information, call GetLastError.
        /// </returns>
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetWindow(IntPtr hWnd, GetWindowType uCmd);
         [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetFocus(IntPtr hWnd);
        [DllImport("user32.dll", SetLastError=true)]
        static extern bool BringWindowToTop(IntPtr hWnd);

        public Windows(EventHandler<string> onMessage, string commandText) : base(onMessage, commandText)
        {
        }

        public void WindowsTab()
        {
            OnMessage(this, "windows tab");
            SendKeys.Send("^%{TAB}");
        }

        public void SwitchApp()
        {
            OnMessage(this, "Switch App activated");
            SendKeys.Send("%({TAB})");
            /*var topMost = GetTopWindow(IntPtr.Zero);
            var nextWindow = GetWindow(topMost, GetWindowType.GW_HWNDNEXT);
            if (nextWindow != IntPtr.Zero)
            {
                BringWindowToTop(nextWindow);
                SetFocus(nextWindow);
            } */
        }

        public void Enter()
        {
            OnMessage(this, "enter");
            SendKeys.Send("{ENTER}");
        }


        public void PageUp()
        {
            OnMessage(this, "page up");
            SendKeys.Send("{PGUP}");
        }

        public void PageDown()
        {
            OnMessage(this, "page down");
            SendKeys.Send("{PGDN}");
        }

        public void PrevTab()
        {
            OnMessage(this, "Previous tab");
            SendKeys.Send("+^({TAB})");
        }

        public void NextTab()
        {
            OnMessage(this, "Next tab");
            SendKeys.Send("^({TAB})");
        }

        public void Left()
        {
            OnMessage(this, "Left");
            SendKeys.Send("{LEFT}");
        }

        public void Right()
        {
            OnMessage(this, "Right");
            SendKeys.Send("{RIGHT}");
        }

        public void Up()
        {
            OnMessage(this, "Up");
            SendKeys.Send("{UP}");
        }

        public void Down()
        {
            OnMessage(this, "Down");
            SendKeys.Send("{DOWN}");
        }

        public void SelectWord()
        {
            
        }
    }
}
