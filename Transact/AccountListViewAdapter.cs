using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Java.Text;
using Android.Support.V7.Widget;

namespace Transact
{
    public class AccountListViewAdapter : RecyclerView.Adapter
    {
        private List<Account> mItems;

        public AccountListViewAdapter(List<Account> items)
        {
            mItems = items;
        }

        public override int ItemCount => mItems.Count;


        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            throw new NotImplementedException();
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            throw new NotImplementedException();
        }
    }
}