using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using System.Collections.Generic;

namespace Transact
{
    [Activity(Label = "@string/app_name", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
		//initalize database and account list
		public static Database db = new Database();
        public static List<Account> accounts;
        public static ListView lstAccounts;
        public static AccountListViewAdapter accountAdapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            //hide the title bar
            RequestWindowFeature(Android.Views.WindowFeatures.NoTitle);

            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource and attach an event to it
            lstAccounts = FindViewById<ListView>(Resource.Id.lstAccounts);
            Button btnBillReminder = FindViewById<Button>(Resource.Id.btnBillReminder);
            Button btnAddAccount = FindViewById<Button>(Resource.Id.btnAddAccount);

            db = new Database();
            accounts = new List<Account>();
            //load accounts from the database
            db.readAccounts();

            accountAdapter = new AccountListViewAdapter(this, accounts);
            lstAccounts.Adapter = accountAdapter;
            
            //click events for short and long of the listview for the accounts
            lstAccounts.ItemClick += LstAccounts_ItemClick;
            lstAccounts.ItemLongClick += LstAccounts_ItemLongClick;

            btnBillReminder.Click += delegate {
                Toast.MakeText(this, "Bill Reminder in future", ToastLength.Short).Show();
            };

            btnAddAccount.Click += delegate {
                var intent = new Intent(this, typeof(AddAccount));
                StartActivity(intent);
            };
        }
        private void LstAccounts_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var intent = new Intent(this, typeof(Transactions));
            intent.PutExtra("AccountPK", accounts[e.Position].PK);
			StartActivity(intent);
        }

        private void LstAccounts_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            Toast.MakeText(this, accounts[e.Position].Name + " was long clicked!", ToastLength.Short).Show();
        }
    }
}