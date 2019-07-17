using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Java.Lang;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Platforms.Android.Views;

namespace Playground.Droid.Activities
{
    public class TabStatePagerAdapter : MvxCachingFragmentStatePagerAdapter
    {
        private const string _bundleFragInfosKey = "TabStatePagerAdapter_fragInfos";
        private FragmentManager _fragmentManager;

        protected TabStatePagerAdapter(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public TabStatePagerAdapter(Context context, FragmentManager fragmentManager,
            List<MvxViewPagerFragmentInfo> fragmentsInfo) : base(context, fragmentManager, fragmentsInfo)
        {
            _fragmentManager = fragmentManager;
        }

        public override IParcelable SaveState()
        {
            var bundle = base.SaveState() as Bundle;

            var fragInfos = FragmentsInfo;

            var array = new IParcelable[fragInfos.Count()];
            int i = 0;

            foreach (var fragInfo in fragInfos)
            {
                var parcelable = new ParcelableFragInfo();
                parcelable.FragmentType = fragInfo.FragmentType;
                parcelable.ViewModelType = fragInfo.ViewModelType;
                parcelable.Title = fragInfo.Title;
                parcelable.Tag = fragInfo.Tag;
                array[i] = parcelable;
                i++;
            }

            bundle.PutParcelableArray(_bundleFragInfosKey, array);

            return bundle;
        }

        public override void RestoreState(IParcelable state, ClassLoader loader)
        {
            base.RestoreState(state, loader);

            if (state is Bundle bundle)
            {
                var array = bundle.GetParcelableArray(_bundleFragInfosKey);

                if (array != null) {

                    var keys = bundle.KeySet();
                    var fragments = new List<Fragment>();

                    foreach (var key in keys)
                    {
                        if (!key.StartsWith("f"))
                            continue;

                        var index = Integer.ParseInt(key.Substring(1));

                        if (_fragmentManager.Fragments == null) return;

                        var f = _fragmentManager.GetFragment(bundle, key);
                        if (f != null)
                        {
                            while (fragments.Count() <= index)
                                fragments.Add(null);

                            fragments[index] = f;
                        }
                    }

                    int i = 0;

                    foreach (ParcelableFragInfo parcelable in array)
                    {
                        MvxViewPagerFragmentInfo fragInfo = null;

                        if (i < fragments.Count)
                        {
                            var f = fragments[i];
                            if (f is IMvxFragmentView fragment && fragment.ViewModel != null)
                            {
                                fragInfo = new MvxViewPagerFragmentInfo(parcelable.Title, parcelable.Tag, parcelable.FragmentType, fragment.ViewModel);
                            }
                        }
                        else
                        {
                            fragInfo = new MvxViewPagerFragmentInfo(parcelable.Title, parcelable.Tag, parcelable.FragmentType, parcelable.ViewModelType);
                        }
                        FragmentsInfo.Add(fragInfo);
                        i++;
                    }

                    NotifyDataSetChanged();
                }
            }
        }
    }
}
