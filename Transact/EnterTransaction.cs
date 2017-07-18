using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace Transact
{
    [Activity(Label = "@string/enter_activity", Icon = "@mipmap/icon")]
    public class EnterTransaction : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.EnterTransaction);

            //get account pk that is passed from transaction screen
            var accountPK = Intent.GetIntExtra("AccountPK",0);
            var accountName = Intent.GetStringExtra("AccountName");

            // Get our controls from the layout resource
            Button insertButton = FindViewById<Button>(Resource.Id.btnAddTransaction);
            Button cancelButton = FindViewById<Button>(Resource.Id.btnCancelTransaction);
            EditText title = FindViewById<EditText>(Resource.Id.txtTitle);
            EditText amount = FindViewById<EditText>(Resource.Id.txtAmount);
            EditText date = FindViewById<EditText>(Resource.Id.txtDate);
            EditText type_toaccount = FindViewById<EditText>(Resource.Id.txtType_ToAccount);
            EditText notes = FindViewById<EditText>(Resource.Id.txtNotes);

            Spinner category = FindViewById<Spinner>(Resource.Id.spinnerCategory);
			var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.category_array, Android.Resource.Layout.SimpleSpinnerItem);
			adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
			category.Adapter = adapter;

            //AutoCompleteTextView test = FindViewById<AutoCompleteTextView>(Resource.Id.autoCompleteTextView1);
            //var categories = new string[] { "ATM", "Auto Maintenance" };
            //ArrayAdapter adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, categories);
            //test.Adapter = adapter;

            //insert button click event (code runs when button on form is clicked)
            insertButton.Click += delegate {
                enterTransaction(accountPK, date, title, amount, category, type_toaccount, notes);
                this.Finish();  //close view when finished entering transaction
            };
            cancelButton.Click += delegate { this.Finish(); };

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
            ActionBar.Title = accountName + " - Enter Transaction";
        }

        private async void enterTransaction(int accountPK, EditText date, EditText title, EditText amount, Spinner category, EditText type_toaccount, EditText notes){
            //do checks to make sure data is entered into form before saving
            if(title.Text != ""){
                if (amount.Text != ""){
                    if (date.Text != ""){
                        if(category.SelectedItem.ToString() != ""){
                            if(type_toaccount.Text != ""){
                                await MainActivity.db.addTransaction(accountPK, Convert.ToDateTime(date.Text.ToString()), title.Text, Convert.ToDecimal(amount.Text), category.SelectedItem.ToString(), type_toaccount.Text, notes.Text);                   
                            }
                            else{
                                type_toaccount.RequestFocus();
								Toast.MakeText(this, "Type cannot be null/empty", ToastLength.Short).Show();
                            }
                        }
                        else{
							category.RequestFocus();
							Toast.MakeText(this, "Category cannot be null/empty", ToastLength.Short).Show();
                        }
                    }
                    else{
						date.RequestFocus();
						Toast.MakeText(this, "Date cannot be null/empty", ToastLength.Short).Show();
                    }
                }
                else{
					amount.RequestFocus();
					Toast.MakeText(this, "Amount cannot be null/empty", ToastLength.Short).Show();
                }  
            }
            else{
                title.RequestFocus();
                Toast.MakeText(this, "Item cannot be null/empty", ToastLength.Short).Show();
            }
           
        }
    }
}