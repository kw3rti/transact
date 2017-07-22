using System;
using Android.App;
using Android.OS;
using Android.Widget;

namespace Transact
{
    [Activity(Label = "@string/add_account", Icon = "@mipmap/icon")]
    public class AddAccount : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.AddAccount);

            // Get our button from the layout resource and attach an event to it
            //Button addButton = FindViewById<Button>(Resource.Id.btnAccountAdd);
            //Button cancelButton = FindViewById<Button>(Resource.Id.btnAccountCancel);

            EditText name = FindViewById<EditText>(Resource.Id.txtAccountName);
            EditText note = FindViewById<EditText>(Resource.Id.txtAccountNote);
            EditText startBalance = FindViewById<EditText>(Resource.Id.txtAccountStartBalance);

			Spinner type = FindViewById<Spinner>(Resource.Id.spinnerAccountType);
			var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.account_array, Android.Resource.Layout.SimpleSpinnerItem);
			adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
			type.Adapter = adapter;
          
            //addButton.Click += async delegate {
                //await MainActivity.db.addAccount(name.Text, note.Text, type.SelectedItem.ToString(), Convert.ToDecimal(startBalance.Text), DateTime.Now, "Initial Balance", "", "");
                //MainActivity.accountAdapter.NotifyDataSetChanged();
               // this.Finish();
           // };
            //cancelButton.Click += delegate { this.Finish(); };

            //top toolbar
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar_top);
            SetActionBar(toolbar);
            ActionBar.Title = "Add Account";

            //bottom toolbar
            var bottomToolbar = FindViewById<Toolbar>(Resource.Id.toolbar_bottom);
            bottomToolbar.Title = "";
            bottomToolbar.InflateMenu(Resource.Menu.bottom_menu_add_account);
            bottomToolbar.MenuItemClick += (sender, e) => {
                if (e.Item.TitleFormatted.ToString() == "Save")
                {
                    MainActivity.db.addAccount(name.Text, note.Text, type.SelectedItem.ToString(), Convert.ToDecimal(startBalance.Text), DateTime.Now, "Initial Balance", "", "");
                    MainActivity.accountAdapter.NotifyDataSetChanged();
                    this.Finish();
                }
                else if (e.Item.TitleFormatted.ToString() == "Cancel")
                {
                    this.Finish();
                }
            };
        }
    }
}