﻿using System;
using Android.App;
using Android.OS;
using Android.Widget;
using Android.Views;

namespace Transact
{
    [Activity(Label = "@string/add_account", Icon = "@mipmap/icon", WindowSoftInputMode = SoftInput.AdjustResize)]
    public class Add_Edit_Account : Activity
    {
        private string type;
        private int accountPK;
        private EditText name;
        private EditText note;
        private EditText startBalance;
        private Spinner accountType;
        private ArrayAdapter accountTypeAdapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Add_Edit_Account);

            type = Intent.GetStringExtra("Type");           

            name = FindViewById<EditText>(Resource.Id.txtAccountName);
            note = FindViewById<EditText>(Resource.Id.txtAccountNote);
            startBalance = FindViewById<EditText>(Resource.Id.txtAccountStartBalance);

			accountType = FindViewById<Spinner>(Resource.Id.spinnerAccountType);
            accountTypeAdapter = ArrayAdapter.CreateFromResource(this, Resource.Array.account_array, Android.Resource.Layout.SimpleSpinnerDropDownItem);
            //accountTypeAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            accountType.Adapter = accountTypeAdapter;

            if (type == "Edit")
            {
                accountPK = Intent.GetIntExtra("AccountPK", 0);
                loadAccount(accountPK);
            }

            //top toolbar
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar_top);
            SetActionBar(toolbar);
            ActionBar.Title = type + " Account";

            //bottom toolbar
            var bottomToolbar = FindViewById<Toolbar>(Resource.Id.toolbar_bottom);
            bottomToolbar.Title = "";
            bottomToolbar.InflateMenu(Resource.Menu.bottom_menu_add_account);
            bottomToolbar.MenuItemClick += (sender, e) => {
                if (e.Item.TitleFormatted.ToString() == "Save")
                {
                    if(type == "Add")
                    {
                        addAccount();
                    }
                    else if(type == "Edit")
                    {
                        updateAccount();
                    }
                }
                else if (e.Item.TitleFormatted.ToString() == "Cancel")
                {
                    this.Finish();
                }
            };
        }

        private void loadAccount(int accountPK)
        {
            //go through each account and update the account with the new balance
            foreach (Account act in MainActivity.accounts)
            {
                if (act.PK == accountPK)
                {
                    name.Text = act.Name;
                    note.Text = act.Note;
                    startBalance.Text = act.InitialBalance.ToString();
                    accountType.SetSelection(accountTypeAdapter.GetPosition(act.Type));
                    break;
                }
            }
            
        }

        private async void addAccount()
        {
            //do checks to make sure data is entered into form before saving
            if (name.Text != "")
            {
                if (accountType.SelectedItem.ToString() != "")
                {
                    if (startBalance.Text != "")
                    {
                        await MainActivity.db.addAccount(name.Text, note.Text, accountType.SelectedItem.ToString(), Convert.ToDecimal(startBalance.Text), DateTime.Now, "Initial Balance", "", "");
                        MainActivity.accountAdapter.NotifyDataSetChanged();
                        //set selected item to the last item in the list
                        MainActivity.mLayoutManager.ScrollToPosition(MainActivity.accounts.Count - 1);
                        this.Finish();  //close view when finished entering transaction                          
                    }
                    else
                    {
                        startBalance.RequestFocus();
                        Toast.MakeText(this, "Staring Balance cannot be null/empty", ToastLength.Short).Show();
                    }
                }
                else
                {
                    accountType.RequestFocus();
                    Toast.MakeText(this, "Type cannot be null/empty", ToastLength.Short).Show();
                }
            }
            else
            {
                name.RequestFocus();
                Toast.MakeText(this, "Name cannot be null/empty", ToastLength.Short).Show();
            }
        }

        private async void updateAccount()
        {
            //do checks to make sure data is entered into form before saving
            if (name.Text != "")
            {
                if (accountType.SelectedItem.ToString() != "")
                {
                    if (startBalance.Text != "")
                    {
                        await MainActivity.db.updateAccount(accountPK, name.Text, note.Text, accountType.SelectedItem.ToString(), Convert.ToDecimal(startBalance.Text));
                        MainActivity.accountAdapter.NotifyDataSetChanged();
                        //set selected item to the last item in the list
                        MainActivity.mLayoutManager.ScrollToPosition(MainActivity.accounts.Count - 1);
                        this.Finish();  //close view when finished entering transaction                          
                    }
                    else
                    {
                        startBalance.RequestFocus();
                        Toast.MakeText(this, "Staring Balance cannot be null/empty", ToastLength.Short).Show();
                    }
                }
                else
                {
                    accountType.RequestFocus();
                    Toast.MakeText(this, "Type cannot be null/empty", ToastLength.Short).Show();
                }
            }
            else
            {
                name.RequestFocus();
                Toast.MakeText(this, "Name cannot be null/empty", ToastLength.Short).Show();
            }
        }
    }
}