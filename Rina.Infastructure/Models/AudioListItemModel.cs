using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKapi.Models;
using System.Diagnostics.Contracts;

namespace Rina.Infastructure.Models
{
    public class AudioListItemModel : IEquatable<AudioListItemModel>
    {
        private readonly VKAudio item;
        private readonly Lazy<Uri> uriLazy;
        private readonly Lazy<TimeSpan> durationLazy;

        public VKAudio Audio
        {
            get { return this.item; }
        }

        public String Artist
        {
            get { return this.item.Artist; }
        }

        public String Title
        {
            get { return this.item.Title; }
        }

        public TimeSpan Duration
        {
            get { return this.durationLazy.Value; }
        }

        public Uri Uri
        {
            get { return this.uriLazy.Value; }
        }

        public AudioListItemModel(VKAudio item)
        {
            Contract.Requires(item != null);

            this.item = item;
            uriLazy = new Lazy<Uri>(() => new Uri(item.Url));
            durationLazy = new Lazy<TimeSpan>(() => TimeSpan.FromSeconds(item.Duration));
        }

        public override string ToString()
        {
            return this.Artist + " - " + this.Title;
        }

        public override int GetHashCode()
        {
            return this.item.Url.GetHashCode();
        }

        public Boolean Equals(AudioListItemModel other)
        {
            if ((Object)other == null) return false;

            return this.item.Url.Equals(other.item.Url, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            AudioListItemModel otherItem = obj as AudioListItemModel;
            if (otherItem == null) return false;

            return Equals(otherItem);
        }

        public static Boolean operator ==(AudioListItemModel left, AudioListItemModel right)
        {
            if ((Object)left == null && (Object)right == null) return true;
            if ((Object)left == null || (Object)right == null) return false;

            return left.Equals(right);
        }

        public static Boolean operator !=(AudioListItemModel left, AudioListItemModel right)
        {
            return !(left == right);
        }
    }
}
