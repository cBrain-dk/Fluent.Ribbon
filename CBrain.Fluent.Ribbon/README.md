# cBrain Fluent Ribbon controls

This project owns the cBrain control behavior in the fluent-wrapper fork.
`CBrain.Fluent.Ribbon.RibbonComboBox` and `RibbonSplitButton` derive from the Fluent controls.
Their quick-access factories return fresh instances of the corresponding cBrain type; public
creation calls inherited Fluent setup and marks the returned item as being in the quick-access
toolbar. Bindings, private clone registration and event handlers remain in Fluent.

The classes stay inheritable. A further subclass overrides the protected factory to return its
own type when its quick-access item must retain that further subclass's style or behavior.
The future F2 seam consumes these controls; it does not move this feature upstream.

## Native tests

`CBrain.Fluent.Ribbon.Tests` instantiates these production classes and asserts the exact runtime
type returned by `CreateQuickAccessItem()`, a fresh object, quick-access membership and inherited
setup. It uses STA NUnit tests and is included in the solution and Nuke Test target.

From the repository root, run the native Visual Studio MSBuild/VSTest entry point:

```powershell
./Build-CBrainWrapper.ps1 -FluentMode Source
./Build-CBrainWrapper.ps1 -FluentMode Package
```

Each mode builds and runs net462, net6.0-windows and net8.0-windows. Source mode references the
sibling Fluent project. Package mode restores the exact patched `Fluent.Ribbon [11.0.2-cbrain.1]`
from the cBrain F2U feed using the repository's exact package-source mapping.

## Package

The new package ID is `CBrain.Fluent.Ribbon`; it contains the cBrain assembly and depends on the
exact existing Fluent package. `Fluent.Ribbon 11.0.2-cbrain.1` is not replaced or republished.
Pack from committed sources after validation:

```powershell
./Build-CBrainWrapper.ps1 -FluentMode Package -Pack -PackageVersion 11.0.2-cbrain.1
```

The script records the current code SHA in repository/informational metadata and fixes the version
through the verified GitVersion switches. Inspect the artifact and run the native net48 consumer
probe before publication. Publication uses the established private cBrain package route.
Code publication or upstream submissions outside the cBrain work domain require explicit owner
approval for the material and destination. The two earlier upstream PRs were closed by the owner.
