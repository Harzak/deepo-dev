using Android.Views;
using AndroidX.RecyclerView.Widget;

namespace Deepo.Client.App.ViewHolder
{
    public class NewReleasesViewHolder : RecyclerView.ViewHolder
    {
        public TextView? Name { get; private set; }
        public TextView? AuthorsNames { get; private set; }
        public TextView? ReleaseDate { get; private set; }
        public ImageView? Cover { get; private set; }

        public NewReleasesViewHolder(View itemView) : base(itemView)
        {
            ArgumentNullException.ThrowIfNull(itemView, nameof(itemView));

            Name = itemView.FindViewById<TextView>(Resource.Id.newReleasesItemName);
            AuthorsNames = itemView.FindViewById<TextView>(Resource.Id.newReleasesItemAuthors);
            ReleaseDate = itemView.FindViewById<TextView>(Resource.Id.newReleasesItemReleaseDate);
            Cover = itemView.FindViewById<ImageView>(Resource.Id.newReleasesCover);
        }
    }
}
