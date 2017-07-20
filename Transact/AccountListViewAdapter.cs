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
        //private Context mContext;

        public AccountListViewAdapter(List<Account> items)
        {
            mItems = items;
            //mContext = context;
        }

        //public override int Count => mItems.Count;

        public override int ItemCount => throw new NotImplementedException();

        public override long GetItemId(int position)
        {
            return position;
        }

        //public override Account this[int position] => mItems[position];



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