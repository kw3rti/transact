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
            Button addButton = FindViewById<Button>(Resource.Id.btnAccountAdd);
            Button cancelButton = FindViewById<Button>(Resource.Id.btnAccountCancel);

            EditText name = FindViewById<EditText>(Resource.Id.txtAccountName);
            EditText note = FindViewById<EditText>(Resource.Id.txtAccountNote);
            EditText startBalance = FindViewById<EditText>(Resource.Id.txtAccountStartBalance);

			Spinner type = FindViewById<Spinner>(Resource.Id.spinnerAccountType);
            //type.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
			var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.account_array, Android.Resource.Layout.SimpleSpinnerItem);
			adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
			type.Adapter = adapter;
          
            addButton.Click += async delegate {
                await MainActivity.db.addAccount(name.Text, note.Text, type.SelectedItem.ToString(), Convert.ToDecimal(startBalance.Text), DateTime.Now, "Initial Balance", "", "");
                MainActivity.lstAccounts.Adapter = MainActivity.accountAdapter;
                this.Finish();
            };
            cancelButton.Click += delegate { this.Finish(); };

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
            ActionBar.Title = "Add Account";
        }

		private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
		{
			//Spinner spinner = (Spinner)sender;
			//string toast = string.Format("The account type is {0}", spinner.GetItemAtPosition(e.Position));
			//Toast.MakeText(this, toast, ToastLength.Long).Show();
		}
    }
}