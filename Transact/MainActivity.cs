using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using System.Collections.Generic;
using System;
using Android.Support.V7.Widget;

namespace Transact
{
    [Activity(Label = "@string/app_name", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
		//database and account list(list, listview, and the custom list view adapter) variables
		public static Database db = new Database();
        public static List<Account> accounts;
        public static RecyclerView mRecyclerView; //change to recyclerview
        RecyclerView.LayoutManager mLayoutManager;
        public static AccountListViewAdapter accountAdapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our controls from the layout resource
            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.lstAccounts);
            Button btnBillReminder = FindViewById<Button>(Resource.Id.btnBillReminder);
            Button btnAddAccount = FindViewById<Button>(Resource.Id.btnAddAccount);

            //initialize the database class
            db = new Database();
            //create a new account list
            accounts = new List<Account>();
            //load accounts from the database (adds the accounts in the account list)
            db.readAccounts();

            mLayoutManager = new LinearLayoutManager(this);
            mRecyclerView.SetLayoutManager(mLayoutManager);

			//create a new account list view adapter and set the listview from the layouts adapter to the custom one
			accountAdapter = new AccountListViewAdapter(accounts);
			mRecyclerView.SetAdapter(accountAdapter);

            //short and long click events for the account listview
            //lstAccounts.ItemClick += LstAccounts_ItemClick;
            //lstAccounts.ItemLongClick += LstAccounts_ItemLongClick;

            //click event for the bill reminder button
            btnBillReminder.Click += delegate {
                Toast.MakeText(this, "Bill Reminder in future", ToastLength.Short).Show();
            };

            //click even for the add account button
            btnAddAccount.Click += delegate {
                var intent = new Intent(this, typeof(AddAccount));
                StartActivity(intent);
            };

            //set the custom toolbar and add a title
            var toolbar = FindViewById<Android.Widget.Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
            ActionBar.Title = "Account Overview";
        }
        private void LstAccounts_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //when the account is clicked - open the transaction list for the account
            //pass the account pk and name to the transaction list page
            var intent = new Intent(this, typeof(Transactions));
            intent.PutExtra("AccountPK", accounts[e.Position].PK);
            intent.PutExtra("AccountName", accounts[e.Position].Name);
			StartActivity(intent);
        }

        private void LstAccounts_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            //display a popup menu when long pressing an item in the account list
            //handle the menu item (edit and delete options for accounts) click event
            Android.Widget.PopupMenu menu = new Android.Widget.PopupMenu(this, mRecyclerView.GetChildAt(e.Position), Android.Views.GravityFlags.Right);
            menu.Inflate(Resource.Layout.popup_menu_account);
            menu.MenuItemClick += (s1, arg1) => {
                Console.WriteLine(accounts[e.Position].Name + " | " + arg1.Item.TitleFormatted + " selected");
                Toast.MakeText(this, accounts[e.Position].Name + " | " + arg1.Item.TitleFormatted + " selected" , ToastLength.Short).Show();
            };
            menu.Show();
        }
    }
}