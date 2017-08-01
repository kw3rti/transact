using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Widget;
using Android.Views;

namespace Transact
{
    [Activity(ParentActivity = typeof(MainActivity), Label = "@string/enter_activity", Icon = "@mipmap/icon", WindowSoftInputMode = SoftInput.AdjustResize)]
    public class Transactions : Activity
    {
        public static List<Transaction> transactions;
		public static RecyclerView mRecyclerView; //change to recyclerview
		public static RecyclerView.LayoutManager mLayoutManager;
		public static TransactionListViewAdapter transactionAdapter;
        private int accountPK;
        private string accountName;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Transactions);

            accountPK = Intent.GetIntExtra("AccountPK",0);
            accountName = Intent.GetStringExtra("AccountName");

            // Get our button from the layout resource and attach an event to it
            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.lstTransactions);

            transactions = new List<Transaction>();

            MainActivity.db.readTransactionRecords(accountPK);		

			mLayoutManager = new LinearLayoutManager(this);
			mRecyclerView.SetLayoutManager(mLayoutManager);
            mLayoutManager.ScrollToPosition(transactions.Count-1);

            transactionAdapter = new TransactionListViewAdapter(transactions);
			transactionAdapter.ItemClick += OnItemClick;
			transactionAdapter.ItemLongClick += OnItemLongClick;
			mRecyclerView.SetAdapter(transactionAdapter);

            //top toolbar
            var toolbar = FindViewById<Android.Widget.Toolbar>(Resource.Id.toolbar_top);
            SetActionBar(toolbar);
            ActionBar.Title = accountName;
            ActionBar.SetDisplayHomeAsUpEnabled(true); //enable the "up" button in the toolbar to navigate back

            //bottom toolbar
            var bottomToolbar = FindViewById<Android.Widget.Toolbar>(Resource.Id.toolbar_bottom);
            bottomToolbar.Title = "";
            bottomToolbar.InflateMenu(Resource.Menu.bottom_menu_transaction_list);
            bottomToolbar.MenuItemClick += (sender, e) => {
                if (e.Item.TitleFormatted.ToString() == "Add")
                {
                    var intent = new Intent(this, typeof(Add_Edit_Transaction));
                    intent.PutExtra("Type", "Add");     //inform this is a new transaction
                    intent.PutExtra("AccountPK", accountPK);
                    intent.PutExtra("AccountName", accountName);
                    StartActivity(intent);
                }
            };
        }

		void OnItemClick(object sender, int position)
		{
            var intent = new Intent(this, typeof(Add_Edit_Transaction));
            intent.PutExtra("Type", "Edit");     //inform this is a edit transaction
            intent.PutExtra("AccountPK", accountPK);
            intent.PutExtra("AccountName", accountName);
            intent.PutExtra("TransactionPK", transactions[position].PK);
            StartActivity(intent);
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
                    if (arg1.Item.TitleFormatted.ToString() == "Delete")
                    {
                        AlertDialog.Builder builder;
                        builder = new AlertDialog.Builder(this);
                        builder.SetTitle("Delete?");
                        builder.SetMessage("Are you sure you want to delete the transaction: " + transactions[position].Title);
                        builder.SetCancelable(false);
                        builder.SetPositiveButton("Yes", delegate { MainActivity.db.deleteTransaction(accountPK, transactions[position].PK); });
                        builder.SetNegativeButton("Cancel", delegate { builder.Dispose(); });
                        builder.Show();

                    }
                    Console.WriteLine(transactions[position].Title + " | " + arg1.Item.TitleFormatted + " selected");
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