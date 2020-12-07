using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BDLauncherCSharp.Extensions;
using BDLauncherCSharp.ViewModels;

namespace BDLauncherCSharp.Data
{
    class UserCustoms
    {
        public static bool IsCNCDDraw => FileHashIdentify.SHA512Verify("ddraw.dll", OverAll.CNCD);
        public static string CurRenderer => IsCNCDDraw ? I18NExtension.I18N("cbRenderer.CNCDDraw") : I18NExtension.I18N("cbRenderer.None");
    }
}
