using Android.Views;
using AndroidX.RecyclerView.Widget;
using Deepo.Client.AndroidApp.ViewHolder;
using System.Globalization;

namespace Deepo.Client.AndroidApp.Adapter
{
    public class NewReleasesAdapter : RecyclerView.Adapter
    {
        public override int ItemCount => _releasesDto.Count();

        private readonly IEnumerable<Dto.ReleaseVinylDto> _releasesDto;

        public NewReleasesAdapter(IEnumerable<Dto.ReleaseVinylDto> content)
        {
            _releasesDto = content;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            NewReleasesViewHolder vh = (NewReleasesViewHolder)holder;
            if (vh?.Name != null)
            {
                vh.Name.Text = _releasesDto.ElementAt(position).Name ?? "Unknown Artist";
            }
            if (vh?.AuthorsNames != null)
            {
                vh.AuthorsNames.Text = _releasesDto.ElementAt(position).AuthorsNames;
            }
            if (vh?.ReleaseDate != null)
            {
                vh.ReleaseDate.Text = _releasesDto.ElementAt(position).ReleaseDate.ToString(CultureInfo.CurrentCulture);
            }
            if (vh?.Cover != null)
            {
                //vh?.Cover.sour = _releasesDto.ElementAt(position).ThumbUrl.ToString();
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View? itemView = LayoutInflater.From(parent?.Context)?.Inflate(Resource.Layout.newReleases_cardView, parent, false);
            ArgumentNullException.ThrowIfNull(itemView);
            NewReleasesViewHolder vh = new NewReleasesViewHolder(itemView);
            return vh;
        }
    }
}
