// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using Playground.Core.ViewModels;

namespace Playground.Droid.Activities
{
    [MvxActivityPresentation]
    [Activity(Theme = "@style/AppTheme", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class TabsRootView : MvxAppCompatActivity<TabsRootViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.TabsRootView);

            var viewPager = FindViewById<ViewPager>(Resource.Id.viewpager);
            viewPager.Adapter = new TabStatePagerAdapter(this, this.SupportFragmentManager, new List<MvxViewPagerFragmentInfo>());

            if (bundle == null)
            {
                ViewModel.ShowInitialViewModelsCommand.Execute();
            }
            else {
                var tabLayout = FindViewById<TabLayout>(Resource.Id.tabs);
                tabLayout.SetupWithViewPager(viewPager);
            }
        }
    }
}
