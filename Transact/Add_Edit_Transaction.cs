using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Views;
using System.Threading.Tasks;

namespace Transact
{
    [Activity(Label = "@string/enter_activity", Icon = "@mipmap/icon", WindowSoftInputMode = SoftInput.AdjustResize)]
    public class Add_Edit_Transaction : Activity
    {
        private string type;
        private int accountPK;
        private string accountName;
        private int transactionPK;
        private EditText title;
        private EditText amount;
        private EditText date;
        private EditText type_toaccount;
        private EditText notes;
        private Spinner category;
        private ArrayAdapter transactionCategoryAdapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Add_Edit_Transaction);

            //get account pk that is passed from transaction screen
            accountPK = Intent.GetIntExtra("AccountPK",0);
            accountName = Intent.GetStringExtra("AccountName");
            type = Intent.GetStringExtra("Type");

            // Get our controls from the layout resource
            title = FindViewById<EditText>(Resource.Id.txtTitle);
            amount = FindViewById<EditText>(Resource.Id.txtAmount);
            date = FindViewById<EditText>(Resource.Id.txtDate);
            type_toaccount = FindViewById<EditText>(Resource.Id.txtType_ToAccount);
            notes = FindViewById<EditText>(Resource.Id.txtNotes);

            category = FindViewById<Spinner>(Resource.Id.spinnerCategory);
            transactionCategoryAdapter = ArrayAdapter.CreateFromResource(this, Resource.Array.category_array, Android.Resource.Layout.SimpleSpinnerItem);
            transactionCategoryAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
			category.Adapter = transactionCategoryAdapter;

            //AutoCompleteTextView test = FindViewById<AutoCompleteTextView>(Resource.Id.autoCompleteTextView1);
            //var categories = new string[] { "ATM", "Auto Maintenance" };
            //ArrayAdapter adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, categories);
            //test.Adapter = adapter;

            if (type == "Edit")
            {
                transactionPK = Intent.GetIntExtra("TransactionPK", 0);
                loadTransaction(transactionPK);
            }

            //top toolbar
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar_top);
            SetActionBar(toolbar);
            ActionBar.Title = accountName + " - " + type + " Transaction";

            //bottom toolbar
            var bottomToolbar = FindViewById<Toolbar>(Resource.Id.toolbar_bottom);
            bottomToolbar.Title = "";
            bottomToolbar.InflateMenu(Resource.Menu.bottom_menu_add_transaction);
            bottomToolbar.MenuItemClick += (sender, e) => {
                if (e.Item.TitleFormatted.ToString() == "Save")
                {
                    if (type == "Add")
                    {
                        addTransaction();
                    }
                    else if (type == "Edit")
                    {
                        editTransaction();
                    }                   
                }
                else if (e.Item.TitleFormatted.ToString() == "Cancel")
                {
                    this.Finish();
                }
            };
        }

        private void loadTransaction(int transactionPK)
        {
            //go through each account and update the account with the new balance
            foreach (Transaction trans in Transactions.transactions)
            {
                if (trans.PK == transactionPK)
                {
                    title.Text = trans.Title;
                    amount.Text = trans.Amount.ToString();
                    date.Text = trans.Date.ToString("MM-dd-yyyy");
                    category.SetSelection(transactionCategoryAdapter.GetPosition(trans.Category));
                    type_toaccount.Text = trans.Type_ToAccount;
                    notes.Text = trans.Notes;
                    break;
                }
            }
        }

        private async void addTransaction(){
            //do checks to make sure data is entered into form before saving
            if(title.Text != ""){
                if (amount.Text != ""){
                    if (date.Text != ""){
                        if(category.SelectedItem.ToString() != ""){
                            if(type_toaccount.Text != ""){
                                await MainActivity.db.addTransaction(accountPK, Convert.ToDateTime(date.Text.ToString()), title.Text, Convert.ToDecimal(amount.Text), category.SelectedItem.ToString(), type_toaccount.Text, notes.Text);
                                //Transactions.transactionAdapter.NotifyDataSetChanged();
                                //set selected item to the last item in the list
                                Transactions.mLayoutManager.ScrollToPosition(Transactions.transactions.Count - 1);
                                this.Finish();  //close view when finished entering transaction
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

        private async void editTransaction()
        {
            //do checks to make sure data is entered into form before saving
            if (title.Text != "")
            {
                if (amount.Text != "")
                {
                    if (date.Text != "")
                    {
                        if (category.SelectedItem.ToString() != "")
                        {
                            if (type_toaccount.Text != "")
                            {
                                await MainActivity.db.updateTransaction(transactionPK, accountPK, Convert.ToDateTime(date.Text.ToString()), title.Text, Convert.ToDecimal(amount.Text), category.SelectedItem.ToString(), type_toaccount.Text, notes.Text);
                                //Transactions.transactionAdapter.NotifyDataSetChanged();
                                //set selected item to the last item in the list
                                Transactions.mLayoutManager.ScrollToPosition(Transactions.transactions.Count - 1);
                                this.Finish();  //close view when finished entering transaction
                            }
                            else
                            {
                                type_toaccount.RequestFocus();
                                Toast.MakeText(this, "Type cannot be null/empty", ToastLength.Short).Show();
                            }
                        }
                        else
                        {
                            category.RequestFocus();
                            Toast.MakeText(this, "Category cannot be null/empty", ToastLength.Short).Show();
                        }
                    }
                    else
                    {
                        date.RequestFocus();
                        Toast.MakeText(this, "Date cannot be null/empty", ToastLength.Short).Show();
                    }
                }
                else
                {
                    amount.RequestFocus();
                    Toast.MakeText(this, "Amount cannot be null/empty", ToastLength.Short).Show();
                }
            }
            else
            {
                title.RequestFocus();
                Toast.MakeText(this, "Item cannot be null/empty", ToastLength.Short).Show();
            }
        }
    }
}