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
using System.Web.UI;

// ReSharper disable ConvertToConstant.Local
namespace Singularity.Web
{
    /// <summary>
    /// Subclass this page to inherit the built-in mobile device detection.
    /// </summary>
    public class MDetect 
    {

        private static string _useragent = "";
        private static string _httpaccept = "";

        #region Fields - Detection Argument Values

        //standardized values for detection arguments.
        private static readonly string DargsIphone = "iphone";
        private static readonly string DargsIpod = "ipod";
        private static readonly string DargsIpad = "ipad";
        private static readonly string DargsIphoneOrIpod = "iphoneoripod";
        private static readonly string DargsIos = "ios";
        private static readonly string DargsAndroid = "android";
        private static readonly string DargsAndroidPhone = "androidphone";
        private static readonly string DargsAndroidTablet = "androidtablet";
        private static readonly string DargsGoogleTv = "googletv";
        private static readonly string DargsWebKit = "webkit";
        private static readonly string DargsSymbianOs = "symbianos";
        private static readonly string DargsS60 = "series60";
        private static readonly string DargsWindowsPhone7 = "windowsphone7";
        private static readonly string DargsWindowsMobile = "windowsmobile";
        //private static readonly string _dargsBlackBerry = "blackberry";
        private static readonly string DargsBlackBerryWebkit = "blackberrywebkit";
        private static readonly string DargsPalmOs = "palmos";
        private static readonly string DargsPalmWebOs = "webos";
        private static readonly string DargsWebOsTablet = "webostablet";
        private static readonly string DargsSmartphone = "smartphone";
        private static readonly string DargsBrewDevice = "brew";
        private static readonly string DargsDangerHiptop = "dangerhiptop";
        private static readonly string DargsOperaMobile = "operamobile";
        private static readonly string DargsWapWml = "wapwml";
        private static readonly string DargsKindle = "kindle";
        private static readonly string DargsMobileQuick = "mobilequick";
        private static readonly string DargsTierTablet = "tiertablet";
        private static readonly string DargsTierIphone = "tieriphone";
        private static readonly string DargsTierRichCss = "tierrichcss";
        private static readonly string DargsTierOtherPhones = "tierotherphones";

        #endregion Fields - Detection Argument Values

        #region Fields - User Agent Keyword Values

        //Initialize some initial smartphone private static string private static stringiables.
        private static readonly string EngineWebKit = "webkit".ToUpper();
        private static readonly string DeviceIphone = "iphone".ToUpper();
        private static readonly string DeviceIpod = "ipod".ToUpper();
        private static readonly string DeviceIpad = "ipad".ToUpper();
        private static readonly string DeviceMacPpc = "macintosh".ToUpper(); //Used for disambiguation

        private static readonly string DeviceAndroid = "android".ToUpper();
        private static readonly string DeviceGoogleTv = "googletv".ToUpper();
        //private static readonly string DeviceXoom = "xoom".ToUpper(); //Motorola Xoom
        private static readonly string DeviceHtcFlyer = "htc_flyer".ToUpper(); //HTC Flyer

        private static readonly string DeviceNuvifone = "nuvifone".ToUpper();  //Garmin Nuvifone

        private static readonly string DeviceSymbian = "symbian".ToUpper();
        private static readonly string DeviceS60 = "series60".ToUpper();
        private static readonly string DeviceS70 = "series70".ToUpper();
        private static readonly string DeviceS80 = "series80".ToUpper();
        private static readonly string DeviceS90 = "series90".ToUpper();

        private static readonly string DeviceWinPhone7 = "windows phone os 7".ToUpper();
        private static readonly string DeviceWinMob = "windows ce".ToUpper();
        private static readonly string DeviceWindows = "windows".ToUpper();
        private static readonly string DeviceIeMob = "iemobile".ToUpper();
        private static readonly string DevicePpc = "ppc".ToUpper(); //Stands for PocketPC
        private static readonly string EnginePie = "wm5 pie".ToUpper(); //An old Windows Mobile

        private static readonly string DeviceBb = "blackberry".ToUpper();
        private static readonly string VndRim = "vnd.rim".ToUpper(); //Detectable when BB devices emulate IE or Firefox
        private static readonly string DeviceBbStorm = "blackberry95".ToUpper(); //Storm 1 and 2
        private static readonly string DeviceBbBold = "blackberry97".ToUpper(); //Bold
        private static readonly string DeviceBbTour = "blackberry96".ToUpper(); //Tour
        private static readonly string DeviceBbCurve = "blackberry89".ToUpper(); //Curve2
        private static readonly string DeviceBbTorch = "blackberry 98".ToUpper(); //Torch
        private static readonly string DeviceBbPlaybook = "playbook".ToUpper(); //PlayBook tablet

        private static readonly string DevicePalm = "palm".ToUpper();
        private static readonly string DeviceWebOs = "webos".ToUpper(); //For Palm's line of WebOS devices
        private static readonly string DeviceWebOShp = "hpwos".ToUpper(); //For HP's line of WebOS devices

        private static readonly string EngineBlazer = "blazer".ToUpper(); //Old Palm
        private static readonly string EngineXiino = "xiino".ToUpper(); //Another old Palm

        private static readonly string DeviceKindle = "kindle".ToUpper();  //Amazon Kindle, eInk one.

        //Initialize private static stringiables for mobile-specific content.
        private static readonly string Vndwap = "vnd.wap".ToUpper();
        private static readonly string Wml = "wml".ToUpper();

        //Initialize private static stringiables for other random devices and mobile browsers.
        private static readonly string DeviceTablet = "tablet".ToUpper(); //Generic term for slate and tablet devices
        private static readonly string DeviceBrew = "brew".ToUpper();
        private static readonly string DeviceDanger = "danger".ToUpper();
        private static readonly string DeviceHiptop = "hiptop".ToUpper();
        private static readonly string DevicePlaystation = "playstation".ToUpper();
        private static readonly string DeviceNintendoDs = "nitro".ToUpper();
        private static readonly string DeviceNintendo = "nintendo".ToUpper();
        private static readonly string DeviceWii = "wii".ToUpper();
        private static readonly string DeviceXbox = "xbox".ToUpper();
        private static readonly string DeviceArchos = "archos".ToUpper();

        private static readonly string EngineOpera = "opera".ToUpper(); //Popular browser
        private static readonly string EngineNetfront = "netfront".ToUpper(); //Common embedded OS browser
        private static readonly string EngineUpBrowser = "up.browser".ToUpper(); //common on some phones
        private static readonly string EngineOpenWeb = "openweb".ToUpper(); //Transcoding by OpenWave server
        private static readonly string DeviceMidp = "midp".ToUpper(); //a mobile Java technology
        private static readonly string Uplink = "up.link".ToUpper();
        private static readonly string EngineTelecaQ = "teleca q".ToUpper(); //a modern feature phone browser

        private static readonly string DevicePda = "pda".ToUpper(); //some devices report themselves as PDAs
        private static readonly string Mini = "mini".ToUpper();  //Some mobile browsers put "mini" in their names.
        private static readonly string Mobile = "mobile".ToUpper(); //Some mobile browsers put "mobile" in their user agent private static strings.
        private static readonly string Mobi = "mobi".ToUpper(); //Some mobile browsers put "mobi" in their user agent private static strings.

        //Use Maemo, Tablet, and Linux to test for Nokia"s Internet Tablets.
        private static readonly string Maemo = "maemo".ToUpper();
        private static readonly string Linux = "linux".ToUpper();
        private static readonly string Qtembedded = "qt embedded".ToUpper(); //for Sony Mylo
        private static readonly string Mylocom2 = "com2".ToUpper(); //for Sony Mylo also

        //In some UserAgents, the only clue is the manufacturer.
        private static readonly string ManuSonyEricsson = "sonyericsson".ToUpper();
        private static readonly string Manuericsson = "ericsson".ToUpper();
        private static readonly string ManuSamsung1 = "sec-sgh".ToUpper();
        private static readonly string ManuSony = "sony".ToUpper();
        private static readonly string ManuHtc = "htc".ToUpper(); //Popular Android and WinMo manufacturer

        //In some UserAgents, the only clue is the operator.
        private static readonly string SvcDocomo = "docomo".ToUpper();
        private static readonly string SvcKddi = "kddi".ToUpper();
        private static readonly string SvcVodafone = "vodafone".ToUpper();

        //Disambiguation strings.
        private static readonly string DisUpdate = "update".ToUpper(); //pda vs. update

        #endregion Fields - User Agent Keyword Values

        static public BrowserDeviceType DetectDevice(Page page)
        {
            BrowserDeviceType type = BrowserDeviceType.Unknown;
            if (_useragent == "" && _httpaccept == "")
            {
                _useragent = (page.Request.ServerVariables["HTTP_USER_AGENT"] ?? "").ToUpper();
                _httpaccept = (page.Request.ServerVariables["HTTP_ACCEPT"] ?? "").ToUpper();
            }
            if (DetectTierTablet())
                type = BrowserDeviceType.Tablet;
            else if (DetectSmartphone())
                type = BrowserDeviceType.SmartPhone;
            else
                type = BrowserDeviceType.Desktop;

            return type;
        
        }


        /// <summary>
        /// To run the device detection methods andd fire 
        /// any existing OnDetectXXX events. 
        /// </summary>
        static public MDetectArgs FireEvents(Page page)
        {
            if (_useragent.IsEmpty() && _httpaccept.IsEmpty())
            {
                _useragent = (page.Request.ServerVariables["HTTP_USER_AGENT"] ?? "").ToUpper();
                _httpaccept = (page.Request.ServerVariables["HTTP_ACCEPT"] ?? "").ToUpper();
            }

            #region Event Fire Methods

            MDetectArgs mda = null;
            if (DetectIpod())
            {
                mda = new MDetectArgs(DargsIpod);
               
            }
            if (DetectIpad())
            {
                mda = new MDetectArgs(DargsIpad);
                
            }
            if (DetectIphone())
            {
                mda = new MDetectArgs(DargsIphone);
                
            }
            if (DetectIphoneOrIpod())
            {
                mda = new MDetectArgs(DargsIphoneOrIpod);
               
            }
            if (DetectIos())
            {
                mda = new MDetectArgs(DargsIos);
               
            }
            if (DetectAndroid())
            {
                mda = new MDetectArgs(DargsAndroid);
                
            }
            if (DetectAndroidPhone())
            {
                mda = new MDetectArgs(DargsAndroidPhone);
                 
            }
            if (DetectAndroidTablet())
            {
                mda = new MDetectArgs(DargsAndroidTablet);
                
            }
            if (DetectGoogleTv())
            {
                mda = new MDetectArgs(DargsGoogleTv);
                
            }
            if (DetectWebkit())
            {
                mda = new MDetectArgs(DargsWebKit);
               
            }
            if (DetectS60OssBrowser())
            {
                mda = new MDetectArgs(DargsS60);
               
            }
            if (DetectSymbianOs())
            {
                mda = new MDetectArgs(DargsSymbianOs);
               
            }
            if (DetectWindowsPhone7())
            {
                mda = new MDetectArgs(DargsWindowsPhone7);
                
            }
            if (DetectWindowsMobile())
            {
                mda = new MDetectArgs(DargsWindowsMobile);
                
            }
            if (DetectBlackBerry())
            {
                mda = new MDetectArgs(DargsBlackBerryWebkit);
                
            }
            if (DetectBlackBerryWebKit())
            {
                mda = new MDetectArgs(DargsBlackBerryWebkit);
               
            }
            if (DetectPalmOs())
            {
                mda = new MDetectArgs(DargsPalmOs);
                
            }
            if (DetectPalmWebOs())
            {
                mda = new MDetectArgs(DargsPalmWebOs);
               
            }
            if (DetectWebOsTablet())
            {
                mda = new MDetectArgs(DargsWebOsTablet);
                
            }
            if (DetectSmartphone())
            {
                mda = new MDetectArgs(DargsSmartphone);
                
            }
            if (DetectBrewDevice())
            {
                mda = new MDetectArgs(DargsBrewDevice);
               
            }
            if (DetectDangerHiptop())
            {
                mda = new MDetectArgs(DargsDangerHiptop);
               
            }
            if (DetectOperaMobile())
            {
                mda = new MDetectArgs(DargsOperaMobile);
                
            }
            if (DetectWapWml())
            {
                mda = new MDetectArgs(DargsWapWml);
                
            }
            if (DetectKindle())
            {
                mda = new MDetectArgs(DargsKindle);
                
            }
            if (DetectMobileQuick())
            {
                mda = new MDetectArgs(DargsMobileQuick);
                
            }
            if (DetectTierTablet())
            {
                mda = new MDetectArgs(DargsTierTablet);
                
            }
            if (DetectTierIphone())
            {
                mda = new MDetectArgs(DargsTierIphone);
                
            }
            if (DetectTierRichCss())
            {
                mda = new MDetectArgs(DargsTierRichCss);
                
            }
            if (DetectTierOtherPhones())
            {
                mda = new MDetectArgs(DargsTierOtherPhones);
                
            }

            #endregion Event Fire Methods
            return mda;
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
        static public bool DetectIpod()
        {
            if (_useragent.IndexOf(DeviceIpod) != -1)
                return true;
            else
                return false;
        }

        //Ipod delegate
        public delegate void DetectIpodHandler(object page, MDetectArgs args);
        public event DetectIpodHandler OnDetectIpod;


        //**************************
        // Detects if the current device is an iPad tablet.
        static public bool DetectIpad()
        {
            if (_useragent.IndexOf(DeviceIpad) != -1 && DetectWebkit())
                return true;
            else
                return false;
        }

        //Ipod delegate
        public delegate void DetectIpadHandler(object page, MDetectArgs args);
        public event DetectIpadHandler OnDetectIpad;


        //**************************
        // Detects if the current device is an iPhone.
        static public bool DetectIphone()
        {
            if (_useragent.IndexOf(DeviceIphone) != -1)
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
        static public bool DetectIphoneOrIpod()
        {
            //We repeat the searches here because some iPods may report themselves as an iPhone, which would be okay.
            if (_useragent.IndexOf(DeviceIphone) != -1 ||
                _useragent.IndexOf(DeviceIpod) != -1)
                return true;
            else
                return false;
        }
        //IPhoneOrIpod delegate
        public delegate void DetectIPhoneOrIpodHandler(object page, MDetectArgs args);
        public event DetectIPhoneOrIpodHandler OnDetectDetectIPhoneOrIpod;

        //**************************
        // Detects *any* iOS device: iPhone, iPod Touch, iPad.
        static public bool DetectIos()
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
        static public bool DetectAndroid()
        {
            if ((_useragent.IndexOf(DeviceAndroid) != -1) ||
                DetectGoogleTv())
                return true;
            //Special check for the HTC Flyer 7" tablet. It should report here.
            if (_useragent.IndexOf(DeviceHtcFlyer) != -1)
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
        static public bool DetectAndroidPhone()
        {
            if (DetectAndroid() &&
                (_useragent.IndexOf(Mobile) != -1))
                return true;
            //Special check for Android phones with Opera Mobile. They should report here.
            if (DetectOperaAndroidPhone())
                return true;
            //Special check for the HTC Flyer 7" tablet. It should report here.
            if (_useragent.IndexOf(DeviceHtcFlyer) != -1)
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
        static public bool DetectAndroidTablet()
        {
            //First, let's make sure we're on an Android device.
            if (!DetectAndroid())
                return false;

            //Special check for Opera Android Phones. They should NOT report here.
            if (DetectOperaMobile())
                return false;
            //Special check for the HTC Flyer 7" tablet. It should NOT report here.
            if (_useragent.IndexOf(DeviceHtcFlyer) != -1)
                return false;

            //Otherwise, if it's Android and does NOT have 'mobile' in it, Google says it's a tablet.
            if (_useragent.IndexOf(Mobile) > -1)
                return false;
            else
                return true;
        }
        //Android Tablet delegate
        public delegate void DetectAndroidTabletHandler(object page, MDetectArgs args);
        public event DetectAndroidTabletHandler OnDetectAndroidTablet;

        //**************************
        // Detects if the current device is a GoogleTV device.
        static public bool DetectGoogleTv()
        {
            if (_useragent.IndexOf(DeviceGoogleTv) != -1)
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
        static public bool DetectAndroidWebKit()
        {
            if (DetectAndroid() && DetectWebkit())
                return true;
            else
                return false;
        }

        //**************************
        // Detects if the current browser is based on WebKit.
        static public bool DetectWebkit()
        {
            if (_useragent.IndexOf(EngineWebKit) != -1)
                return true;
            else
                return false;
        }

        //Webkit delegate
        public delegate void DetectWebkitHandler(object page, MDetectArgs args);
        public event DetectWebkitHandler OnDetectWebkit;

        //**************************
        // Detects if the current browser is the Nokia S60 Open Source Browser.
        static public bool DetectS60OssBrowser()
        {
            //First, test for WebKit, then make sure it's either Symbian or S60.
            if (DetectWebkit())
            {
                if (_useragent.IndexOf(DeviceSymbian) != -1 ||
                    _useragent.IndexOf(DeviceS60) != -1)
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
        static public bool DetectSymbianOs()
        {
            if (_useragent.IndexOf(DeviceSymbian) != -1 ||
                _useragent.IndexOf(DeviceS60) != -1 ||
                _useragent.IndexOf(DeviceS70) != -1 ||
                _useragent.IndexOf(DeviceS80) != -1 ||
                _useragent.IndexOf(DeviceS90) != -1)
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
        static public bool DetectWindowsPhone7()
        {
            if (_useragent.IndexOf(DeviceWinPhone7) != -1)
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
        static public bool DetectWindowsMobile()
        {
            //Exclude new Windows Phone 7.
            if (DetectWindowsPhone7())
                return false;
            //Most devices use 'Windows CE', but some report 'iemobile' 
            //  and some older ones report as 'PIE' for Pocket IE. 
            if (_useragent.IndexOf(DeviceWinMob) != -1 ||
                _useragent.IndexOf(DeviceIeMob) != -1 ||
                _useragent.IndexOf(EnginePie) != -1)
                return true;
            //Test for Windows Mobile PPC but not old Macintosh PowerPC.
            if (_useragent.IndexOf(DevicePpc) != -1 &&
                !(_useragent.IndexOf(DeviceMacPpc) != -1))
                return true;
            //Test for certain Windwos Mobile-based HTC devices.
            if (_useragent.IndexOf(ManuHtc) != -1 &&
                _useragent.IndexOf(DeviceWindows) != -1)
                return true;
            if (DetectWapWml() == true &&
                _useragent.IndexOf(DeviceWindows) != -1)
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
        static public bool DetectBlackBerry()
        {
            if ((_useragent.IndexOf(DeviceBb) != -1) ||
                (_httpaccept.IndexOf(VndRim) != -1))
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
        static public bool DetectBlackBerryTablet()
        {
            if (_useragent.IndexOf(DeviceBbPlaybook) != -1)
                return true;
            else
                return false;
        }

        //**************************
        // Detects if the current browser is a BlackBerry device AND uses a
        //    WebKit-based browser. These are signatures for the new BlackBerry OS 6.
        //    Examples: Torch. Includes the Playbook.
        static public bool DetectBlackBerryWebKit()
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
        static public bool DetectBlackBerryTouch()
        {
            if (DetectBlackBerry() &&
                (_useragent.IndexOf(DeviceBbStorm) != -1 ||
                _useragent.IndexOf(DeviceBbTorch) != -1))
                return true;
            else
                return false;
        }

        //**************************
        // Detects if the current browser is a BlackBerry device AND
        //    has a more capable recent browser. Excludes the Playbook.
        //    Examples, Storm, Bold, Tour, Curve2
        //    Excludes the new BlackBerry OS 6 browser!!
        static public bool DetectBlackBerryHigh()
        {
            //Disambiguate for BlackBerry OS 6 (WebKit) browser
            if (DetectBlackBerryWebKit())
                return false;
            if (DetectBlackBerry())
            {
                if (DetectBlackBerryTouch() ||
                    _useragent.IndexOf(DeviceBbBold) != -1 ||
                    _useragent.IndexOf(DeviceBbTour) != -1 ||
                    _useragent.IndexOf(DeviceBbCurve) != -1)
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
        static public bool DetectBlackBerryLow()
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
        static public bool DetectPalmOs()
        {
            //Most devices nowadays report as 'Palm', but some older ones reported as Blazer or Xiino.
            if (_useragent.IndexOf(DevicePalm) != -1 ||
                _useragent.IndexOf(EngineBlazer) != -1 ||
                _useragent.IndexOf(EngineXiino) != -1)
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
        static public bool DetectPalmWebOs()
        {
            if (_useragent.IndexOf(DeviceWebOs) != -1)
                return true;
            else
                return false;
        }

        //PalmWebOS delegate
        public delegate void DetectPalmWebOsHandler(object page, MDetectArgs args);
        public event DetectPalmWebOsHandler OnDetectPalmWebOs;


        //**************************
        // Detects if the current browser is on an HP tablet running WebOS.
        static public bool DetectWebOsTablet()
        {
            if (_useragent.IndexOf(DeviceWebOShp) != -1 &&
                _useragent.IndexOf(DeviceTablet) != -1)
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
        static public bool DetectGarminNuvifone()
        {
            if (_useragent.IndexOf(DeviceNuvifone) != -1)
                return true;
            else
                return false;
        }


        //**************************
        // Check to see whether the device is any device
        //   in the 'smartphone' category.
        static public bool DetectSmartphone()
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
        static public bool DetectBrewDevice()
        {
            if (_useragent.IndexOf(DeviceBrew) != -1)
                return true;
            else
                return false;
        }

        //BrewDevice delegate
        public delegate void DetectBrewDeviceHandler(object page, MDetectArgs args);
        public event DetectBrewDeviceHandler OnDetectBrewDevice;

        //**************************
        // Detects the Danger Hiptop device.
        static public bool DetectDangerHiptop()
        {
            if (_useragent.IndexOf(DeviceDanger) != -1 ||
                _useragent.IndexOf(DeviceHiptop) != -1)
                return true;
            else
                return false;
        }
        //DangerHiptop delegate
        public delegate void DetectDangerHiptopHandler(object page, MDetectArgs args);
        public event DetectDangerHiptopHandler OnDetectDangerHiptop;

        //**************************
        // Detects if the current browser is Opera Mobile or Mini.
        static public bool DetectOperaMobile()
        {
            if (_useragent.IndexOf(EngineOpera) != -1)
            {
                if ((_useragent.IndexOf(Mini) != -1) ||
                 (_useragent.IndexOf(Mobi) != -1))
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
        static public bool DetectOperaAndroidPhone()
        {
            if ((_useragent.IndexOf(EngineOpera) != -1) &&
                (_useragent.IndexOf(DeviceAndroid) != -1) &&
                (_useragent.IndexOf(Mobi) != -1))
                return true;
            else
                return false;
        }

        // Detects if the current browser is Opera Mobile
        // running on an Android tablet.
        static public bool DetectOperaAndroidTablet()
        {
            if ((_useragent.IndexOf(EngineOpera) != -1) &&
                (_useragent.IndexOf(DeviceAndroid) != -1) &&
                (_useragent.IndexOf(DeviceTablet) != -1))
                return true;
            else
                return false;
        }

        //**************************
        // Detects whether the device supports WAP or WML.
        static public bool DetectWapWml()
        {
            if (_httpaccept.IndexOf(Vndwap) != -1 ||
                _httpaccept.IndexOf(Wml) != -1)
                return true;
            else
                return false;
        }
        //WapWml delegate
        public delegate void DetectWapWmlHandler(object page, MDetectArgs args);
        public event DetectWapWmlHandler OnDetectWapWml;


        //**************************
        // Detects if the current device is an Amazon Kindle.
        static public bool DetectKindle()
        {
            if (_useragent.IndexOf(DeviceKindle) != -1)
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
        static public bool DetectMobileQuick()
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

            if ((_useragent.IndexOf(EngineNetfront) != -1) ||
                (_useragent.IndexOf(EngineUpBrowser) != -1) ||
                (_useragent.IndexOf(EngineOpenWeb) != -1))
                return true;

            if (DetectDangerHiptop() ||
                DetectMidpCapable() ||
                DetectMaemoTablet() ||
                DetectArchos())
                return true;

            if ((_useragent.IndexOf(DevicePda) != -1) &&
                (_useragent.IndexOf(DisUpdate) < 0)) //no index found
                return true;
            if (_useragent.IndexOf(Mobile) != -1)
                return true;

            else
                return false;
        }

        //DetectMobileQuick delegate
        public delegate void DetectMobileQuickHandler(object page, MDetectArgs args);
        public event DetectMobileQuickHandler OnDetectMobileQuick;


        //**************************
        // Detects if the current device is a Sony Playstation.
        static public bool DetectSonyPlaystation()
        {
            if (_useragent.IndexOf(DevicePlaystation) != -1)
                return true;
            else
                return false;
        }

        //**************************
        // Detects if the current device is a Nintendo game device.
        static public bool DetectNintendo()
        {
            if (_useragent.IndexOf(DeviceNintendo) != -1 ||
                 _useragent.IndexOf(DeviceWii) != -1 ||
                 _useragent.IndexOf(DeviceNintendoDs) != -1)
                return true;
            else
                return false;
        }

        //**************************
        // Detects if the current device is a Microsoft Xbox.
        static public bool DetectXbox()
        {
            if (_useragent.IndexOf(DeviceXbox) != -1)
                return true;
            else
                return false;
        }

        //**************************
        // Detects if the current device is an Internet-capable game console.
        static public bool DetectGameConsole()
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
        static public bool DetectMidpCapable()
        {
            if (_useragent.IndexOf(DeviceMidp) != -1 ||
                _httpaccept.IndexOf(DeviceMidp) != -1)
                return true;
            else
                return false;
        }

        //**************************
        // Detects if the current device is on one of the Maemo-based Nokia Internet Tablets.
        static public bool DetectMaemoTablet()
        {
            if (_useragent.IndexOf(Maemo) != -1)
                return true;
            //For Nokia N810, must be Linux + Tablet, or else it could be something else. 
            else if (_useragent.IndexOf(Linux) != -1 &&
                _useragent.IndexOf(DeviceTablet) != -1 &&
                !DetectWebOsTablet() &&
                !DetectAndroid())
                return true;
            else
                return false;
        }

        //**************************
        // Detects if the current device is an Archos media player/Internet tablet.
        static public bool DetectArchos()
        {
            if (_useragent.IndexOf(DeviceArchos) != -1)
                return true;
            else
                return false;
        }

        //**************************
        // Detects if the current browser is a Sony Mylo device.
        static public bool DetectSonyMylo()
        {
            if (_useragent.IndexOf(ManuSony) != -1)
            {
                if ((_useragent.IndexOf(Qtembedded) != -1) ||
                 (_useragent.IndexOf(Mylocom2) != -1))
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
        static public bool DetectMobileLong()
        {
            if (DetectMobileQuick())
                return true;
            if (DetectGameConsole() ||
                DetectSonyMylo())
                return true;

            //Detect older phones from certain manufacturers and operators. 
            if (_useragent.IndexOf(Uplink) != -1)
                return true;
            if (_useragent.IndexOf(ManuSonyEricsson) != -1)
                return true;
            if (_useragent.IndexOf(Manuericsson) != -1)
                return true;
            if (_useragent.IndexOf(ManuSamsung1) != -1)
                return true;

            if (_useragent.IndexOf(SvcDocomo) != -1)
                return true;
            if (_useragent.IndexOf(SvcKddi) != -1)
                return true;
            if (_useragent.IndexOf(SvcVodafone) != -1)
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
        static public bool DetectTierTablet()
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
        static public bool DetectTierIphone()
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
        static public bool DetectTierRichCss()
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
                if (_useragent.IndexOf(EngineTelecaQ) != -1)
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
        static public bool DetectTierOtherPhones()
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

       
    }

}
