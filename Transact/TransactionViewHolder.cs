using System;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace Transact
{
	public class TransactionViewHolder : RecyclerView.ViewHolder
	{
		public TextView TransactionDate { get; private set; }
		public TextView TransactionName { get; private set; }
        public TextView TransactionCategory { get; private set; }
        public TextView TransactionAmount { get; private set; }

		public TransactionViewHolder(View itemView, Action<int> listener, Action<int> listener2) : base(itemView)
		{
			// Locate and cache view references:
			TransactionDate = itemView.FindViewById<TextView>(Resource.Id.txtTransactionDate);
			TransactionName = itemView.FindViewById<TextView>(Resource.Id.txtTransactionName);
            TransactionCategory = itemView.FindViewById<TextView>(Resource.Id.txtTransactionCategory);
            TransactionAmount = itemView.FindViewById<TextView>(Resource.Id.txtTransactionAmount);

            itemView.Click += (sender, e) => listener(base.Position);
            itemView.LongClick += (sender, e) => listener2(base.Position);
		}
	}
}
