; ModuleID = 'obj\Debug\130\android\marshal_methods.armeabi-v7a.ll'
source_filename = "obj\Debug\130\android\marshal_methods.armeabi-v7a.ll"
target datalayout = "e-m:e-p:32:32-Fi8-i64:64-v128:64:128-a:0:32-n32-S64"
target triple = "armv7-unknown-linux-android"


%struct.MonoImage = type opaque

%struct.MonoClass = type opaque

%struct.MarshalMethodsManagedClass = type {
	i32,; uint32_t token
	%struct.MonoClass*; MonoClass* klass
}

%struct.MarshalMethodName = type {
	i64,; uint64_t id
	i8*; char* name
}

%class._JNIEnv = type opaque

%class._jobject = type {
	i8; uint8_t b
}

%class._jclass = type {
	i8; uint8_t b
}

%class._jstring = type {
	i8; uint8_t b
}

%class._jthrowable = type {
	i8; uint8_t b
}

%class._jarray = type {
	i8; uint8_t b
}

%class._jobjectArray = type {
	i8; uint8_t b
}

%class._jbooleanArray = type {
	i8; uint8_t b
}

%class._jbyteArray = type {
	i8; uint8_t b
}

%class._jcharArray = type {
	i8; uint8_t b
}

%class._jshortArray = type {
	i8; uint8_t b
}

%class._jintArray = type {
	i8; uint8_t b
}

%class._jlongArray = type {
	i8; uint8_t b
}

%class._jfloatArray = type {
	i8; uint8_t b
}

%class._jdoubleArray = type {
	i8; uint8_t b
}

; assembly_image_cache
@assembly_image_cache = local_unnamed_addr global [0 x %struct.MonoImage*] zeroinitializer, align 4
; Each entry maps hash of an assembly name to an index into the `assembly_image_cache` array
@assembly_image_cache_hashes = local_unnamed_addr constant [208 x i32] [
	i32 32687329, ; 0: Xamarin.AndroidX.Lifecycle.Runtime => 0x1f2c4e1 => 62
	i32 34715100, ; 1: Xamarin.Google.Guava.ListenableFuture.dll => 0x211b5dc => 91
	i32 39109920, ; 2: Newtonsoft.Json.dll => 0x254c520 => 18
	i32 57263871, ; 3: Xamarin.Forms.Core.dll => 0x369c6ff => 86
	i32 101534019, ; 4: Xamarin.AndroidX.SlidingPaneLayout => 0x60d4943 => 76
	i32 120558881, ; 5: Xamarin.AndroidX.SlidingPaneLayout.dll => 0x72f9521 => 76
	i32 165246403, ; 6: Xamarin.AndroidX.Collection.dll => 0x9d975c3 => 43
	i32 182336117, ; 7: Xamarin.AndroidX.SwipeRefreshLayout.dll => 0xade3a75 => 77
	i32 209399409, ; 8: Xamarin.AndroidX.Browser.dll => 0xc7b2e71 => 41
	i32 230216969, ; 9: Xamarin.AndroidX.Legacy.Support.Core.Utils.dll => 0xdb8d509 => 57
	i32 230752869, ; 10: Microsoft.CSharp.dll => 0xdc10265 => 10
	i32 232815796, ; 11: System.Web.Services => 0xde07cb4 => 99
	i32 261689757, ; 12: Xamarin.AndroidX.ConstraintLayout.dll => 0xf99119d => 46
	i32 278686392, ; 13: Xamarin.AndroidX.Lifecycle.LiveData.dll => 0x109c6ab8 => 61
	i32 280482487, ; 14: Xamarin.AndroidX.Interpolator => 0x10b7d2b7 => 55
	i32 318968648, ; 15: Xamarin.AndroidX.Activity.dll => 0x13031348 => 33
	i32 321597661, ; 16: System.Numerics => 0x132b30dd => 25
	i32 342366114, ; 17: Xamarin.AndroidX.Lifecycle.Common => 0x146817a2 => 59
	i32 382771021, ; 18: Microsoft.Bcl.Memory.dll => 0x16d09f4d => 8
	i32 385762202, ; 19: System.Memory.dll => 0x16fe439a => 24
	i32 441335492, ; 20: Xamarin.AndroidX.ConstraintLayout.Core => 0x1a4e3ec4 => 45
	i32 442521989, ; 21: Xamarin.Essentials => 0x1a605985 => 85
	i32 450948140, ; 22: Xamarin.AndroidX.Fragment.dll => 0x1ae0ec2c => 54
	i32 465846621, ; 23: mscorlib => 0x1bc4415d => 17
	i32 469710990, ; 24: System.dll => 0x1bff388e => 22
	i32 476646585, ; 25: Xamarin.AndroidX.Interpolator.dll => 0x1c690cb9 => 55
	i32 485463106, ; 26: Microsoft.IdentityModel.Abstractions => 0x1cef9442 => 12
	i32 486930444, ; 27: Xamarin.AndroidX.LocalBroadcastManager.dll => 0x1d05f80c => 66
	i32 526420162, ; 28: System.Transactions.dll => 0x1f6088c2 => 93
	i32 548916678, ; 29: Microsoft.Bcl.AsyncInterfaces => 0x20b7cdc6 => 7
	i32 577335427, ; 30: System.Security.Cryptography.Cng => 0x22697083 => 102
	i32 605376203, ; 31: System.IO.Compression.FileSystem => 0x24154ecb => 97
	i32 627609679, ; 32: Xamarin.AndroidX.CustomView => 0x2568904f => 50
	i32 662205335, ; 33: System.Text.Encodings.Web.dll => 0x27787397 => 29
	i32 663517072, ; 34: Xamarin.AndroidX.VersionedParcelable => 0x278c7790 => 82
	i32 666292255, ; 35: Xamarin.AndroidX.Arch.Core.Common.dll => 0x27b6d01f => 38
	i32 690569205, ; 36: System.Xml.Linq.dll => 0x29293ff5 => 32
	i32 775507847, ; 37: System.IO.Compression => 0x2e394f87 => 96
	i32 809851609, ; 38: System.Drawing.Common.dll => 0x30455ad9 => 95
	i32 843511501, ; 39: Xamarin.AndroidX.Print => 0x3246f6cd => 73
	i32 928116545, ; 40: Xamarin.Google.Guava.ListenableFuture => 0x3751ef41 => 91
	i32 955402788, ; 41: Newtonsoft.Json => 0x38f24a24 => 18
	i32 967690846, ; 42: Xamarin.AndroidX.Lifecycle.Common.dll => 0x39adca5e => 59
	i32 974778368, ; 43: FormsViewGroup.dll => 0x3a19f000 => 5
	i32 1012816738, ; 44: Xamarin.AndroidX.SavedState.dll => 0x3c5e5b62 => 75
	i32 1035644815, ; 45: Xamarin.AndroidX.AppCompat => 0x3dbaaf8f => 37
	i32 1042160112, ; 46: Xamarin.Forms.Platform.dll => 0x3e1e19f0 => 88
	i32 1052210849, ; 47: Xamarin.AndroidX.Lifecycle.ViewModel.dll => 0x3eb776a1 => 63
	i32 1098259244, ; 48: System => 0x41761b2c => 22
	i32 1162065116, ; 49: Microsoft.Bcl.Memory => 0x4543b4dc => 8
	i32 1175144683, ; 50: Xamarin.AndroidX.VectorDrawable.Animated => 0x460b48eb => 80
	i32 1178241025, ; 51: Xamarin.AndroidX.Navigation.Runtime.dll => 0x463a8801 => 70
	i32 1204270330, ; 52: Xamarin.AndroidX.Arch.Core.Common => 0x47c7b4fa => 38
	i32 1267360935, ; 53: Xamarin.AndroidX.VectorDrawable => 0x4b8a64a7 => 81
	i32 1293217323, ; 54: Xamarin.AndroidX.DrawerLayout.dll => 0x4d14ee2b => 52
	i32 1365406463, ; 55: System.ServiceModel.Internals.dll => 0x516272ff => 100
	i32 1376866003, ; 56: Xamarin.AndroidX.SavedState => 0x52114ed3 => 75
	i32 1395857551, ; 57: Xamarin.AndroidX.Media.dll => 0x5333188f => 67
	i32 1406073936, ; 58: Xamarin.AndroidX.CoordinatorLayout => 0x53cefc50 => 47
	i32 1411638395, ; 59: System.Runtime.CompilerServices.Unsafe => 0x5423e47b => 27
	i32 1460219004, ; 60: Xamarin.Forms.Xaml => 0x57092c7c => 89
	i32 1460893475, ; 61: System.IdentityModel.Tokens.Jwt => 0x57137723 => 23
	i32 1462112819, ; 62: System.IO.Compression.dll => 0x57261233 => 96
	i32 1469204771, ; 63: Xamarin.AndroidX.AppCompat.AppCompatResources => 0x57924923 => 36
	i32 1498168481, ; 64: Microsoft.IdentityModel.JsonWebTokens.dll => 0x594c3ca1 => 13
	i32 1582372066, ; 65: Xamarin.AndroidX.DocumentFile.dll => 0x5e5114e2 => 51
	i32 1592978981, ; 66: System.Runtime.Serialization.dll => 0x5ef2ee25 => 3
	i32 1622152042, ; 67: Xamarin.AndroidX.Loader.dll => 0x60b0136a => 65
	i32 1624863272, ; 68: Xamarin.AndroidX.ViewPager2 => 0x60d97228 => 84
	i32 1636350590, ; 69: Xamarin.AndroidX.CursorAdapter => 0x6188ba7e => 49
	i32 1639515021, ; 70: System.Net.Http.dll => 0x61b9038d => 2
	i32 1657153582, ; 71: System.Runtime => 0x62c6282e => 28
	i32 1658241508, ; 72: Xamarin.AndroidX.Tracing.Tracing.dll => 0x62d6c1e4 => 78
	i32 1658251792, ; 73: Xamarin.Google.Android.Material.dll => 0x62d6ea10 => 90
	i32 1670060433, ; 74: Xamarin.AndroidX.ConstraintLayout => 0x638b1991 => 46
	i32 1729485958, ; 75: Xamarin.AndroidX.CardView.dll => 0x6715dc86 => 42
	i32 1766324549, ; 76: Xamarin.AndroidX.SwipeRefreshLayout => 0x6947f945 => 77
	i32 1776026572, ; 77: System.Core.dll => 0x69dc03cc => 20
	i32 1788241197, ; 78: Xamarin.AndroidX.Fragment => 0x6a96652d => 54
	i32 1796167890, ; 79: Microsoft.Bcl.AsyncInterfaces.dll => 0x6b0f58d2 => 7
	i32 1808609942, ; 80: Xamarin.AndroidX.Loader => 0x6bcd3296 => 65
	i32 1813201214, ; 81: Xamarin.Google.Android.Material => 0x6c13413e => 90
	i32 1818569960, ; 82: Xamarin.AndroidX.Navigation.UI.dll => 0x6c652ce8 => 71
	i32 1828688058, ; 83: Microsoft.Extensions.Logging.Abstractions.dll => 0x6cff90ba => 11
	i32 1867746548, ; 84: Xamarin.Essentials.dll => 0x6f538cf4 => 85
	i32 1878053835, ; 85: Xamarin.Forms.Xaml.dll => 0x6ff0d3cb => 89
	i32 1885316902, ; 86: Xamarin.AndroidX.Arch.Core.Runtime.dll => 0x705fa726 => 39
	i32 1919157823, ; 87: Xamarin.AndroidX.MultiDex.dll => 0x7264063f => 68
	i32 1932718519, ; 88: Microsoft.Bcl.TimeProvider => 0x7332f1b7 => 9
	i32 1986222447, ; 89: Microsoft.IdentityModel.Tokens.dll => 0x7663596f => 15
	i32 2011961780, ; 90: System.Buffers.dll => 0x77ec19b4 => 19
	i32 2019465201, ; 91: Xamarin.AndroidX.Lifecycle.ViewModel => 0x785e97f1 => 63
	i32 2055257422, ; 92: Xamarin.AndroidX.Lifecycle.LiveData.Core.dll => 0x7a80bd4e => 60
	i32 2079903147, ; 93: System.Runtime.dll => 0x7bf8cdab => 28
	i32 2090596640, ; 94: System.Numerics.Vectors => 0x7c9bf920 => 26
	i32 2097448633, ; 95: Xamarin.AndroidX.Legacy.Support.Core.UI => 0x7d0486b9 => 56
	i32 2126786730, ; 96: Xamarin.Forms.Platform.Android => 0x7ec430aa => 87
	i32 2192057212, ; 97: Microsoft.Extensions.Logging.Abstractions => 0x82a8237c => 11
	i32 2195490522, ; 98: FlashCardMobileApp.dll => 0x82dc86da => 4
	i32 2201231467, ; 99: System.Net.Http => 0x8334206b => 2
	i32 2217644978, ; 100: Xamarin.AndroidX.VectorDrawable.Animated.dll => 0x842e93b2 => 80
	i32 2244775296, ; 101: Xamarin.AndroidX.LocalBroadcastManager => 0x85cc8d80 => 66
	i32 2256548716, ; 102: Xamarin.AndroidX.MultiDex => 0x8680336c => 68
	i32 2261435625, ; 103: Xamarin.AndroidX.Legacy.Support.V4.dll => 0x86cac4e9 => 58
	i32 2279755925, ; 104: Xamarin.AndroidX.RecyclerView.dll => 0x87e25095 => 74
	i32 2315684594, ; 105: Xamarin.AndroidX.Annotation.dll => 0x8a068af2 => 34
	i32 2369706906, ; 106: Microsoft.IdentityModel.Logging => 0x8d3edb9a => 14
	i32 2409053734, ; 107: Xamarin.AndroidX.Preference.dll => 0x8f973e26 => 72
	i32 2465532216, ; 108: Xamarin.AndroidX.ConstraintLayout.Core.dll => 0x92f50938 => 45
	i32 2471841756, ; 109: netstandard.dll => 0x93554fdc => 1
	i32 2475788418, ; 110: Java.Interop.dll => 0x93918882 => 6
	i32 2501346920, ; 111: System.Data.DataSetExtensions => 0x95178668 => 94
	i32 2505896520, ; 112: Xamarin.AndroidX.Lifecycle.Runtime.dll => 0x955cf248 => 62
	i32 2562349572, ; 113: Microsoft.CSharp => 0x98ba5a04 => 10
	i32 2570120770, ; 114: System.Text.Encodings.Web => 0x9930ee42 => 29
	i32 2581819634, ; 115: Xamarin.AndroidX.VectorDrawable.dll => 0x99e370f2 => 81
	i32 2620871830, ; 116: Xamarin.AndroidX.CursorAdapter.dll => 0x9c375496 => 49
	i32 2624644809, ; 117: Xamarin.AndroidX.DynamicAnimation => 0x9c70e6c9 => 53
	i32 2633051222, ; 118: Xamarin.AndroidX.Lifecycle.LiveData => 0x9cf12c56 => 61
	i32 2640290731, ; 119: Microsoft.IdentityModel.Logging.dll => 0x9d5fa3ab => 14
	i32 2701096212, ; 120: Xamarin.AndroidX.Tracing.Tracing => 0xa0ff7514 => 78
	i32 2719963679, ; 121: System.Security.Cryptography.Cng.dll => 0xa21f5a1f => 102
	i32 2732626843, ; 122: Xamarin.AndroidX.Activity => 0xa2e0939b => 33
	i32 2737747696, ; 123: Xamarin.AndroidX.AppCompat.AppCompatResources.dll => 0xa32eb6f0 => 36
	i32 2766581644, ; 124: Xamarin.Forms.Core => 0xa4e6af8c => 86
	i32 2778768386, ; 125: Xamarin.AndroidX.ViewPager.dll => 0xa5a0a402 => 83
	i32 2810250172, ; 126: Xamarin.AndroidX.CoordinatorLayout.dll => 0xa78103bc => 47
	i32 2819470561, ; 127: System.Xml.dll => 0xa80db4e1 => 31
	i32 2853208004, ; 128: Xamarin.AndroidX.ViewPager => 0xaa107fc4 => 83
	i32 2855708567, ; 129: Xamarin.AndroidX.Transition => 0xaa36a797 => 79
	i32 2903344695, ; 130: System.ComponentModel.Composition => 0xad0d8637 => 98
	i32 2905242038, ; 131: mscorlib.dll => 0xad2a79b6 => 17
	i32 2916838712, ; 132: Xamarin.AndroidX.ViewPager2.dll => 0xaddb6d38 => 84
	i32 2919462931, ; 133: System.Numerics.Vectors.dll => 0xae037813 => 26
	i32 2921128767, ; 134: Xamarin.AndroidX.Annotation.Experimental.dll => 0xae1ce33f => 35
	i32 2978675010, ; 135: Xamarin.AndroidX.DrawerLayout => 0xb18af942 => 52
	i32 3024354802, ; 136: Xamarin.AndroidX.Legacy.Support.Core.Utils => 0xb443fdf2 => 57
	i32 3044182254, ; 137: FormsViewGroup => 0xb57288ee => 5
	i32 3046403891, ; 138: FlashCardMobileApp => 0xb5946f33 => 4
	i32 3057625584, ; 139: Xamarin.AndroidX.Navigation.Common => 0xb63fa9f0 => 69
	i32 3084678329, ; 140: Microsoft.IdentityModel.Tokens => 0xb7dc74b9 => 15
	i32 3111772706, ; 141: System.Runtime.Serialization => 0xb979e222 => 3
	i32 3124832203, ; 142: System.Threading.Tasks.Extensions => 0xba4127cb => 101
	i32 3204380047, ; 143: System.Data.dll => 0xbefef58f => 92
	i32 3211777861, ; 144: Xamarin.AndroidX.DocumentFile => 0xbf6fd745 => 51
	i32 3247949154, ; 145: Mono.Security => 0xc197c562 => 103
	i32 3258312781, ; 146: Xamarin.AndroidX.CardView => 0xc235e84d => 42
	i32 3265893370, ; 147: System.Threading.Tasks.Extensions.dll => 0xc2a993fa => 101
	i32 3267021929, ; 148: Xamarin.AndroidX.AsyncLayoutInflater => 0xc2bacc69 => 40
	i32 3312457198, ; 149: Microsoft.IdentityModel.JsonWebTokens => 0xc57015ee => 13
	i32 3317135071, ; 150: Xamarin.AndroidX.CustomView.dll => 0xc5b776df => 50
	i32 3317144872, ; 151: System.Data => 0xc5b79d28 => 92
	i32 3340431453, ; 152: Xamarin.AndroidX.Arch.Core.Runtime => 0xc71af05d => 39
	i32 3346324047, ; 153: Xamarin.AndroidX.Navigation.Runtime => 0xc774da4f => 70
	i32 3353484488, ; 154: Xamarin.AndroidX.Legacy.Support.Core.UI.dll => 0xc7e21cc8 => 56
	i32 3358260929, ; 155: System.Text.Json => 0xc82afec1 => 30
	i32 3362522851, ; 156: Xamarin.AndroidX.Core => 0xc86c06e3 => 48
	i32 3366347497, ; 157: Java.Interop => 0xc8a662e9 => 6
	i32 3374999561, ; 158: Xamarin.AndroidX.RecyclerView => 0xc92a6809 => 74
	i32 3395150330, ; 159: System.Runtime.CompilerServices.Unsafe.dll => 0xca5de1fa => 27
	i32 3404865022, ; 160: System.ServiceModel.Internals => 0xcaf21dfe => 100
	i32 3428672310, ; 161: FlashCardMobileApp.Android.dll => 0xcc5d6336 => 0
	i32 3429136800, ; 162: System.Xml => 0xcc6479a0 => 31
	i32 3430777524, ; 163: netstandard => 0xcc7d82b4 => 1
	i32 3441283291, ; 164: Xamarin.AndroidX.DynamicAnimation.dll => 0xcd1dd0db => 53
	i32 3442995112, ; 165: FlashCardMobileApp.Android => 0xcd37efa8 => 0
	i32 3476120550, ; 166: Mono.Android => 0xcf3163e6 => 16
	i32 3485117614, ; 167: System.Text.Json.dll => 0xcfbaacae => 30
	i32 3486566296, ; 168: System.Transactions => 0xcfd0c798 => 93
	i32 3493954962, ; 169: Xamarin.AndroidX.Concurrent.Futures.dll => 0xd0418592 => 44
	i32 3501239056, ; 170: Xamarin.AndroidX.AsyncLayoutInflater.dll => 0xd0b0ab10 => 40
	i32 3509114376, ; 171: System.Xml.Linq => 0xd128d608 => 32
	i32 3536029504, ; 172: Xamarin.Forms.Platform.Android.dll => 0xd2c38740 => 87
	i32 3567349600, ; 173: System.ComponentModel.Composition.dll => 0xd4a16f60 => 98
	i32 3618140916, ; 174: Xamarin.AndroidX.Preference => 0xd7a872f4 => 72
	i32 3627220390, ; 175: Xamarin.AndroidX.Print.dll => 0xd832fda6 => 73
	i32 3632359727, ; 176: Xamarin.Forms.Platform => 0xd881692f => 88
	i32 3633644679, ; 177: Xamarin.AndroidX.Annotation.Experimental => 0xd8950487 => 35
	i32 3641597786, ; 178: Xamarin.AndroidX.Lifecycle.LiveData.Core => 0xd90e5f5a => 60
	i32 3672681054, ; 179: Mono.Android.dll => 0xdae8aa5e => 16
	i32 3676310014, ; 180: System.Web.Services.dll => 0xdb2009fe => 99
	i32 3682565725, ; 181: Xamarin.AndroidX.Browser => 0xdb7f7e5d => 41
	i32 3684561358, ; 182: Xamarin.AndroidX.Concurrent.Futures => 0xdb9df1ce => 44
	i32 3689375977, ; 183: System.Drawing.Common => 0xdbe768e9 => 95
	i32 3700591436, ; 184: Microsoft.IdentityModel.Abstractions.dll => 0xdc928b4c => 12
	i32 3718780102, ; 185: Xamarin.AndroidX.Annotation => 0xdda814c6 => 34
	i32 3724971120, ; 186: Xamarin.AndroidX.Navigation.Common.dll => 0xde068c70 => 69
	i32 3748608112, ; 187: System.Diagnostics.DiagnosticSource => 0xdf6f3870 => 21
	i32 3758932259, ; 188: Xamarin.AndroidX.Legacy.Support.V4 => 0xe00cc123 => 58
	i32 3786282454, ; 189: Xamarin.AndroidX.Collection => 0xe1ae15d6 => 43
	i32 3822602673, ; 190: Xamarin.AndroidX.Media => 0xe3d849b1 => 67
	i32 3829621856, ; 191: System.Numerics.dll => 0xe4436460 => 25
	i32 3885922214, ; 192: Xamarin.AndroidX.Transition.dll => 0xe79e77a6 => 79
	i32 3896760992, ; 193: Xamarin.AndroidX.Core.dll => 0xe843daa0 => 48
	i32 3920810846, ; 194: System.IO.Compression.FileSystem.dll => 0xe9b2d35e => 97
	i32 3921031405, ; 195: Xamarin.AndroidX.VersionedParcelable.dll => 0xe9b630ed => 82
	i32 3931092270, ; 196: Xamarin.AndroidX.Navigation.UI => 0xea4fb52e => 71
	i32 3945713374, ; 197: System.Data.DataSetExtensions.dll => 0xeb2ecede => 94
	i32 3955647286, ; 198: Xamarin.AndroidX.AppCompat.dll => 0xebc66336 => 37
	i32 4017318820, ; 199: Microsoft.Bcl.TimeProvider.dll => 0xef736ba4 => 9
	i32 4025784931, ; 200: System.Memory => 0xeff49a63 => 24
	i32 4105002889, ; 201: Mono.Security.dll => 0xf4ad5f89 => 103
	i32 4151237749, ; 202: System.Core => 0xf76edc75 => 20
	i32 4182413190, ; 203: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll => 0xf94a8f86 => 64
	i32 4213026141, ; 204: System.Diagnostics.DiagnosticSource.dll => 0xfb1dad5d => 21
	i32 4260525087, ; 205: System.Buffers => 0xfdf2741f => 19
	i32 4263231520, ; 206: System.IdentityModel.Tokens.Jwt.dll => 0xfe1bc020 => 23
	i32 4292120959 ; 207: Xamarin.AndroidX.Lifecycle.ViewModelSavedState => 0xffd4917f => 64
], align 4
@assembly_image_cache_indices = local_unnamed_addr constant [208 x i32] [
	i32 62, i32 91, i32 18, i32 86, i32 76, i32 76, i32 43, i32 77, ; 0..7
	i32 41, i32 57, i32 10, i32 99, i32 46, i32 61, i32 55, i32 33, ; 8..15
	i32 25, i32 59, i32 8, i32 24, i32 45, i32 85, i32 54, i32 17, ; 16..23
	i32 22, i32 55, i32 12, i32 66, i32 93, i32 7, i32 102, i32 97, ; 24..31
	i32 50, i32 29, i32 82, i32 38, i32 32, i32 96, i32 95, i32 73, ; 32..39
	i32 91, i32 18, i32 59, i32 5, i32 75, i32 37, i32 88, i32 63, ; 40..47
	i32 22, i32 8, i32 80, i32 70, i32 38, i32 81, i32 52, i32 100, ; 48..55
	i32 75, i32 67, i32 47, i32 27, i32 89, i32 23, i32 96, i32 36, ; 56..63
	i32 13, i32 51, i32 3, i32 65, i32 84, i32 49, i32 2, i32 28, ; 64..71
	i32 78, i32 90, i32 46, i32 42, i32 77, i32 20, i32 54, i32 7, ; 72..79
	i32 65, i32 90, i32 71, i32 11, i32 85, i32 89, i32 39, i32 68, ; 80..87
	i32 9, i32 15, i32 19, i32 63, i32 60, i32 28, i32 26, i32 56, ; 88..95
	i32 87, i32 11, i32 4, i32 2, i32 80, i32 66, i32 68, i32 58, ; 96..103
	i32 74, i32 34, i32 14, i32 72, i32 45, i32 1, i32 6, i32 94, ; 104..111
	i32 62, i32 10, i32 29, i32 81, i32 49, i32 53, i32 61, i32 14, ; 112..119
	i32 78, i32 102, i32 33, i32 36, i32 86, i32 83, i32 47, i32 31, ; 120..127
	i32 83, i32 79, i32 98, i32 17, i32 84, i32 26, i32 35, i32 52, ; 128..135
	i32 57, i32 5, i32 4, i32 69, i32 15, i32 3, i32 101, i32 92, ; 136..143
	i32 51, i32 103, i32 42, i32 101, i32 40, i32 13, i32 50, i32 92, ; 144..151
	i32 39, i32 70, i32 56, i32 30, i32 48, i32 6, i32 74, i32 27, ; 152..159
	i32 100, i32 0, i32 31, i32 1, i32 53, i32 0, i32 16, i32 30, ; 160..167
	i32 93, i32 44, i32 40, i32 32, i32 87, i32 98, i32 72, i32 73, ; 168..175
	i32 88, i32 35, i32 60, i32 16, i32 99, i32 41, i32 44, i32 95, ; 176..183
	i32 12, i32 34, i32 69, i32 21, i32 58, i32 43, i32 67, i32 25, ; 184..191
	i32 79, i32 48, i32 97, i32 82, i32 71, i32 94, i32 37, i32 9, ; 192..199
	i32 24, i32 103, i32 20, i32 64, i32 21, i32 19, i32 23, i32 64 ; 208..207
], align 4

@marshal_methods_number_of_classes = local_unnamed_addr constant i32 0, align 4

; marshal_methods_class_cache
@marshal_methods_class_cache = global [0 x %struct.MarshalMethodsManagedClass] [
], align 4; end of 'marshal_methods_class_cache' array


@get_function_pointer = internal unnamed_addr global void (i32, i32, i32, i8**)* null, align 4

; Function attributes: "frame-pointer"="all" "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind sspstrong "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+armv7-a,+d32,+dsp,+fp64,+neon,+thumb-mode,+vfp2,+vfp2sp,+vfp3,+vfp3d16,+vfp3d16sp,+vfp3sp,-aes,-fp-armv8,-fp-armv8d16,-fp-armv8d16sp,-fp-armv8sp,-fp16,-fp16fml,-fullfp16,-sha2,-vfp4,-vfp4d16,-vfp4d16sp,-vfp4sp" uwtable willreturn writeonly
define void @xamarin_app_init (void (i32, i32, i32, i8**)* %fn) local_unnamed_addr #0
{
	store void (i32, i32, i32, i8**)* %fn, void (i32, i32, i32, i8**)** @get_function_pointer, align 4
	ret void
}

; Names of classes in which marshal methods reside
@mm_class_names = local_unnamed_addr constant [0 x i8*] zeroinitializer, align 4
@__MarshalMethodName_name.0 = internal constant [1 x i8] c"\00", align 1

; mm_method_names
@mm_method_names = local_unnamed_addr constant [1 x %struct.MarshalMethodName] [
	; 0
	%struct.MarshalMethodName {
		i64 0, ; id 0x0; name: 
		i8* getelementptr inbounds ([1 x i8], [1 x i8]* @__MarshalMethodName_name.0, i32 0, i32 0); name
	}
], align 8; end of 'mm_method_names' array


attributes #0 = { "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind sspstrong "stack-protector-buffer-size"="8" uwtable willreturn writeonly "frame-pointer"="all" "target-cpu"="generic" "target-features"="+armv7-a,+d32,+dsp,+fp64,+neon,+thumb-mode,+vfp2,+vfp2sp,+vfp3,+vfp3d16,+vfp3d16sp,+vfp3sp,-aes,-fp-armv8,-fp-armv8d16,-fp-armv8d16sp,-fp-armv8sp,-fp16,-fp16fml,-fullfp16,-sha2,-vfp4,-vfp4d16,-vfp4d16sp,-vfp4sp" }
attributes #1 = { "min-legal-vector-width"="0" mustprogress "no-trapping-math"="true" nounwind sspstrong "stack-protector-buffer-size"="8" uwtable "frame-pointer"="all" "target-cpu"="generic" "target-features"="+armv7-a,+d32,+dsp,+fp64,+neon,+thumb-mode,+vfp2,+vfp2sp,+vfp3,+vfp3d16,+vfp3d16sp,+vfp3sp,-aes,-fp-armv8,-fp-armv8d16,-fp-armv8d16sp,-fp-armv8sp,-fp16,-fp16fml,-fullfp16,-sha2,-vfp4,-vfp4d16,-vfp4d16sp,-vfp4sp" }
attributes #2 = { nounwind }

!llvm.module.flags = !{!0, !1, !2}
!llvm.ident = !{!3}
!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!2 = !{i32 1, !"min_enum_size", i32 4}
!3 = !{!"Xamarin.Android remotes/origin/d17-5 @ 45b0e144f73b2c8747d8b5ec8cbd3b55beca67f0"}
!llvm.linker.options = !{}
