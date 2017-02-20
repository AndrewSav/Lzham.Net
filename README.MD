# Lzham.Net
This is a managed wrapper around [LZHAM - Lossless Data Compression Codec](https://github.com/richgel999/lzham_codec) for Windows.

## Status
This project is something that I needed elsewhere, so it was most driven by these needs, however I also tried
to make in generaly useful. Caution advised: test it before you use it.

## API
Native Lzham provides three ways of operation:

  - Block-by-block (you call init first and deinit when done)
  - All-in-one (you provide block of memory to compress and it calls init/deinit for you)
  - Zlib style

**Zlib style** is not exposed by this wrapper. **All-in-one** is exposed via static methods on `Lzham` class:

```
public static DecompressStatus DecompressMemory(
  DecompressionParameters parameters, 
  byte[] inBuf,int inBufSize, int inBufOffset, 
  byte[] outBuf, ref int outBufSize, int outBufOffset, 
  ref uint adler32)

public static CompressStatus CompressMemory(
  CompressionParameters parameters, 
  byte[] inBuf, int inBufSize, int inBufOffset, 
  byte[] outBuf, ref int outBufSize, int outBufOffset, 
  ref uint adler32)
```

Here you provide compression/decompression parameter, a byte array where lzham reads/writes to/from and offsets and lengths in the arrays. 
It returns adler32 checksum of the completed operation. For better description of compression/decompression parameters and status codes 
please refer to original [lzham documentation](https://github.com/richgel999/lzham_codec/blob/master/include/lzham.h).

**Block-by-block** mode is abstracted via `LzhamStream` class to provide more natural feeling .net way. 
Please see  https://github.com/AndrewSav/Lzham.Net/blob/master/LzhamTest/Program.cs for an example on usage.

## Test project
Note, that the test project in minimal and does not do any parameter validation or error handling. Make sure to implement those in your
real life scenario.

## Building
This project was build with Visual Studion 2015. The test project uses CommandLine, Fody and Fody.Costura dependecies but these are not required
for the wrapper itself. When you use the wrapper in your application make sure that lzham.dll is available for your application and that it's 
of the correct architecture (x86/x64). Normally lzham.dll should be in the same folder as the main application and LzhamWrapper.dll.

[Fody.Costura](https://github.com/Fody/Costura) project allow embedding native and managed dependencies in the application executable.
It's used in the test project to provide a single LzhamTest.exe file (plus obligatory LzhamTest.exe.config), where LzhamWrapper.dll 
and lzham.dll are embedded as resources. You do not have to use the same approach in your app.

Finally, the lzham.dll that you find in the test project where built by me from sources of [lzham_codec](https://github.com/AndrewSav/lzham_codec).
I made two small patches of the project in my fork which are not critical for LzhamWrapper:

  - Added z option to the test app to be able to test zlib-like streams
  - Attempted to fix [an issue](https://github.com/richgel999/lzham_codec/issues/13) with zlib parameters.
  - In addition, made some headers edit to allow the lzham library to be compiled by my VS 2015 without errors

Since both of the above are related to zlib interface which are not exposed in LzhamWrapper anyway they are not required for LzhamWrapper to work,
having said that, the result of my custom compilation is the only dlls that I tested the Wrapper with so your milage may vary.

Also I'm guessing that the resulting lzham.dll require vc++ runtime libraries installed in order to function properly, but I'm not sure.

The lzham_code project, actually generates output like: lzham_x86D.dll lzham_x86.dll lzham_x64D.dll lzham_x64.dll, I renamed both x64 and x86 dlls
to lzham.dll for the purposes of my wrapper.
