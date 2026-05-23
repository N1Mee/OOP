# Lab 5 - Functional plugins (process structures on save/load)

Builds on Lab 4. The plugin contract now has two flavors:

* `IMediaItemPlugin` — adds new sport classes (Hockey, Esports, MMA).
* `IProcessingPlugin` — transforms the BSON byte stream on save and
  reverses the transform on load. Several plugins can be chained.

Three processing plugins are provided:

| Plugin           | Variant | Settings exposed in UI                        |
|------------------|---------|-----------------------------------------------|
| `GZipPlugin`     | 2       | Compression level (Optimal / Fastest / None)  |
| `AesPlugin`      | 3       | Passphrase + key size (128 / 192 / 256)       |
| `ChecksumPlugin` | 5       | Hash algorithm (MD5 / SHA1 / SHA256 / SHA512) |

## How plugins reach the host

* Signed plugin DLLs are placed under `lab5\bin\Debug\Plugins\` and are
  auto-loaded on startup (each with its `.manifest.xml` side-car).
* "Settings -> Load plugin from file..." lets the user pick a DLL by hand.
* "Settings -> Plugin settings..." opens a tabbed dialog where every
  loaded processing plugin can be toggled and reconfigured — this
  satisfies the 10-point additional task.

The pipeline runs plugins in registration order on save and in reverse on
load, so e.g. **GZip -> AES -> Checksum** at save time becomes
**verify checksum -> decrypt -> decompress** at load time.

For key generation and plugin signing, see `PluginSigner` (same usage as
in Lab 4).
