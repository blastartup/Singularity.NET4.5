/* *******************************************
// Copyright 2010-2011, Anthony Hand
//
// File version date: August 22, 2011
//		Update: 
//		- Updated DetectAndroidTablet() to fix a bug introduced in the last fix! The true/false returns were mixed up. 
//
// File version date: August 16, 2011
//		Update: 
//		- Updated DetectAndroidTablet() to exclude Opera Mini, which was falsely reporting as running on a tablet device when on a phone.
//		- FireEvents(): Updated the useragent and httpaccept init technique to handle spiders and such with null values.
//
// File version date: August 7, 2011
//		Update: 
//		- The Opera for Android browser doesn't follow Google's recommended useragent string guidelines, so some fixes were needed.
//		- Updated DetectAndroidPhone() and DetectAndroidTablet() to properly detect devices running Opera Mobile.
//		- Created 2 new methods: DetectOperaAndroidPhone() and DetectOperaAndroidTablet(). 
//		- Updated DetectTierIphone(). Removed the call to DetectMaemoTablet(), an obsolete mobile OS.
//		- Fixed some minor bugs in FireEvents() (the DetectIos() event), DetectWebOSTablet(), and DetectGarminNuvifone().
//
// File version date: July 15, 2011
//		Update: 
//		- Refactored the variable called maemoTablet. Its new name is the more generic deviceTablet.
//		- Created the variable deviceWebOShp for HP's line of WebOS devices starting with the TouchPad tablet.
//		- Created the DetectWebOSTablet() method for HP's line of WebOS tablets starting with the TouchPad tablet.
//		- Updated the DetectTierTablet() method to also search for WebOS tablets. 
//		- Updated the DetectMaemoTablet() method to disambiguate against WebOS tablets which share some signature traits. 
//
//
// LICENSE INFORMATION
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
//        http://www.apache.org/licenses/LICENSE-2.0 
// Unless required by applicable law or agreed to in writing, 
// software distributed under the License is distributed on an 
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, 
// either express or implied. See the License for the specific 
// language governing permissions and limitations under the License. 
//
//
// ABOUT THIS PROJECT
//   Project Owner: Anthony Hand
//   Email: anthony.hand@gmail.com
//   Web Site: http://www.mobileesp.com
//   Source Files: http://code.google.com/p/mobileesp/
//   
//   Versions of this code are available for:
//      PHP, JavaScript, Java, ASP.NET (C#), and Ruby
//
// *******************************************
*/


using System;


namespace Singularity.Web
{
	/// <summary>
	/// Subclass this page to inherit the built-in mobile device detection.
	/// </summary>
	public class MDetectPage : System.Web.UI.Page
	{

		private string _useragent = "";
		private string _httpaccept = "";

		#region Fields - Detection Argument Values

		//standardized values for detection arguments.
		private readonly string _dargsIphone = "iphone";
		private readonly string _dargsIpod = "ipod";
		private readonly string _dargsIpad = "ipad";
		private readonly string _dargsIphoneOrIpod = "iphoneoripod";
		private readonly string _dargsIos = "ios";
		private readonly string _dargsAndroid = "android";
		private readonly string _dargsAndroidPhone = "androidphone";
		private readonly string _dargsAndroidTablet = "androidtablet";
		private readonly string _dargsGoogleTv = "googletv";
		private readonly string _dargsWebKit = "webkit";
		private readonly string _dargsSymbianOs = "symbianos";
		private readonly string _dargsS60 = "series60";
		private readonly string _dargsWindowsPhone7 = "windowsphone7";
		private readonly string _dargsWindowsMobile = "windowsmobile";
		//private string dargsBlackBerry = "blackberry";
		private readonly string _dargsBlackBerryWebkit = "blackberrywebkit";
		private readonly string _dargsPalmOs = "palmos";
		private readonly string _dargsPalmWebOs = "webos";
		private readonly string _dargsWebOsTablet = "webostablet";
		private readonly string _dargsSmartphone = "smartphone";
		private readonly string _dargsBrewDevice = "brew";
		private readonly string _dargsDangerHiptop = "dangerhiptop";
		private readonly string _dargsOperaMobile = "operamobile";
		private readonly string _dargsWapWml = "wapwml";
		private readonly string _dargsKindle = "kindle";
		private readonly string _dargsMobileQuick = "mobilequick";
		private readonly string _dargsTierTablet = "tiertablet";
		private readonly string _dargsTierIphone = "tieriphone";
		private readonly string _dargsTierRichCss = "tierrichcss";
		private readonly string _dargsTierOtherPhones = "tierotherphones";

		#endregion Fields - Detection Argument Values

		#region Fields - User Agent Keyword Values

		//Initialize some initial smartphone private string private stringiables.
		private readonly string _engineWebKit = "webkit".ToUpper();
		private readonly string _deviceIphone = "iphone".ToUpper();
		private readonly string _deviceIpod = "ipod".ToUpper();
		private readonly string _deviceIpad = "ipad".ToUpper();
		private readonly string _deviceMacPpc = "macintosh".ToUpper(); //Used for disambiguation

		private readonly string _deviceAndroid = "android".ToUpper();
		private readonly string _deviceGoogleTv = "googletv".ToUpper();
		//private readonly string _deviceXoom = "xoom".ToUpper(); //Motorola Xoom
		private readonly string _deviceHtcFlyer = "htc_flyer".ToUpper(); //HTC Flyer

		private readonly string _deviceNuvifone = "nuvifone".ToUpper();  //Garmin Nuvifone

		private readonly string _deviceSymbian = "symbian".ToUpper();
		private readonly string _deviceS60 = "series60".ToUpper();
		private readonly string _deviceS70 = "series70".ToUpper();
		private readonly string _deviceS80 = "series80".ToUpper();
		private readonly string _deviceS90 = "series90".ToUpper();

		private readonly string _deviceWinPhone7 = "windows phone os 7".ToUpper();
		private readonly string _deviceWinMob = "windows ce".ToUpper();
		private readonly string _deviceWindows = "windows".ToUpper();
		private readonly string _deviceIeMob = "iemobile".ToUpper();
		private readonly string _devicePpc = "ppc".ToUpper(); //Stands for PocketPC
		private readonly string _enginePie = "wm5 pie".ToUpper(); //An old Windows Mobile

		private readonly string _deviceBb = "blackberry".ToUpper();
		private readonly string _vndRim = "vnd.rim".ToUpper(); //Detectable when BB devices emulate IE or Firefox
		private readonly string _deviceBbStorm = "blackberry95".ToUpper(); //Storm 1 and 2
		private readonly string _deviceBbBold = "blackberry97".ToUpper(); //Bold
		private readonly string _deviceBbTour = "blackberry96".ToUpper(); //Tour
		private readonly string _deviceBbCurve = "blackberry89".ToUpper(); //Curve2
		private readonly string _deviceBbTorch = "blackberry 98".ToUpper(); //Torch
		private readonly string _deviceBbPlaybook = "playbook".ToUpper(); //PlayBook tablet

		private readonly string _devicePalm = "palm".ToUpper();
		private readonly string _deviceWebOs = "webos".ToUpper(); //For Palm's line of WebOS devices
		private readonly string _deviceWebOShp = "hpwos".ToUpper(); //For HP's line of WebOS devices

		private readonly string _engineBlazer = "blazer".ToUpper(); //Old Palm
		private readonly string _engineXiino = "xiino".ToUpper(); //Another old Palm

		private readonly string _deviceKindle = "kindle".ToUpper();  //Amazon Kindle, eInk one.

		//Initialize private stringiables for mobile-specific content.
		private readonly string _vndwap = "vnd.wap".ToUpper();
		private readonly string _wml = "wml".ToUpper();

		//Initialize private stringiables for other random devices and mobile browsers.
		private readonly string _deviceTablet = "tablet".ToUpper(); //Generic term for slate and tablet devices
		private readonly string _deviceBrew = "brew".ToUpper();
		private readonly string _deviceDanger = "danger".ToUpper();
		private readonly string _deviceHiptop = "hiptop".ToUpper();
		private readonly string _devicePlaystation = "playstation".ToUpper();
		private readonly string _deviceNintendoDs = "nitro".ToUpper();
		private readonly string _deviceNintendo = "nintendo".ToUpper();
		private readonly string _deviceWii = "wii".ToUpper();
		private readonly string _deviceXbox = "xbox".ToUpper();
		private readonly string _deviceArchos = "archos".ToUpper();

		private readonly string _engineOpera = "opera".ToUpper(); //Popular browser
		private readonly string _engineNetfront = "netfront".ToUpper(); //Common embedded OS browser
		private readonly string _engineUpBrowser = "up.browser".ToUpper(); //common on some phones
		private readonly string _engineOpenWeb = "openweb".ToUpper(); //Transcoding by OpenWave server
		private readonly string _deviceMidp = "midp".ToUpper(); //a mobile Java technology
		private readonly string _uplink = "up.link".ToUpper();
		private readonly string _engineTelecaQ = "teleca q".ToUpper(); //a modern feature phone browser

		private readonly string _devicePda = "pda".ToUpper(); //some devices report themselves as PDAs
		private readonly string _mini = "mini".ToUpper();  //Some mobile browsers put "mini" in their names.
		private readonly string _mobile = "mobile".ToUpper(); //Some mobile browsers put "mobile" in their user agent private strings.
		private readonly string _mobi = "mobi".ToUpper(); //Some mobile browsers put "mobi" in their user agent private strings.

		//Use Maemo, Tablet, and Linux to test for Nokia"s Internet Tablets.
		private readonly string _maemo = "maemo".ToUpper();
		private readonly string _linux = "linux".ToUpper();
		private readonly string _qtembedded = "qt embedded".ToUpper(); //for Sony Mylo
		private readonly string _mylocom2 = "com2".ToUpper(); //for Sony Mylo also

		//In some UserAgents, the only clue is the manufacturer.
		private readonly string _manuSonyEricsson = "sonyericsson".ToUpper();
		private readonly string _manuericsson = "ericsson".ToUpper();
		private readonly string _manuSamsung1 = "sec-sgh".ToUpper();
		private readonly string _manuSony = "sony".ToUpper();
		private readonly string _manuHtc = "htc".ToUpper(); //Popular Android and WinMo manufacturer

		//In some UserAgents, the only clue is the operator.
		private readonly string _svcDocomo = "docomo".ToUpper();
		private readonly string _svcKddi = "kddi".ToUpper();
		private readonly string _svcVodafone = "vodafone".ToUpper();

		//Disambiguation strings.
		private readonly string _disUpdate = "update".ToUpper(); //pda vs. update

		#endregion Fields - User Agent Keyword Values

		/// <summary>
		/// To instantiate a WebPage sub-class with built-in
		/// mobile device detection delegates and events.
		/// </summary>
		public MDetectPage()
		{

		}

		/// <summary>
		/// To run the device detection methods andd fire 
		/// any existing OnDetectXXX events. 
		/// </summary>
		public void FireEvents()
		{
			if (_useragent == "" && _httpaccept == "")
			{
				_useragent = (Request.ServerVariables["HTTP_USER_AGENT"] ?? "").ToUpper();
				_httpaccept = (Request.ServerVariables["HTTP_ACCEPT"] ?? "").ToUpper();
			}

			#region Event Fire Methods

			MDetectArgs mda = null;
			if (this.DetectIpod())
			{
				mda = new MDetectArgs(_dargsIpod);
				if (this.OnDetectIpod != null)
				{
					this.OnDetectIpod(this, mda);
				}
			}
			if (this.DetectIpad())
			{
				mda = new MDetectArgs(_dargsIpad);
				if (this.OnDetectIpad != null)
				{
					this.OnDetectIpad(this, mda);
				}
			}
			if (this.DetectIphone())
			{
				mda = new MDetectArgs(_dargsIphone);
				if (this.OnDetectIphone != null)
				{
					this.OnDetectIphone(this, mda);
				}
			}
			if (this.DetectIphoneOrIpod())
			{
				mda = new MDetectArgs(_dargsIphoneOrIpod);
				this.OnDetectDetectIPhoneOrIpod?.Invoke(this, mda);
			}
			if (this.DetectIos())
			{
				mda = new MDetectArgs(_dargsIos);
				this.OnDetectIos?.Invoke(this, mda);
			}
			if (this.DetectAndroid())
			{
				mda = new MDetectArgs(_dargsAndroid);
				this.OnDetectAndroid?.Invoke(this, mda);
			}
			if (this.DetectAndroidPhone())
			{
				mda = new MDetectArgs(_dargsAndroidPhone);
				this.OnDetectAndroidPhone?.Invoke(this, mda);
			}
			if (this.DetectAndroidTablet())
			{
				mda = new MDetectArgs(_dargsAndroidTablet);
				this.OnDetectAndroidTablet?.Invoke(this, mda);
			}
			if (this.DetectGoogleTv())
			{
				mda = new MDetectArgs(_dargsGoogleTv);
				this.OnDetectGoogleTv?.Invoke(this, mda);
			}
			if (this.DetectWebkit())
			{
				mda = new MDetectArgs(_dargsWebKit);
				this.OnDetectWebkit?.Invoke(this, mda);
			}
			if (this.DetectS60OssBrowser())
			{
				mda = new MDetectArgs(_dargsS60);
				this.OnDetectS60OssBrowser?.Invoke(this, mda);
			}
			if (this.DetectSymbianOs())
			{
				mda = new MDetectArgs(_dargsSymbianOs);
				this.OnDetectSymbianOs?.Invoke(this, mda);
			}
			if (this.DetectWindowsPhone7())
			{
				mda = new MDetectArgs(_dargsWindowsPhone7);
				this.OnDetectWindowsPhone7?.Invoke(this, mda);
			}
			if (this.DetectWindowsMobile())
			{
				mda = new MDetectArgs(_dargsWindowsMobile);
				this.OnDetectWindowsMobile?.Invoke(this, mda);
			}
			if (this.DetectBlackBerry())
			{
				mda = new MDetectArgs(_dargsBlackBerryWebkit);
				this.OnDetectBlackBerry?.Invoke(this, mda);
			}
			if (this.DetectBlackBerryWebKit())
			{
				mda = new MDetectArgs(_dargsBlackBerryWebkit);
				this.OnDetectBlackBerryWebkit?.Invoke(this, mda);
			}
			if (this.DetectPalmOs())
			{
				mda = new MDetectArgs(_dargsPalmOs);
				this.OnDetectPalmOs?.Invoke(this, mda);
			}
			if (this.DetectPalmWebOs())
			{
				mda = new MDetectArgs(_dargsPalmWebOs);
				this.OnDetectPalmWebOs?.Invoke(this, mda);
			}
			if (this.DetectWebOsTablet())
			{
				mda = new MDetectArgs(_dargsWebOsTablet);
				this.OnDetectWebOsTablet?.Invoke(this, mda);
			}
			if (this.DetectSmartphone())
			{
				mda = new MDetectArgs(_dargsSmartphone);
				this.OnDetectSmartphone?.Invoke(this, mda);
			}
			if (this.DetectBrewDevice())
			{
				mda = new MDetectArgs(_dargsBrewDevice);
				this.OnDetectBrewDevice?.Invoke(this, mda);
			}
			if (this.DetectDangerHiptop())
			{
				mda = new MDetectArgs(_dargsDangerHiptop);
				this.OnDetectDangerHiptop?.Invoke(this, mda);
			}
			if (this.DetectOperaMobile())
			{
				mda = new MDetectArgs(_dargsOperaMobile);
				this.OnDetectOperaMobile?.Invoke(this, mda);
			}
			if (this.DetectWapWml())
			{
				mda = new MDetectArgs(_dargsWapWml);
				this.OnDetectWapWml?.Invoke(this, mda);
			}
			if (this.DetectKindle())
			{
				mda = new MDetectArgs(_dargsKindle);
				this.OnDetectKindle?.Invoke(this, mda);
			}
			if (this.DetectMobileQuick())
			{
				mda = new MDetectArgs(_dargsMobileQuick);
				this.OnDetectMobileQuick?.Invoke(this, mda);
			}
			if (this.DetectTierTablet())
			{
				mda = new MDetectArgs(_dargsTierTablet);
				this.OnDetectTierTablet?.Invoke(this, mda);
			}
			if (this.DetectTierIphone())
			{
				mda = new MDetectArgs(_dargsTierIphone);
				this.OnDetectTierIphone?.Invoke(this, mda);
			}
			if (this.DetectTierRichCss())
			{
				mda = new MDetectArgs(_dargsTierRichCss);
				this.OnDetectTierRichCss?.Invoke(this, mda);
			}
			if (this.DetectTierOtherPhones())
			{
				mda = new MDetectArgs(_dargsTierOtherPhones);
				this.OnDetectTierOtherPhones?.Invoke(this, mda);
			}

			#endregion Event Fire Methods

		}

		public class MDetectArgs : EventArgs
		{
			public MDetectArgs(string type)
			{
				this.Type = type;
			}

			public readonly string Type;
		}

		#region Mobile Device Detection Methods

		//**************************
		// Detects if the current device is an iPod Touch.
		public bool DetectIpod()
		{
			if (_useragent.IndexOf(_deviceIpod) != -1)
				return true;
			else
				return false;
		}

		//Ipod delegate
		public delegate void DetectIpodHandler(object page, MDetectArgs args);
		public event DetectIpodHandler OnDetectIpod;


		//**************************
		// Detects if the current device is an iPad tablet.
		public bool DetectIpad()
		{
			if (_useragent.IndexOf(_deviceIpad) != -1 && DetectWebkit())
				return true;
			else
				return false;
		}

		//Ipod delegate
		public delegate void DetectIpadHandler(object page, MDetectArgs args);
		public event DetectIpadHandler OnDetectIpad;


		//**************************
		// Detects if the current device is an iPhone.
		public bool DetectIphone()
		{
			if (_useragent.IndexOf(_deviceIphone) != -1)
			{
				//The iPad and iPod touch say they're an iPhone! So let's disambiguate.
				if (DetectIpad() || DetectIpod())
				{
					return false;
				}
				else
					return true;
			}
			else
				return false;
		}
		//IPhone delegate
		public delegate void DetectIphoneHandler(object page, MDetectArgs args);
		public event DetectIphoneHandler OnDetectIphone;

		//**************************
		// Detects if the current device is an iPhone or iPod Touch.
		public bool DetectIphoneOrIpod()
		{
			//We repeat the searches here because some iPods may report themselves as an iPhone, which would be okay.
			if (_useragent.IndexOf(_deviceIphone) != -1 ||
				 _useragent.IndexOf(_deviceIpod) != -1)
				return true;
			else
				return false;
		}
		//IPhoneOrIpod delegate
		public delegate void DetectIPhoneOrIpodHandler(object page, MDetectArgs args);
		public event DetectIPhoneOrIpodHandler OnDetectDetectIPhoneOrIpod;

		//**************************
		// Detects *any* iOS device: iPhone, iPod Touch, iPad.
		public bool DetectIos()
		{
			if (DetectIphoneOrIpod() || DetectIpad())
				return true;
			else
				return false;
		}

		//Ios delegate
		public delegate void DetectIosHandler(object page, MDetectArgs args);
		public event DetectIosHandler OnDetectIos;


		//**************************
		// Detects *any* Android OS-based device: phone, tablet, and multi-media player.
		// Also detects Google TV.
		public bool DetectAndroid()
		{
			if ((_useragent.IndexOf(_deviceAndroid) != -1) ||
				 DetectGoogleTv())
				return true;
			//Special check for the HTC Flyer 7" tablet. It should report here.
			if (_useragent.IndexOf(_deviceHtcFlyer) != -1)
				return true;
			else
				return false;
		}
		//Android delegate
		public delegate void DetectAndroidHandler(object page, MDetectArgs args);
		public event DetectAndroidHandler OnDetectAndroid;

		//**************************
		// Detects if the current device is a (small-ish) Android OS-based device
		// used for calling and/or multi-media (like a Samsung Galaxy Player).
		// Google says these devices will have 'Android' AND 'mobile' in user agent.
		// Ignores tablets (Honeycomb and later).
		public bool DetectAndroidPhone()
		{
			if (DetectAndroid() &&
				 (_useragent.IndexOf(_mobile) != -1))
				return true;
			//Special check for Android phones with Opera Mobile. They should report here.
			if (DetectOperaAndroidPhone())
				return true;
			//Special check for the HTC Flyer 7" tablet. It should report here.
			if (_useragent.IndexOf(_deviceHtcFlyer) != -1)
				return true;
			else
				return false;
		}
		//Android Phone delegate
		public delegate void DetectAndroidPhoneHandler(object page, MDetectArgs args);
		public event DetectAndroidPhoneHandler OnDetectAndroidPhone;

		//**************************
		// Detects if the current device is a (self-reported) Android tablet.
		// Google says these devices will have 'Android' and NOT 'mobile' in their user agent.
		public bool DetectAndroidTablet()
		{
			//First, let's make sure we're on an Android device.
			if (!DetectAndroid())
				return false;

			//Special check for Opera Android Phones. They should NOT report here.
			if (DetectOperaMobile())
				return false;
			//Special check for the HTC Flyer 7" tablet. It should NOT report here.
			if (_useragent.IndexOf(_deviceHtcFlyer) != -1)
				return false;

			//Otherwise, if it's Android and does NOT have 'mobile' in it, Google says it's a tablet.
			if (_useragent.IndexOf(_mobile) > -1)
				return false;
			else
				return true;
		}
		//Android Tablet delegate
		public delegate void DetectAndroidTabletHandler(object page, MDetectArgs args);
		public event DetectAndroidTabletHandler OnDetectAndroidTablet;

		//**************************
		// Detects if the current device is a GoogleTV device.
		public bool DetectGoogleTv()
		{
			if (_useragent.IndexOf(_deviceGoogleTv) != -1)
				return true;
			else
				return false;
		}
		//GoogleTV delegate
		public delegate void DetectGoogleTvHandler(object page, MDetectArgs args);
		public event DetectGoogleTvHandler OnDetectGoogleTv;

		//**************************
		// Detects if the current device is an Android OS-based device and
		//   the browser is based on WebKit.
		public bool DetectAndroidWebKit()
		{
			if (DetectAndroid() && DetectWebkit())
				return true;
			else
				return false;
		}

		//**************************
		// Detects if the current browser is based on WebKit.
		public bool DetectWebkit()
		{
			if (_useragent.IndexOf(_engineWebKit) != -1)
				return true;
			else
				return false;
		}

		//Webkit delegate
		public delegate void DetectWebkitHandler(object page, MDetectArgs args);
		public event DetectWebkitHandler OnDetectWebkit;

		//**************************
		// Detects if the current browser is the Nokia S60 Open Source Browser.
		public bool DetectS60OssBrowser()
		{
			//First, test for WebKit, then make sure it's either Symbian or S60.
			if (DetectWebkit())
			{
				if (_useragent.IndexOf(_deviceSymbian) != -1 ||
					 _useragent.IndexOf(_deviceS60) != -1)
				{
					return true;
				}
				else
					return false;
			}
			else
				return false;
		}

		//S60OssBrowser delegate
		public delegate void DetectS60OssBrowserHandler(object page, MDetectArgs args);
		public event DetectS60OssBrowserHandler OnDetectS60OssBrowser;

		//**************************
		// Detects if the current device is any Symbian OS-based device,
		//   including older S60, Series 70, Series 80, Series 90, and UIQ, 
		//   or other browsers running on these devices.
		public bool DetectSymbianOs()
		{
			if (_useragent.IndexOf(_deviceSymbian) != -1 ||
				 _useragent.IndexOf(_deviceS60) != -1 ||
				 _useragent.IndexOf(_deviceS70) != -1 ||
				 _useragent.IndexOf(_deviceS80) != -1 ||
				 _useragent.IndexOf(_deviceS90) != -1)
				return true;
			else
				return false;
		}

		//SymbianOS delegate
		public delegate void DetectSymbianOsHandler(object page, MDetectArgs args);
		public event DetectSymbianOsHandler OnDetectSymbianOs;

		//**************************
		// Detects if the current browser is a 
		// Windows Phone 7 device.
		public bool DetectWindowsPhone7()
		{
			if (_useragent.IndexOf(_deviceWinPhone7) != -1)
				return true;
			else
				return false;
		}

		//WindowsPhone7 delegate
		public delegate void DetectWindowsPhone7Handler(object page, MDetectArgs args);
		public event DetectWindowsPhone7Handler OnDetectWindowsPhone7;

		//**************************
		// Detects if the current browser is a Windows Mobile device.
		// Excludes Windows Phone 7 devices. 
		// Focuses on Windows Mobile 6.xx and earlier.
		public bool DetectWindowsMobile()
		{
			//Exclude new Windows Phone 7.
			if (DetectWindowsPhone7())
				return false;
			//Most devices use 'Windows CE', but some report 'iemobile' 
			//  and some older ones report as 'PIE' for Pocket IE. 
			if (_useragent.IndexOf(_deviceWinMob) != -1 ||
				 _useragent.IndexOf(_deviceIeMob) != -1 ||
				 _useragent.IndexOf(_enginePie) != -1)
				return true;
			//Test for Windows Mobile PPC but not old Macintosh PowerPC.
			if (_useragent.IndexOf(_devicePpc) != -1 &&
				 !(_useragent.IndexOf(_deviceMacPpc) != -1))
				return true;
			//Test for certain Windwos Mobile-based HTC devices.
			if (_useragent.IndexOf(_manuHtc) != -1 &&
				 _useragent.IndexOf(_deviceWindows) != -1)
				return true;
			if (DetectWapWml() == true &&
				 _useragent.IndexOf(_deviceWindows) != -1)
				return true;
			else
				return false;
		}

		//WindowsMobile delegate
		public delegate void DetectWindowsMobileHandler(object page, MDetectArgs args);
		public event DetectWindowsMobileHandler OnDetectWindowsMobile;

		//**************************
		// Detects if the current browser is any BlackBerry device.
		// Includes the PlayBook.
		public bool DetectBlackBerry()
		{
			if ((_useragent.IndexOf(_deviceBb) != -1) ||
				 (_httpaccept.IndexOf(_vndRim) != -1))
				return true;
			else
				return false;
		}
		//BlackBerry delegate
		public delegate void DetectBlackBerryHandler(object page, MDetectArgs args);
		public event DetectBlackBerryHandler OnDetectBlackBerry;


		//**************************
		// Detects if the current browser is on a BlackBerry tablet device.
		//    Example: PlayBook
		public bool DetectBlackBerryTablet()
		{
			if (_useragent.IndexOf(_deviceBbPlaybook) != -1)
				return true;
			else
				return false;
		}

		//**************************
		// Detects if the current browser is a BlackBerry device AND uses a
		//    WebKit-based browser. These are signatures for the new BlackBerry OS 6.
		//    Examples: Torch. Includes the Playbook.
		public bool DetectBlackBerryWebKit()
		{
			if (DetectBlackBerry() && DetectWebkit())
				return true;
			else
				return false;
		}
		//BlackBerry Webkit delegate
		public delegate void DetectBlackBerryWebkitHandler(object page, MDetectArgs args);
		public event DetectBlackBerryWebkitHandler OnDetectBlackBerryWebkit;


		//**************************
		// Detects if the current browser is a BlackBerry Touch
		//    device, such as the Storm or Touch. Excludes the Playbook.
		public bool DetectBlackBerryTouch()
		{
			if (DetectBlackBerry() &&
				 (_useragent.IndexOf(_deviceBbStorm) != -1 ||
				 _useragent.IndexOf(_deviceBbTorch) != -1))
				return true;
			else
				return false;
		}

		//**************************
		// Detects if the current browser is a BlackBerry device AND
		//    has a more capable recent browser. Excludes the Playbook.
		//    Examples, Storm, Bold, Tour, Curve2
		//    Excludes the new BlackBerry OS 6 browser!!
		public bool DetectBlackBerryHigh()
		{
			//Disambiguate for BlackBerry OS 6 (WebKit) browser
			if (DetectBlackBerryWebKit())
				return false;
			if (DetectBlackBerry())
			{
				if (DetectBlackBerryTouch() ||
					 _useragent.IndexOf(_deviceBbBold) != -1 ||
					 _useragent.IndexOf(_deviceBbTour) != -1 ||
					 _useragent.IndexOf(_deviceBbCurve) != -1)
					return true;
				else
					return false;
			}
			else
				return false;
		}

		//**************************
		// Detects if the current browser is a BlackBerry device AND
		//    has an older, less capable browser. 
		//    Examples: Pearl, 8800, Curve1.
		public bool DetectBlackBerryLow()
		{
			if (DetectBlackBerry())
			{
				//Assume that if it's not in the High tier, then it's Low.
				if (DetectBlackBerryHigh() || DetectBlackBerryWebKit())
					return false;
				else
					return true;
			}
			else
				return false;
		}

		//**************************
		// Detects if the current browser is on a PalmOS device.
		public bool DetectPalmOs()
		{
			//Most devices nowadays report as 'Palm', but some older ones reported as Blazer or Xiino.
			if (_useragent.IndexOf(_devicePalm) != -1 ||
				 _useragent.IndexOf(_engineBlazer) != -1 ||
				 _useragent.IndexOf(_engineXiino) != -1)
			{
				//Make sure it's not WebOS first
				if (DetectPalmWebOs() == true)
					return false;
				else
					return true;
			}
			else
				return false;
		}
		//PalmOS delegate
		public delegate void DetectPalmOsHandler(object page, MDetectArgs args);
		public event DetectPalmOsHandler OnDetectPalmOs;


		//**************************
		// Detects if the current browser is on a Palm device
		//    running the new WebOS.
		public bool DetectPalmWebOs()
		{
			if (_useragent.IndexOf(_deviceWebOs) != -1)
				return true;
			else
				return false;
		}

		//PalmWebOS delegate
		public delegate void DetectPalmWebOsHandler(object page, MDetectArgs args);
		public event DetectPalmWebOsHandler OnDetectPalmWebOs;


		//**************************
		// Detects if the current browser is on an HP tablet running WebOS.
		public bool DetectWebOsTablet()
		{
			if (_useragent.IndexOf(_deviceWebOShp) != -1 &&
				 _useragent.IndexOf(_deviceTablet) != -1)
			{
				return true;
			}
			else
				return false;
		}
		//WebOS tablet delegate
		public delegate void DetectWebOsTabletHandler(object page, MDetectArgs args);
		public event DetectWebOsTabletHandler OnDetectWebOsTablet;


		//**************************
		// Detects if the current browser is a
		//    Garmin Nuvifone.
		public bool DetectGarminNuvifone()
		{
			if (_useragent.IndexOf(_deviceNuvifone) != -1)
				return true;
			else
				return false;
		}


		//**************************
		// Check to see whether the device is any device
		//   in the 'smartphone' category.
		public bool DetectSmartphone()
		{
			if (DetectIphoneOrIpod() ||
				 DetectAndroidPhone() ||
				 DetectS60OssBrowser() ||
				 DetectSymbianOs() ||
				 DetectWindowsMobile() ||
				 DetectWindowsPhone7() ||
				 DetectBlackBerry() ||
				 DetectPalmWebOs() ||
				 DetectPalmOs() ||
				 DetectGarminNuvifone())
				return true;
			else
				return false;
		}

		//DetectSmartphone delegate
		public delegate void DetectSmartphoneHandler(object page, MDetectArgs args);
		public event DetectSmartphoneHandler OnDetectSmartphone;


		//**************************
		// Detects whether the device is a Brew-powered device.
		public bool DetectBrewDevice()
		{
			if (_useragent.IndexOf(_deviceBrew) != -1)
				return true;
			else
				return false;
		}

		//BrewDevice delegate
		public delegate void DetectBrewDeviceHandler(object page, MDetectArgs args);
		public event DetectBrewDeviceHandler OnDetectBrewDevice;

		//**************************
		// Detects the Danger Hiptop device.
		public bool DetectDangerHiptop()
		{
			if (_useragent.IndexOf(_deviceDanger) != -1 ||
				 _useragent.IndexOf(_deviceHiptop) != -1)
				return true;
			else
				return false;
		}
		//DangerHiptop delegate
		public delegate void DetectDangerHiptopHandler(object page, MDetectArgs args);
		public event DetectDangerHiptopHandler OnDetectDangerHiptop;

		//**************************
		// Detects if the current browser is Opera Mobile or Mini.
		public bool DetectOperaMobile()
		{
			if (_useragent.IndexOf(_engineOpera) != -1)
			{
				if ((_useragent.IndexOf(_mini) != -1) ||
				 (_useragent.IndexOf(_mobi) != -1))
				{
					return true;
				}
				else
					return false;
			}
			else
				return false;
		}

		//Opera Mobile delegate
		public delegate void DetectOperaMobileHandler(object page, MDetectArgs args);
		public event DetectOperaMobileHandler OnDetectOperaMobile;

		//**************************
		// Detects if the current browser is Opera Mobile
		// running on an Android phone.
		public bool DetectOperaAndroidPhone()
		{
			if ((_useragent.IndexOf(_engineOpera) != -1) &&
				 (_useragent.IndexOf(_deviceAndroid) != -1) &&
				 (_useragent.IndexOf(_mobi) != -1))
				return true;
			else
				return false;
		}

		// Detects if the current browser is Opera Mobile
		// running on an Android tablet.
		public bool DetectOperaAndroidTablet()
		{
			if ((_useragent.IndexOf(_engineOpera) != -1) &&
				 (_useragent.IndexOf(_deviceAndroid) != -1) &&
				 (_useragent.IndexOf(_deviceTablet) != -1))
				return true;
			else
				return false;
		}

		//**************************
		// Detects whether the device supports WAP or WML.
		public bool DetectWapWml()
		{
			if (_httpaccept.IndexOf(_vndwap) != -1 ||
				 _httpaccept.IndexOf(_wml) != -1)
				return true;
			else
				return false;
		}
		//WapWml delegate
		public delegate void DetectWapWmlHandler(object page, MDetectArgs args);
		public event DetectWapWmlHandler OnDetectWapWml;


		//**************************
		// Detects if the current device is an Amazon Kindle.
		public bool DetectKindle()
		{
			if (_useragent.IndexOf(_deviceKindle) != -1)
				return true;
			else
				return false;
		}

		//Kindle delegate
		public delegate void DetectKindleHandler(object page, MDetectArgs args);
		public event DetectKindleHandler OnDetectKindle;


		//**************************
		//   Detects if the current device is a mobile device.
		//   This method catches most of the popular modern devices.
		//   Excludes Apple iPads and other modern tablets.
		public bool DetectMobileQuick()
		{
			//Let's exclude tablets
			if (DetectTierTablet())
				return false;

			//Most mobile browsing is done on smartphones
			if (DetectSmartphone())
				return true;

			if (DetectWapWml() ||
				 DetectBrewDevice() ||
				 DetectOperaMobile())
				return true;

			if ((_useragent.IndexOf(_engineNetfront) != -1) ||
				 (_useragent.IndexOf(_engineUpBrowser) != -1) ||
				 (_useragent.IndexOf(_engineOpenWeb) != -1))
				return true;

			if (DetectDangerHiptop() ||
				 DetectMidpCapable() ||
				 DetectMaemoTablet() ||
				 DetectArchos())
				return true;

			if ((_useragent.IndexOf(_devicePda) != -1) &&
				 (_useragent.IndexOf(_disUpdate) < 0)) //no index found
				return true;
			if (_useragent.IndexOf(_mobile) != -1)
				return true;

			else
				return false;
		}

		//DetectMobileQuick delegate
		public delegate void DetectMobileQuickHandler(object page, MDetectArgs args);
		public event DetectMobileQuickHandler OnDetectMobileQuick;


		//**************************
		// Detects if the current device is a Sony Playstation.
		public bool DetectSonyPlaystation()
		{
			if (_useragent.IndexOf(_devicePlaystation) != -1)
				return true;
			else
				return false;
		}

		//**************************
		// Detects if the current device is a Nintendo game device.
		public bool DetectNintendo()
		{
			if (_useragent.IndexOf(_deviceNintendo) != -1 ||
				  _useragent.IndexOf(_deviceWii) != -1 ||
				  _useragent.IndexOf(_deviceNintendoDs) != -1)
				return true;
			else
				return false;
		}

		//**************************
		// Detects if the current device is a Microsoft Xbox.
		public bool DetectXbox()
		{
			if (_useragent.IndexOf(_deviceXbox) != -1)
				return true;
			else
				return false;
		}

		//**************************
		// Detects if the current device is an Internet-capable game console.
		public bool DetectGameConsole()
		{
			if (DetectSonyPlaystation())
				return true;
			else if (DetectNintendo())
				return true;
			else if (DetectXbox())
				return true;
			else
				return false;
		}

		//**************************
		// Detects if the current device supports MIDP, a mobile Java technology.
		public bool DetectMidpCapable()
		{
			if (_useragent.IndexOf(_deviceMidp) != -1 ||
				 _httpaccept.IndexOf(_deviceMidp) != -1)
				return true;
			else
				return false;
		}

		//**************************
		// Detects if the current device is on one of the Maemo-based Nokia Internet Tablets.
		public bool DetectMaemoTablet()
		{
			if (_useragent.IndexOf(_maemo) != -1)
				return true;
			//For Nokia N810, must be Linux + Tablet, or else it could be something else. 
			else if (_useragent.IndexOf(_linux) != -1 &&
				 _useragent.IndexOf(_deviceTablet) != -1 &&
				 !DetectWebOsTablet() &&
				 !DetectAndroid())
				return true;
			else
				return false;
		}

		//**************************
		// Detects if the current device is an Archos media player/Internet tablet.
		public bool DetectArchos()
		{
			if (_useragent.IndexOf(_deviceArchos) != -1)
				return true;
			else
				return false;
		}

		//**************************
		// Detects if the current browser is a Sony Mylo device.
		public bool DetectSonyMylo()
		{
			if (_useragent.IndexOf(_manuSony) != -1)
			{
				if ((_useragent.IndexOf(_qtembedded) != -1) ||
				 (_useragent.IndexOf(_mylocom2) != -1))
				{
					return true;
				}
				else
					return false;
			}
			else
				return false;
		}

		//**************************
		// The longer and more thorough way to detect for a mobile device.
		//   Will probably detect most feature phones,
		//   smartphone-class devices, Internet Tablets, 
		//   Internet-enabled game consoles, etc.
		//   This ought to catch a lot of the more obscure and older devices, also --
		//   but no promises on thoroughness!
		public bool DetectMobileLong()
		{
			if (DetectMobileQuick())
				return true;
			if (DetectGameConsole() ||
				 DetectSonyMylo())
				return true;

			//Detect older phones from certain manufacturers and operators. 
			if (_useragent.IndexOf(_uplink) != -1)
				return true;
			if (_useragent.IndexOf(_manuSonyEricsson) != -1)
				return true;
			if (_useragent.IndexOf(_manuericsson) != -1)
				return true;
			if (_useragent.IndexOf(_manuSamsung1) != -1)
				return true;

			if (_useragent.IndexOf(_svcDocomo) != -1)
				return true;
			if (_useragent.IndexOf(_svcKddi) != -1)
				return true;
			if (_useragent.IndexOf(_svcVodafone) != -1)
				return true;

			else
				return false;
		}



		//*****************************
		// For Mobile Web Site Design
		//*****************************

		//**************************
		// The quick way to detect for a tier of devices.
		//   This method detects for the new generation of
		//   HTML 5 capable, larger screen tablets.
		//   Includes iPad, Android (e.g., Xoom), BB Playbook, WebOS, etc.
		public bool DetectTierTablet()
		{
			if (DetectIpad()
				 || DetectAndroidTablet()
				 || DetectBlackBerryTablet()
				 || DetectWebOsTablet())
				return true;
			else
				return false;
		}

		//DetectTierTablet delegate
		public delegate void DetectTierTabletHandler(object page, MDetectArgs args);
		public event DetectTierTabletHandler OnDetectTierTablet;


		//**************************
		// The quick way to detect for a tier of devices.
		//   This method detects for devices which can 
		//   display iPhone-optimized web content.
		//   Includes iPhone, iPod Touch, Android, etc.
		public bool DetectTierIphone()
		{
			if (DetectIphoneOrIpod() ||
				 DetectAndroidPhone() ||
				 (DetectBlackBerryWebKit() &&
					  DetectBlackBerryTouch()) ||
				 DetectPalmWebOs() ||
				 DetectGarminNuvifone())
				return true;
			else
				return false;
		}

		//DetectTierIphone delegate
		public delegate void DetectTierIphoneHandler(object page, MDetectArgs args);
		public event DetectTierIphoneHandler OnDetectTierIphone;


		//**************************
		// The quick way to detect for a tier of devices.
		//   This method detects for devices which are likely to be capable 
		//   of viewing CSS content optimized for the iPhone, 
		//   but may not necessarily support JavaScript.
		//   Excludes all iPhone Tier devices.
		public bool DetectTierRichCss()
		{
			if (DetectMobileQuick())
			{
				if (DetectTierIphone())
					return false;

				if (DetectWebkit() ||
					 DetectS60OssBrowser())
					return true;

				//Note: 'High' BlackBerry devices ONLY
				if (DetectBlackBerryHigh() == true)
					return true;

				//WP7's IE-7-based browser isn't good enough for iPhone Tier.
				if (DetectWindowsPhone7() == true)
					return true;
				if (DetectWindowsMobile() == true)
					return true;
				if (_useragent.IndexOf(_engineTelecaQ) != -1)
					return true;

				else
					return false;
			}
			else
				return false;
		}

		//DetectTierRichCss delegate
		public delegate void DetectTierRichCssHandler(object page, MDetectArgs args);
		public event DetectTierRichCssHandler OnDetectTierRichCss;


		//**************************
		// The quick way to detect for a tier of devices.
		//   This method detects for all other types of phones,
		//   but excludes the iPhone and Smartphone Tier devices.
		public bool DetectTierOtherPhones()
		{
			if (DetectMobileLong() == true)
			{
				//Exclude devices in the other 2 categories
				if (DetectTierIphone() ||
					 DetectTierRichCss())
					return false;
				else
					return true;
			}
			else
				return false;
		}

		//DetectTierOtherPhones delegate
		public delegate void DetectTierOtherPhonesHandler(object page, MDetectArgs args);
		public event DetectTierOtherPhonesHandler OnDetectTierOtherPhones;

		//***************************************************************
		#endregion

		protected override void OnPreInit(EventArgs e)
		{
			base.OnPreInit(e);
			_useragent = Request.ServerVariables["HTTP_USER_AGENT"].ToUpper();
			_httpaccept = Request.ServerVariables["HTTP_ACCEPT"].ToUpper();
		}
	}

}
