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

namespace Transact
{
    public class TransactionListViewAdapter : BaseAdapter<Transaction>
    {
        private List<Transaction> mItems;
        private Context mContext;

        public TransactionListViewAdapter(Context context, List<Transaction> items)
        {
            mItems = items;
            mContext = context;
        }

        public override int Count => mItems.Count;

        public override long GetItemId(int position)
        {
            return position;
        }

        public override Transaction this[int position] => mItems[position];

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;

            if(row == null)
            {
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.listView_transactions, null, false);
            }

            TextView txtTransactionDate = row.FindViewById<TextView>(Resource.Id.txtTransactionDate);
            var date = mItems[position].Date.ToString("yyyy-MM-dd");

            txtTransactionDate.Text = date;

            TextView txtTransactionName = row.FindViewById<TextView>(Resource.Id.txtTransactionName);
            txtTransactionName.Text = mItems[position].Title;

            TextView txtTransactionCategory = row.FindViewById<TextView>(Resource.Id.txtTransactionCategory);
            txtTransactionCategory.Text = mItems[position].Category;

            TextView txtTransactionAmount = row.FindViewById<TextView>(Resource.Id.txtTransactionAmount);

            //if balanace is 0, text color is black; if balance is greater than 0, text color is green; if balance is less than 0, text color is red
            if(mItems[position].Amount == 0)
            {
                txtTransactionAmount.SetTextColor(Color.Black);
            }
            else if(mItems[position].Amount > 0)
            {
                txtTransactionAmount.SetTextColor(Color.DarkGreen);
            }
            else
            {
                txtTransactionAmount.SetTextColor(Color.Red);
            }
            txtTransactionAmount.Text = "$" + mItems[position].Amount.ToString("0.00");

            return row;
        }
    }
}