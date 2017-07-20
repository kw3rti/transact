using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace Transact
{
    [Activity(Label = "@string/enter_activity", Icon = "@mipmap/icon")]
    public class Transactions : Activity
    {
        public static List<Transaction> transactons;
		public static ListView lstTransactions;
		public static TransactionListViewAdapter transactionAdapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Transactions);

            var accountPK = Intent.GetIntExtra("AccountPK",0);
            var accountName = Intent.GetStringExtra("AccountName");

            transactons = new List<Transaction>();

            MainActivity.db.readTransactionRecords(accountPK);

            // Get our button from the layout resource and attach an event to it
            Button addTransaction = FindViewById<Button>(Resource.Id.btnAddTransaction1);
            lstTransactions = FindViewById<ListView>(Resource.Id.lstTransactions);
          
            transactionAdapter = new TransactionListViewAdapter(this, transactons);
            //set selected item to the last item in the list
            lstTransactions.Adapter = transactionAdapter;
            //scroll list to bottom
            //lstTransactions.SetSelection(transactionAdapter.Count - 1);

            //click events for short and long of the listview for the accounts
            lstTransactions.ItemClick += LstTransactions_ItemClick;
			lstTransactions.ItemLongClick += LstTransactions_ItemLongClick;

            addTransaction.Click += delegate {
				var intent = new Intent(this, typeof(EnterTransaction));
				intent.PutExtra("AccountPK", accountPK);
                intent.PutExtra("AccountName", accountName);
				StartActivity(intent);
            };

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
            ActionBar.Title = accountName;
        }

		private void LstTransactions_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
            Toast.MakeText(this, transactons[e.Position].Title + " | open to edit screen", ToastLength.Short).Show();
        }

		private void LstTransactions_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
		{
            //display a popup menu when long pressing an item in the transaction list
            //handle the menu item (only delete option for transactions) click event
            PopupMenu menu = new PopupMenu(this, lstTransactions.GetChildAt(e.Position), Android.Views.GravityFlags.Right);
            menu.Inflate(Resource.Layout.popup_menu_transaction);
            menu.MenuItemClick += (s1, arg1) => {
                Console.WriteLine(transactons[e.Position].Title + " | " + arg1.Item.TitleFormatted + " selected");
                Toast.MakeText(this, transactons[e.Position].Title + " | " + arg1.Item.TitleFormatted + " selected", ToastLength.Short).Show();
            };
            menu.Show();
        }
    }
}