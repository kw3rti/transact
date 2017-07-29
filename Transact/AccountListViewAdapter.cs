﻿﻿using System;
using System.Collections.Generic;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Support.V7.Widget;

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

    public class AccountListViewAdapter : RecyclerView.Adapter
    {
        private List<Account> mItems;

        public AccountListViewAdapter(List<Account> items)
        {
            mItems = items;
        }

        public override int ItemCount => mItems.Count;

		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
		{
			View itemView = LayoutInflater.From(parent.Context).
					Inflate(Resource.Layout.listView_accounts, parent, false);
            AccountViewHolder vh = new AccountViewHolder(itemView, OnClick, OnLongClick);
			return vh;
		}

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
			AccountViewHolder vh = holder as AccountViewHolder;
            vh.AccountName.Text = mItems[position].Name;
			vh.AccountNote.Text = mItems[position].Note;

			//if balanace is 0, text color is black; if balance is greater than 0, text color is green; if balance is less than 0, text color is red
			if (mItems[position].Balance == 0)
			{
				vh.AccountBalance.SetTextColor(Color.Black);
			}
			else if (mItems[position].Balance > 0)
			{
				vh.AccountBalance.SetTextColor(Color.DarkGreen);
			}
			else
			{
				vh.AccountBalance.SetTextColor(Color.Red);
			}
			vh.AccountBalance.Text = "$" + mItems[position].Balance.ToString("0.00");
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