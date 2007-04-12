// VsPkg.cs : Implementation of LuaLangPack
//

using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using Microsoft.Win32;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;
using EnvDTE;
using IOleServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

[assembly:ComVisible(true)]

namespace Vsip.LuaLangPack
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the 
    /// IVsPackage interface and uses the registration attributes defined in the framework to 
    /// register itself and its components with the shell.
    ///    // In order be loaded inside Visual Studio in a machine that has not the VS SDK installed, 
    // package needs to have a valid load key (it can be requested at 
    // http://msdn.microsoft.com/vstudio/extend/). This attributes tells the shell that this 
    // package has a load key embedded in its resources.
    // A Visual Studio component can be registered under different regitry roots; for instance
    // when you debug your package you want to register it in the experimental hive. This
    // attribute specifies the registry root to use if no one is provided to regpkg.exe with
    // the /root switch.
    /// </summary>

    [PackageRegistration(UseManagedResourcesOnly = true)]
    [DefaultRegistryRoot("Software\\Microsoft\\VisualStudio\\8.0Exp")]
    [InstalledProductRegistration(true, "Lua Language Integration Pack", "Provides Lua 5.0 language integration with Visual Studio", "0.3", IconResourceID = 400)]
    [ProvideLoadKey("Standard", "0.3", "Lua Language Integration Pack", "trystan.org", 110)]
    [ProvideProjectFactory(typeof(LuaProjectFactory), "Lua Project", "LuaProject Project Files (*.luaproj);*.luaproj", "luaproj", "luaproj", "..\\Templates\\Projects")]
    [ProvideProjectItem(typeof(LuaProjectFactory), "Lua Items", "..\\Templates\\ProjectItems", 500)]
    [ProvideServiceAttribute(typeof(LuaLangService), ServiceName = "VS 2005 Lua Language Service")]
    [ProvideLanguageServiceAttribute(typeof(LuaLangService), "VS 2005 Lua Language Service", 103, CodeSense = true, AutoOutlining = true, EnableAsyncCompletion = true, EnableCommenting = true)]
    [ProvideLanguageExtensionAttribute(typeof(LuaLangService),".lua")]
    [ProvideObject(typeof(LuaPropertyPage))]

    [Guid("b4d22fe4-9555-4288-a8f9-70ed3119eaad")]
    public sealed class LuaLangPack : Package, IOleComponent
    {
        private uint m_componentID;

        /// <summary>
        /// Default constructor of the package.
        /// Inside this method you can place any initialization code that does not require 
        /// any Visual Studio service because at this point the package object is created but 
        /// not sited yet inside Visual Studio environment. The place to do all the other 
        /// initialization is the Initialize method.
        /// </summary>
        public LuaLangPack()
        {
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this.ToString()));
        }
        
        protected override void Initialize()
        {
            base.Initialize();  // required
            this.RegisterProjectFactory(new LuaProjectFactory(this));

            // Proffer the service.
            IServiceContainer serviceContainer = this as IServiceContainer;
            LuaLangService langService = new LuaLangService();
            langService.SetSite(this);
            serviceContainer.AddService(typeof(LuaLangService),
                                        langService,
                                        true);

            // Register a timer to call our language service during
            // idle periods.
            IOleComponentManager mgr = GetService(typeof(SOleComponentManager))
                                       as IOleComponentManager;
            if (m_componentID == 0 && mgr != null)
            {
                OLECRINFO[] crinfo = new OLECRINFO[1];
                crinfo[0].cbSize = (uint)Marshal.SizeOf(typeof(OLECRINFO));
                crinfo[0].grfcrf = (uint)_OLECRF.olecrfNeedIdleTime |
                                              (uint)_OLECRF.olecrfNeedPeriodicIdleTime;
                crinfo[0].grfcadvf = (uint)_OLECADVF.olecadvfModal |
                                              (uint)_OLECADVF.olecadvfRedrawOff |
                                              (uint)_OLECADVF.olecadvfWarningsOff;
                crinfo[0].uIdleTimeInterval = 1000;
                int hr = mgr.FRegisterComponent(this, crinfo, out m_componentID);
            }
 
           Trace.WriteLine( "LuaLangPack: Finished Initialization" );
        }

        protected override void Dispose(bool disposing)
        {
            if (m_componentID != 0)
            {
                IOleComponentManager mgr = GetService(typeof(SOleComponentManager))
                                           as IOleComponentManager;
                if (mgr != null)
                {
                    int hr = mgr.FRevokeComponent(m_componentID);
                }
                m_componentID = 0;
            }

            base.Dispose(disposing);
         }

        #region IOleComponent Members

        public int FDoIdle(uint grfidlef)
        {
            bool bPeriodic = (grfidlef & (uint)_OLEIDLEF.oleidlefPeriodic) != 0;
            // Use typeof(MyLanguageService) because we need to
            // reference the GUID for our language service.
            LanguageService service = GetService(typeof(LuaLangService))
                                      as LanguageService;
            if (service != null)
            {
                service.OnIdle(bPeriodic);
            }
            return 0;
        }

        public int FContinueMessageLoop(uint uReason,
                                        IntPtr pvLoopData,
                                        MSG[] pMsgPeeked)
        {
            return 1;
        }

        public int FPreTranslateMessage(MSG[] pMsg)
        {
            return 0;
        }

        public int FQueryTerminate(int fPromptUser)
        {
            return 1;
        }

        public int FReserved1(uint dwReserved,
                              uint message,
                              IntPtr wParam,
                              IntPtr lParam)
        {
            return 1;
        }

        public IntPtr HwndGetWindow(uint dwWhich, uint dwReserved)
        {
            return IntPtr.Zero;
        }

        public void OnActivationChange(IOleComponent pic,
                                       int fSameComponent,
                                       OLECRINFO[] pcrinfo,
                                       int fHostIsActivating,
                                       OLECHOSTINFO[] pchostinfo,
                                       uint dwReserved)
        {
        }

        public void OnAppActivate(int fActive, uint dwOtherThreadID)
        {
        }

        public void OnEnterState(uint uStateID, int fEnter)
        {
        }

        public void OnLoseActivation()
        {
        }

        public void Terminate()
        {
        }

        #endregion


        /// <summary>
        /// Helper function that will load a resource string using the standard Visual Studio Resource Manager
        /// Service (SVsResourceManager). Because of the fact that it is using a service, this method can be
        /// called only after the package is sited.
        /// </summary>
        internal static string GetResourceString(string resourceName)
        {
            string resourceValue;
            IVsResourceManager resourceManager = (IVsResourceManager)GetGlobalService(typeof(SVsResourceManager));
            if (resourceManager == null)
            {
                throw new InvalidOperationException("Could not get SVsResourceManager service. Make sure the package is Sited before calling this method.");
            }
            Guid packageGuid = typeof(LuaLangPack).GUID;
            int hr = resourceManager.LoadResourceString(ref packageGuid, -1, resourceName, out resourceValue);
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(hr);
            return resourceValue;
        }
        internal static string GetResourceString(int resourceID)
        {
            return GetResourceString(string.Format("@{0}", resourceID));
        }

    }

    [GuidAttribute("74F2664D-668E-4058-AF23-3C959863AEE8")]
    public class LuaProjectFactory : Microsoft.VisualStudio.Package.ProjectFactory
    {
       public LuaProjectFactory(LuaLangPack package) : base(package)
       {
          Trace.WriteLine("LuaLangPack: Project Factory Construction");
       }

       protected override Microsoft.VisualStudio.Package.ProjectNode CreateProject()
       {
          LuaPrj project = new LuaPrj(this.Package as LuaLangPack);
          project.SetSite((IOleServiceProvider)((System.IServiceProvider)this.Package).GetService(typeof(IOleServiceProvider)));
          Trace.WriteLine("LuaLangPack: Create Project");
          return project;
       }
    }

    [Guid("B416BD01-3431-4262-87D4-977196E5E832")]
    public class LuaPrj : Microsoft.VisualStudio.Package.ProjectNode
    {
        private LuaLangPack package;

        public LuaPrj(LuaLangPack pkg)
        {
            Trace.WriteLine("LuaLangPack: Project Node Construction");
            this.package = pkg;
        }
       
        public override Guid ProjectGuid
        {
            get
            {
                return typeof(LuaProjectFactory).GUID;
            }
        }
       
        public override string ProjectType
        {
            get
            {
                return "LuaPrj";
            }
        }

        public override int ImageIndex
        {
            get { return NoImage; }
        }

        public override object GetIconHandle(bool open)
        {
            return typeof(ProjectNode).Assembly.GetManifestResourceStream("Resources.Lua Project.ico");   
        }

        protected override Guid[]  GetConfigurationIndependentPropertyPages()
        {
            Trace.WriteLine("LuaLangPack: Prop Page Guids");
            Guid[] result = new Guid[1];
            result[0] = typeof(LuaPropertyPage).GUID;
            return result;
        }

       /// <summary>
       /// Returns the configuration dependent property pages.
       /// Specify here a property page. By returning no property page the configuartion dependent properties will be neglected.
       /// Overriding, but current implementation does nothing
       /// To provide configuration specific page project property page, this should return an array bigger then 0
       /// (you can make it do the same as GetPropertyPageGuids() to see its impact)
       /// </summary>
       /// <param name="config"></param>
       /// <returns></returns>
       protected override Guid[] GetConfigurationDependentPropertyPages()
       {
          Trace.WriteLine("LuaLangPack: Conf Dependent Prop Page Guids");
          Guid[] result = new Guid[0];
          return result;
       }
    }
}