using System;
using System.IO;
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
         // Fix for MS bug on first time parse
         if( this.LastParseTime == Int32.MaxValue )
            this.LastParseTime = this.LanguageService.Preferences.CodeSenseDelay;

         base.OnIdle(periodic);
      }
	}
}
