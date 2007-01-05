using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.OLE.Interop;
using Tools;

namespace Vsip.LuaLangPack 
{
    class LuaSource : Source
    {
        public LuaSource(LuaLangService service, IVsTextLines buf, Colorizer c)
            : base(service, buf, c)
        {
        }

        public override void OnIdle(bool periodic)
        {
            // We're not yet doing an explicit first parse and the MPF assumes that 
            // we are. 
            if( this.LastParseTime == Int32.MaxValue )
                this.LastParseTime = this.LanguageService.Preferences.CodeSenseDelay;

            base.OnIdle(periodic);
        }

        public override CommentInfo GetCommentFormat()
        {
            CommentInfo inf = new CommentInfo();
            inf.BlockStart = "--[[";
            inf.BlockEnd = "]]";
            inf.LineStart = "--";
            inf.UseLineComments = true;
            return inf;
        }
	}
}
