using System;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Java.Interop;
using Java.Lang;

namespace Playground.Droid.Activities
{
    public class ParcelableFragInfo : Java.Lang.Object, IParcelable
    {
        public Type FragmentType;
        public Type ViewModelType;
        public string Title;
        public string Tag;

        [ExportField("CREATOR")]
        public static ParcelableFragInfoCreator InititalizeCreator()
        {
            return new ParcelableFragInfoCreator();
        }

        public ParcelableFragInfo()
        {
        }

        public ParcelableFragInfo(Parcel source) 
        {
            string fragmentType = source.ReadString();
            string viewModelType = source.ReadString();
            Title = source.ReadString();
            Tag = source.ReadString();

            FragmentType = Type.GetType(fragmentType);
            ViewModelType = Type.GetType(viewModelType);
        }

        public void WriteToParcel(Parcel dest, ParcelableWriteFlags flags)
        {
            dest.WriteString(FragmentType.AssemblyQualifiedName);
            dest.WriteString(ViewModelType.AssemblyQualifiedName);
            dest.WriteString(Title);
            dest.WriteString(Tag);
        }

        public int DescribeContents()
        {
            return 0;
        }
    }

    /// <summary>
    /// Auto sizing header widget parcelable creator.
    /// </summary>
    public sealed class ParcelableFragInfoCreator : Java.Lang.Object, IParcelableCreator
    {
        public Java.Lang.Object CreateFromParcel(Parcel source)
        {
            return new ParcelableFragInfo(source);
        }

        public Java.Lang.Object[] NewArray(int size)
        {
            return new ParcelableFragInfo[size];
        }
    }
}
