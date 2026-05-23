# Lab 4 - Plugins (hierarchy extension with signed plugins)

## Build order

1. `Lab4.Core` (class library — domain + plugin contracts + signing helpers)
2. `PluginSigner` (console tool)
3. `HockeyPlugin`, `EsportsPlugin` (plugin DLLs)
4. `lab4` (WinForms host)

## How to enable signed plugins

```
:: 1) Generate a key pair once
PluginSigner.exe genkeys keys\private.xml keys\public.xml

:: 2) Put the public key next to the host executable
copy keys\public.xml lab4\bin\Debug\trusted_public_key.xml

:: 3) Build a plugin, copy it to lab4\bin\Debug\Plugins\, and sign it
copy HockeyPlugin\bin\Debug\HockeyPlugin.dll lab4\bin\Debug\Plugins\
PluginSigner.exe sign lab4\bin\Debug\Plugins\HockeyPlugin.dll keys\private.xml 2026-01-01T00:00:00Z 2027-01-01T00:00:00Z me@example.com

:: 4) Run lab4.exe — plugin types appear in the combobox.
```

If the manifest is missing, expired, signed by a different key, or the DLL
is modified after signing, the plugin is rejected and the reason is shown
in the "Plugins" report area at the bottom of the form.
