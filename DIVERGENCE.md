# cBrain Fluent wrapper baseline

Current delivery: a single `Fluent.Ribbon` package and `Fluent.dll`, including the private cBrain
control additions. Verified 2026-10-10 under F2DEV-110306. The owner authorized source push to the cBrain GitHub repository; code and native tests live in the
`fluent-wrapper` branch at https://github.com/cBrain-dk/Fluent.Ribbon. Packing and package publication are on hold; no upstream PR is authorized.

## Current baseline

| Item | Value |
| --- | --- |
| Upstream stable release | v11.0.2, published 2026-01-19 |
| Upstream SHA | `ce56b60ca892d1505b7f80826330c676f41fda0a` |
| Current source SHA | `840986f34c7e07da0fa5f5e69665c38aa32385d1` |
| Planned package | `Fluent.Ribbon 11.0.2-0.2` |
| Local package SHA256 | Not packed for this version |
| Intended private feed | `https://git.cbrain.net/api/packages/F2U/nuget/index.json` |
| Packing / publication | On hold at the owner's request; this version is neither packed nor uploaded |
| Matching source checkout | `D:/git/fluent-wrapper-11.0.2-0.2`, at the current source SHA |
| Release tag | Not created while packing/publication are on hold |
| Assembly identity | `Fluent, Version=11.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f` |

The later F2 bump must pin this exact package and set `FluentSourcePath` to the matching checkout,
referencing `Fluent.Ribbon/Fluent.Ribbon.csproj`. Verify HEAD and clean source. Documentation-only
commits on the working branch do not advance the source/package baseline. The published `.1`
package remains unchanged. The unshipped separate `CBrain.Fluent.Ribbon .1` package is superseded.

## Private fork features

`Fluent.IKeyTipServiceHost` retains the old cBrain fork's public contract: enabled/focus/collapsed/
keytip state, `AsControl()`, `GetKeyTipServiceTarget()`, `CanSetFocusFromKeyTipService()` and
`SetFocusFromKeyTipService()`. `Ribbon` implements it; the new interface-based service constructor
coexists with the original `KeyTipService(Ribbon)` overload. The newer service and Ribbon target/
focus behavior are retained rather than replacing them with the old service implementation.
This supports the owned ItemsControl-based CommandBar.

The protected parameterless factories `ComboBox.CreateQuickAccessComboBox()` and
`SplitButton.CreateQuickAccessSplitButton()` replace only allocation. Fluent's private clone
registration, bindings, item transfer, event handlers and small-size/no-re-add setup remain in
its inherited creation methods.

The real public `CBrain.Fluent.Ribbon.RibbonComboBox` and `RibbonSplitButton` now live in
`Fluent.Ribbon/CBrain/` and compile into `Fluent.dll`. Their factory overrides return fresh
instances of the matching cBrain type. Public creation calls inherited Fluent setup, sets
`IsElementInQuickAccessToolBar=true` and returns the clone. Further F2 seam subclasses inherit
that setup/marking and override the factory if their exact further-derived type must be retained.
No separate cBrain production assembly/package remains; cBrain namespace is preserved.

`CBrain.Fluent.Ribbon.Tests` stays a native, non-packable STA test project. Its two cases use the
actual production classes and assert exact clone type, fresh objects, QAT marking, inherited setup
and assembly identity shared with the Fluent base controls. No synthetic derived marker types,
private reflection or timer sleeps. Source mode references Fluent; package mode pins exact `11.0.2-0.2` and awaits the future package.
The solution and Nuke test target include it; TRX collection recurses through project directories.

The owner closed the earlier upstream [factory PR1285](https://github.com/fluentribbon/Fluent.Ribbon/pull/1285)
and [host PR1286](https://github.com/fluentribbon/Fluent.Ribbon/pull/1286), unmerged. These are historical
contributions, not active delivery gates. Only source push to the owner-authorized https://github.com/cBrain-dk/Fluent.Ribbon repository is permitted; upstream PRs and package publication remain unauthorized. Rebase future
baselines on stable upstream releases; retire private host/factory patches only if a released
equivalent passes the native clone and CommandBar/keytip contracts. The cBrain control feature stays
owned by the fork.

## Normal package workflow

The format is `<Fluent version>-0.<cBrain revision>`, matching remote `cdevelop`'s `7.0.0-0.80` at `ff5dbc768f12931d4f5b85aafcfefdbacc4cacf5`. The candidate retains cBrain revision 2 as `11.0.2-0.2`. Packing and publication are on hold; the command below documents the route for when the owner resumes that work.

Version properties are maintained in `Fluent.Ribbon/Fluent.Ribbon.csproj`:
`Version=11.0.2-0.2`, `PackageVersion=$(Version)`, `AssemblyVersion=11.0.0.0`,
`FileVersion=11.0.2.0`, `InformationalVersion=$(Version)`. Project properties disable GitVersion's
version/assembly-info overrides; Nuke Compile/Pack also no longer supply overriding version values.
The SDK records the source revision. No command-line version workaround is needed. Nuke's legacy ZIP filenames still use GitVersion labels; the normal NuGet package workflow does not use those ZIPs.

From the clean matching source checkout:

```powershell
dotnet pack ./Fluent.Ribbon/Fluent.Ribbon.csproj -c Release -o C:/tmp/fluent/wrapper-single-package/package
```

After inspection, consumer validation and explicit owner approval for this specific package/feed,
use the normal internal `rdadm nuget push` route. Check the chosen version is still unused before
publication, never replace an existing version, and validate an isolated internal-feed restore
with no local fallback. Preserve credentials in the established encrypted RDAdm mechanism.
`Build-CBrainWrapper.ps1` builds/tests only; it no longer packs. Rebuilding can change package
bytes; the hash in the historical validation section identifies only the prior `11.0.2-cbrain.2` artifact.

Build/package targets are exactly net48 and net10.0-windows. A net48 consumer selects net48; resolved
legacy dependencies remain ControlzEx7.0.3, Microsoft.Xaml.Behaviors.Wpf1.1.135, Memory4.6.3,
Buffers4.6.1, Vectors4.6.1 and Unsafe6.1.2. Modern groups include System.Text.Json8.0.5.
The original upstream license and signing are preserved. Generated XAML is built normally.

## Current source validation — 2026-10-10

Source `840986f34c7e07da0fa5f5e69665c38aa32385d1` declares exactly `net48;net10.0-windows`
for the Fluent library, native tests and upstream test projects. The serial test helper uses
those targets. net48 retains Framework-specific references, binding redirects and dependencies;
the modern-only explicit palette generator fixture is excluded through `NETFRAMEWORK`.
The version stays `11.0.2-0.2`; this candidate is not packed or published.

The checked feature and main EsdhBrowser projects both declare only `TargetFrameworkVersion=v4.8`
(`D:/projects/f2-main-fluent/client/EsdhBrowser/EsdhBrowser.csproj:46` and the equivalent
`D:/projects/f2-main/` path). Their evaluated/generated assets confirm net48. No net10 EsdhBrowser
project was found in the checked local workspaces. The owner-requested .NET 10 wrapper target is
validated directly, independently of that application finding.

- Tests-first build before the target edit fails with NETSDK1005 for missing net48 assets.
- VS18 MSBuild restore/Rebuild, Release/AnyCPU, `/m:1 /nr:false`, succeeds for native tests,
  upstream tests and the source consumer probe on both targets, with warnings-as-errors in the fork.
- Native clone tests pass **2/2 per target** through `Build-CBrainWrapper.ps1 -FluentMode Source`.
- Source consumer tests pass **9/9 per target**, including exact cBrain clone type, shared Fluent
  assembly, host/focus behavior and Danish æ/ø/å through the real keyboard-layout path.
- Full upstream suites pass **246/247 executed tests per target**. The sole unchanged failure is
  `TestImmediateDismissIfNoMatchesInRootLayer`, accepted by the owner for the shared desktop.
  It remains included; VSTest exits 1. Existing explicit tests remain unselected.
- VSTest runs each assembly sequentially with `/Logger:trx` and no test filter. Upstream/consumer
  runs use process-local `__COMPAT_LAYER=DPIUNAWARE`. No display settings are changed.

Evidence: `C:/tmp/fluent/wrapper-supported-targets/` contains `net48-red.log`, native/upstream/
consumer build logs and target-specific TRX/logs. Independent review is recorded at
`C:/tmp/F2DEV-110306/two-target-review.md`. No pack target, package-mode test, NuGet upload or
external CI job is run. Only the reviewed source branch is pushed to the authorized cBrain fork.

## Prior version-format source validation — 2026-10-10

Version-format commit `eedff87ed818da9dc3c64da153f5825a7720d752` changes only the Fluent project
version, native package-test pin and control README. Real control/host behavior is unchanged.
Native source tests pass **2/2** on net462, net6.0-windows and net8.0-windows, with VS18
MSBuild/VSTest, Release/AnyCPU and normal warnings-as-errors. The pinned source net48 probe passes
**9/9**, including real derived clones, shared Fluent assembly identity and Danish keytips.
Evidence: `C:/tmp/fluent/wrapper-version-format/`, `wrapper-version-format-source.log` and
`wrapper-version-format-net48-source.log` under `C:/tmp/fluent`.
No packing, package-mode test or package publication was run for `11.0.2-0.2`.

The evidence below belongs to the earlier local-only `11.0.2-cbrain.2` package from source
`751837ee0888b191a796c642bdc925b66762f910`, SHA256
`0A7F675BE084008F60B347ADD32AD019E603264E2CE84EF6D9D5CF02FD6108F7`; it does not identify the
future package using the legacy version format. It was never published.
## Prior combined-package validation — 2026-10-09

Tests-first commit `f2cf40da` proves runtime RED: both native clone tests failed because their
assembly was the separate cBrain DLL. The combined implementation passes both cases in the Fluent
assembly; exact type, cloning setup and QAT behavior are unchanged.

- Native tests: **2/2** on net462, net6.0-windows and net8.0-windows, both source mode and actual
  local package mode. Fresh local-cache metadata and package hash match the inspected artifact.
- Separate net48 probes: **9/9** from the pinned source checkout and **9/9** from the actual local
  package; the local probe restores with an isolated cache and exact source mapping.
- Complete upstream suites: **246/247 executed cases pass** on every target. Consolidation adds
  seven generic logical-tree cases for the public cBrain controls; all seven pass. The only failure
  remains unchanged `TestImmediateDismissIfNoMatchesInRootLayer`, accepted by the owner for the
  shared desktop. It remains included and runner exits are 1. Existing explicit tests remain
  unselected by the normal runner; process-local DPIUNAWARE avoids baseline DPI sizing failures.
- Normal direct `dotnet pack` succeeds for all three targets, with project-owned version `11.0.2-cbrain.2` and
  repository SHA `751837ee0888b191a796c642bdc925b66762f910`; no version command-line overrides.
  The package has only Fluent DLL/XML target assets, original license, expected dependencies and
  signing. Source/package tests prove the cBrain classes are inside that Fluent assembly.
- Nuke integration compilation succeeds, retaining its pre-existing dependency security-audit
  warnings. No external CI job was triggered.
- This prior artifact was not published; it is superseded by the unpacked `11.0.2-0.2` candidate.

Tooling: SDK10.0.401, VS18 Enterprise MSBuild/VSTest, Release/AnyCPU, normal warnings-as-errors.
Builds/tests are serialized. The owner-requested direct `dotnet pack` workflow is exercised for
this package. The build/test helper keeps native targets sequential.

Evidence: `C:/tmp/fluent/wrapper-single-package/`, including RED and native green TRX, normal-pack
log/package inspection, complete upstream results and isolated net48 consumer runs.
The net48 probe combines the native two clone cases with the existing seven host/keytip cases,
including Danish æ/ø/å. Input is through public routed WPF key events and the real Danish thread
layout/ToUnicode path; this is not a physical keyboard automation claim.

## Original base history and continuation

The original `Fluent.Ribbon 11.0.2-cbrain.1` was published from
`818659a55cc95ada45f2644c15dcce8df005deaf`, SHA256
`A8CF3C9B8A672335365D43E5699A951B8F35A751735EA2F337CC2114CB5391AA`.
Its source tag/checkout is `v11.0.2-cbrain.1` / `D:/git/fluent-wrapper-11.0.2-cbrain.1`.
Original source/local/internal-feed net48 probes passed9/9. Full suites then passed241/242 each
with the unchanged owner-accepted desktop failure. Historical evidence lives in
`C:/tmp/fluent/wrapper-baseline`; Git record `f3660847` preserves the earlier delivery details.
The original cdevelop checkout stays untouched at
`c3acf87e46c65d7c0e3543e15de87c9a1d9dfa31`.

The wrapper remains active pending publication and isolated feed validation. WPF tools, seam
extraction and F2 dependency changes have not started. Product builds and repository-root
BuildSolutions.ps1 remain later consumer gates. Preserve the WindowChrome namespace, explicit
legacy child sizing and keytip-host deltas already handed to the bump plans. DarkBlue remains
excluded from theme-specific checks and WebViewEditorApplication from automated captures;
normal theme-resource compilation continues.
