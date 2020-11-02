using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BDLauncherCSharp.Controls;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace BDLauncherCSharp.Data
{
    public class GameExecuteOptions
    {
        public bool LogMode;
        public bool RunAs;
        public string[] Others;
    }
}