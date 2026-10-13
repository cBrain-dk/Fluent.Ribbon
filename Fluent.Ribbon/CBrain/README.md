# cBrain Fluent Ribbon controls

`Fluent.Ribbon/Fluent.Ribbon.csproj` builds one production assembly, `Fluent.dll`, and one
package, `Fluent.Ribbon`. It includes `Fluent.CBrain.RibbonComboBox` and
`Fluent.CBrain.RibbonSplitButton` alongside their Fluent base controls. The cBrain
namespace is `Fluent.CBrain`; there is no separate cBrain production assembly or package.

Their quick-access factories return fresh instances of the corresponding cBrain type. Public
creation calls inherited Fluent setup and marks the returned item as being in the quick-access
toolbar. Bindings, private clone registration and event handlers remain in Fluent.

The classes stay inheritable. A further subclass overrides the protected factory to return its
own type when its quick-access item must retain that subclass's style or behavior. The future
F2 seam consumes these controls; this feature remains in the cBrain fork.

## Native tests

`CBrain.Fluent.Ribbon.Tests` stays a separate, non-packable project. Its STA NUnit tests
instantiate the real production controls and assert the exact runtime type returned by
`CreateQuickAccessItem()`, a fresh object, quick-access membership and inherited setup. The
runtime contract also requires each cBrain control and its Fluent base control to come from
the same `Fluent` assembly. The tests are included in the solution and Nuke Test target.

From the repository root, use the serial Visual Studio MSBuild/VSTest entry point:

```powershell
./Build-CBrainWrapper.ps1 -FluentMode Source
./Build-CBrainWrapper.ps1 -FluentMode Package
```

Each mode builds and runs net48 and net10.0-windows. Source mode is the default
and references `Fluent.Ribbon/Fluent.Ribbon.csproj`. Package mode restores exactly
`Fluent.Ribbon [11.0.2-0.3]` from the cBrain F2U feed using the repository's package-source
mapping. Package mode becomes available after the approved package publication.

## Package

The version format is `<Fluent version>-0.<cBrain revision>`, matching `cdevelop`'s `7.0.0-0.80`. The suffix is a NuGet prerelease label. Version properties live in `Fluent.Ribbon/Fluent.Ribbon.csproj`: `Version` is
`11.0.2-0.3`, `PackageVersion` follows `Version`, `AssemblyVersion` is `11.0.0.0`,
`FileVersion` is `11.0.2.0`, and `InformationalVersion` follows `Version`. The SDK may append
the actual source revision to informational metadata. GitVersion version and assembly-info
overrides are disabled for this project. The already published `11.0.2-0.2` and `11.0.2-cbrain.1` must not be
replaced or republished.

Local packing is authorized. Use the normal project workflow:

```powershell
dotnet pack ./Fluent.Ribbon/Fluent.Ribbon.csproj -c Release -o ./artifacts
```

Inspect the package and run native and host/clone consumer tests on net48 and net10.0-windows before requesting publication.
RDAdm is the established internal publication route and requires explicit owner approval for
publication. No code or package may be transferred outside the cBrain work domain without
explicit owner approval for the material and destination. The two earlier upstream PRs were
closed by the owner.
