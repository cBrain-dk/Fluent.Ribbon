# cBrain Fluent Ribbon controls

`Fluent.Ribbon/Fluent.Ribbon.csproj` builds the `Fluent.Ribbon` package and
`Fluent.dll`, including `Fluent.CBrain.RibbonComboBox` and
`Fluent.CBrain.RibbonSplitButton` alongside their Fluent base controls.
The namespace keeps `Fluent.*` references usable inside `CBrain.*` consumers.

Their quick-access factories return fresh instances of the corresponding native
type. Public creation calls inherited Fluent setup and marks the returned item as
being in the quick-access toolbar. Fluent retains the bindings, clone registration
and event handlers. Further subclasses override the protected factory when their
quick-access item must retain the subclass's style or behavior.

## Consumer migration

For `11.0.2-0.3`, replace native-control references to `CBrain.Fluent.Ribbon` with
`Fluent.CBrain`, including XAML declarations:

```xml
xmlns:native="clr-namespace:Fluent.CBrain;assembly=Fluent"
```

Recompile consumers and update dynamic XAML or persisted type names that refer
to these controls. Remove `global::Fluent` prefixes only where they are redundant;
retain type qualifications and aliases needed to disambiguate WPF controls.

F2's package pin belongs in
`src/CBrain.WPF/CBrain.WPF.Ribbons/CBrain.WPF.Ribbons.csproj`. Update it to
`[11.0.2-0.3]` after publication. For `UseFluentSource=true`, set `FluentSourcePath`
to a clean checkout of the corresponding source revision containing
`Fluent.Ribbon/Fluent.Ribbon.csproj`.

## Native tests

`CBrain.Fluent.Ribbon.Tests` is a separate, non-packable STA NUnit project.
It covers native clone type, freshness, quick-access membership, inherited setup
and shared Fluent assembly identity. Consumer probes cover ordinary `Fluent.*`
references inside `CBrain.*` and XAML creation through the native CLR namespace.
The tests are included in the solution and Nuke Test target.

From the repository root:

```powershell
./Build-CBrainWrapper.ps1 -FluentMode Source
./Build-CBrainWrapper.ps1 -FluentMode Package
```

Each mode builds and runs net48 and net10.0-windows. Source mode references the
Fluent project. Package mode restores `[11.0.2-0.3]` from the internal F2U feed
and is available after publication.

## Package

Version properties live in `Fluent.Ribbon/Fluent.Ribbon.csproj`. The current
candidate is `11.0.2-0.3`, following `<Fluent version>-0.<cBrain revision>`.
Published versions, including `11.0.2-0.2`, must never be replaced.

To pack locally:

```powershell
dotnet pack ./Fluent.Ribbon/Fluent.Ribbon.csproj -c Release -o ./artifacts
```

Validate both targets in source and package modes before publication.
`Publish-CBrainWrapper.ps1` publishes through the internal RDAdm route and
requires explicit owner authorization.
