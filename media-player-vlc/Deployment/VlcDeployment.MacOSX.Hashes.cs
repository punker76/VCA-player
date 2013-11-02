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
		/// Get default package file name
		/// </summary>
		/// <returns>Package file name</returns>
		public static string GetMacOSPackageFileName() {
			return "libvlc-1.1.9-macosx.zip";
		}

		/// <summary>
		/// Hash of last package.
		/// </summary>
		/// <returns>Package hash as base64.</returns>
		/// <remarks>It is not verified at current version.</remarks>
		public static string GetMacOSPackageHash() {
			return ("YaePPyYr4v4RxYtclTrSow==");
		}

		/// <summary>
		/// Returns default location of deployment. This is
		/// location of execution assembly.
		/// </summary>
		/// <returns>Directory where to deploy library.</returns>
		public static string GetMacOSDeploymentLocation() {
			Assembly entry = Assembly.GetEntryAssembly();
			if ( entry == null ) {
				entry = Assembly.GetExecutingAssembly();
			}
			return (GetMacOSDeploymentLocation(entry));
		}

		/// <summary>
		/// Returns default location of deployment. This is
		/// location of execution assembly.
		/// </summary>
		/// <returns>Directory where to deploy library.</returns>
		public static string GetMacOSDeploymentLocation(Assembly relativeTo) {
			return (Path.GetDirectoryName(relativeTo.Location));
		}

		/// <summary>
		/// Get's default package content dictionary.
		/// </summary>
		/// <returns>Dictionary of files from zip package and it's hashes.</returns>
		public static Dictionary<string, string> GetMacOSPackageContents() {
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("AUTHORS.txt", @"gYHmSjBV/useZpxnCJ/zzQ==");
			dictionary.Add("COPYING.txt", @"URD6dSF5un7nhwFhD+qIcw==");
			dictionary.Add("README.txt", @"zkqTeP5wQ3A2Et1VV3B+Fg==");
			dictionary.Add("THANKS.txt", @"SFLeJFuPwiU9gcPBz70UlQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "lib" + Path.DirectorySeparatorChar + "libFLAC.8.dylib", @"8vjJLUaw+ge3ezdECAd98A==");
			dictionary.Add(Path.DirectorySeparatorChar + "lib" + Path.DirectorySeparatorChar + "libSDL-1.3.0.dylib", @"9x+8LvBQRBDjsyrpg4Phlw==");
			dictionary.Add(Path.DirectorySeparatorChar + "lib" + Path.DirectorySeparatorChar + "libSDL_image.0.dylib", @"wmpk3uYuIWCbkcPgydLQBA==");
			dictionary.Add(Path.DirectorySeparatorChar + "lib" + Path.DirectorySeparatorChar + "libcaca.0.dylib", @"BsBZtXV/8P4KzS6GXL4qRQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "lib" + Path.DirectorySeparatorChar + "libdca.0.dylib", @"2Hw1mrH/z6qYawWpJK1sOA==");
			dictionary.Add(Path.DirectorySeparatorChar + "lib" + Path.DirectorySeparatorChar + "libdvbpsi.6.dylib", @"zBOLnJSqi+t6vLqLVoW9+Q==");
			dictionary.Add(Path.DirectorySeparatorChar + "lib" + Path.DirectorySeparatorChar + "libdvdcss.2.dylib", @"7dv+Si7/XP5z1IIYyvws+g==");
			dictionary.Add(Path.DirectorySeparatorChar + "lib" + Path.DirectorySeparatorChar + "libdvdnav.4.dylib", @"73JFxNYl/D6o8VFXHzRpJg==");
			dictionary.Add(Path.DirectorySeparatorChar + "lib" + Path.DirectorySeparatorChar + "libdvdread.4.dylib", @"6n9rTz+YBiACUab32k45WQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "lib" + Path.DirectorySeparatorChar + "libfluidsynth.1.dylib", @"hcCx6g4l+qCAGigdQXZ7/Q==");
			dictionary.Add(Path.DirectorySeparatorChar + "lib" + Path.DirectorySeparatorChar + "libfontconfig.1.dylib", @"HrMscZU1YD7H5vgAfLR9ng==");
			dictionary.Add(Path.DirectorySeparatorChar + "lib" + Path.DirectorySeparatorChar + "libfreetype.6.dylib", @"Vg59X57q8+HAmZsjp/9c2g==");
			dictionary.Add(Path.DirectorySeparatorChar + "lib" + Path.DirectorySeparatorChar + "libfribidi.0.dylib", @"/9R2CaO9SfqX0VINiCAMSw==");
			dictionary.Add(Path.DirectorySeparatorChar + "lib" + Path.DirectorySeparatorChar + "libgcrypt.11.dylib", @"7PJayNln2S8331k+nS27Kg==");
			dictionary.Add(Path.DirectorySeparatorChar + "lib" + Path.DirectorySeparatorChar + "libgnutls.26.dylib", @"S+etClmvC1NsYXmi8ih0pQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "lib" + Path.DirectorySeparatorChar + "libintl.8.dylib", @"2h++/YAmUgVU7k59hswy7A==");
			dictionary.Add(Path.DirectorySeparatorChar + "lib" + Path.DirectorySeparatorChar + "libixml.2.dylib", @"avqlLLmv5X/WMkAntIxW3A==");
			dictionary.Add(Path.DirectorySeparatorChar + "lib" + Path.DirectorySeparatorChar + "libkate.1.dylib", @"YR0i1oCi/vrNVDm94FbD1Q==");
			dictionary.Add(Path.DirectorySeparatorChar + "lib" + Path.DirectorySeparatorChar + "libmad.0.dylib", @"NijrwV2YHgVPoEqtOjLDPw==");
			dictionary.Add(Path.DirectorySeparatorChar + "lib" + Path.DirectorySeparatorChar + "libmpcdec.dylib", @"RFqRsNZVZYARSpX9zHOf/A==");
			dictionary.Add(Path.DirectorySeparatorChar + "lib" + Path.DirectorySeparatorChar + "libmpeg2.0.dylib", @"FO/RJZhUi03b3ekYxtnurg==");
			dictionary.Add(Path.DirectorySeparatorChar + "lib" + Path.DirectorySeparatorChar + "libogg.0.dylib", @"LrNVZ5DgNq257N2CS6OuGQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "lib" + Path.DirectorySeparatorChar + "liborc-0.4.0.dylib", @"6JGvX3IlnwuQ34WAyXfZPw==");
			dictionary.Add(Path.DirectorySeparatorChar + "lib" + Path.DirectorySeparatorChar + "libpng14.14.dylib", @"pcir45uoES8OYSddmQL81A==");
			dictionary.Add(Path.DirectorySeparatorChar + "lib" + Path.DirectorySeparatorChar + "libschroedinger-1.0.0.dylib", @"cRxw1FslUfMdy8rpX4k1SQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "lib" + Path.DirectorySeparatorChar + "libshout.3.dylib", @"Ee0tH3dg1S3nDj/eXxEWcA==");
			dictionary.Add(Path.DirectorySeparatorChar + "lib" + Path.DirectorySeparatorChar + "libspeex.1.dylib", @"fDOolE0dFeY71SM13tbRjw==");
			dictionary.Add(Path.DirectorySeparatorChar + "lib" + Path.DirectorySeparatorChar + "libsqlite3.0.dylib", @"on7szv1B2JEcYvi9Zj7eYw==");
			dictionary.Add(Path.DirectorySeparatorChar + "lib" + Path.DirectorySeparatorChar + "libtheora.0.dylib", @"6dwNvZWZyEPfIhPSBG3Sdw==");
			dictionary.Add(Path.DirectorySeparatorChar + "lib" + Path.DirectorySeparatorChar + "libthreadutil.6.dylib", @"qFC3V0N+sfUh3HtCEz+d4w==");
			dictionary.Add(Path.DirectorySeparatorChar + "lib" + Path.DirectorySeparatorChar + "libtwolame.0.dylib", @"UUVcRa0CCuIo4eG17Q3TXg==");
			dictionary.Add(Path.DirectorySeparatorChar + "lib" + Path.DirectorySeparatorChar + "libupnp.6.dylib", @"Hfbi8bCDZ0b6421SUiW4iA==");
			dictionary.Add(Path.DirectorySeparatorChar + "lib" + Path.DirectorySeparatorChar + "libvlc.5.dylib", @"xgboR+u+eJCLYHUYyTvUIg==");
			dictionary.Add(Path.DirectorySeparatorChar + "lib" + Path.DirectorySeparatorChar + "libvlc.dylib", @"xgboR+u+eJCLYHUYyTvUIg==");
			dictionary.Add(Path.DirectorySeparatorChar + "lib" + Path.DirectorySeparatorChar + "libvlccore.4.dylib", @"uLNra/JRmv0ziuSewUx1DQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "lib" + Path.DirectorySeparatorChar + "libvlccore.dylib", @"uLNra/JRmv0ziuSewUx1DQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "lib" + Path.DirectorySeparatorChar + "libvorbis.0.dylib", @"MamfGb1IOa/fWcBagC1PKg==");
			dictionary.Add(Path.DirectorySeparatorChar + "lib" + Path.DirectorySeparatorChar + "libvorbisenc.2.dylib", @"zcnYai1+LtyzRPiMtBhKpQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "lib" + Path.DirectorySeparatorChar + "libxml2.2.dylib", @"h0qCKUjrk8I1SZw7BwlweQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "lib" + Path.DirectorySeparatorChar + "libzvbi.0.dylib", @"XdTKdF1MSoWWLt+fNXXjug==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + ".DS_Store", @"GUV3p+IL3Mevu3GPUCwTTA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "liba52_plugin.dylib", @"75N55qiCgwoeaWIxVGlXNg==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "liba52tofloat32_plugin.dylib", @"pD6qdoPzfAD6rejrbMQF3w==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "liba52tospdif_plugin.dylib", @"8kHrlP9emulunSuFAoVE9w==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaccess_attachment_plugin.dylib", @"rfeycBLrliUUkD0GfMMeFg==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaccess_avio_plugin.dylib", @"z+fnf3wZ3vEWL6wsJv5f5A==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaccess_bd_plugin.dylib", @"0g0UYBxh5iuuQDgGKHQz4Q==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaccess_eyetv_plugin.dylib", @"bLH6FHhBCoWZMKYv6PMLnQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaccess_fake_plugin.dylib", @"s1r/0RobNg+kQrZkjfuxIQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaccess_ftp_plugin.dylib", @"+I/132CpNfownZv1A8oaNQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaccess_http_plugin.dylib", @"vzlQXXX77I1RXj7qnfkGkw==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaccess_imem_plugin.dylib", @"9ixIm5fDbzQDXiD0SeeCLw==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaccess_mmap_plugin.dylib", @"Qklrjv+hdzLkLQmZhBYT1A==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaccess_mms_plugin.dylib", @"SImw9+sVtREzpBZP7P9wBg==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaccess_output_dummy_plugin.dylib", @"qR2b1O95w/5yZ1dKvXSl7w==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaccess_output_file_plugin.dylib", @"Iy+plDpxM2O6VN/UpMaZRw==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaccess_output_http_plugin.dylib", @"gJcPPawPlIg09w0gme+bxQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaccess_output_shout_plugin.dylib", @"RCBIfTJQATUwJ3GqLqqiVA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaccess_output_udp_plugin.dylib", @"dXwkJ/mop67ZGFIHVl1Kcw==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaccess_realrtsp_plugin.dylib", @"4x9++MddobQQD4+OND6EgA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaccess_tcp_plugin.dylib", @"DT8qpgxZpjk5sCxFKGDu0w==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaccess_udp_plugin.dylib", @"deAykIJLwhfuFAzLnr7zLA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libadjust_plugin.dylib", @"/vAELOuKiOpdPSsemWaULQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libadpcm_plugin.dylib", @"dnMz1RgxfdUBcNvfqdM2uw==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaes3_plugin.dylib", @"ADhi9Qy4WDEmcPJtz3/efg==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaiff_plugin.dylib", @"rXPYdiie5D8DlYglyZrh3w==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libalphamask_plugin.dylib", @"W5194zppgjJ9Mu7wvmfHjw==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaout_file_plugin.dylib", @"idzpHb9Z8OjE2qGiWb2UNA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaraw_plugin.dylib", @"g4/80D/rEH68+ZKDrv0wKg==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libasf_plugin.dylib", @"CY2YMhsty3cqFQKfI9JESQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libau_plugin.dylib", @"UTqsRXT26Z7hq0L4B4F/kA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaudio_format_plugin.dylib", @"lW0AjooiE/zqsNq1CvDyNA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaudiobargraph_a_plugin.dylib", @"dlOzw5ylG3EJO4WN4hpWIw==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaudiobargraph_v_plugin.dylib", @"uSH7TBD4ZHill+wJQw/PlQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libaudioscrobbler_plugin.dylib", @"9jpEZcSRPMtktomMmkGLbw==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libauhal_plugin.dylib", @"nKRF94EuNvY555x/uDw3bw==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libavcodec_plugin.dylib", @"aFvirexan99UQiq4ey0CLQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libavformat_plugin.dylib", @"DrwgimC1uegUwc5SascHzA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libavi_plugin.dylib", @"Yj1cOlvweWorZ1WddbkG6g==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libball_plugin.dylib", @"uqWVFAEqViTf1q635UkWBg==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libblend_plugin.dylib", @"EAwKTghLqXdd1XXRyYsP/A==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libblendbench_plugin.dylib", @"WefPUccLs4mrkP8U1KKUkw==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libbluescreen_plugin.dylib", @"uzlcwxfvt2Zr9NcLoksgqA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libcaca_plugin.dylib", @"VCzl2IKq73c4RfwFMxlNBQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libcanvas_plugin.dylib", @"4CXLSFOro/ZCQPtOych1TQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libcc_plugin.dylib", @"GfS/eIx+cnTZJ55sj6asog==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libcdda_plugin.dylib", @"NHsNhAgj8QzBIf6qTgMF9A==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libcdg_plugin.dylib", @"a2aJo0hHjzBWd8in0l2m8A==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libchain_plugin.dylib", @"uEezjt7CUkNnkf8SXUd0Jw==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libchorus_flanger_plugin.dylib", @"FsPkw2WzflVHR+jhrb1gag==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libclone_plugin.dylib", @"3m+Y3PPSJTlduv9/yhHFyQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libcolorthres_plugin.dylib", @"kR6soxJumWkNfZwtNL0fGA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libconverter_fixed_plugin.dylib", @"RKLvu8/qdVvY6MK9udLysg==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libcrop_plugin.dylib", @"MQ2UN76lSr6kOCStKM1c8g==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libcroppadd_plugin.dylib", @"jA+bU1HX5hjEKEzU0O+BJQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libcvdsub_plugin.dylib", @"Gom9Ac+vqPmj7oeyjr7BdA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libdecomp_plugin.dylib", @"ETEoFhiU5dJO9Lp2nmYSEQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libdeinterlace_plugin.dylib", @"oKPP/KhzAFqHGPJNhmA/3g==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libdemux_cdg_plugin.dylib", @"I1lmy3cyYvz+aWGDTsjVMA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libdemuxdump_plugin.dylib", @"c6Juji/o6D55oR15qhg+cg==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libdirac_plugin.dylib", @"YaiUZh5zoRkSOtFs6DhB2w==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libdolby_surround_decoder_plugin.dylib", @"YGKxuZPRpI09Pz7xHLLR+g==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libdts_plugin.dylib", @"Ge0lN2ds5zJ1JJ8UH6rvHQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libdtstofloat32_plugin.dylib", @"kSYYgyK+c//iCrW5dH1BYA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libdtstospdif_plugin.dylib", @"osADytzrB+zceSPfscjncg==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libdummy_plugin.dylib", @"FRHulUi1JlQpaY/CSqf5YA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libdvbsub_plugin.dylib", @"4WcdgilTQD4BAmb+iwmugQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libdvdnav_plugin.dylib", @"V2xExwWpyQA3vXAXCPCwpQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libdvdread_plugin.dylib", @"tZcUlbSAjFK+0lfXqwLmjA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libdynamicoverlay_plugin.dylib", @"v7gzJX/ECvRswMdKxGGopg==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libequalizer_plugin.dylib", @"1I68oNleJX+lX2wVoeR9DA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "liberase_plugin.dylib", @"7F6F0V7m452GXWrVrRKVjQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libes_plugin.dylib", @"Ht4jhh6TkrG9UT54yYV3lg==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libexport_plugin.dylib", @"jFqlC6ZGP2D2oZ5/9joApQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libextract_plugin.dylib", @"uCQ60myuXQHLsU1WpkrCsA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libfaad_plugin.dylib", @"++4eRvHXeR/wQu4Lh6gUFQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libfake_plugin.dylib", @"Zh1GGypm36y2eLAleaolPQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libfilesystem_plugin.dylib", @"kpnHbieuVBEEY2++q2pJeg==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libflac_plugin.dylib", @"SVcTOKO0rcIKKYbgNob1BA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libflacsys_plugin.dylib", @"2GDY7QyAGUgDNemE2HDxOA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libfloat32_mixer_plugin.dylib", @"NzyVBtX/FgDRQ+erWFU/aw==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libfluidsynth_plugin.dylib", @"gZKk8r/yeTHpg43OoNrPCw==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libfolder_plugin.dylib", @"lfYMGJc/k1z0USbQQRrV1A==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libfreetype_plugin.dylib", @"7TZR17UG5YcLtyxZXMpu7w==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libgaussianblur_plugin.dylib", @"3lkWY8mnqnIbZVK6o1ki3A==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libgestures_plugin.dylib", @"r/hTe/O4FSzPLiCvI/wPeg==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libgnutls_plugin.dylib", @"Lg0tuL1LzI0Bg8Tl0yz6Gw==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libgoom_plugin.dylib", @"r6pzXw9UFfJkD7scUD6Stg==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libgradient_plugin.dylib", @"Rv64UhJhqA+3/zNNDGKKDg==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libgrain_plugin.dylib", @"kgJY/ZkobNc7PNyueHiLhg==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libgrey_yuv_plugin.dylib", @"/zUe2gQ7bN/9u7LM3v3QvQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libgrowl_plugin.dylib", @"CKla2KwL6IkRsBKQgIt9IQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libgrowl_udp_plugin.dylib", @"3d2bsX34mp5bsTZ4ov3vjQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libh264_plugin.dylib", @"B8z+Op+LIFnTAFapDd0lfA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libheadphone_channel_mixer_plugin.dylib", @"CekW144zuFolKEhEa1wHMQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libhotkeys_plugin.dylib", @"oeGIRUZlfqUrshGQO7lk7w==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libi420_rgb_mmx_plugin.dylib", @"lp42qtoulg9buzYjJaZkvQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libi420_rgb_plugin.dylib", @"zGpYuMYMnXhhiQS1KFLtMQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libi420_rgb_sse2_plugin.dylib", @"RJkngfTlAxWz9ZWvrSs0lQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libi420_yuy2_mmx_plugin.dylib", @"kQhfLFwp2npM5wDnnWxwNw==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libi420_yuy2_plugin.dylib", @"CGyIH8QdaqigW+OaIbPDGA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libi420_yuy2_sse2_plugin.dylib", @"Q2/04qF1yWwJzkPeQwtuMA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libi422_i420_plugin.dylib", @"XQEVvF20PyRq2hB4N3at+g==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libi422_yuy2_mmx_plugin.dylib", @"FIzDo5I85sYR1/w33cwM9g==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libi422_yuy2_plugin.dylib", @"TSgEK3aYLO6c5oDQZUVBXg==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libi422_yuy2_sse2_plugin.dylib", @"FkdTCCILrI0zLsljzgTvBg==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libinvert_plugin.dylib", @"CWQBsJx9zC4eA6+YIgYTmQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libinvmem_plugin.dylib", @"7uTkOONnJ8v2QZd7/xXD1g==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libkate_plugin.dylib", @"HfpQrNPnO3c1XZ2kdfb+uw==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "liblibass_plugin.dylib", @"1rYRadsbmVBZw0BC04SooQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "liblibmpeg2_plugin.dylib", @"T9N8HVkqnNIh9NYJzTs52A==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "liblive555_plugin.dylib", @"v8RjQfTUTJ7bpSLPAjp5fQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "liblogger_plugin.dylib", @"TxhHUuvJMsOdcPtBZhWC/w==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "liblogo_plugin.dylib", @"4suO/wIzCadujDidVhARZw==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "liblpcm_plugin.dylib", @"3/QNFN0KjJhUx2AMrki29g==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "liblua_plugin.dylib", @"jQDNX+GAm4YvfFwCAAXMJw==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmacosx_dialog_provider_plugin.dylib", @"tmPoQEtDi9OFEotKb7LY/g==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmacosx_plugin.dylib", @"hlSpQn3TI2LpIsT24ncXNw==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmagnify_plugin.dylib", @"LGbD+U+ZCIFFA8fu/VqLQw==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmarq_plugin.dylib", @"AWPE6wm/eKuWXtNI7w/71g==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmediadirs_plugin.dylib", @"o0qKO4CWLktYhfEdym0esw==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmemcpy3dn_plugin.dylib", @"GzpCF+3xdk/2K4Aa7NMaeA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmemcpymmx_plugin.dylib", @"lOhZhws8pXplpE4kBfB30A==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmemcpymmxext_plugin.dylib", @"IoRnyC4A9BifpIo1rGuRLg==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libminimal_macosx_plugin.dylib", @"451SGuKoUaZLANsNucuw0A==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmirror_plugin.dylib", @"2mi7kgF03ri+W69iUt1mfQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmjpeg_plugin.dylib", @"KBH4QWX09flkNEErktPC8g==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmkv_plugin.dylib", @"JQtlqVtTXbIGrznzICNrwA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmod_plugin.dylib", @"qa2cWHaPQJUBYQYZxD93hg==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmono_plugin.dylib", @"pmw9lpgRnowFQbG7TPAuTA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmosaic_plugin.dylib", @"zaxh8q/a1LlJGdJjcH8Sog==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmotion_plugin.dylib", @"Y4a/xJXsIORKwg3AUXuWRQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmotionblur_plugin.dylib", @"KJq6KDD6ft4y72SJZpcwHA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmotiondetect_plugin.dylib", @"Zt4fjPGR8GFf+j0f7HgXaA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmp4_plugin.dylib", @"NL39IakzF4qZmLO/Yxad7g==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmpc_plugin.dylib", @"PvHabomXK+rwxJxMgwF4gw==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmpeg_audio_plugin.dylib", @"3a0MVybS3Gf6boZRNvW24g==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmpgatofixed32_plugin.dylib", @"5em9vHdBy5AVJsMFa4Z3GQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmpgv_plugin.dylib", @"LCOWtLUu+lU29+qgIBShTQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmux_asf_plugin.dylib", @"36li0hMS+ZFFumHIHtMmKQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmux_avi_plugin.dylib", @"yk84UDKU6A9tLutR8E3I4Q==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmux_dummy_plugin.dylib", @"TUFwz3B55RN1sQpkGLPmrw==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmux_mp4_plugin.dylib", @"oC6BnQYbc0eT5X8P1PAC/w==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmux_mpjpeg_plugin.dylib", @"w7fTo7RA7J9VhXdkh3sjOg==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmux_ogg_plugin.dylib", @"yA1DI0DmCtKd2dFaQcv41A==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmux_ps_plugin.dylib", @"6oZ+1oYH+88IczUhyd1OuA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmux_ts_plugin.dylib", @"H9j/mey6ZT7A7Xlm33Y3pA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libmux_wav_plugin.dylib", @"tyYgY6+D+Fba/DSubvoazw==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libncurses_plugin.dylib", @"qt+OrdQBrI/H5IbU7LPY1g==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libnetsync_plugin.dylib", @"srjQRJKcZS4ultM/ndlFSQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libnoise_plugin.dylib", @"hx3kQ/QkQ1NY0RGVypa60w==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libnormvol_plugin.dylib", @"YHQsE0719T8TFPEvOz2/bw==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libnsc_plugin.dylib", @"5sLctaYLOpB3OxnmqI/m6g==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libnsv_plugin.dylib", @"0QziMFaGHaSWZ9hqlDmVqw==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libnuv_plugin.dylib", @"vCB7A6d6JFyUyUszxBtv8g==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libogg_plugin.dylib", @"AdrZBu86PBIDceDmPmXsgg==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "liboldhttp_plugin.dylib", @"Y7K/mCUKccqxg3/WuayENg==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "liboldrc_plugin.dylib", @"s9UbpqgbE3j9Du4o5Z3VDA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "liboldtelnet_plugin.dylib", @"iQOwcU9Qf4dsCOLnPNUueg==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libopengl_plugin.dylib", @"3rVlZfSZmOYAWQ4xL0/Wsg==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libosd_parser_plugin.dylib", @"WfXx2x0aBN6X8zzagC1gyg==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libosdmenu_plugin.dylib", @"bo4Ldf6imnNj6BgUVbYyhA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libpacketizer_copy_plugin.dylib", @"e5OVqzB4zPAm71FIDNq11g==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libpacketizer_dirac_plugin.dylib", @"wmLF8OGNpbV0kGmutHY6xw==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libpacketizer_flac_plugin.dylib", @"bFlil1yU3/BmYQWj2ShqeA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libpacketizer_h264_plugin.dylib", @"HtLYEPr9+2SPHTAYagX6PQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libpacketizer_mlp_plugin.dylib", @"PYBJteryX8KEIhOdm7ryDg==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libpacketizer_mpeg4audio_plugin.dylib", @"rdT+p/pLuXvBmONC64J8XA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libpacketizer_mpeg4video_plugin.dylib", @"qfu4owLlESwLycKyONhC/g==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libpacketizer_mpegvideo_plugin.dylib", @"hRSBAP4PaJgPt2CnWa6m3w==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libpacketizer_vc1_plugin.dylib", @"RXCbT4ls+l2Bhl1VCMJWLg==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libparam_eq_plugin.dylib", @"IomaWsgbAz6r/OUojR2lvQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libplaylist_plugin.dylib", @"AVY1defa+QH6IoIIyKrCQQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libpng_plugin.dylib", @"PcQOEbPeehi7fcs4M618MQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libpodcast_plugin.dylib", @"wo40TDrh3g9zNhIRcnhNBg==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libpostproc_plugin.dylib", @"G5B8QAV44r+52q4IfFV3uA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libps_plugin.dylib", @"jWSQSdYUSFZ9lVlUidB3Aw==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libpsychedelic_plugin.dylib", @"skdZxSZDvaryR0XbHJ+Umg==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libpuzzle_plugin.dylib", @"qk7DICwj2zdvsWUtMRNMLg==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libpva_plugin.dylib", @"fxeHITZO93WkmX7CsXQ3rQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libqtcapture_plugin.dylib", @"zFEw/YGux4JLLQJ1ODc2OQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libquartztext_plugin.dylib", @"Q1D6lA0iFEXRwnKqTg4leg==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "librawaud_plugin.dylib", @"gfYjeQraANuR5BjJH/cjoA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "librawdv_plugin.dylib", @"x6Rxo8h+3oI4ejn4ChLmdg==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "librawvid_plugin.dylib", @"k3+B3gQwNoxtVYH87Ah/7Q==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "librawvideo_plugin.dylib", @"ImfMfpcVfMLBFAxf/eZzuw==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libreal_plugin.dylib", @"hiZXbu34urOvPDY0lCl2eQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libremoteosd_plugin.dylib", @"gC57uXBD7PVfPxHURwHI3A==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libripple_plugin.dylib", @"M/iowo+9QxWWjF4LhfiG3w==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "librotate_plugin.dylib", @"fD1E9xRcjhPLX6TTpA1n8A==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "librss_plugin.dylib", @"VjJAXad03LjZhC2NAnXZww==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "librtp_plugin.dylib", @"VLM5JXWPw01tr70/dxuVMQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "librv32_plugin.dylib", @"8NQz/qd33rhh2QNIX09VPg==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libsap_plugin.dylib", @"dLAs4NX6OP/epcCFz1ZOfg==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libscale_plugin.dylib", @"u0sv1Ys/swJZ8CQLHjRkPA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libscaletempo_plugin.dylib", @"H+fmws9kj/CneE01hwhOmA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libscene_plugin.dylib", @"kEG1E7PSNvvnLZbJP5rCGA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libschroedinger_plugin.dylib", @"sb6QjBrxGvLIKAzNqj4+rQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libscreen_plugin.dylib", @"R9mIuj0k6mfrJhi5lA4fJA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libsdl_image_plugin.dylib", @"seqU1z47JTrgqpcjMrJuOA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libsharpen_plugin.dylib", @"Q3T0GuBP5YAk1IJT9WSwbw==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libsignals_plugin.dylib", @"k2I46fLycCD+4kg67+gsGw==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libsimple_channel_mixer_plugin.dylib", @"h0cnGzCOkQkplO8nuVJ5Bg==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libsmf_plugin.dylib", @"9zkERWkaGmKgsLgoT9MtbA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libspatializer_plugin.dylib", @"QunwhsYFmDLTAnFgRXOJCg==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libspdif_mixer_plugin.dylib", @"2UIG8vWRAzy92aSc6j42mw==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libspeex_plugin.dylib", @"wTe2oLjuq43+GsWp3GsLJw==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libspudec_plugin.dylib", @"6wnvAPgg34d+IaAeE21FWA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libsqlite_plugin.dylib", @"pHMJqyJ9TyCBE+k9bHz/iA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libstats_plugin.dylib", @"mrrret0lJCYWwz9DhSX4Ww==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libstream_filter_rar_plugin.dylib", @"tbq9Jlqz8j3qLPXwpjqmtQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libstream_filter_record_plugin.dylib", @"IkmYRuQ3sBAdsMU7FZ9z2A==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libstream_out_autodel_plugin.dylib", @"KH26IQ5iNgJGJkwTl376fQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libstream_out_bridge_plugin.dylib", @"GJlBeyCN2k4IJ974I5bZKw==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libstream_out_description_plugin.dylib", @"tGnoStNQfD+tfxhBGlN5AQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libstream_out_display_plugin.dylib", @"kDyUZmb0JKtU9yt4R/AfkQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libstream_out_dummy_plugin.dylib", @"FgADEZ55+657iijNVny/4A==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libstream_out_duplicate_plugin.dylib", @"A8UUlAm8Fic3tvH4rjL54g==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libstream_out_es_plugin.dylib", @"66cDruR4y61OPgMwUbVmVg==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libstream_out_gather_plugin.dylib", @"+uzpNNeE8X+BetxcZTPNoQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libstream_out_mosaic_bridge_plugin.dylib", @"KOulInseR6diztxTINVqaw==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libstream_out_raop_plugin.dylib", @"4pqMyPJzSWitWGcYHQZurA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libstream_out_record_plugin.dylib", @"0RUf8a9r5oxVHIsO9+hJfg==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libstream_out_rtp_plugin.dylib", @"s/LRTWUp4dtBOIPisF5VBw==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libstream_out_smem_plugin.dylib", @"XTC/Xun6mVLd8k08a954eQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libstream_out_standard_plugin.dylib", @"MRNEXlIsnYrbhC1IRZZ2zg==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libstream_out_transcode_plugin.dylib", @"4j9n61c1T5grElC4eOKxIA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libsubsdec_plugin.dylib", @"BBtEJbsq46wrfuY+jZIbhQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libsubsusf_plugin.dylib", @"9J2TKav7DYo+BnbM8yYB7A==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libsubtitle_plugin.dylib", @"9lvTL2wG0VSR2SqgmazCHg==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libsvcdsub_plugin.dylib", @"NVWj2TSp1aRdA7XrhOrASg==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libswscale_plugin.dylib", @"ZAOAWEUyBBfG1VFvXnukSg==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libt140_plugin.dylib", @"8E/gmyjh2Pccj0X2NiUTtA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libtaglib_plugin.dylib", @"vqRn9pKt6LpZINY//cyK4Q==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libtelx_plugin.dylib", @"IqfElK81pQm7/N7n2EN/Iw==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libtheora_plugin.dylib", @"bAHTTJ1EQyorMr7IxHUTVA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libtransform_plugin.dylib", @"8gvI4FLt633RVV6Ejybn8w==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libtrivial_channel_mixer_plugin.dylib", @"IHgTEkEC76QHg+etCXENUw==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libtrivial_mixer_plugin.dylib", @"WfjeA5SHCJ2caIGoNKbSVA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libts_plugin.dylib", @"+DMVnXmRC4SWBIS3gEnsHw==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libtta_plugin.dylib", @"OIxRC2sezN5vG6ekp8a1Aw==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libtwolame_plugin.dylib", @"bYldHuIN7jTKwP87IzQwFA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libty_plugin.dylib", @"1AygVR94ttd5P9fQ/U3iJQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libugly_resampler_plugin.dylib", @"ZJS2mELH65S5dgThA/9Y9A==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libupnp_intel_plugin.dylib", @"RL7We9bZ/3WN+EREN75ivg==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libvc1_plugin.dylib", @"S4fqJ2KN9eF6YHPVxK5BeQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libvcd_plugin.dylib", @"QAci9SavtkR0TfBG8F1Cqw==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libvcdx_plugin.dylib", @"kBlnFHnxnGrR/LFZz83izQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libvideo_filter_wrapper_plugin.dylib", @"5Ln4SJDt51dFF80lFh164Q==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libvisual_plugin.dylib", @"Ky71ndAQUr0V0dMrH2ZzfA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libvmem_plugin.dylib", @"AyclLxedbRb4cH6rZPk3uA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libvobsub_plugin.dylib", @"hPDMnMfG/JN0yiEOJqqFIg==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libvoc_plugin.dylib", @"KgnAgLDnL5PFjpatJYjtZQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libvod_rtsp_plugin.dylib", @"Avz7he68u6EtFJ7xidlwtQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libvorbis_plugin.dylib", @"bBztaDY+IdkMjdORSgRBKQ==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libvout_macosx_plugin.dylib", @"QHkeMKzeNg19ZL48vSVY2A==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libvout_wrapper_plugin.dylib", @"k0LudYuvkJGmOGflaht+pw==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libwall_plugin.dylib", @"hDWp46IYhCW1B7RaOyAQSA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libwav_plugin.dylib", @"2rT00FyHmKZY3c8c32iwWw==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libwave_plugin.dylib", @"Ko4PS4FpusIfQzNyZ6/+3Q==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libx264_plugin.dylib", @"LAdATJ5bmGCUbLlvkaePiA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libxa_plugin.dylib", @"9sCaO9xgrQLcraZX3piNuA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libxml_plugin.dylib", @"lBRRwC6Wq99lJGLB36HtZA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libxtag_plugin.dylib", @"G4zjDQWnjx//TOLJ73pWIg==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libyuv_plugin.dylib", @"EWk8AqnLZHeHC6uNt4uDPg==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libyuvp_plugin.dylib", @"FcGF9ehPG+CLKKdgNJIrNA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libyuy2_i420_plugin.dylib", @"S1pKAwzRhPe6d0AA30hW5Q==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libyuy2_i422_plugin.dylib", @"lNexyrbSxaI8kmsWSIKXyg==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libzip_plugin.dylib", @"fZ4ccXA2ayrQuF4WbzSoCA==");
			dictionary.Add(Path.DirectorySeparatorChar + "plugins" + Path.DirectorySeparatorChar + "libzvbi_plugin.dylib", @"lRauSPD9Hut1O5se2pZl1A==");

			return dictionary;
		}
	}
}