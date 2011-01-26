#if !NOT_CLSCOMPLIANT
using System;
#endif
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

[assembly: AssemblyProduct("NHaml")]
[assembly: AssemblyCopyright("MIT License")]
[assembly: ComVisible(false)]
[assembly: AssemblyVersion("1.6.0.0")]
[assembly: AssemblyFileVersion("1.6.0.0")]
[assembly: NeutralResourcesLanguage("")]
[assembly: AllowPartiallyTrustedCallers]
[assembly: SecurityRules(SecurityRuleSet.Level1)]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, Execution = true)]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
//[assembly: SecurityCritical]
#if !NOT_CLSCOMPLIANT
[assembly: CLSCompliant(true)]
#endif