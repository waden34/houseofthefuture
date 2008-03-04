/*
 * Copyright (C) 2008 Jeremiah Johnson
 * Taken verbatim from USB Wireless Security http://www.sourceforge.net/projects/usbwisec
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
*/

using System;
using System.Runtime.InteropServices;
//using System.Windows.Forms;

namespace HouseOfTheFuture
{
	public class USBSharp : IDisposable
	{
		bool disposed = false;

		//
		//
		// Required constants, pointers, handles and variables 
		public int HidHandle =						-1;				// file handle for a Hid devices
		public int hDevInfo =						-1;				// handle for the device infoset
		public string DevicePathName = "";
		public const int  DIGCF_PRESENT				= 0x00000002;
		public const int  DIGCF_DEVICEINTERFACE		= 0x00000010;
		public const int  DIGCF_INTERFACEDEVICE		= 0x00000010;
		public const uint GENERIC_READ				= 0x80000000;
		public const uint GENERIC_WRITE				= 0x40000000;
		public const uint FILE_SHARE_READ			= 0x00000001;
		public const uint FILE_SHARE_WRITE			= 0x00000002;
		public const int  OPEN_EXISTING				= 3;
		public const int  EV_RXFLAG = 0x0002;    // received certain character
		


		// This are the NTSTATUS codes if we want to retrieve and handle our return codes&errors
		/*
		 * 
		 * #define HIDP_ERROR_CODES(SEV, CODE) \
        ((NTSTATUS) (((SEV) << 28) | (FACILITY_HID_ERROR_CODE << 16) | (CODE)))
		 * #define HIDP_STATUS_SUCCESS                  (HIDP_ERROR_CODES(0x0,0))
		#define HIDP_STATUS_NULL                     (HIDP_ERROR_CODES(0x8,1))
		#define HIDP_STATUS_INVALID_PREPARSED_DATA   (HIDP_ERROR_CODES(0xC,1))
		#define HIDP_STATUS_INVALID_REPORT_TYPE      (HIDP_ERROR_CODES(0xC,2))
		#define HIDP_STATUS_INVALID_REPORT_LENGTH    (HIDP_ERROR_CODES(0xC,3))
		#define HIDP_STATUS_USAGE_NOT_FOUND          (HIDP_ERROR_CODES(0xC,4))
		#define HIDP_STATUS_VALUE_OUT_OF_RANGE       (HIDP_ERROR_CODES(0xC,5))
		#define HIDP_STATUS_BAD_LOG_PHY_VALUES       (HIDP_ERROR_CODES(0xC,6))
		#define HIDP_STATUS_BUFFER_TOO_SMALL         (HIDP_ERROR_CODES(0xC,7))
		#define HIDP_STATUS_INTERNAL_ERROR           (HIDP_ERROR_CODES(0xC,8))
		#define HIDP_STATUS_I8042_TRANS_UNKNOWN      (HIDP_ERROR_CODES(0xC,9))
		#define HIDP_STATUS_INCOMPATIBLE_REPORT_ID   (HIDP_ERROR_CODES(0xC,0xA))
		#define HIDP_STATUS_NOT_VALUE_ARRAY          (HIDP_ERROR_CODES(0xC,0xB))
		#define HIDP_STATUS_IS_VALUE_ARRAY           (HIDP_ERROR_CODES(0xC,0xC))
		#define HIDP_STATUS_DATA_INDEX_NOT_FOUND     (HIDP_ERROR_CODES(0xC,0xD))
		#define HIDP_STATUS_DATA_INDEX_OUT_OF_RANGE  (HIDP_ERROR_CODES(0xC,0xE))
		#define HIDP_STATUS_BUTTON_NOT_PRESSED       (HIDP_ERROR_CODES(0xC,0xF))
		#define HIDP_STATUS_REPORT_DOES_NOT_EXIST    (HIDP_ERROR_CODES(0xC,0x10))
		#define HIDP_STATUS_NOT_IMPLEMENTED          (HIDP_ERROR_CODES(0xC,0x20))
		*/


		// specified in DCB
		public const int INVALID_HANDLE_VALUE = -1;
		public const int ERROR_INVALID_HANDLE = 6;
		public const int FILE_FLAG_OVERLAPPED = 0x40000000;
		

		
		// GUID structure
		[StructLayout(LayoutKind.Sequential)]
		public unsafe struct GUID 
		{
			public int Data1;
			public System.UInt16 Data2;
			public System.UInt16 Data3;	
			[MarshalAs(UnmanagedType.ByValArray, SizeConst=8)]
			public byte[] data4;
		}

		
		// Device interface data
		[StructLayout(LayoutKind.Sequential)]
		public unsafe struct SP_DEVICE_INTERFACE_DATA 
		{
			public int cbSize;
			public GUID InterfaceClassGuid;
			public int Flags;
			public int Reserved;
		}

		
		// Device interface detail data
		[StructLayout(LayoutKind.Sequential, CharSet= CharSet.Ansi)]
		public unsafe struct PSP_DEVICE_INTERFACE_DETAIL_DATA
		{
			public int  cbSize;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst= 256)]
			public string DevicePath;
		}

		// HIDD_ATTRIBUTES
		[StructLayout(LayoutKind.Sequential)]
		public unsafe struct HIDD_ATTRIBUTES 
		{
			public int   Size; // = sizeof (struct _HIDD_ATTRIBUTES) = 10

			//
			// Vendor ids of this hid device
			//
			public System.UInt16	VendorID;
			public System.UInt16	ProductID;
			public System.UInt16	VersionNumber;

			//
			// Additional fields will be added to the end of this structure.
			//
		} 

		

		// 
		[StructLayout(LayoutKind.Sequential)]
		public unsafe struct HIDP_CAPS 
		{
			public System.UInt16  Usage;					// USHORT
			public System.UInt16   UsagePage;				// USHORT
			public System.UInt16   InputReportByteLength;
			public System.UInt16   OutputReportByteLength;
			public System.UInt16   FeatureReportByteLength;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst=17)]
			public System.UInt16[] Reserved;				// USHORT  Reserved[17];			
			public System.UInt16  NumberLinkCollectionNodes;
			public System.UInt16  NumberInputButtonCaps;
			public System.UInt16  NumberInputValueCaps;
			public System.UInt16  NumberInputDataIndices;
			public System.UInt16  NumberOutputButtonCaps;
			public System.UInt16  NumberOutputValueCaps;
			public System.UInt16  NumberOutputDataIndices;
			public System.UInt16  NumberFeatureButtonCaps;
			public System.UInt16  NumberFeatureValueCaps;
			public System.UInt16  NumberFeatureDataIndices;
		}
		

		// 
		public enum HIDP_REPORT_TYPE 
		{
			HidP_Input,		// 0 input
			HidP_Output,	// 1 output
			HidP_Feature	// 2 feature
		}

		// Structures in the union belonging to HIDP_VALUE_CAPS (see below)

		// Range
		[StructLayout(LayoutKind.Sequential)]
			public unsafe struct Range 
		{
			public System.UInt16 UsageMin;			// USAGE	UsageMin; // USAGE  Usage; 
			public System.UInt16 UsageMax; 			// USAGE	UsageMax; // USAGE	Reserved1;
			public System.UInt16 StringMin;			// USHORT  StringMin; // StringIndex; 
			public System.UInt16 StringMax;			// USHORT	StringMax;// Reserved2;
			public System.UInt16 DesignatorMin;		// USHORT  DesignatorMin; // DesignatorIndex; 
			public System.UInt16 DesignatorMax;		// USHORT	DesignatorMax; //Reserved3; 
			public System.UInt16 DataIndexMin;		// USHORT  DataIndexMin;  // DataIndex; 
			public System.UInt16 DataIndexMax;		// USHORT	DataIndexMax; // Reserved4;
		}

		// Range
		[StructLayout(LayoutKind.Sequential)]
			public unsafe struct NotRange 
		{
			public System.UInt16 Usage; 
			public System.UInt16 Reserved1;
			public System.UInt16 StringIndex; 
			public System.UInt16 Reserved2;
			public System.UInt16 DesignatorIndex; 
			public System.UInt16 Reserved3; 
			public System.UInt16 DataIndex; 
			public System.UInt16 Reserved4;
		}

		
		
		// Very many thanks to Mathias Sjogren for his help in
		// the proper way of marshalling this structure into C#
		//
		//
		[StructLayout(LayoutKind.Explicit, CharSet= CharSet.Ansi)]
			public unsafe struct HIDP_VALUE_CAPS 
		{
			//
			[FieldOffset(0)] public System.UInt16  UsagePage;					// USHORT
			[FieldOffset(2)] public System.Byte ReportID;						// UCHAR  ReportID;
			[MarshalAs(UnmanagedType.I1)]
			[FieldOffset(3)] public System.Boolean IsAlias;						// BOOLEAN  IsAlias;
			[FieldOffset(4)] public System.UInt16 BitField;						// USHORT  BitField;
			[FieldOffset(6)] public System.UInt16 LinkCollection;				//USHORT  LinkCollection;
			[FieldOffset(8)] public System.UInt16 LinkUsage;					// USAGE  LinkUsage;
			[FieldOffset(10)] public System.UInt16 LinkUsagePage;				// USAGE  LinkUsagePage;
			[MarshalAs(UnmanagedType.I1)]
			[FieldOffset(12)] public System.Boolean IsRange;					// BOOLEAN  IsRange;
			[MarshalAs(UnmanagedType.I1)]
			[FieldOffset(13)] public System.Boolean IsStringRange;				// BOOLEAN  IsStringRange;
			[MarshalAs(UnmanagedType.I1)]
			[FieldOffset(14)] public System.Boolean IsDesignatorRange;			// BOOLEAN  IsDesignatorRange;
			[MarshalAs(UnmanagedType.I1)]
			[FieldOffset(15)] public System.Boolean IsAbsolute;					// BOOLEAN  IsAbsolute;
			[MarshalAs(UnmanagedType.I1)]
			[FieldOffset(16)] public System.Boolean HasNull;					// BOOLEAN  HasNull;
			[FieldOffset(17)] public System.Char Reserved;						// UCHAR  Reserved;
			[FieldOffset(18)] public System.UInt16 BitSize;						// USHORT  BitSize;
			[FieldOffset(20)] public System.UInt16 ReportCount;					// USHORT  ReportCount;
			[FieldOffset(22)] public System.UInt16  Reserved2a;					// USHORT  Reserved2[5];		
			[FieldOffset(24)] public System.UInt16  Reserved2b;					// USHORT  Reserved2[5];
			[FieldOffset(26)] public System.UInt16  Reserved2c;					// USHORT  Reserved2[5];
			[FieldOffset(28)] public System.UInt16  Reserved2d;					// USHORT  Reserved2[5];
			[FieldOffset(30)] public System.UInt16  Reserved2e;					// USHORT  Reserved2[5];
			[FieldOffset(32)] public System.UInt16 UnitsExp;					// ULONG  UnitsExp;
			[FieldOffset(34)] public System.UInt16 Units;						// ULONG  Units;
			[FieldOffset(36)] public System.Int16 LogicalMin;					// LONG  LogicalMin;   ;
			[FieldOffset(38)] public System.Int16 LogicalMax;					// LONG  LogicalMax
			[FieldOffset(40)] public System.Int16 PhysicalMin;					// LONG  PhysicalMin, 
			[FieldOffset(42)] public System.Int16 PhysicalMax;					// LONG  PhysicalMax;
			// The Structs in the Union			
			[FieldOffset(44)] public Range Range;
			[FieldOffset(44)] public Range NotRange;				
		} 

		//*  Comm Timeout Grasp at Straws
		[StructLayout(LayoutKind.Sequential)]
		public struct COMMTIMEOUTS
		{
			public int ReadIntervalTimeout;
			public int ReadTotalTimeoutMultiplier;
			public int ReadTotalTimeoutConstant;
			public int WriteTotalTimeoutMultiplier;
			public int WriteTotalTimeoutConstant;
		}

		[StructLayout(LayoutKind.Sequential)]
			public struct OVERLAPPED 
		{
			public int Internal;
			public int InternalHigh;
			public int Offset;
			public int OffsetHigh;
			public int hEvent;
		} 
		



		// ----------------------------------------------------------------------------------
		//
		//
		//
		// 
		// Define istances of the structures
		//
		//

		private GUID MYguid = new GUID();
		//
		// SP_DEVICE_INTERFACE_DATA  mySP_DEVICE_INTERFACE_DATA = new SP_DEVICE_INTERFACE_DATA();
		//
		public SP_DEVICE_INTERFACE_DATA  mySP_DEVICE_INTERFACE_DATA;
		//
		public PSP_DEVICE_INTERFACE_DETAIL_DATA  myPSP_DEVICE_INTERFACE_DETAIL_DATA; 
		// 
		public HIDD_ATTRIBUTES myHIDD_ATTRIBUTES;
		//
		public HIDP_CAPS myHIDP_CAPS;
		//
		public HIDP_VALUE_CAPS[] myHIDP_VALUE_CAPS;
		//
		//
		//
		//
		//
		//
		//

		// DLL Calls

		// 1-- Get GUID for the HID Class
		[DllImport("hid.dll", SetLastError=true)]
		static extern  unsafe void  HidD_GetHidGuid(
			ref GUID lpHidGuid);

		// 2- Get array of structures with the HID info
		[DllImport("setupapi.dll", SetLastError=true)]
		static extern  unsafe int SetupDiGetClassDevs(
			ref GUID  lpHidGuid,
			int*  Enumerator,
			int*  hwndParent,
			int  Flags);


		// 3- Get context structure for a device interface element
		/*
		  SetupDiEnumDeviceInterfaces returns a context structure for a device 
		  interface element of a device information set. Each call returns information 
		  about one device interface; the function can be called repeatedly to get information 
		  about several interfaces exposed by one or more devices.
		*/
		[DllImport("setupapi.dll", SetLastError=true)]
		static extern  unsafe int SetupDiEnumDeviceInterfaces(
			int  DeviceInfoSet,
			int  DeviceInfoData,
			ref  GUID  lpHidGuid,
			int  MemberIndex,
			ref  SP_DEVICE_INTERFACE_DATA lpDeviceInterfaceData);

	
		//	4 a- Get device Path name
		//  Works for the first pass  --> to get the required size
		//
		[DllImport("setupapi.dll", SetLastError=true)]
		static extern  unsafe int SetupDiGetDeviceInterfaceDetail(
			int  DeviceInfoSet,
			ref SP_DEVICE_INTERFACE_DATA lpDeviceInterfaceData,
			int* aPtr,
			int detailSize,
			ref int requiredSize,
			int* bPtr);
		
		//	4 b- Get device Path name
		//  Works for second pass (overide), once size value is known
		//	
		// 
		[DllImport("setupapi.dll", SetLastError=true)]
		static extern  unsafe int SetupDiGetDeviceInterfaceDetail(
			int  DeviceInfoSet,
			ref SP_DEVICE_INTERFACE_DATA lpDeviceInterfaceData,
			ref PSP_DEVICE_INTERFACE_DETAIL_DATA myPSP_DEVICE_INTERFACE_DETAIL_DATA,
			int detailSize,
			ref int requiredSize,
			int* bPtr);

		// 5
		[DllImport("kernel32.dll", SetLastError=true)]
		private static extern int CreateFile(
			string lpFileName,							// file name
			uint dwDesiredAccess,						// access mode
			uint dwShareMode,							// share mode
			uint lpSecurityAttributes,					// SD
			uint dwCreationDisposition,					// how to create
			uint dwFlagsAndAttributes,					// file attributes
			uint hTemplateFile							// handle to template file
			);

		// 6
		[DllImport("kernel32.dll", SetLastError=true)]
		private static extern int CloseHandle(
			int hObject									// handle to object
			);


		
		 // 7
		[DllImport("hid.dll", SetLastError=true)]
		private static extern int HidD_GetAttributes(
			int hObject,								// IN HANDLE  HidDeviceObject,
			ref HIDD_ATTRIBUTES Attributes);			// OUT PHIDD_ATTRIBUTES  Attributes

	
		// 8
		[DllImport("hid.dll", SetLastError=true)]
		private unsafe static  extern int HidD_GetPreparsedData(
			int hObject,								// IN HANDLE  HidDeviceObject,
			ref int pPHIDP_PREPARSED_DATA);				// OUT PHIDP_PREPARSED_DATA  *PreparsedData



		// 9
		[DllImport("hid.dll", SetLastError=true)]
		private unsafe static  extern int HidP_GetCaps(
			int pPHIDP_PREPARSED_DATA,					// IN PHIDP_PREPARSED_DATA  PreparsedData,
			ref HIDP_CAPS myPHIDP_CAPS);				// OUT PHIDP_CAPS  Capabilities


		// 10
		[DllImport("hid.dll", SetLastError=true)]
		private unsafe static  extern int HidP_GetValueCaps(
			HIDP_REPORT_TYPE ReportType,								// IN HIDP_REPORT_TYPE  ReportType,
			[In, Out] HIDP_VALUE_CAPS[] ValueCaps,						// OUT PHIDP_VALUE_CAPS  ValueCaps,
			ref int ValueCapsLength,									// IN OUT PULONG  ValueCapsLength,
			int pPHIDP_PREPARSED_DATA);									// IN PHIDP_PREPARSED_DATA  PreparsedData

		
		[DllImport("kernel32.dll", SetLastError=true)]
		public static extern bool GetCommTimeouts(
			int hFile,
			ref COMMTIMEOUTS lpCommTimeouts
			);

		[DllImport("kernel32.dll", SetLastError=true)]
		public static extern bool SetCommTimeouts(
			int hFile,
			ref COMMTIMEOUTS lpCommTimeouts
			);


		// 11
		[DllImport("kernel32.dll", SetLastError=true)]
		public unsafe static extern bool ReadFile(
			int hFile,						// handle to file
			byte[] lpBuffer,				// data buffer
			int nNumberOfBytesToRead,		// number of bytes to read
			ref int lpNumberOfBytesRead,	// number of bytes read
			ref OVERLAPPED lpOverlapped		// overlapped buffer
			);

		[DllImport("kernel32.dll")]
		public static extern bool GetOverlappedResult(
			IntPtr hFile, 
			[In] ref OVERLAPPED lpOverlapped, 
			out uint lpNumberOfBytesTransferred,
			bool bWait);

		[DllImport("kernel32.dll")]
		public static extern bool HasOverlappedIoCompleted(
			[In] ref OVERLAPPED lpOverlapped
			);
		

		// 12
		[DllImport("setupapi.dll", SetLastError=true)]
		static extern  unsafe int SetupDiDestroyDeviceInfoList(
			int DeviceInfoSet				// IN HDEVINFO  DeviceInfoSet
			);

		// 13
		[DllImport("hid.dll", SetLastError=true)]
		static extern  unsafe int HidD_FreePreparsedData(
			int pPHIDP_PREPARSED_DATA			// IN PHIDP_PREPARSED_DATA  PreparsedData
			);


		//
		//	--------------------------------------
		// Managed Code wrappers for the DLL Calls
		// Naming convention ---> same as unmaneged DLL call with prefix CT_xxxx 
		// 
		// 
		// 1 GUID for HID
		public unsafe void CT_HidGuid()
		{		        
			HidD_GetHidGuid(ref MYguid);	// 
		}


		// 2
		public unsafe int CT_SetupDiGetClassDevs()
		{		        
			hDevInfo = SetupDiGetClassDevs(
				ref MYguid,
				null, 
				null,
				DIGCF_INTERFACEDEVICE | DIGCF_PRESENT);
			return hDevInfo;
		}


		// 3
		public unsafe int CT_SetupDiEnumDeviceInterfaces(int memberIndex)
		{	
			mySP_DEVICE_INTERFACE_DATA = new SP_DEVICE_INTERFACE_DATA();
			mySP_DEVICE_INTERFACE_DATA.cbSize = Marshal.SizeOf(mySP_DEVICE_INTERFACE_DATA);
	        int result =  SetupDiEnumDeviceInterfaces(
				hDevInfo,
				0,
				ref  MYguid,
				memberIndex,
				ref mySP_DEVICE_INTERFACE_DATA);
			return result;
		}
	
			
		
		// 4a
		// results = 0 is OK with the first pass of the routine since we are
		// trying to get the RequiredSize parameter so in the next call we can read the entire detail

		public unsafe int CT_SetupDiGetDeviceInterfaceDetail(ref int RequiredSize, int DeviceInterfaceDetailDataSize)
		{	
			int results =
			SetupDiGetDeviceInterfaceDetail(
				hDevInfo,							// IN HDEVINFO  DeviceInfoSet,
				ref mySP_DEVICE_INTERFACE_DATA,		// IN PSP_DEVICE_INTERFACE_DATA  DeviceInterfaceData,
				null,								// OUT PSP_DEVICE_INTERFACE_DETAIL_DATA  DeviceInterfaceDetailData,  OPTIONAL
				DeviceInterfaceDetailDataSize,		// IN DWORD  DeviceInterfaceDetailDataSize,
				ref RequiredSize,					// OUT PDWORD  RequiredSize,  OPTIONAL
				null); // 
			 return results;
		}

		// 4b
		// results = 1 iin the second pass of the routine is success
		// DeviceInterfaceDetailDataSize parameter (RequiredSize) came from the first pass 

		public unsafe int CT_SetupDiGetDeviceInterfaceDetailx(ref int RequiredSize, int DeviceInterfaceDetailDataSize)
		{	
			myPSP_DEVICE_INTERFACE_DETAIL_DATA = new PSP_DEVICE_INTERFACE_DETAIL_DATA();
			
			//
			// This part needs some work
			
			// if I use the following line of code 
			// myPSP_DEVICE_INTERFACE_DETAIL_DATA.cbSize = sizeof(PSP_DEVICE_INTERFACE_DETAIL_DATA);
			// I get the following error
			// !! Cannot take the address or size of a variable of a managed type ('USBSharp.USBSharp.PSP_DEVICE_INTERFACE_DETAIL_DATA')
			// 

			// As a result.. this is hard coded for now !!! 
			// for the c struct PSP_DEVICE_INTERFACE_DETAIL_DATA dizeof = DWORD cbSize (size 4) + Char[0] (size 1) = Total size 5 ?
			//
			myPSP_DEVICE_INTERFACE_DETAIL_DATA.cbSize = 5;
			
			int results =
				SetupDiGetDeviceInterfaceDetail(
				hDevInfo,									// IN HDEVINFO  DeviceInfoSet,
				ref mySP_DEVICE_INTERFACE_DATA,				// IN PSP_DEVICE_INTERFACE_DATA  DeviceInterfaceData,
				ref myPSP_DEVICE_INTERFACE_DETAIL_DATA,		// DeviceInterfaceDetailData,  OPTIONAL
				DeviceInterfaceDetailDataSize,				// IN DWORD  DeviceInterfaceDetailDataSize,
				ref RequiredSize,							// OUT PDWORD  RequiredSize,  OPTIONAL
				null); // 
			DevicePathName = myPSP_DEVICE_INTERFACE_DETAIL_DATA.DevicePath;
			return results;
		}
		
		// 5
		// Get a handle (opens file) to the HID device
		// Returns  0 is no success
		// Returns 1 if success
		public unsafe  int CT_CreateFile(string DeviceName)
		{
		
			HidHandle = CreateFile(
				DeviceName,
				GENERIC_READ | GENERIC_WRITE,
				FILE_SHARE_READ | FILE_SHARE_WRITE,
				0,
				OPEN_EXISTING,
				FILE_FLAG_OVERLAPPED, 
				0);
			if (HidHandle == -1)
			{
				return 0;
			}
			else
			{
				return 1;
			}

		}

		// 6
		// Closed the file and disposes of the handle
		public unsafe int CT_CloseHandle(int hObject) 
		{
			HidHandle = -1;
			return CloseHandle(hObject);
			
		}

		// 7
		// Get a handle to the HID device
		public unsafe  int CT_HidD_GetAttributes(int hObject)
		{	
			// Create an instance of HIDD_ATTRIBUTES
			myHIDD_ATTRIBUTES = new HIDD_ATTRIBUTES();
			// Calculate its size
			myHIDD_ATTRIBUTES.Size = sizeof(HIDD_ATTRIBUTES);
			
			return HidD_GetAttributes(
					hObject,
					ref myHIDD_ATTRIBUTES);
		}

		// 8
		// Gets a pointer to the preparsed data buffer
		public unsafe int CT_HidD_GetPreparsedData(int hObject, ref int pPHIDP_PREPARSED_DATA)
		{
			return HidD_GetPreparsedData(
			hObject,
			ref pPHIDP_PREPARSED_DATA);
		}

		// 9
		// Gets the capabilities report
		public unsafe int CT_HidP_GetCaps(int pPreparsedData)
		{
			myHIDP_CAPS = new HIDP_CAPS();
			return HidP_GetCaps(
				pPreparsedData,
				ref myHIDP_CAPS);
		}
		
		//
		// 10
		// Value Capabilities
		public int CT_HidP_GetValueCaps(ref int CalsCapsLength, int pPHIDP_PREPARSED_DATA)
		{
			
			HIDP_REPORT_TYPE myType = 0;
			myHIDP_VALUE_CAPS = new HIDP_VALUE_CAPS[5];
			return HidP_GetValueCaps(
				myType,
				myHIDP_VALUE_CAPS,
				ref CalsCapsLength,
				pPHIDP_PREPARSED_DATA);			

		}


		
								
		// 11
		// read Port
		public unsafe byte[] CT_ReadFile(int InputReportByteLength, ref OVERLAPPED ovl)
		{
			int BytesRead =0;
			byte[] BufBytes = new byte[InputReportByteLength];
			if (ReadFile(HidHandle, BufBytes, InputReportByteLength, ref BytesRead, ref ovl))
			{
				byte[] OutBytes = new byte[BytesRead];
				Array.Copy(BufBytes, OutBytes, BytesRead);
				return OutBytes;
			}
			else
			{
				return null;
			}
		}

		// 12
		// DestroyDeviceInfoList
		public int CT_SetupDiDestroyDeviceInfoList()
		{
			return SetupDiDestroyDeviceInfoList(hDevInfo);
			
		}

		// 13
		// FreePreparsedData
		public int CT_HidD_FreePreparsedData(int pPHIDP_PREPARSED_DATA)
		{
			return SetupDiDestroyDeviceInfoList(pPHIDP_PREPARSED_DATA);			
		}




		#region IDisposable Members

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected void Dispose(bool disposeManagedResources)
		{
			if (!this.disposed)
			{
				if (disposeManagedResources)
				{
					//only clear up managed stuff here
				}

				//clear up unmanaged stuff here
				if (HidHandle != -1)
				{
					CT_CloseHandle(HidHandle);
				}

				if (hDevInfo != -1)
				{
					CT_SetupDiDestroyDeviceInfoList();
				}

				this.disposed = true;
			}
		}

		#endregion
}
}
