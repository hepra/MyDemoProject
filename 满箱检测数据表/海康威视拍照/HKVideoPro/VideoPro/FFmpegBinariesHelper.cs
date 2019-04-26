using System;
using System.IO;
using System.Runtime.InteropServices;

namespace VideoPro
{
	public class FFmpegBinariesHelper
	{
		private const string LD_LIBRARY_PATH = "LD_LIBRARY_PATH";

		internal static void RegisterFFmpegBinaries()
		{
			switch (Environment.OSVersion.Platform)
			{
			case PlatformID.WinCE:
			case PlatformID.Xbox:
				break;
			case PlatformID.Win32S:
			case PlatformID.Win32Windows:
			case PlatformID.Win32NT:
			{
				string current = Environment.CurrentDirectory;
				string probe = Path.Combine("FFmpeg", "bin", Environment.Is64BitProcess ? "x64" : "x86");
				string ffmpegDirectory;
				while (true)
				{
					if (current != null)
					{
						ffmpegDirectory = Path.Combine(current, probe);
						if (!Directory.Exists(ffmpegDirectory))
						{
							DirectoryInfo parent = Directory.GetParent(current);
							current = ((parent != null) ? parent.FullName : null);
							continue;
						}
						break;
					}
					return;
				}
				Console.WriteLine("FFmpeg binaries found in: " + ffmpegDirectory);
				FFmpegBinariesHelper.RegisterLibrariesSearchPath(ffmpegDirectory);
				break;
			}
			case PlatformID.Unix:
			case PlatformID.MacOSX:
			{
				string libraryPath = Environment.GetEnvironmentVariable("LD_LIBRARY_PATH");
				FFmpegBinariesHelper.RegisterLibrariesSearchPath(libraryPath);
				break;
			}
			}
		}

		private static void RegisterLibrariesSearchPath(string path)
		{
			switch (Environment.OSVersion.Platform)
			{
			case PlatformID.WinCE:
			case PlatformID.Xbox:
				break;
			case PlatformID.Win32S:
			case PlatformID.Win32Windows:
			case PlatformID.Win32NT:
				FFmpegBinariesHelper.SetDllDirectory(path);
				break;
			case PlatformID.Unix:
			case PlatformID.MacOSX:
			{
				string currentValue = Environment.GetEnvironmentVariable("LD_LIBRARY_PATH");
				if (!string.IsNullOrWhiteSpace(currentValue) && !currentValue.Contains(path))
				{
					string newValue = currentValue + Path.PathSeparator.ToString() + path;
					Environment.SetEnvironmentVariable("LD_LIBRARY_PATH", newValue);
				}
				break;
			}
			}
		}

		[DllImport("kernel32", SetLastError = true)]
		private static extern bool SetDllDirectory(string lpPathName);
	}
}
