using System;
using Foundation;
using UIKit;
using Template.Interfaces;

namespace Template.Platforms.Source
{
    public class DeviceOrientation : IDeviceOrientationService
    {
        private static readonly IReadOnlyDictionary<DisplayOrientation, UIInterfaceOrientation> _iosDisplayOrientationMap =
            new Dictionary<DisplayOrientation, UIInterfaceOrientation>
            {
                [DisplayOrientation.Landscape] = UIInterfaceOrientation.LandscapeLeft,
                [DisplayOrientation.Portrait] = UIInterfaceOrientation.Portrait,
            };

        private static readonly IReadOnlyDictionary<DisplayOrientation, UIInterfaceOrientationMask> _iosDisplayOrientationMaskMap =
            new Dictionary<DisplayOrientation, UIInterfaceOrientationMask>
            {
                [DisplayOrientation.Landscape] = UIInterfaceOrientationMask.LandscapeLeft,
                [DisplayOrientation.Portrait] = UIInterfaceOrientationMask.Portrait,
            };

        public void SetDeviceOrientation(DisplayOrientation displayOrientation)
        {


            if (UIDevice.CurrentDevice.CheckSystemVersion(16, 0))
            {
                if (_iosDisplayOrientationMaskMap.TryGetValue(displayOrientation, out UIInterfaceOrientationMask screenOrientation))
                {

                    var scene = (UIApplication.SharedApplication.ConnectedScenes.ToArray()[0] as UIWindowScene);
                    if (scene != null)
                    {
                        var uiAppplication = UIApplication.SharedApplication;
                        var test = UIApplication.SharedApplication.KeyWindow?.RootViewController;
                        if (test != null)
                        {
                            test.SetNeedsUpdateOfSupportedInterfaceOrientations();
                            scene.RequestGeometryUpdate(
                                new UIWindowSceneGeometryPreferencesIOS(screenOrientation), error => { });
                        }
                    }
                }
            }
            else
            {
                if (_iosDisplayOrientationMap.TryGetValue(displayOrientation, out UIInterfaceOrientation screenOrientation))
                    UIDevice.CurrentDevice.SetValueForKey(new NSNumber((int)screenOrientation), new NSString("orientation"));
            }


        }
    }
}

