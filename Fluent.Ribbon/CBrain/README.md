# cBrain Fluent Ribbon controls

`Fluent.Ribbon/Fluent.Ribbon.csproj` builds one production assembly, `Fluent.dll`, and one
package, `Fluent.Ribbon`. It includes `Fluent.CBrain.RibbonComboBox` and
`Fluent.CBrain.RibbonSplitButton` alongside their Fluent base controls. The cBrain
namespace is `Fluent.CBrain`, so `Fluent.*` resolves normally inside `CBrain.*` consumers.
There is no separate cBrain production assembly or package.

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
the same `Fluent` assembly. Consumer probes in `CBrain.Consumer.Probes` compile ordinary
`Fluent.*` references without `global::` and create both controls through
`clr-namespace:Fluent.CBrain;assembly=Fluent`. The tests are included in the solution and Nuke Test target.

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

## F2 dependency and reference updates

After approved publication, change the sole Fluent package pin in
`src/CBrain.WPF/CBrain.WPF.Ribbons/CBrain.WPF.Ribbons.csproj` from
`[11.0.2-0.2]` to `[11.0.2-0.3]`. For `UseFluentSource=true`, pass
`FluentSourcePath` pointing to a clean checkout of the corresponding committed source,
with `Fluent.Ribbon/Fluent.Ribbon.csproj` beneath it. The old `.2` checkout cannot
supply the renamed types. Application projects continue receiving Fluent through the seam;
the legacy EsdhBrowser compile-reference target needs no namespace-specific change.

Consumers of either native control must replace `CBrain.Fluent.Ribbon` imports/type names
with `Fluent.CBrain`, and update any XAML declaration to
`clr-namespace:Fluent.CBrain;assembly=Fluent`. This is a breaking CLR type-name change:
recompile consumers and update dynamic XAML or persisted assembly-qualified names that
name these controls. No compatibility alias is added because it would recreate the shadow.

The checked `D:/projects/f2-main-fluent` source tree has no direct references to the old
native namespace. Its owned `RibbonSplitButton` currently derives from `Fluent.SplitButton`;
this namespace change does not require a base-control migration. Once the dependency is
updated, remove only the `global::` prefix from `global::Fluent.*` where compilation confirms
it is redundant. Keep `Fluent.` qualification and existing aliases when they disambiguate WPF
controls. The measured list of 33 affected C# files is retained in the namespace-change evidence.
Update the current dependency baseline in `docs/architecture/ribbon-seam.md` and
`fluent-wrapper-triage.md`, including package hash and committed source revision; preserve
historical `.2` validation records. Validate the F2 seam, WPF tests and Cabinet in both dependency
modes and rerun the repository build gate. Product QAT/restart QA remains a separate owner gate.

Local candidate validation and artifact provenance are recorded in
[the divergence record](../../DIVERGENCE.md#namespace-rename-candidate--2026-10-13).
