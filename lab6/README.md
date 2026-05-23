# Lab 6 - Patterns (Adapter, Singleton, Decorator)

Builds on Lab 5. Adds an adapter for a classmate plugin and applies two
extra GoF patterns in the host.

See `PATTERNS.md` for the justification of each pattern.

## New projects

| Project                    | Role                                                              |
|----------------------------|-------------------------------------------------------------------|
| `ClassmateApi`             | Stand-in for a classmate's library exposing `IClassmateTransform` |
| `ClassmateAdapterPlugin`   | Adapter from `IClassmateTransform` into the host's `IProcessingPlugin` |

## Patterns in this lab

1. **Adapter** — `ClassmateAdapterPlugin.ClassmateTransformAdapter` bridges a
   foreign processing API into the host's own `IProcessingPlugin` interface
   without modifying either side.
2. **Singleton** — `Lab6.Core.Patterns.PluginManager` keeps a single,
   process-wide view of the plugin pipeline; the form and any future
   subsystem reach for `PluginManager.Instance` instead of threading
   references around.
3. **Decorator** — `Lab6.Core.Patterns.LoggingProcessingPlugin` invisibly
   wraps every processing plugin to record byte counts and timings. The
   "Settings → View processing log" menu shows the captured trace.

## Building

Same flow as Lab 4 / Lab 5:

```
PluginSigner genkeys keys\private.xml keys\public.xml
copy keys\public.xml lab6\bin\Debug\trusted_public_key.xml
copy *Plugin\bin\Debug\*Plugin.dll lab6\bin\Debug\Plugins\
copy ClassmateApi\bin\Debug\ClassmateApi.dll lab6\bin\Debug\Plugins\
PluginSigner sign lab6\bin\Debug\Plugins\<each>.dll keys\private.xml 2026-01-01T00:00:00Z 2027-01-01T00:00:00Z me@example.com
```
