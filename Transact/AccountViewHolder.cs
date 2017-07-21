using System;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace Transact
{
	public class AccountViewHolder : RecyclerView.ViewHolder
	{
		public TextView AccountName { get; private set; }
		public TextView AccountNote { get; private set; }
        public TextView AccountBalance { get; private set; }

		public AccountViewHolder(View itemView, Action<int> listener, Action<int> listener2) : base(itemView)
		{
			// Locate and cache view references:
			AccountName = itemView.FindViewById<TextView>(Resource.Id.txtAccountName);
			AccountNote = itemView.FindViewById<TextView>(Resource.Id.txtAccountNote);
            AccountBalance = itemView.FindViewById<TextView>(Resource.Id.txtAccountBalance);

            itemView.Click += (sender, e) => listener(base.Position);
            itemView.LongClick += (sender, e) => listener2(base.Position);
		}
	}
}
