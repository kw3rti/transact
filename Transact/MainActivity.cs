﻿using Android.App;
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
        public static RecyclerView mRecyclerView;
        public static RecyclerView.LayoutManager mLayoutManager;
        public static AccountListViewAdapter accountAdapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our controls from the layout resource
            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.lstAccounts);

            //initialize the database class
            db = new Database();
            //create a new account list
            accounts = new List<Account>();
            //load accounts from the database (adds the accounts in the account list)
            db.readAccounts();

            //setup the layout manager and set the recyclerview to use it
            mLayoutManager = new LinearLayoutManager(this);
            mRecyclerView.SetLayoutManager(mLayoutManager);

			//create a new account list view adapter and set the listview from the layouts adapter to the custom one
			accountAdapter = new AccountListViewAdapter(accounts);
            accountAdapter.ItemClick += OnItemClick;
            accountAdapter.ItemLongClick += OnItemLongClick;
			mRecyclerView.SetAdapter(accountAdapter);         

            //top toolbar
            var toolbar = FindViewById<Android.Widget.Toolbar>(Resource.Id.toolbar_top);
            SetActionBar(toolbar);
            ActionBar.Title = "Account Overview";

            //bottom toolbar
            var bottomToolbar = FindViewById<Android.Widget.Toolbar>(Resource.Id.toolbar_bottom);
            bottomToolbar.Title = "";
            bottomToolbar.InflateMenu(Resource.Menu.bottom_menu_account_list);
            bottomToolbar.MenuItemClick += (sender, e) => {
                if(e.Item.TitleFormatted.ToString() == "Add")
                {
                    var intent = new Intent(this, typeof(AddAccount));
                    StartActivity(intent);
                }
                else if(e.Item.TitleFormatted.ToString() == "Bill Reminder")
                {
                    Toast.MakeText(this, "Bill Reminder: Furture Enhancement", ToastLength.Short).Show();                    
                }                
            };
        }

		void OnItemClick(object sender, int position)
		{
			//when the account is clicked - open the transaction list for the account
			//pass the account pk and name to the transaction list page
			var intent = new Intent(this, typeof(Transactions));
			intent.PutExtra("AccountPK", accounts[position].PK);
			intent.PutExtra("AccountName", accounts[position].Name);
			StartActivity(intent);
		}

		void OnItemLongClick(object sender, int position)
		{
			//display a popup menu when long pressing an item in the account list
			//handle the menu item (edit and delete options for accounts) click event
            try{
				Android.Widget.PopupMenu menu = new Android.Widget.PopupMenu(this, mRecyclerView.FindViewHolderForAdapterPosition(position).ItemView, Android.Views.GravityFlags.Right);
				menu.Inflate(Resource.Layout.popup_menu_account);
				menu.MenuItemClick += (s1, arg1) =>
				{
                    if(arg1.Item.TitleFormatted.ToString() == "Edit")
                    {
                        Toast.MakeText(this, accounts[position].Name + " | " + arg1.Item.TitleFormatted + " selected", ToastLength.Short).Show();
                    }
                    else if(arg1.Item.TitleFormatted.ToString() == "Delete")
                    {
                        AlertDialog.Builder builder;
                        builder = new AlertDialog.Builder(this);
                        builder.SetTitle("Delete?");
                        builder.SetMessage("Are you sure you want to delete the account: " + accounts[position].Name);
                        builder.SetCancelable(false);
                        builder.SetPositiveButton("Yes", delegate { db.deleteAccount(accounts[position].PK); });
                        builder.SetNegativeButton("Cancel", delegate { builder.Dispose(); });
                        builder.Show();
                    }
					Console.WriteLine(accounts[position].Name + " | " + arg1.Item.TitleFormatted + " selected");               
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