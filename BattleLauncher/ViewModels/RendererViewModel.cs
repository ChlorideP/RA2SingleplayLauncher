using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleLauncher.ViewModels
{
    public class RendererViewModel
    {
        public string UIName { get; set; }
        public string Id { get; set; }

        public static RendererViewModel CreateById(string id)
        {
            // TODO: 此处写根据操作系统判断推荐图形API逻辑
            var uiName = id;
            if (id.Equals("null", StringComparison.OrdinalIgnoreCase))
                uiName = "None";
            else if ("cnc-ddraw" == id)
                uiName += " (推荐)";
            return new RendererViewModel { Id = id, UIName = uiName };
        }

        public override string ToString() => UIName;
    }
}
