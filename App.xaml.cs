using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using PresentationODST.ManagedBlam;

namespace PresentationODST
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            PresentationODST.Properties.Settings.Default.OpenTags = new System.Collections.Specialized.StringCollection();
            if (!Bungie.ManagedBlamSystem.IsInitialized) return;
            if (Tags.OpenTags == null) return;
            foreach (Bungie.Tags.TagFile tagFile in Tags.OpenTags)
            {
                if (tagFile == null) continue;
                if (tagFile.Path == null) continue;
                PresentationODST.Properties.Settings.Default.OpenTags.Add(tagFile.Path.RelativePathWithExtension);
            }
            PresentationODST.Properties.Settings.Default.Save();
        }

		/// <summary>
		/// Load assemblies from the bin folder in the working directory
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private static Assembly LoadFromBinFolder(object sender, ResolveEventArgs args)
		{
			AssemblyName assemblyName = new AssemblyName(args.Name);
			Trace.WriteLine($"Loading assembly: {assemblyName}");

			string currentPath = Directory.GetCurrentDirectory();
			string assemblyPath = Path.Join(currentPath, "bin", assemblyName.Name) + ".dll";

			if (File.Exists(assemblyPath))
			{
				Assembly assembly = Assembly.LoadFrom(assemblyPath);
				return assembly;
			}
			else
			{
				return null;
			}
		}

		[STAThread]
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		public static void Main()
		{
			// redirect loading to the bin directory
			AppDomain currentDomain = AppDomain.CurrentDomain;
			currentDomain.AssemblyResolve += new ResolveEventHandler(LoadFromBinFolder);

			// copied from default implementation 
			SplashScreen splashScreen = new SplashScreen("images/chiefsplash.png");
			splashScreen.Show(true);

			var app = new App();
			app.InitializeComponent();
			app.Run();
		}
	}
}
