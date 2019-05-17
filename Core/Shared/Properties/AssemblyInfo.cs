using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("SAC.Stock.Shared")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("SAC")]
[assembly: AssemblyProduct("SAC.Stock.Shared")]
[assembly: AssemblyCopyright("Copyright ©  2017")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("adad7401-50eb-426b-808e-f33660ea3b51")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: InternalsVisibleTo("SAC.Stock.Core.Domain")]
[assembly: InternalsVisibleTo("SAC.Stock.Core.Domain.Query")]
[assembly: InternalsVisibleTo("SAC.Stock.Core.Application")]
[assembly: InternalsVisibleTo("SAC.Stock.Core.Data")]
[assembly: InternalsVisibleTo("SAC.Stock.Front.MigrateDatabase")]
[assembly: InternalsVisibleTo("SAC.Stock.Front.InitializeDatabase")]
[assembly: InternalsVisibleTo("SAC.Stock.Front.WebSiteStock")]
[assembly: InternalsVisibleTo("SAC.Stock.Front.RolesCompositionLoader")]
[assembly: InternalsVisibleTo("SAC.Stock.Test.TestServices")]