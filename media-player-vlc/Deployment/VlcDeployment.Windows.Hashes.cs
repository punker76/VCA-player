// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston MA 02110-1301, USA.

#region Usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;

#endregion

namespace DZ.MediaPlayer.Vlc.Deployment {
	public sealed partial class VlcDeployment {
		/// <summary>
		/// Last compiled compatible version of VLC.
		/// </summary>
		public static readonly string DefVlcVersion = "1.1.9 The Luggage";

		/// <summary>
		/// Get' default package path.
		/// </summary>
		/// <returns>Path to zip</returns>
		public static string GetDefaultPackagePath(string fileName) {
			Assembly entry = Assembly.GetEntryAssembly();
			if (entry == null) {
				entry = Assembly.GetCallingAssembly();
			}
			return GetDefaultPackagePath(entry, fileName);
		}

		/// <summary>
		/// Get' default package path.
		/// </summary>
		/// <returns>Path to zip</returns>
		public static string GetDefaultPackagePath(Assembly relativeTo, string fileName) {
			Assembly entryAssembly = relativeTo;
			//
			string path = Path.Combine(Path.GetDirectoryName(relativeTo.CodeBase.Substring(8)), fileName);
			return (File.Exists(path)) ? (path) : (Path.Combine(Path.GetDirectoryName(entryAssembly.Location), fileName));
		}

		/// <summary>
		/// Get default package file name
		/// </summary>
		/// <returns>Package file name</returns>
		public static string GetWindowsOSPackageFileName() {
			return "libvlc-1.1.9-win32.zip";
		}

		/// <summary>
		/// Hash of last package.
		/// </summary>
		/// <returns>Package hash as base64.</returns>
		/// <remarks>It is not verified at current version.</remarks>
		public static string GetWindowsOSPackageHash() {
            return ("JlfuAIn+QRfBrDYGQ1Zsxw==");
		}

		/// <summary>
		/// Returns default location of deployment. This is
		/// location of execution assembly.
		/// </summary>
		/// <returns>Directory where to deploy library.</returns>
		public static string GetWindowsOSDeploymentLocation() {
			Assembly entry = Assembly.GetEntryAssembly();
			if ( entry == null ) {
				entry = Assembly.GetExecutingAssembly();
			}
			return (GetWindowsOSDeploymentLocation(entry));
		}

		/// <summary>
		/// Returns default location of deployment. This is
		/// location of execution assembly.
		/// </summary>
		/// <returns>Directory where to deploy library.</returns>
		public static string GetWindowsOSDeploymentLocation(Assembly relativeTo) {
			return (Path.GetDirectoryName(relativeTo.Location));
		}

		/// <summary>
		/// Creates default <see cref="HashAlgorithm"/>. This is MD5
		/// instance.
		/// </summary>
		/// <returns>MD5 hash algorithm instance.</returns>
		public static HashAlgorithm GetDefaultHashAlgorithm() {
			return (MD5.Create());
		}

		/// <summary>
		/// Get's default package content dictionary.
		/// </summary>
		/// <returns>Dictionary of files from zip package and it's hashes.</returns>
		public static Dictionary<string, string> GetWindowsOSPackageContents() {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("AUTHORS.txt", @"gYHmSjBV/useZpxnCJ/zzQ==");
            dictionary.Add("COPYING.txt", @"URD6dSF5un7nhwFhD+qIcw==");
            dictionary.Add("libvlc.dll", @"5wcFJxNddMrO2UG2eM59mQ==");
            dictionary.Add("libvlc.dll.manifest", @"fgrIGoxT+o5TMNRLlvOH/A==");
            dictionary.Add("libvlccore.dll", @"Z5sA87OZYO0Dvw2T4bCOeQ==");
            dictionary.Add("README.txt", @"zkqTeP5wQ3A2Et1VV3B+Fg==");
            dictionary.Add("THANKS.txt", @"SFLeJFuPwiU9gcPBz70UlQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "liba52tofloat32_plugin.dll", @"PWH6iVnTsXh1PpCKlHjSAA==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "liba52tospdif_plugin.dll", @"ZyjtYZNUP2h9VXUK4kDAPw==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "liba52_plugin.dll", @"4d4DTtkB05Tq15xkUZknLA==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaccess_attachment_plugin.dll", @"8diIPzx78vP0hk9D8XJNxQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaccess_bd_plugin.dll", @"Ge28m3sJDsp3xGWw06BLiQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaccess_fake_plugin.dll", @"5rgsHvDLdftF+AweuSZARg==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaccess_ftp_plugin.dll", @"1ozJPwNdEqT1Zaw3kLrbIA==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaccess_http_plugin.dll", @"LRcsyOVlTCD+9sAGayEjEw==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaccess_imem_plugin.dll", @"1y4lRUfJDfJzb0xdsOvGeg==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaccess_mms_plugin.dll", @"xeHgezsiNxmLs9297rI7mw==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaccess_output_dummy_plugin.dll", @"Qg1yR6TQX4TQOckvAXAorg==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaccess_output_file_plugin.dll", @"XTlMkCLIe736nFFxUTstPw==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaccess_output_http_plugin.dll", @"8UzV4J8NV5aUv9aV1bj0UQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaccess_output_shout_plugin.dll", @"95TrKqNI2kLtbtWw3vAUNg==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaccess_output_udp_plugin.dll", @"xN7esSWvszgnVo+eoDBuPQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaccess_realrtsp_plugin.dll", @"x5V3xJYqWQu0cPzWizNUVQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaccess_smb_plugin.dll", @"HKsGtOnaQxFjE7obRaBt4w==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaccess_tcp_plugin.dll", @"TCZnn8rzFFCVE6C3dN/cTQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaccess_udp_plugin.dll", @"RBka7H+8Jd0EOQ5DRnyYeQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libadjust_plugin.dll", @"PdBOiENdUYFbSpM+j5BaQA==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libadpcm_plugin.dll", @"pflQj51LCSBGXXOi+j+9XQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaes3_plugin.dll", @"VsTcwgwgqVL9ChjYvUPs8g==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaiff_plugin.dll", @"d2fPsmZ3xoAtMkVvwQuGfQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libalphamask_plugin.dll", @"suR4tk/Z48QiRJCn76Tn4A==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaout_directx_plugin.dll", @"LlAWRUPi6s9tEkzkl4L4aw==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaout_file_plugin.dll", @"2EqyKbtyKEx7biFGjp+RDw==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaout_sdl_plugin.dll", @"aUXhOj7lbIuVAurhwdLIQg==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaraw_plugin.dll", @"5BDFerew3Ow6AhRllCeiDA==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libasf_plugin.dll", @"EFN9qdA02TcSRyOE2SurWw==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libatmo_plugin.dll", @"Un9C2i+Cc4tM2lLfPpBsFw==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaudiobargraph_a_plugin.dll", @"02VOvQR4wCXQFIZmqtBOyw==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaudiobargraph_v_plugin.dll", @"SQwTJfsX54x6y2Asel3XzA==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaudioscrobbler_plugin.dll", @"9GVqF+XNoyo3i8vmhVWXEw==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaudio_format_plugin.dll", @"RsDVXy9zd3yUHtV5y5xQXQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libau_plugin.dll", @"P81icBHpNBCMC8gRluv7zA==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libavcodec_plugin.dll", @"+fekUmgb51aoOqozxTaAuQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libavi_plugin.dll", @"Tzu1wDnvfZspXM7waeYsfQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libball_plugin.dll", @"Jtaji/9KhAmS366qjo3qOA==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libbandlimited_resampler_plugin.dll", @"B+Ga1VcAfKa+vEG9Sy96ng==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libbda_plugin.dll", @"zZh8Hod39lqPdycn+PyKGg==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libblendbench_plugin.dll", @"hBPpzH/m5yHsqdYOE8iaTA==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libblend_plugin.dll", @"mX9lIqquK3myFWEfM//Jhg==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libbluescreen_plugin.dll", @"ZcQiXjSSmRbFsiO0FxJ0SQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libcaca_plugin.dll", @"Ebp/aDU2jraD/Dew6FNuaw==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libcanvas_plugin.dll", @"Hej4Uv6ymHRZ6TVoW5iKgg==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libcc_plugin.dll", @"mrqaalisvF0U3W3+KOXfgQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libcdda_plugin.dll", @"mt0Ac8n00Zlup1A8VidspA==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libcdg_plugin.dll", @"vc4l+/OanPOsHbjGpzxs+Q==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libchain_plugin.dll", @"BYwDJilzitHUqbSMH+TvOQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libchorus_flanger_plugin.dll", @"0qizOICpq7omLyKqS7tPeg==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libclone_plugin.dll", @"0ipAV1bb6W8gv/BLck3BeQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libcolorthres_plugin.dll", @"oz9kS7JsC3gC4BK5Xw594A==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libconverter_fixed_plugin.dll", @"ZOQ6HB1QOd4cj79c5BmKKg==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libcroppadd_plugin.dll", @"MzW+PXWrIxC6zSOgRval9Q==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libcrop_plugin.dll", @"fDok/ToFcfzJSWUBl3IcSQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libcvdsub_plugin.dll", @"B+APjGYGa3e5N6CNfUoLFw==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libdeinterlace_plugin.dll", @"MWAh9CqS+Jmahxf8hmcPjA==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libdemuxdump_plugin.dll", @"bQLJn2rHN7aWEiiivjQ6dQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libdemux_cdg_plugin.dll", @"QxnpC8PigQ+cA0e2riYmsg==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libdirac_plugin.dll", @"jvLhB+Zq5LdxbY0SKlJpjg==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libdirect3d_plugin.dll", @"lzfePBJIi6FxU3oqDllm9w==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libdirectx_plugin.dll", @"/LdXB6Ek6aiKxdCCphyCMQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libdmo_plugin.dll", @"V2QF2+AGWl9s7DFnFDQoPg==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libdolby_surround_decoder_plugin.dll", @"rXBYQ+bKpfHLBhwK24G0lA==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libdrawable_plugin.dll", @"G8yvUg1hXFHo1tWGWIbPkw==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libdshow_plugin.dll", @"9MKA4jaGahIt0GmNpTeYOA==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libdtstofloat32_plugin.dll", @"svyH2CxMIZaw7B2rxp20jQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libdtstospdif_plugin.dll", @"Jp+XJW02o6BMFqEz161/Fw==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libdts_plugin.dll", @"Em3Lfl8H7l3af2emYCg7WA==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libdummy_plugin.dll", @"J1SadRXBYfVmQ3LfSJeNTw==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libdvbsub_plugin.dll", @"c8D7suZj0T9NaxfFc0g5UQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libdvdnav_plugin.dll", @"PSinQsr2eTmmN1R236zl2w==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libdvdread_plugin.dll", @"I7CC/KAGssX8LKnnTkyfzw==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libequalizer_plugin.dll", @"CiIb/vIPrXqFv521TEXNbQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "liberase_plugin.dll", @"8WERoESG4wIToLHaxmbjAg==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libes_plugin.dll", @"7RtG806HL5QJMmdzXy3IRQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libexport_plugin.dll", @"xkqycBEeXcPCsVbCjpwf0A==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libextract_plugin.dll", @"zryFUaOBRTX9T6DKqHgtZw==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libfaad_plugin.dll", @"VKzu0mgy9RqpX1PUxk1SDQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libfake_plugin.dll", @"Rpc2G/DsqCt1DfE3oJxl5g==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libfilesystem_plugin.dll", @"OQaIqQVOCLviNzV7cojJYQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libflacsys_plugin.dll", @"/E72ahl0uVJRJpzTWLIPOQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libflac_plugin.dll", @"eYPgeX5G0Hz5KKcZIc62Jg==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libfloat32_mixer_plugin.dll", @"xbOsVDYHjmyY/zjaxd/omw==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libfluidsynth_plugin.dll", @"lognnImeCQNCbvsutDUWHQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libfolder_plugin.dll", @"ISsgr6s2cmf6ByBpB9YgVQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libfreetype_plugin.dll", @"YiImM0mwNBhTkHGz4kpEGQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libgaussianblur_plugin.dll", @"wly9kNWMcWNsti+nm8AXVw==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libgestures_plugin.dll", @"dyPQjCT7VOHytoRAlamhiA==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libglobalhotkeys_plugin.dll", @"7yJg6VJpjNleqIHAPZ6Ifg==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libglwin32_plugin.dll", @"uvZPjWmm6AZ2z+HD1f37JA==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libgme_plugin.dll", @"gMIiALhliLmnGytbLPpYcg==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libgnutls_plugin.dll", @"bD6WlMDIR02+fVVFsUfwbw==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libgoom_plugin.dll", @"PcQaKKalM0rHzQcSrlz6Ug==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libgradient_plugin.dll", @"QY2FlOvT1Bls+gPquYwwWQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libgrain_plugin.dll", @"wuvohx+kt9pFjOnfj2NfHA==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libgrey_yuv_plugin.dll", @"I9EfXKcIkpfTlOlUrHxPXA==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libh264_plugin.dll", @"11xVn3C9GQlM2csOIVT9pA==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libheadphone_channel_mixer_plugin.dll", @"qOUPHP1pBkUFec1NbXa/SQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libhotkeys_plugin.dll", @"UHaj8DL6FojteR1gbZe9dQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libi420_rgb_mmx_plugin.dll", @"4jH3bBVizV9CEDdC8bSkag==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libi420_rgb_plugin.dll", @"Je5kltWHngsshgBhJS0GcQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libi420_rgb_sse2_plugin.dll", @"DVAB4//bLCMiv+ADsOPZJA==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libi420_yuy2_mmx_plugin.dll", @"UjHhFGhp/GaueLeyOsgOSw==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libi420_yuy2_plugin.dll", @"rWP2sDIO+ZGnMXgKqXjxfg==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libi420_yuy2_sse2_plugin.dll", @"JPBQqki9Ynm20GCvXEmjKA==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libi422_i420_plugin.dll", @"nSJPJpjM1z79hprv+AC39Q==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libi422_yuy2_mmx_plugin.dll", @"8T5w8eKcTs2oEHCyM7Zscg==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libi422_yuy2_plugin.dll", @"SaFJozIG1EhCvNKtRJb+9Q==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libi422_yuy2_sse2_plugin.dll", @"PjDdo+TsYW6nGeYXOOfjpg==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libinvert_plugin.dll", @"QkegzQOl4rwvCYY2CeeDgw==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libinvmem_plugin.dll", @"/+f2p4MJRSIHCp6YLLhz5w==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libkate_plugin.dll", @"mFBCvJzq4inJMJtzrN2Qwg==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "liblibass_plugin.dll", @"hL2ntpCE74TgfBEZ40sRow==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "liblibmpeg2_plugin.dll", @"l6GeQ1IbXWtZeXY22ktN1g==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "liblive555_plugin.dll", @"++pu0SqHJ6YsXwOzHF6rpg==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "liblogger_plugin.dll", @"zo+uQfq9YIv/RAo75mlRww==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "liblogo_plugin.dll", @"+w3tElPuWmaiYMjjEwYgOQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "liblpcm_plugin.dll", @"iJgP7FtFg7k45aN0CwSRSg==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "liblua_plugin.dll", @"/UFk8lgOtgYesTacz+jfIg==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmagnify_plugin.dll", @"2MEAgc8fDkWj8j+J1pGepA==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmarq_plugin.dll", @"lBrcaDU/8qkHeIfB/lZjXw==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmediadirs_plugin.dll", @"v8ri01/gzbVcsQK6WrRE0w==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmemcpy3dn_plugin.dll", @"4SGRi1bb+2MAOBMgi2J0ZA==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmemcpymmxext_plugin.dll", @"dp7kCtRUZtXHgNQo4TdR2A==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmemcpymmx_plugin.dll", @"yPw2CW2myMM+9QlfRVpz8Q==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmirror_plugin.dll", @"ZYwR2PxWT84hKzPDdmEDVw==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmjpeg_plugin.dll", @"9WluOxQhnB2RONsm0O24ig==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmkv_plugin.dll", @"AUfA8BxxrwCLYAxsb5F0Lw==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmod_plugin.dll", @"F/Yi6zJKKMSaf3DviC8MmQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmono_plugin.dll", @"7NJKKbvLAPgIfukfKtrZ9w==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmosaic_plugin.dll", @"jLWrxKGsS6i5Pd00QdeE/Q==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmotionblur_plugin.dll", @"v79vp7aFzi06LppV5nM6dg==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmotiondetect_plugin.dll", @"tOOiav74DmxWVP/O9yjSdw==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmp4_plugin.dll", @"s+P93kZf23przpuPEwZ2fw==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmpc_plugin.dll", @"RaUikVULQqSDi4XJ4/ANAA==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmpeg_audio_plugin.dll", @"9dDZ98v8l48nx1updh1rNw==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmpgatofixed32_plugin.dll", @"D7TjEj+JtoH2oi5VAF44mg==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmpgv_plugin.dll", @"DpvIrAhojN+nopVDhAKkoA==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmsn_plugin.dll", @"fcZCohcGGPAuZaTMMVMJpA==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmux_asf_plugin.dll", @"eV1WIbyzoSfK59m5hchwfA==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmux_avi_plugin.dll", @"WeRdDFNHidDEgdymBrWxaw==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmux_dummy_plugin.dll", @"qW75L3P0aIa5SI+Jctgz8A==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmux_mp4_plugin.dll", @"zIYjQPJN88REywEtpRAyew==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmux_mpjpeg_plugin.dll", @"iO0xHpqLV+CvOTXjqVg8ow==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmux_ogg_plugin.dll", @"aJAUX8s9x91eCoW3JNp3qQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmux_ps_plugin.dll", @"fP8fCQb813ozpktOXNMqLg==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmux_ts_plugin.dll", @"HnE7EX8gbMdImEcwevXkug==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmux_wav_plugin.dll", @"hsSNX7RlRl5C4/+UCY/www==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libnetsync_plugin.dll", @"+xzMnUgXwhkfDYR4NOKALg==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libnoise_plugin.dll", @"J8hJ90MPhFszWoGXbhmmfA==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libnormvol_plugin.dll", @"nLW5OUVbHz0JF91THEYJyg==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libnsc_plugin.dll", @"hhQWNLYOt5Mvu2fap5C4Wg==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libnsv_plugin.dll", @"jvKOoN61zk/JTAqYZPAVnw==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libntservice_plugin.dll", @"uleaEA2hVtkOB50Uvlh9vA==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libnuv_plugin.dll", @"RX33sAe6WFXcFIj2oiuRtw==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libogg_plugin.dll", @"zkEtVTytnciot38D4k/kuA==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "liboldhttp_plugin.dll", @"SuU6MMEDmm1rQEdUxzpoNw==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "liboldrc_plugin.dll", @"HGQAHEtkJbx68fFAfqSr3A==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "liboldtelnet_plugin.dll", @"E56f1dG2+jp04ua2acyCew==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libosdmenu_plugin.dll", @"HvZezfamPIET2MdEZ/WWdA==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libosd_parser_plugin.dll", @"JhuSBHdbhOWGf5f2b6TXDA==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libpacketizer_copy_plugin.dll", @"svTHEm/LFY9EGMhKe8Kyrw==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libpacketizer_dirac_plugin.dll", @"RQ4Qcz1jiYqNZZN4C/MuCw==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libpacketizer_flac_plugin.dll", @"B2SxjwR47Lzc/6dF4VZx+g==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libpacketizer_h264_plugin.dll", @"jWEP8B+PFqoi10JbbBCHvA==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libpacketizer_mlp_plugin.dll", @"p4G7T88OFosR5K2nQkDn7g==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libpacketizer_mpeg4audio_plugin.dll", @"ykfJOtod+6BIGuvLtPbXkg==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libpacketizer_mpeg4video_plugin.dll", @"uGdr1tBS4Ua75MCjv6xEDg==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libpacketizer_mpegvideo_plugin.dll", @"8U++ANA21r8EjKkEYT8UNw==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libpacketizer_vc1_plugin.dll", @"UXzcCe8UDUlzf3a1NXzfyA==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libpanoramix_plugin.dll", @"ozXSAjzxPehC8BlSU9itDQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libparam_eq_plugin.dll", @"AZCM1fP+QDFp+5qYiquUuQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libplaylist_plugin.dll", @"AtVo/wXxUy6LoYQ5HZKXQw==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libpng_plugin.dll", @"tu+hqAY+bJbCL8vLeNnBcg==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libpodcast_plugin.dll", @"8CfpZD/V2ro2pIF8fdRsWQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libportaudio_plugin.dll", @"iZ8lNBoVdA15d7Is7soM3A==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libpostproc_plugin.dll", @"CEgD78nErKuH2n7sGEhgzg==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libprojectm_plugin.dll", @"ZabP9vtQVtMHN5SZN2O0cw==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libpsychedelic_plugin.dll", @"3KHqqSyFiI9KUk0BDBL0wA==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libps_plugin.dll", @"JzjtfW2TwaYwU3GEdkd4Gg==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libpuzzle_plugin.dll", @"cNaDW14SD21ecajAf4EyDQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libpva_plugin.dll", @"57hgrzfh6EplLHg3bne4KA==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libqt4_plugin.dll", @"ZB77cLV1pfXdBLA7oTuqyQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libquicktime_plugin.dll", @"s8FmAE5hc1i63WnA1mA7LA==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "librawaud_plugin.dll", @"vxE7wnEK3P7LB7B2ux9eGQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "librawdv_plugin.dll", @"Sp5bdGTgltd55X12D6FdGw==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "librawvideo_plugin.dll", @"yaKRqVaLDDucR82181S8Cw==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "librawvid_plugin.dll", @"NuqKGB1EEwoCvDFAzXYqdw==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "librealvideo_plugin.dll", @"m1UWsRc94ndi4Q8MiYX19g==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libreal_plugin.dll", @"rOVb2C8SlqqxMVSdieW9NA==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libremoteosd_plugin.dll", @"cXMDF4+EBRYN175LtzheVQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libripple_plugin.dll", @"PhCPHPpeGKyEjJM3niNpvQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "librotate_plugin.dll", @"kK0ctUq1jTE0XgbWant3sg==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "librss_plugin.dll", @"u0Kr9U/DwOkz0bxIVOzwRg==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "librtp_plugin.dll", @"nnFq780EWzPFkbKoPb5gqQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "librv32_plugin.dll", @"OXN5pc4APOJ9SRWMrI/+hg==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libsap_plugin.dll", @"vZRMazCcfoCumBIJk/WO/Q==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libscaletempo_plugin.dll", @"e0ZtgJ6pZbza4HSkuDDO3Q==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libscale_plugin.dll", @"WIprHpYFlwSEijfmAkEI4Q==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libscene_plugin.dll", @"4zOouXJnvTwvuBvwSh+DZA==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libschroedinger_plugin.dll", @"dbLy5M2CFkyH/YTtMTG79A==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libscreen_plugin.dll", @"L/5wCIB1fbcnmFX7e3XAhQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libsdl_image_plugin.dll", @"5udsmSHXF9AaLRWgB711rw==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libsharpen_plugin.dll", @"A6lkUKsvA7e32AG0sxZSdw==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libsimple_channel_mixer_plugin.dll", @"V+HzB2qJw0LjaBpJhL7MnA==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libskins2_plugin.dll", @"yuJaI14xF7zyt0X7PjGY5A==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libsmf_plugin.dll", @"iZgOz+B6HGbyJHPGJwlLDQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libspatializer_plugin.dll", @"4wkhjUxXT5KDcFJj3RMoXg==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libspdif_mixer_plugin.dll", @"b1D+YdgiHkwaW7xyGu+W4g==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libspeex_plugin.dll", @"zCtZWe4f5yPe4My0rkSc6w==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libspudec_plugin.dll", @"7eWynsuXn3T+Mes213TNbQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libstats_plugin.dll", @"OMlpQXlGF23mN3t7cSVt2Q==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libstream_filter_rar_plugin.dll", @"gP0TMpCAkfqZzg5+IiLu7Q==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libstream_filter_record_plugin.dll", @"/frABF0JmVKUDQiEK3p9ug==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libstream_out_autodel_plugin.dll", @"9oGIS93h9N/iBuipCJYMuw==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libstream_out_bridge_plugin.dll", @"KeAeSrDsRhIM3A5oDTTo5w==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libstream_out_description_plugin.dll", @"g9Pd0FKpgKV9hH2pecG8/A==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libstream_out_display_plugin.dll", @"LolCoaP864yUcjexiJXGsQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libstream_out_dummy_plugin.dll", @"ipF51qNSs58GCpxt5XsalA==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libstream_out_duplicate_plugin.dll", @"Um78g/T1xMpLQJkEDoLVgQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libstream_out_es_plugin.dll", @"/diN+duT65g0JIjlv4mlQQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libstream_out_gather_plugin.dll", @"JpLfvde0ukyMqhsxa8Ej5Q==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libstream_out_mosaic_bridge_plugin.dll", @"8fKfpR4jzNTIEA+3c1onZA==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libstream_out_raop_plugin.dll", @"f6ZAcqprAuvgPz8kI2qQVw==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libstream_out_record_plugin.dll", @"FpD8hRm22iOQ2/p/BZy/yg==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libstream_out_rtp_plugin.dll", @"Lhwh9RyLLtxoDhCO3Ga6Qg==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libstream_out_smem_plugin.dll", @"aPQkcYk1L7WnyVxmUKWvfQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libstream_out_standard_plugin.dll", @"Gxn8ZQUbVvvX/N0CXzppng==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libstream_out_transcode_plugin.dll", @"KOmheS/wqcx6egS2x0Vhog==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libsubsdec_plugin.dll", @"KHYbSsCG4BedGPTZruEsXA==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libsubsusf_plugin.dll", @"KKHQa0jNwpa0exZCeq4GVg==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libsubtitle_plugin.dll", @"WiFhIfcNsFmXR5hHaQ/QEw==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libsvcdsub_plugin.dll", @"dJ4cL+hGSA+iDk0gqZc1mQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libswscale_plugin.dll", @"gwAcRGO24bqnW/IqgG1R9A==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libt140_plugin.dll", @"xsC0boNSJzpSwyOPWoto3A==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libtaglib_plugin.dll", @"WZGG1TJmAfsxDBEhT2A9Iw==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libtheora_plugin.dll", @"nzwm9Vwoo0B0uo1BTxUTtA==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libtransform_plugin.dll", @"cexYnqV8KyLv/YT8ENy/9g==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libtrivial_channel_mixer_plugin.dll", @"ZA+JpuV0+FoEP4deXKuW7Q==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libtrivial_mixer_plugin.dll", @"tmt+2C1oIF6umWgBf20c/A==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libts_plugin.dll", @"dxBKHETgoj9CkYsnJ0XO8g==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libtta_plugin.dll", @"dKnhHh15RK3x8oM5oyjfxA==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libtwolame_plugin.dll", @"hSbPD+Y8DdKg1hyun5VFvQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libty_plugin.dll", @"8s1r/aAb2+j87anRhA8EWQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libugly_resampler_plugin.dll", @"/hy39olSEtckA0F6W8TC0g==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libvc1_plugin.dll", @"kZYmE8qP47u81K1EaFP2Kw==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libvcd_plugin.dll", @"oNoCq6d+Fse0hX/D8lfY2g==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libvideo_filter_wrapper_plugin.dll", @"QteLLGOREXD8e8jtXzpT6g==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libvisual_plugin.dll", @"xi8WSlyD1Pum7fxnA3Z+8g==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libvmem_plugin.dll", @"9bzwuMHlnUPr2w6hdc0DMg==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libvobsub_plugin.dll", @"NbREaUT4eDyi/+wihHfZcQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libvoc_plugin.dll", @"urugR8glSNjtK8YXZ9keLQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libvod_rtsp_plugin.dll", @"q7nAzKNI/Q5g/Ef7IdjWLA==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libvorbis_plugin.dll", @"a56mN530pTmRwhgps3uywA==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libvout_sdl_plugin.dll", @"mft6QiUC/i0J7mscdQRpxg==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libvout_wrapper_plugin.dll", @"HrRhL0/a2gEwC4l5HJ+PVA==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libwall_plugin.dll", @"U35/Ke6fgHYqS5d9oyljqw==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libwaveout_plugin.dll", @"eLlyETpbt2S/6TYplm5H1A==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libwave_plugin.dll", @"B/sAq0tx49XU5OlNEvVbDg==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libwav_plugin.dll", @"qYF4gJxMgdLpq+Pdofx9dg==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libwingdi_plugin.dll", @"BBs5xI+MYffcmSLTQ4MzHA==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libx264_plugin.dll", @"lyTIxIPDtbI1IpgPScaUGQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libxa_plugin.dll", @"CVHkNEQ0UxAdNjbvW2Oy+g==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libxml_plugin.dll", @"XvBd0vPdP8sjAkBDtH1oqg==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libxtag_plugin.dll", @"7LrlNa83qyEELk1+HumcGw==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libyuvp_plugin.dll", @"Dr/S+t41OAcn0Tjz8aZhfQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libyuv_plugin.dll", @"mWOen8Nrq7qsDvAnEmsPNQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libyuy2_i420_plugin.dll", @"uQsMdaTkhuaG3pot2ZsVeQ==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libyuy2_i422_plugin.dll", @"JORqXCrXJf+TUcHnt+fCDg==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libzip_plugin.dll", @"g/LZpQpagpTFfdvinmqAEw==");
            dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libzvbi_plugin.dll", @"71/CMzthuz6u3NOUnbbc+A==");
            //
			return dictionary;
		}
	}
}