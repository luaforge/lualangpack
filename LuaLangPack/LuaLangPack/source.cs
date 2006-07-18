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

      public override void ProcessHiddenRegions(System.Collections.ArrayList hiddenRegions)
      {
         IVsHiddenTextSession ht = GetHiddenTextSession();
         if (ht != null)
         {
              base.ProcessHiddenRegions(hiddenRegions);            
         }

      }

      public override void  OnHiddenRegionChange(IVsHiddenRegion region, HIDDEN_REGION_EVENT evt, int fBufferModifiable)
      {
 	      base.OnHiddenRegionChange(region, evt, fBufferModifiable);
      }
	}
}
