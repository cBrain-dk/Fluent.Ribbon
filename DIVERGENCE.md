# cBrain Fluent wrapper baseline

Verified 2026-10-09 under F2DEV-110306. This record accompanies `fluent-wrapper` in
https://github.com/cBrain-dk/Fluent.Ribbon. The published package and source-mode baseline use the
code commit below; later documentation commits on the branch do not advance that baseline.

## Original Fluent base and package

| Item | Verified value |
| --- | --- |
| Upstream stable release | [v11.0.2](https://github.com/fluentribbon/Fluent.Ribbon/releases/tag/v11.0.2), published 2026-01-19 |
| Upstream commit | `ce56b60ca892d1505b7f80826330c676f41fda0a` |
| Patched code commit | `818659a55cc95ada45f2644c15dcce8df005deaf` |
| Immutable source tag | `v11.0.2-cbrain.1` (also `fluent-wrapper-11.0.2-cbrain.1`) |
| Package | `Fluent.Ribbon` **`11.0.2-cbrain.1`** |
| Internal NuGet v3 feed | `https://git.cbrain.net/api/packages/F2U/nuget/index.json` |
| Package SHA256 | `A8CF3C9B8A672335365D43E5699A951B8F35A751735EA2F337CC2114CB5391AA` |
| Source-mode checkout | `D:/git/fluent-wrapper-11.0.2-cbrain.1`, detached at the patched code commit |

`D:/git/cbrain.Fluent.Ribbon-wrapper` is the working branch checkout, including this record.
The original base probes pin the exact Fluent package version and use the immutable
source checkout above. The later F2 bump must instead pin the native cBrain wrapper package and set `FluentSourcePath` to `D:/git/cbrain-fluent-wrapper-11.0.2-cbrain.1` at `9d17c8376e8c47b40c57e735feb9f7ebd44df4ed`, as recorded below. Verify HEAD and clean sources; do not use a floating branch tip for source
mode. Both `UseFluentSource` settings must resolve the same code commit and dependencies.

The original `D:/git/cbrain.Fluent.Ribbon` remains clean on `cdevelop` at
`c3acf87e46c65d7c0e3543e15de87c9a1d9dfa31`. No historical fork internals were copied wholesale.
No upstream cherry-picks were needed. Future baselines rebase onto stable upstream releases and
receive a new unused package version; never replace this published version.

## Retained patches and retirement

| Patch | Reason and contract | Upstream contribution | Retirement condition |
| --- | --- | --- | --- |
| Keytip service host | The owned CommandBar is an ItemsControl rather than a Ribbon. `Ribbon` implements `IKeyTipServiceHost`; `KeyTipService(IKeyTipServiceHost)` accepts that host, while the existing `KeyTipService(Ribbon)` overload remains. Newer upstream service internals, Ribbon menus/start screen and focus behavior remain. | [PR 1286](https://github.com/fluentribbon/Fluent.Ribbon/pull/1286), closed by the owner, unmerged; historical contribution commit `5594775e6c630645096103870fcf402351f2b010` | A stable release exposes the equivalent host contract and passes the direct-child activation, cancellation, execution, focus and Danish contracts. |
| Quick-access factories | Derived seam controls need their own instances with all inherited clone setup. Allocation alone calls the protected factories; bindings, private field assignments, handlers, size and split-button quick-access prohibition stay in `CreateQuickAccessItem()`. | [PR 1285](https://github.com/fluentribbon/Fluent.Ribbon/pull/1285), closed by the owner, unmerged; historical contribution commit `fbf66af545b43284bf95a40a47c4231ef58d4b47` | A stable release provides equivalent override hooks and both factory tests prove those hooks are used with inherited setup. |

The owner withdrew both upstream contributions. The cBrain derived-control feature and its tests
belong to this wrapper fork. No further upstream submission is authorized; any external source
transfer requires explicit owner approval for the material and destination.

Public host shape:

```csharp
public interface IKeyTipServiceHost
{
    bool IsEnabled { get; }
    bool IsKeyboardFocusWithin { get; }
    bool IsCollapsed { get; }
    bool IsKeyTipHandlingEnabled { get; }
    Control AsControl();
    FrameworkElement GetKeyTipServiceTarget();
    bool CanSetFocusFromKeyTipService(FrameworkElement element);
    void SetFocusFromKeyTipService();
}
```

The factory signatures are protected virtual parameterless instance methods:

```csharp
protected virtual ComboBox CreateQuickAccessComboBox();
protected virtual SplitButton CreateQuickAccessSplitButton();
```

These hooks remain private-fork extension points. Their base implementations return fresh instances.
Derived factories return a fresh non-null
instance of the declared type. The native cBrain wrapper overrides them for `RibbonComboBox` and
`RibbonSplitButton`, calls inherited creation setup, then sets `IsElementInQuickAccessToolBar`. Further F2 seam subclasses override only the factories and inherit that public creation/marking.

## Assets, dependencies and identity

The nupkg contains `lib/net462`, `lib/net6.0-windows7.0` and `lib/net8.0-windows7.0`. A net48 consumer
selects net462 without changing F2's runtime. The restored net48 graph is ControlzEx **7.0.3**,
Microsoft.Xaml.Behaviors.Wpf **1.1.135**, System.Memory **4.6.3**, System.Buffers **4.6.1**,
System.Numerics.Vectors **4.6.1**, System.Runtime.CompilerServices.Unsafe **6.1.2**.
The modern target groups additionally declare System.Text.Json **8.0.5**.
The later bump selects and validates a compatible MahApps version against this graph.

All three Fluent assemblies retain upstream signing and identity:
`Fluent, Version=11.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f`.
File version is `11.0.2.0`; informational version is
`11.0.2-cbrain.1+Sha.818659a55cc95ada45f2644c15dcce8df005deaf`.
The packaged License.txt matches upstream content. The net48 package probe loads generated
Generic.xaml successfully. Build regeneration owns XAML; generated resources were not hand-edited.

## Reproduce build and pack

Upstream `global.json` requests SDK 10.0.100 with feature roll-forward. Validation used SDK
10.0.401 and Visual Studio 18 Enterprise MSBuild, Release/AnyCPU, with the repository's normal
warnings-as-errors policy. Ensure upstream master/develop history and tags are available for
GitVersion; the source checkout is tagged at the exact patched commit.

Run from that clean source checkout (verify `git rev-parse HEAD` against the recorded code SHA):

```powershell
$msbuild = 'C:\Program Files\Microsoft Visual Studio\18\Enterprise\MSBuild\Current\Bin\MSBuild.exe'
& $msbuild Fluent.Ribbon.Tests\Fluent.Ribbon.Tests.csproj /restore /t:Rebuild /p:Configuration=Release /p:Platform=AnyCPU /m /nr:false
& $msbuild Fluent.Ribbon\Fluent.Ribbon.csproj /restore /t:Pack /p:Configuration=Release /p:Platform=AnyCPU /p:UpdateVersionProperties=false /p:UpdateAssemblyInfo=false /p:Version=11.0.2-cbrain.1 /p:PackageVersion=11.0.2-cbrain.1 /p:AssemblyVersion=11.0.0.0 /p:FileVersion=11.0.2.0 /p:InformationalVersion=11.0.2-cbrain.1+Sha.818659a55cc95ada45f2644c15dcce8df005deaf /p:IncludeSourceRevisionInInformationalVersion=false /p:RepositoryBranch=fluent-wrapper /p:RepositoryCommit=818659a55cc95ada45f2644c15dcce8df005deaf /p:RepositoryUrl=https://github.com/cBrain-dk/Fluent.Ribbon.git /p:PackageOutputPath=C:\tmp\fluent\wrapper-baseline\package /m /nr:false
```

GitVersion overwrites Version/PackageVersion alone. The explicit UpdateVersionProperties and
UpdateAssemblyInfo switches above preserve the inspected assembly metadata while fixing the
package version. Upstream's Nuke `.build/Build.cs` Pack supplies GitVersion FullSemVer; do not claim
that a lone project property reproduces this package. Rebuilding can change archive bytes, so inspect
metadata and rerun validation before publishing a *new* version. The hash above identifies the
already validated and published artifact.

## Validation and publication

Tests-first commit `05bfa130` produces the intended missing-interface/factory compile RED before
production changes. The final wrapper builds all three supported targets with zero warnings/errors.
VSTest 18.10/NUnit Adapter 5.2.0 results:

| Run | Result |
| --- | --- |
| Unpatched upstream net462 suite, DPIUNAWARE | 232/233 |
| Patched net462 suite | 241/242 |
| Patched net6.0-windows suite | 241/242 |
| Patched net8.0-windows suite | 241/242 |
| Independent factory contribution, net462 | 234/235 |
| Independent host contribution, net462 | 239/240 |
| net48 ProjectReference, same patched code commit | 9/9 |
| net48 local package, exact version | 9/9 |
| net48 internal-feed package, isolated cache, no local fallback | 9/9 |
| net48 pinned/tagged source checkout | 9/9 |

The single retained upstream failure in every suite is
`TestImmediateDismissIfNoMatchesInRootLayer`: its window cannot activate on the shared desktop
(`Activate()` returns false, native foreground/active window absent). The owner explicitly accepted
this as a known failure when owner and agent use the computer together. The original test is
unchanged and remains in every run; those suite runners exit 1. New host/factory tests all pass.
The new dedicated helper uses ShowActivated=true and retains real activation/focus assertions.

The workstation is 120 DPI; process-local `__COMPAT_LAYER=DPIUNAWARE` removes 20 pre-existing exact
size/layout rounding failures. It is restored after testing; system display settings are unchanged.
The net48 probe exercises Alt, Escape/Alt cancellation, command execution, focus return and
`æ`, `ø`, `å` using the real Danish thread keyboard layout/ToUnicode path. Inputs are routed WPF
key events, not physical SendInput. The prior thread layout is restored.

`rdadm nuget push` published the validated package via the existing encrypted credential mechanism
to the F2U feed after verifying the version was unused. No secret was copied into this repository.
A fresh cache `C:/tmp/fluent/wcache/24fc4209b231` restores the exact version from that feed; its NuGet
metadata records that source, and the restored nupkg hash equals the published hash. The historical
RDAdm route and developer restore configuration establish the internal destination. CI uses normal
NuGet restore without a script source override; no new CI job was triggered and the CI service
account's effective configuration was not independently inspected.

Evidence: `C:/tmp/fluent/wrapper-baseline` contains build/binlogs, TRX results, package inspection,
publication log, `internal-feed-restoration.json`, `Pack-ExactWrapper.ps1`, and the separate
`net48-contract-probe/Run-Probe.ps1` harness. Its modes are `Source`, `PackageLocal`, `PackageInternal`;
the internal mode maps Fluent exclusively to the F2U feed and uses an isolated short cache.
The pinned-source run is `Source-07d137efa0844c67be199ad393003d3d`.

## Consumer continuation

The original Fluent base was delivered; the native cBrain extension awaits publication approval.
WPF tools/seam extraction and the F2 dependency bump have
not started. Product solution builds and repository-root `BuildSolutions.ps1` remain gates after
consumer edits; wrapper validation alone does not validate F2's cutover.

The bump must preserve the three recorded CommandBar deltas: move to `ControlzEx.WindowChrome`
with chrome hit testing intact; replace the four obsolete `Fluent.Ribbon.Values.*` definitions with
explicit child-style sizing (small/middle button heights 40, small button/drop-down widths NaN);
and retain the public host/interface constructor before changing the seam package reference.
DarkBlue stays excluded from theme-specific checks, and WebViewEditorApplication stays excluded
from automated captures. Normal theme-resource compilation continues.

## Native cBrain controls — owner correction, 2026-10-09

`CBrain.Fluent.Ribbon` is a sibling production project inside this fork. Its real public types are
`CBrain.Fluent.Ribbon.RibbonComboBox` and `RibbonSplitButton`. Their protected factory overrides
allocate their corresponding cBrain types; public creation delegates all clone setup to Fluent,
sets `IsElementInQuickAccessToolBar=true` on the returned item, and returns it. They remain
inheritable; further subclasses override the factory when they need their own exact clone type.
No templates, dependency properties or upstream production logic were copied.

`CBrain.Fluent.Ribbon.Tests` is the native test project. Its two STA tests instantiate those actual
production classes, call `CreateQuickAccessItem()`, assert the exact cBrain runtime type and a fresh
object, and verify the QAT marker plus inherited editable/text/checkable/Small/no-re-add setup.
The synthetic-marker factory fixture was removed from `Fluent.Ribbon.Tests`. Both new projects
are in the solution; Nuke Test runs the native project before the upstream project. The CI result
collector recurses through the per-project TRX directories. No external CI job was run.

Tests-first commit `95efb962` proves runtime RED: both plain derived controls returned Fluent base
objects. Native implementation commit **`9d17c8376e8c47b40c57e735feb9f7ebd44df4ed`** passes **2/2**
on net462, net6.0-windows and net8.0-windows, in both source and exact-base-package modes.
A separate net48 consumer reuses that native fixture and passes **2/2** with the actual local nupkg
and **2/2** with the pinned source checkout. Core Fluent production files are unchanged from the
published base commit `818659a55cc95ada45f2644c15dcce8df005deaf`.

The new artifact is **`CBrain.Fluent.Ribbon 11.0.2-cbrain.1`**, containing the cBrain DLL/XML for all
three target groups and depending on exact **`Fluent.Ribbon [11.0.2-cbrain.1]`**. Inherited central
pinning additionally records ControlzEx 7.0.3 and System.Memory 4.6.3 or System.Text.Json 8.0.5.
Its inspected repository SHA is the native implementation commit above; assembly identity is
`CBrain.Fluent.Ribbon, Version=11.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f`.
Local nupkg SHA256: **`F242AB4A42C443A6773DA44E397FE345D7859C2BB251D85AE7C2721E785E2E66`**.
This is a new package ID; the original published Fluent artifact is unchanged.

**Publication pending:** automatic approval review rejected publication of this new package to
`https://git.cbrain.net/api/packages/F2U/nuget/index.json`, requiring explicit owner approval for
package publication under the transfer rule. No upload occurred. The version was verified unused.
The package remains at `C:/tmp/fluent/wrapper-owned-controls/package/` for that final approval gate.
No GitHub push or new upstream PR was made. Both implementation commits and the native source tag
remain local.

The verified native source checkout is `D:/git/cbrain-fluent-wrapper-11.0.2-cbrain.1`, at the native
implementation SHA, tagged locally `cbrain-fluent-wrapper-11.0.2-cbrain.1`. The later F2 seam must
reference this cBrain project/package, with Fluent retained transitively. Its controls may derive
further and override the protected factories to preserve their own seam types. Package mode uses
the existing published Fluent base; source mode uses the identical Fluent production source tree
included at this native wrapper SHA. Publication/source pins must be closed before the bump.

Native build/test/pack entry point: `Build-CBrainWrapper.ps1`. `-FluentMode Source` and
`-FluentMode Package` run the native tests on all three targets using VS MSBuild/VSTest.
`-FluentMode Package -Pack` produces the cBrain artifact with the inspected version/commit metadata;
Nuke Pack still produces the base Fluent artifact. Evidence is in
`C:/tmp/fluent/wrapper-owned-controls`, including RED/green TRX, package inspection and
`net48-consumer/Run-Net48.ps1`. Nuke integration compilation succeeded with pre-existing dependency
security-audit warnings. No F2 consumer code or dependency cutover was performed.

After moving the two cBrain cases out of the upstream fixture, the complete original suites were
rebuilt and rerun: **239/240 executed cases pass** on each of net462, net6.0-windows and
net8.0-windows. The same unchanged owner-accepted `TestImmediateDismissIfNoMatchesInRootLayer`
is the sole failure on every target; runner exits are 1. Existing explicit tests remain unselected
by the normal runner. Native exact-type tests stay in their own project and pass independently.
TRX evidence: `C:/tmp/fluent/wrapper-owned-controls/upstream-results/`.
