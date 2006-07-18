using System;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Package;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Globalization;
using System.Resources;
using System.Text;
using System.Threading;
using System.Security.Permissions;

namespace Vsip.LuaLangPack
{
   [AttributeUsage(AttributeTargets.All)]
   internal sealed class SRDescriptionAttribute : DescriptionAttribute
   {
      private bool replaced = false;

      public SRDescriptionAttribute(string description)
         : base(description)
      {
      }

      public override string Description
      {
         get
         {
            if (!replaced)
            {
               replaced = true;
               DescriptionValue = SR.GetString(base.Description);
            }
            return base.Description;
         }
      }
   }

   [AttributeUsage(AttributeTargets.All)]
   internal sealed class SRCategoryAttribute : CategoryAttribute
   {
      public SRCategoryAttribute(string category)
         : base(category)
      {
      }

      protected override string GetLocalizedString(string value)
      {
         return SR.GetString(value);
      }
   }

   internal sealed class SR
   {
      internal const string LuaRuntimes = "LuaRuntimes";
      internal const string LUA_PATH = "LUA_PATH";
      internal const string LUA_CPATH = "LUA_CPATH";
      internal const string GeneralCaption = "GeneralCaption";
      internal const string LuaSettings = "LuaSettings";
      internal const string EntryScript = "EntryScript";

      static SR loader = null;
      ResourceManager resources;

      private static Object s_InternalSyncObject;
      private static Object InternalSyncObject
      {
         get
         {
            if (s_InternalSyncObject == null)
            {
               Object o = new Object();
               Interlocked.CompareExchange(ref s_InternalSyncObject, o, null);
            }
            return s_InternalSyncObject;
         }
      }

      internal SR()
      {
         resources = new System.Resources.ResourceManager("VSPackage", this.GetType().Assembly);
      }

      private static SR GetLoader()
      {
         if (loader == null)
         {
            lock (InternalSyncObject)
            {
               if (loader == null)
               {
                  loader = new SR();
               }
            }
         }

         return loader;
      }

      private static CultureInfo Culture
      {
         get { return null/*use ResourceManager default, CultureInfo.CurrentUICulture*/; }
      }

      public static ResourceManager Resources
      {
         get
         {
            return GetLoader().resources;
         }
      }

      public static string GetString(string name, params object[] args)
      {
         SR sys = GetLoader();
         if (sys == null)
            return null;
         string res = sys.resources.GetString(name, SR.Culture);

         if (args != null && args.Length > 0)
         {
            return String.Format(CultureInfo.CurrentCulture, res, args);
         }
         else
         {
            return res;
         }
      }

      public static string GetString(string name)
      {
         SR sys = GetLoader();
         if (sys == null)
            return null;
         return sys.resources.GetString(name, SR.Culture);
      }

      public static object GetObject(string name)
      {
         SR sys = GetLoader();
         if (sys == null)
            return null;
         return sys.resources.GetObject(name, SR.Culture);
      }
   }

   [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
   internal sealed class LocDisplayNameAttribute : DisplayNameAttribute
   {
      private string name;

      public LocDisplayNameAttribute(string name)
      {
         this.name = name;
      }

      public override string DisplayName
      {
         get
         {
            string result = SR.GetString(this.name);

            if (result == null)
            {
               Debug.Assert(false, "String resource '" + this.name + "' is missing");
               result = this.name;
            }

            return result;
         }
      }
   }

   [ComVisible(true), Guid("BF6D251F-51CC-4a5b-B56B-CB791B7F2724")]
   class LuaPropertyPage : SettingsPage
	{
      private string luaRuntimes;
      private string luaPath;
      private string luaCPath;
      private string entryScript;

      public LuaPropertyPage()
      {
            this.Name = SR.GetString(SR.GeneralCaption);
      }

      public override string GetClassName()
        {
            return this.GetType().FullName;
        }

      protected override void BindProperties()
      {
         if (this.ProjectMgr == null)
         {
            Debug.Assert(false);
            return;
         }

         luaRuntimes = this.ProjectMgr.GetProjectProperty("LuaRuntimes", false);
         luaPath = this.ProjectMgr.GetProjectProperty("LUA_PATH", false);
         luaCPath = this.ProjectMgr.GetProjectProperty("LUA_CPATH", false);
         entryScript = this.ProjectMgr.GetProjectProperty("EntryScript", false);
      }

		protected override int ApplyChanges()
      {
         if (this.ProjectMgr == null)
         {
            Debug.Assert(false);
            return Microsoft.VisualStudio.VSConstants.E_INVALIDARG;
         }

         this.ProjectMgr.SetProjectProperty("LuaRuntimes", luaRuntimes);
         this.ProjectMgr.SetProjectProperty("LUA_PATH", luaPath);
         this.ProjectMgr.SetProjectProperty("LUA_CPATH", luaCPath);
         this.ProjectMgr.SetProjectProperty("EntryScript", entryScript);
         this.IsDirty = false;
         return Microsoft.VisualStudio.VSConstants.S_OK;
		}

      [SRCategoryAttribute(SR.LuaSettings)]
      [LocDisplayName(SR.LuaRuntimes)]
      [SRDescriptionAttribute(SR.LuaRuntimes)]
      public string LuaRuntimes
      {
         get { return luaRuntimes; }
         set { luaRuntimes = value; IsDirty = true; }
      }

      [SRCategoryAttribute(SR.LuaSettings)]
      [LocDisplayName(SR.LUA_PATH)]
      [SRDescriptionAttribute(SR.LUA_PATH)]
      public string LUA_PATH
      {
         get { return this.luaPath; }
         set { this.luaPath = value; IsDirty = true; }
      }

      [SRCategoryAttribute(SR.LuaSettings)]
      [LocDisplayName(SR.LUA_CPATH)]
      [SRDescriptionAttribute(SR.LUA_CPATH)]
      public string LUA_CPATH
      {
         get { return this.luaCPath; }
         set { this.luaCPath = value; IsDirty = true; }
      }

      [SRCategoryAttribute(SR.LuaSettings)]
      [LocDisplayName(SR.EntryScript)]
      [SRDescriptionAttribute(SR.EntryScript)]
      public string EntryScript
      {
         get { return this.entryScript; }
         set { this.entryScript = value; IsDirty = true; }
      }
   }
}
