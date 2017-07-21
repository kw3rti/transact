using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Widget;

namespace Transact
{
    [Activity(Label = "@string/enter_activity", Icon = "@mipmap/icon")]
    public class Transactions : Activity
    {
        public static List<Transaction> transactons;
		public static RecyclerView mRecyclerView; //change to recyclerview
		RecyclerView.LayoutManager mLayoutManager;
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
            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.lstTransactions);

			mLayoutManager = new LinearLayoutManager(this);
			mRecyclerView.SetLayoutManager(mLayoutManager);

            transactionAdapter = new TransactionListViewAdapter(transactons);
			//create a new account list view adapter and set the listview from the layouts adapter to the custom one
			transactionAdapter.ItemClick += OnItemClick;
			transactionAdapter.ItemLongClick += OnItemLongClick;
			mRecyclerView.SetAdapter(transactionAdapter);
            //scroll list to bottom
            //lstTransactions.SetSelection(transactionAdapter.Count - 1);

            //click events for short and long of the listview for the accounts
            //lstTransactions.ItemClick += LstTransactions_ItemClick;
			//lstTransactions.ItemLongClick += LstTransactions_ItemLongClick;

            addTransaction.Click += delegate {
				var intent = new Intent(this, typeof(EnterTransaction));
				intent.PutExtra("AccountPK", accountPK);
                intent.PutExtra("AccountName", accountName);
				StartActivity(intent);
            };

            var toolbar = FindViewById<Android.Widget.Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
            ActionBar.Title = accountName;
        }


		void OnItemClick(object sender, int position)
		{
			Toast.MakeText(this, transactons[position].Title + " | open to edit screen", ToastLength.Short).Show();
		}

		void OnItemLongClick(object sender, int position)
		{
			//display a popup menu when long pressing an item in the account list
			//handle the menu item (edit and delete options for accounts) click event
			try
			{
				Android.Widget.PopupMenu menu = new Android.Widget.PopupMenu(this, mRecyclerView.FindViewHolderForAdapterPosition(position).ItemView, Android.Views.GravityFlags.Right);
				menu.Inflate(Resource.Layout.popup_menu_transaction);
				menu.MenuItemClick += (s1, arg1) =>
				{
					Console.WriteLine(transactons[position].Title + " | " + arg1.Item.TitleFormatted + " selected");
					Toast.MakeText(this, transactons[position].Title + " | " + arg1.Item.TitleFormatted + " selected", ToastLength.Short).Show();
				};
				menu.Show();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
    }
}