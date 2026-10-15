# cBrain Fluent wrapper baseline

Current delivery: a single `Fluent.Ribbon` package and `Fluent.dll`, including the private cBrain
control additions. Verified 2026-10-15 under F2DEV-110306. The owner authorized source push to the cBrain GitHub repository; code and native tests live in the
`fluent-wrapper` branch at https://github.com/cBrain-dk/Fluent.Ribbon. Publication and isolated internal-feed validation are complete. No upstream PR is authorized.

## Current baseline

| Item | Value |
| --- | --- |
| Upstream stable release | v11.0.2, published 2026-01-19 |
| Upstream SHA | `ce56b60ca892d1505b7f80826330c676f41fda0a` |
| Current source SHA | `53490eeb8291fb342534022db84bed3fd6dd4ffc` |
| Local prerelease package | `Fluent.Ribbon 11.0.2-0.4` |
| Local package SHA256 | `293530629EAC8467D4B0AA13A3EAC4BC182B0C0BBD919865D0F74E6DC0CD6A0E` |
| Intended private feed | `https://git.cbrain.net/api/packages/F2U/nuget/index.json` |
| Packing / publication | Published through RDAdm; isolated F2U restoration verified |
| Matching source checkout | `D:/git/fluent-wrapper-11.0.2-0.4`, at the current source SHA |
| Release tag | Not created; package source is pinned by SHA in the matching checkout |
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

The protected parameterless factories `ComboBox.CreateQuickAccessComboBox()`,
`SplitButton.CreateQuickAccessSplitButton()` and
`RibbonGroupBox.CreateQuickAccessRibbonGroupBox()` replace only allocation. Fluent's private clone
registration, bindings, item transfer, event handlers and small-size/no-re-add setup remain in
its inherited creation methods.

The public `Fluent.CBrain.RibbonComboBox`, `RibbonSplitButton` and `RibbonGroupBox` live in
`Fluent.Ribbon/CBrain/` and compile into `Fluent.dll`. Their factory overrides return fresh
instances of the matching cBrain type. Public creation calls inherited Fluent setup, sets
`IsElementInQuickAccessToolBar=true` and returns the clone. Further F2 seam subclasses inherit
that setup/marking and override the factory if their exact further-derived type must be retained.
No separate cBrain production assembly/package remains; native namespace is `Fluent.CBrain`.

`CBrain.Fluent.Ribbon.Tests` stays a native, non-packable STA test project. Its three cases use the
actual production classes and assert exact clone type, fresh objects, QAT marking, inherited setup
and assembly identity shared with the Fluent base controls. No synthetic derived marker types,
private reflection or timer sleeps. Source mode references Fluent; package mode pins exact `11.0.2-0.4`.
The solution and Nuke test target include it; TRX collection recurses through project directories.

The owner closed the earlier upstream [factory PR1285](https://github.com/fluentribbon/Fluent.Ribbon/pull/1285)
and [host PR1286](https://github.com/fluentribbon/Fluent.Ribbon/pull/1286), unmerged. These are historical
contributions, not active delivery gates. Source push to the owner-authorized https://github.com/cBrain-dk/Fluent.Ribbon repository is permitted; upstream PRs remain unauthorized. Rebase future
baselines on stable upstream releases; retire private host/factory patches only if a released
equivalent passes the native clone and CommandBar/keytip contracts. The cBrain control feature stays
owned by the fork.

## Normal package workflow

The format is `<Fluent version>-0.<cBrain revision>`, matching remote `cdevelop`'s `7.0.0-0.80` at `ff5dbc768f12931d4f5b85aafcfefdbacc4cacf5`. The current package uses cBrain revision 3 as `11.0.2-0.3`. The `0.3` suffix is a NuGet prerelease label. Local packing is authorized; publication requires specific owner approval.

Version properties are maintained in `Fluent.Ribbon/Fluent.Ribbon.csproj`:
`Version=11.0.2-0.3`, `PackageVersion=$(Version)`, `AssemblyVersion=11.0.0.0`,
`FileVersion=11.0.2.0`, `InformationalVersion=$(Version)`. Project properties disable GitVersion's
version/assembly-info overrides; Nuke Compile/Pack also no longer supply overriding version values.
The SDK records the source revision. No command-line version workaround is needed. Nuke's legacy ZIP filenames still use GitVersion labels; the normal NuGet package workflow does not use those ZIPs.

From the clean matching source checkout, `Publish-CBrainWrapper.ps1` runs the native source tests, packs Release and publishes the exact package for the project version through `rdadm nuget push`. Update `Version` in the project before each release. For packaging without publication:

```powershell
dotnet pack ./Fluent.Ribbon/Fluent.Ribbon.csproj -c Release -o C:/tmp/fluent/wrapper-prerelease/package
```

After inspection, consumer validation and explicit owner approval for this specific package/feed,
use the normal internal `rdadm nuget push` route. Check the chosen version is still unused before
publication, never replace an existing version, and validate an isolated internal-feed restore
with no local fallback. Preserve credentials in the established encrypted RDAdm mechanism.
`Build-CBrainWrapper.ps1` builds/tests only; it no longer packs. Rebuilding can change package
bytes; the hash in the historical validation section identifies only the prior `11.0.2-cbrain.2` artifact.

Build/package targets are exactly net48 and net10.0-windows. A net48 consumer selects net48; resolved
legacy dependencies remain ControlzEx7.0.3, Microsoft.Xaml.Behaviors.Wpf1.1.135, Memory4.6.3,
Buffers4.6.1, Vectors4.6.1 and Unsafe6.1.2. The net10 group directly depends on ControlzEx7.0.3 and resolves Xaml.Behaviors1.1.135 transitively; it has no System.Text.Json package dependency.
The original upstream license and signing are preserved. Generated XAML is built normally.

## Source validation — 2026-10-10

Source `840986f34c7e07da0fa5f5e69665c38aa32385d1` declares exactly `net48;net10.0-windows`
for the Fluent library, native tests and upstream test projects. The serial test helper uses
those targets. net48 retains Framework-specific references, binding redirects and dependencies;
the modern-only explicit palette generator fixture is excluded through `NETFRAMEWORK`.
The version is `11.0.2-0.2`. Package verification is recorded below.

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
`C:/tmp/F2DEV-110306/two-target-review.md`. These source verification runs used no pack target, package-mode test, NuGet upload or
external CI job. Only the reviewed source branch is pushed to the authorized cBrain fork.

## Local prerelease package validation — 2026-10-10

`Fluent.Ribbon 11.0.2-0.2` is packed from clean source
`840986f34c7e07da0fa5f5e69665c38aa32385d1` in `D:/git/fluent-wrapper-11.0.2-0.2`, using the
normal direct `dotnet pack` command above with project-owned version properties and no version
overrides. NuGet.Versioning confirms `IsPrerelease=true`. The exact validated artifact is
`C:/tmp/fluent/wrapper-prerelease/package/Fluent.Ribbon.11.0.2-0.2.nupkg`, SHA256
`A859F4CC505D8904C0FC6B8AD05C764395D87C52593262C2CFB1BBB55AFA736B`.

Inspection confirms package ID/version, the source repository SHA, original license and signed
`Fluent, Version=11.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f` assembly identity.
Library assets are only DLL/XML pairs in `lib/net48` and `lib/net10.0-windows7.0`; the latter is
the normalized package TFM for the project's `net10.0-windows`. No separate cBrain DLL ships.
Direct dependencies are ControlzEx7.0.3 for both groups and System.Memory4.6.3 additionally for
net48. The net48 consumer resolves the recorded legacy graph; net10 resolves Xaml.Behaviors1.1.135
through ControlzEx and has no System.Text.Json NuGet dependency.

The real native wrapper tests pass **2/2 per target** against this package. A separate package
consumer probe passes **9/9 per target**, including the cBrain clone/same-assembly assertions,
CommandBar-shaped host behavior and Danish æ/ø/å. VS18 MSBuild restore/Rebuild uses Release/AnyCPU,
`/m:1 /nr:false`, a fresh isolated package cache and exact Fluent-only local source mapping.
VSTest executes each target serially with no filter; host probes use process-local DPIUNAWARE.
The cached nupkg hash equals the inspected artifact, metadata proves the local source, and both
native/consumer test output Fluent DLL hashes equal their corresponding packaged assets.

Evidence: `C:/tmp/fluent/wrapper-prerelease/` contains `pack.log`, `inspection.json`,
`restoration.json`, `consumer-targets.json`, build logs, exact local NuGet configuration and TRX.
Packing reports the pre-existing missing-package-readme advisory; build/test commands succeed.
The preceding source evidence covers this same source SHA and both target frameworks.

The package has not been uploaded. Specific owner approval for this artifact and the internal
F2U feed is still required before the normal `rdadm nuget push` route. Then restore the exact
version from that feed with an isolated cache and no local fallback before completing the wrapper
publication gate. Do not overwrite an existing version or rebuild and substitute bytes silently.
The next conversation starts the independent Shared WPF tools plan on the existing product
cFluent dependency. Tools/seam preparation can proceed while publication is pending; the dependency
bump requires the wrapper's published, isolated-feed-validated package.

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
- This prior artifact was not published; it is superseded by the `11.0.2-0.2` candidate.

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
extraction and F2 dependency changes have not started; the handoff starts the Shared WPF tools plan. Product builds and repository-root
BuildSolutions.ps1 remain later consumer gates. Preserve the WindowChrome namespace, explicit
legacy child sizing and keytip-host deltas already handed to the bump plans. DarkBlue remains
excluded from theme-specific checks and WebViewEditorApplication from automated captures;
normal theme-resource compilation continues.

## Published combined delivery — 2026-10-13

Owner authorization covers `Fluent.Ribbon 11.0.2-0.2` publication on F2U through `Publish-CBrainWrapper.ps1`. The `-PackagePath` option published the validated artifact without rebuilding: SHA256 `A859F4CC505D8904C0FC6B8AD05C764395D87C52593262C2CFB1BBB55AFA736B`, source `840986f34c7e07da0fa5f5e69665c38aa32385d1`. A fresh isolated restore selects Fluent only from the F2U feed; restored package provenance/hash/source and selected DLL hashes match. VS MSBuild Release/AnyCPU Rebuild succeeds; combined native-clone/host/Danish-keytip probes pass 9/9 on each target, VSTest exit 0. This does not rerun or replace the recorded upstream suite failure.

Evidence: `C:/tmp/fluent/wrapper-delivery-check-20261013T104731Z/`. Delivery is complete; product dependency migration and its acceptance gates are separate. The script's default workflow still tests and packs a new Release package before publishing. Its existing-package option preserves previously validated bytes.

## GroupBox quick-access delivery - 2026-10-15

`Fluent.Ribbon 11.0.2-0.4` includes `Fluent.CBrain.RibbonGroupBox` and the protected
`RibbonGroupBox.CreateQuickAccessRibbonGroupBox()` factory. Allocation is the only
change inside the existing public creation method. Launcher bindings, popup
handlers and inline-item transfer remain in Fluent. The cBrain control returns
fresh exact-type copies and marks them as QAT items. Further seam subclasses can
override the factory while inheriting the public clone setup and marking.

The owner authorized this new release through `Publish-CBrainWrapper.ps1` to F2U.
The script's source/native tests pass 3/3 on each supported target. The upstream
GroupBox fixture, including popup open/close item transfer, passes 4/4 on each.
A fresh feed-only consumer restore builds with warnings-as-errors and passes
14/14 on each target: native controls, GroupBox lifecycle/sizing and existing
host/Danish-keytip contracts. Package provenance, repository commit, package hash
and both selected Fluent.dll hashes match the published artifact. This does not
rerun or replace the separately recorded full upstream suite result.

Published source: `53490eeb8291fb342534022db84bed3fd6dd4ffc`.
Package SHA256: `293530629EAC8467D4B0AA13A3EAC4BC182B0C0BBD919865D0F74E6DC0CD6A0E`.
Matching clean source checkout: `D:/git/fluent-wrapper-11.0.2-0.4`.
Evidence: `C:/tmp/fluent/wrapper-0.4-publication/`, including internal-verification.json,
publisher log, source GroupBox results and isolated package-consumer TRX files.
The existing missing-package-readme advisory remains recorded in the pack log.
