# Pattern justification

## 1. Adapter — required by task

The classmate ships a library (`ClassmateApi`) whose interface,
`IClassmateTransform`, exposes `Encode` / `Decode` methods and the
`DisplayName` property. Our host pipeline talks to `IProcessingPlugin`
with `ProcessOnSave` / `ProcessOnLoad` and `Name`. Rewriting the
classmate's binary is not an option, and we do not want to widen our own
plugin contract just for one third-party shape.

`ClassmateTransformAdapter` implements `IProcessingPlugin` and forwards
each call to the wrapped `IClassmateTransform`:

```
ProcessOnSave(input) -> adaptee.Encode(input)
ProcessOnLoad(input) -> adaptee.Decode(input)
Name                 -> adaptee.DisplayName
```

The pipeline stays single-interface; the foreign vocabulary is contained
in a single 20-line class. Other classmate transforms (`InvertTransform`,
future ones) are accepted by the same adapter — only the constructor
argument changes.

## 2. Singleton — `PluginManager`

The plugin set is process-wide state. Once an assembly is loaded with
`Assembly.LoadFrom` it cannot be unloaded without recycling the AppDomain,
so a "second pipeline" would silently lie about what is really live in
the host.

`PluginManager.Instance` is the single access point for both the
processing pipeline and the cross-cutting log. The Form, the settings
dialog and any future subsystem read state from the same place instead of
juggling fields. The double-check lock makes the lazy initialization
explicit for educational purposes; `Lazy<T>` would also work.

## 3. Decorator — `LoggingProcessingPlugin`

We want optional, composable instrumentation (byte counts, elapsed time,
future audit trail) for every processing plugin without editing the dozen
or more individual implementations — and without inheriting from a base
class that would force all plugins into a single hierarchy.

The decorator implements the same `IProcessingPlugin` interface, holds a
reference to the wrapped plugin, forwards every property, and adds
side-effects around the byte-array methods. `PluginManager.RegisterProcessingPlugin`
quietly wraps each incoming plugin so the rest of the host never sees the
raw instance. New decorators (encryption-counter, telemetry shipper) can
be stacked the same way without touching the existing code — the open /
closed principle in action.
