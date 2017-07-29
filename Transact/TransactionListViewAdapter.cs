﻿﻿using System;
using System.Collections.Generic;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Support.V7.Widget;

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

    public class TransactionListViewAdapter : RecyclerView.Adapter
    {
        private List<Transaction> mItems;

        public TransactionListViewAdapter(List<Transaction> items)
        {
            mItems = items;
        }

        public override int ItemCount => mItems.Count;

		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
		{
			View itemView = LayoutInflater.From(parent.Context).
					Inflate(Resource.Layout.listView_transactions, parent, false);
            TransactionViewHolder vh = new TransactionViewHolder(itemView, OnClick, OnLongClick);
			return vh;
		}

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
			TransactionViewHolder vh = holder as TransactionViewHolder;
            vh.TransactionDate.Text = mItems[position].Date.ToString("MMM-dd");
			vh.TransactionName.Text = mItems[position].Title;
            vh.TransactionCategory.Text = mItems[position].Category;

            //if balanace is 0, text color is black; if balance is greater than 0, text color is green; if balance is less than 0, text color is red
            if (mItems[position].Amount == 0)
			{
				vh.TransactionAmount.SetTextColor(Color.Black);
			}
			else if (mItems[position].Amount > 0)
			{
				vh.TransactionAmount.SetTextColor(Color.DarkGreen);
			}
			else
			{
				vh.TransactionAmount.SetTextColor(Color.Red);
			}
			vh.TransactionAmount.Text = "$" + mItems[position].Amount.ToString("0.00");
        }

        public event EventHandler<int> ItemClick;
        public event EventHandler<int> ItemLongClick;

		void OnClick(int position)
		{
			if (ItemClick != null)
				ItemClick(this, position);
		}

		void OnLongClick(int position)
		{
			if (ItemLongClick != null)
				ItemLongClick(this, position);
		}
    }
}