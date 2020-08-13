using System;
using AplicativoMeditacao.Interfaces;
using Xamarin.Forms;
using AplicativoMeditacao.Droid.NetworkManager;
using Android.Net;
using Android.Content;

[assembly: Dependency(typeof(NetworkConnection))]
namespace AplicativoMeditacao.Droid.NetworkManager
{
    public class NetworkConnection : INetworkConnectivity
    {
        public bool HasConnectivity()
        {
            var context = Android.App.Application.Context;

            ConnectivityManager connectivityManager = (ConnectivityManager)context.GetSystemService(Context.ConnectivityService);
            NetworkInfo activeConnection = connectivityManager.ActiveNetworkInfo;
            bool isOnline = (activeConnection != null) && activeConnection.IsConnected;

            return isOnline;
        }
    }
}