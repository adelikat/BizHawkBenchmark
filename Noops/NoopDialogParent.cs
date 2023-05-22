using System;
using System.Collections.Generic;
using BizHawk.Client.Common;

namespace BizHawkBenchmark.Noops
{
    public class NoopDialogParent : IDialogParent
    {
        public IDialogController DialogController { get; } = new NoopDialogController();

        public class NoopDialogController : IDialogController
        {
            public void AddOnScreenMessage(string message, int? duration = null)
            {
            }

            public IReadOnlyList<string> ShowFileMultiOpenDialog(IDialogParent dialogParent, string filterStr, ref int filterIndex,
                string initDir, bool discardCWDChange = false, string initFileName = null, bool maySelectMultiple = false,
                string windowTitle = null)
            {
                return Array.Empty<string>();
            }

            public string ShowFileSaveDialog(IDialogParent dialogParent, bool discardCWDChange, string fileExt, string filterStr,
                string initDir, string initFileName, bool muteOverwriteWarning)
            {
                return null;
            }

            public void ShowMessageBox(IDialogParent owner, string text, string caption, EMsgBoxIcon? icon)
            {
            }

            public bool ShowMessageBox2(IDialogParent owner, string text, string caption, EMsgBoxIcon? icon,
                bool useOKCancel = false)
            {
                return false;
            }

            public bool? ShowMessageBox3(IDialogParent owner, string text, string caption = null, EMsgBoxIcon? icon = null)
            {
                return false;
            }

            public void StartSound()
            {
            }

            public void StopSound()
            {
            }
        }
    }
}

